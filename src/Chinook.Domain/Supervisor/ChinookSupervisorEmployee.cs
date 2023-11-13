using Chinook.Domain.ApiModels;
using Chinook.Domain.Entities;
using Chinook.Domain.Extensions;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;

namespace Chinook.Domain.Supervisor;

public partial class ChinookSupervisor
{
    public List<EmployeeApiModel> GetAllEmployee()
    {
        List<Employee> employees = _employeeRepository.GetAll();
        var employeeApiModels = employees.ConvertAll();

        foreach (var employee in employeeApiModels)
        {
            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);
            ;
            _cache.Set(string.Concat("Employee-", employee.Id), employee, (TimeSpan)cacheEntryOptions);
        }

        return employeeApiModels;
    }

    public EmployeeApiModel? GetEmployeeById(int id)
    {
        var employeeApiModelCached = _cache.Get<EmployeeApiModel>(string.Concat("Employee-", id));

        if (employeeApiModelCached != null)
        {
            return employeeApiModelCached;
        }
        else
        {
            var employee = _employeeRepository.GetById(id);
            var employeeApiModel = employee.Convert();

            var cacheEntryOptions =
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800))
                    .AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(604800);
            ;
            _cache.Set(string.Concat("Employee-", employeeApiModel.Id), employeeApiModel,
                (TimeSpan)cacheEntryOptions);

            return employeeApiModel;
        }
    }

    public EmployeeApiModel? GetEmployeeReportsTo(int id)
    {
        var employee = _employeeRepository.GetReportsTo(id);
        return employee.Convert();
    }

    public EmployeeApiModel AddEmployee(EmployeeApiModel newEmployeeApiModel)
    {
        _employeeValidator.ValidateAndThrowAsync(newEmployeeApiModel);

        var employee = newEmployeeApiModel.Convert();

        employee = _employeeRepository.Add(employee);
        newEmployeeApiModel.Id = employee.Id;
        return newEmployeeApiModel;
    }

    public bool UpdateEmployee(EmployeeApiModel employeeApiModel)
    {
        _employeeValidator.ValidateAndThrowAsync(employeeApiModel);

        var employee = _employeeRepository.GetById(employeeApiModel.Id);
        
        employee.Id = employeeApiModel.Id;
        employee.LastName = employeeApiModel.LastName;
        employee.FirstName = employeeApiModel.FirstName;
        employee.Title = employeeApiModel.Title ?? string.Empty;
        employee.ReportsTo = employeeApiModel.ReportsTo;
        employee.BirthDate = employeeApiModel.BirthDate;
        employee.HireDate = employeeApiModel.HireDate;
        employee.Address = employeeApiModel.Address ?? string.Empty;
        employee.City = employeeApiModel.City ?? string.Empty;
        employee.State = employeeApiModel.State ?? string.Empty;
        employee.Country = employeeApiModel.Country ?? string.Empty;
        employee.PostalCode = employeeApiModel.PostalCode ?? string.Empty;
        employee.Phone = employeeApiModel.Phone ?? string.Empty;
        employee.Fax = employeeApiModel.Fax ?? string.Empty;
        employee.Email = employeeApiModel.Email ?? string.Empty;

        return _employeeRepository.Update(employee);
    }

    public bool DeleteEmployee(int id)
        => _employeeRepository.Delete(id);

    public List<EmployeeApiModel> GetEmployeeDirectReports(int id)
    {
        var employees = _employeeRepository.GetDirectReports(id);
        return employees.ConvertAll();
    }

    public List<EmployeeApiModel> GetDirectReports(int id)
    {
        var employees = _employeeRepository.GetDirectReports(id);
        return employees.ConvertAll();
    }
}