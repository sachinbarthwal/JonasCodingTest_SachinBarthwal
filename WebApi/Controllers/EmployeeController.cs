using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using BusinessLayer.Logging;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using WebApi.Models;

namespace WebApi.Controllers
{
    // WebApi.Controllers/EmployeeController.cs
    [RoutePrefix("api/employees")]
    public class EmployeeController : ApiController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper, ILoggerService logger)
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            try
            {
                _logger.Info("Retrieving all employees");
                var employees = await _employeeService.GetAllEmployeesAsync();
                var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
                return Ok(employeeDtos);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while getting all employees");
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("{employeeCode}")]
        public async Task<IHttpActionResult> Get(string employeeCode)
        {
            try
            {
                _logger.Info($"Retrieving employee with code: {employeeCode}");
                var employee = await _employeeService.GetEmployeeByCodeAsync(employeeCode);
                if (employee == null)
                {
                    _logger.Warn($"Employee with code {employeeCode} not found");
                    return NotFound();
                }
                var employeeDto = _mapper.Map<EmployeeDto>(employee);
                return Ok(employeeDto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while getting employee with code {employeeCode}");
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Post([FromBody] EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.Warn("Invalid model state for employee creation");
                return BadRequest(ModelState);
            }

            try
            {
                _logger.Info($"Creating new employee: {employeeDto.EmployeeCode}");
                var employee = _mapper.Map<EmployeeInfo>(employeeDto);
                var result = await _employeeService.SaveEmployeeAsync(employee);
                return Created($"api/employees/{employee.EmployeeCode}", employeeDto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while creating an employee");
                return InternalServerError();
            }
        }

        [HttpPut]
        [Route("{employeeCode}")]
        public async Task<IHttpActionResult> Put(string employeeCode, [FromBody] EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.Warn("Invalid model state for employee update");
                return BadRequest(ModelState);
            }

            try
            {
                _logger.Info($"Updating employee: {employeeCode}");
                var employee = _mapper.Map<EmployeeInfo>(employeeDto);
                employee.EmployeeCode = employeeCode;
                var result = await _employeeService.SaveEmployeeAsync(employee);
                return Ok(employeeDto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while updating employee with code {employeeCode}");
                return InternalServerError();
            }
        }

        [HttpDelete]
        [Route("{employeeCode}")]
        public async Task<IHttpActionResult> Delete(string employeeCode)
        {
            try
            {
                _logger.Info($"Deleting employee: {employeeCode}");
                var result = await _employeeService.DeleteEmployeeAsync(employeeCode);
                if (!result)
                {
                    _logger.Warn($"Employee with code {employeeCode} not found for deletion");
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while deleting employee with code {employeeCode}");
                return InternalServerError();
            }
        }
    }
}
