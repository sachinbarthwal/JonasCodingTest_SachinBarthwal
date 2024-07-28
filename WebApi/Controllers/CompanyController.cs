using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using BusinessLayer.Model.Interfaces;
using WebApi.Models;
using BusinessLayer.Logging;
using System.Threading.Tasks;
using BusinessLayer.Model.Models;

namespace WebApi.Controllers
{
    public class CompanyController : ApiController
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public CompanyController(ICompanyService companyService, IMapper mapper, ILoggerService logger)
        {
            _companyService = companyService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET api/company
        public async Task<IHttpActionResult> GetAll()
      {
            try
            {
                var items = await _companyService.GetAllCompaniesAsync();
                var dtos = _mapper.Map<IEnumerable<CompanyDto>>(items);
                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while getting all companies");
                return InternalServerError();
            }
        }

        // GET api/company/{companyCode}
        public async Task<IHttpActionResult> Get(string companyCode)
        {
            try
            {
                var item = await _companyService.GetCompanyByCodeAsync(companyCode);
                if (item == null)
                {
                    return NotFound();
                }
                var dto = _mapper.Map<CompanyDto>(item);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while getting company with code {companyCode}");
                return InternalServerError();
            }
        }

        // POST api/company
        public async Task<IHttpActionResult> Post([FromBody] CompanyDto companyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var company = _mapper.Map<CompanyInfo>(companyDto);
                var result = await _companyService.SaveCompanyAsync(company);
                if (result)
                {
                    return Created($"api/company/{company.CompanyCode}", companyDto);
                }
                return BadRequest("Failed to create company");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while creating a company");
                return InternalServerError();
            }
        }

        // PUT api/company/{companyCode}
        public async Task<IHttpActionResult> Put(string companyCode, [FromBody] CompanyDto companyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var company = _mapper.Map<CompanyInfo>(companyDto);
                company.CompanyCode = companyCode;
                var result = await _companyService.SaveCompanyAsync(company);
                if (result)
                {
                    return Ok(companyDto);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while updating company with code {companyCode}");
                return InternalServerError();
            }
        }

        // DELETE api/company/{companyCode}
        public async Task<IHttpActionResult> Delete(string companyCode)
        {
            try
            {
                var result = await _companyService.DeleteCompanyAsync(companyCode);
                if (result)
                {
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while deleting company with code {companyCode}");
                return InternalServerError();
            }
        }
    }
}