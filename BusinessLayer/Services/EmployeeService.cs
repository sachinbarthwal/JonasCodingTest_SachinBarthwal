using BusinessLayer.Model.Interfaces;
using System;
using System.Collections.Generic;
using AutoMapper;
using BusinessLayer.Model.Models;
using DataAccessLayer.Model.Interfaces;
using System.Threading.Tasks;
using BusinessLayer.Logging;
using DataAccessLayer.Model.Models;

namespace BusinessLayer.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public EmployeeService(IEmployeeRepository employeeRepository, ICompanyRepository companyRepository, IMapper mapper, ILoggerService logger)
        {
            _employeeRepository = employeeRepository;
            _companyRepository = companyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<EmployeeInfo>> GetAllEmployeesAsync()
        {
            try
            {
                _logger.Info("Getting all employees");
                var employees = await _employeeRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<EmployeeInfo>>(employees);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while getting all employees");
                throw;
            }
        }

        public async Task<EmployeeInfo> GetEmployeeByCodeAsync(string employeeCode)
        {
            try
            {
                _logger.Info($"Getting employee by code: {employeeCode}");
                var employee = await _employeeRepository.GetByCodeAsync(employeeCode);
                return _mapper.Map<EmployeeInfo>(employee);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while getting employee by code: {employeeCode}");
                throw;
            }
        }

        public async Task<bool> SaveEmployeeAsync(EmployeeInfo employeeInfo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(employeeInfo.EmployeeCode))
                {
                    _logger.Error("Employee code cannot be empty");
                    throw new ArgumentException("Employee code cannot be empty");
                }

                _logger.Info($"Saving employee: {employeeInfo.EmployeeCode}");
                var employee = _mapper.Map<Employee>(employeeInfo);
                return await _employeeRepository.SaveEmployeeAsync(employee);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while saving employee: {employeeInfo.EmployeeCode}");
                throw;
            }
        }

        public async Task<bool> DeleteEmployeeAsync(string employeeCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(employeeCode))
                {
                    _logger.Error("Employee code cannot be empty for deletion");
                    throw new ArgumentException("Employee code cannot be empty for deletion");
                }

                _logger.Info($"Deleting employee: {employeeCode}");
                return await _employeeRepository.DeleteEmployeeAsync(employeeCode);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while deleting employee: {employeeCode}");
                throw;
            }
        }
    }
}
