using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;
using BusinessLayer.Logging;

namespace DataAccessLayer.Repositories
{


    public class CompanyRepository : ICompanyRepository
    {
        private readonly IDbWrapper<Company> _companyDbWrapper;
        private readonly ILoggerService _logger;

        public CompanyRepository(IDbWrapper<Company> companyDbWrapper, ILoggerService logger)
        {
            _companyDbWrapper = companyDbWrapper;
            _logger = logger;
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            try
            {
                _logger.Info("Fetching all companies from database");
                return await _companyDbWrapper.FindAllAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while fetching all companies");
                throw;
            }
        }

        public async Task<Company> GetByCodeAsync(string companyCode)
        {
            try
            {
                _logger.Info($"Fetching company with code: {companyCode}");
                var companies = await _companyDbWrapper.FindAsync(t => t.CompanyCode.Equals(companyCode));
                return companies.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while fetching company with code: {companyCode}");
                throw;
            }
        }

        public async Task<bool> SaveCompanyAsync(Company company)
        {
            try
            {
                var companies = await _companyDbWrapper.FindAsync(t =>
                    t.SiteId.Equals(company.SiteId) && t.CompanyCode.Equals(company.CompanyCode));
                var existingCompany = companies.FirstOrDefault();

                if (existingCompany != null)
                {
                    _logger.Info($"Updating existing company: {company.CompanyCode}");
                    // Update existing company fields...
                    return await _companyDbWrapper.UpdateAsync(existingCompany);
                }

                _logger.Info($"Inserting new company: {company.CompanyCode}");
                company.LastModified = DateTime.UtcNow;
                return await _companyDbWrapper.InsertAsync(company);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while saving company: {company.CompanyCode}");
                throw;
            }
        }

        public async Task<bool> DeleteCompanyAsync(string companyCode)
        {
            try
            {
                _logger.Info($"Deleting company with code: {companyCode}");
                return await _companyDbWrapper.DeleteAsync(t => t.CompanyCode.Equals(companyCode));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while deleting company with code: {companyCode}");
                throw;
            }
        }
    }
}
