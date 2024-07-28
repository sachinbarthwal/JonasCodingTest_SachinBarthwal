using BusinessLayer.Model.Interfaces;
using System.Collections.Generic;
using AutoMapper;
using BusinessLayer.Model.Models;
using DataAccessLayer.Model.Interfaces;
using System.Threading.Tasks;
using DataAccessLayer.Model.Models;
using BusinessLayer.Logging;
using System;

namespace BusinessLayer.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerService _logger;

        public CompanyService(ICompanyRepository companyRepository, IMapper mapper, ILoggerService logger)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<CompanyInfo>> GetAllCompaniesAsync()
        {
            try
            {
                _logger.Info("Getting all companies");
                var companies = await _companyRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<CompanyInfo>>(companies);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while getting all companies");
                throw;
            }
        }

        public async Task<CompanyInfo> GetCompanyByCodeAsync(string companyCode)
        {
            try
            {
                _logger.Info($"Getting company by code: {companyCode}");
                var company = await _companyRepository.GetByCodeAsync(companyCode);
                return _mapper.Map<CompanyInfo>(company);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while getting company by code: {companyCode}");
                throw;
            }
        }

        public async Task<bool> SaveCompanyAsync(CompanyInfo companyInfo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(companyInfo.CompanyCode))
                {
                    _logger.Error("Company code cannot be empty");
                    throw new ArgumentException("Company code cannot be empty");
                }

                _logger.Info($"Saving company: {companyInfo.CompanyCode}");
                var company = _mapper.Map<Company>(companyInfo);
                return await _companyRepository.SaveCompanyAsync(company);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while saving company: {companyInfo.CompanyCode}");
                throw;
            }
        }

        public async Task<bool> DeleteCompanyAsync(string companyCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(companyCode))
                {
                    _logger.Error("Company code cannot be empty for deletion");
                    throw new ArgumentException("Company code cannot be empty for deletion");
                }

                _logger.Info($"Deleting company: {companyCode}");
                return await _companyRepository.DeleteCompanyAsync(companyCode);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while deleting company: {companyCode}");
                throw;
            }
        }
    }
}
