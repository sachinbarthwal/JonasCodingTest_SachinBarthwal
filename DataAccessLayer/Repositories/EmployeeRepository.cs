using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Logging;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;

namespace DataAccessLayer.Repositories
{
    // DataLayer.Repositories/EmployeeRepository.cs
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDbWrapper<Employee> _employeeDbWrapper;
        private readonly ILoggerService _logger;

        public EmployeeRepository(IDbWrapper<Employee> employeeDbWrapper, ILoggerService logger)
        {
            _employeeDbWrapper = employeeDbWrapper;
            _logger = logger;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            try
            {
                _logger.Info("Fetching all employees from database");
                return await _employeeDbWrapper.FindAllAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error occurred while fetching all employees");
                throw;
            }
        }

        public async Task<Employee> GetByCodeAsync(string employeeCode)
        {
            try
            {
                _logger.Info($"Fetching employee with code: {employeeCode}");
                var employees = await _employeeDbWrapper.FindAsync(e => e.EmployeeCode.Equals(employeeCode));
                return employees.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while fetching employee with code: {employeeCode}");
                throw;
            }
        }

        public async Task<bool> SaveEmployeeAsync(Employee employee)
        {
            try
            {
                var employees = await _employeeDbWrapper.FindAsync(e =>
                    e.SiteId.Equals(employee.SiteId) && e.EmployeeCode.Equals(employee.EmployeeCode));
                var existingEmployee = employees.FirstOrDefault();

                if (existingEmployee != null)
                {
                    _logger.Info($"Updating existing employee: {employee.EmployeeCode}");
                    // Update existing employee fields...
                    existingEmployee.EmployeeName = employee.EmployeeName;
                    existingEmployee.Occupation = employee.Occupation;
                    existingEmployee.EmployeeStatus = employee.EmployeeStatus;
                    existingEmployee.EmailAddress = employee.EmailAddress;
                    existingEmployee.Phone = employee.Phone;
                    existingEmployee.LastModified = DateTime.UtcNow;
                    return await _employeeDbWrapper.UpdateAsync(existingEmployee);
                }

                _logger.Info($"Inserting new employee: {employee.EmployeeCode}");
                employee.LastModified = DateTime.UtcNow;
                return await _employeeDbWrapper.InsertAsync(employee);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while saving employee: {employee.EmployeeCode}");
                throw;
            }
        }

        public async Task<bool> DeleteEmployeeAsync(string employeeCode)
        {
            try
            {
                _logger.Info($"Deleting employee with code: {employeeCode}");
                return await _employeeDbWrapper.DeleteAsync(e => e.EmployeeCode.Equals(employeeCode));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error occurred while deleting employee with code: {employeeCode}");
                throw;
            }
        }
    }
}
