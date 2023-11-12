﻿using System.Net;
using Chinook.Domain.ApiModels;
using Chinook.Domain.Supervisor;
using FluentValidation;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Chinook.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableCors("CorsPolicy")]
public class CustomerController : ControllerBase
{
    private readonly IChinookSupervisor _chinookSupervisor;
    private readonly ILogger<CustomerController> _logger;

    public CustomerController(IChinookSupervisor chinookSupervisor, ILogger<CustomerController> logger)
    {
        _chinookSupervisor = chinookSupervisor;
        _logger = logger;
    }

    [HttpGet]
    [Produces("application/json")]
    public ActionResult<List<CustomerApiModel>> Get()
    {
        try
        {
            var customers = _chinookSupervisor.GetAllCustomer();

            if (customers.Any())
            {
                return Ok(customers);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.NotFound, "No Customers Could Be Found");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong inside the CustomerController Get action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "Error occurred while executing Get All Customers");
        }
    }

    [HttpGet("{id}", Name = "GetCustomerById")]
    [Produces("application/json")]
    public ActionResult<CustomerApiModel> Get(int id)
    {
        try
        {
            var customer = _chinookSupervisor.GetCustomerById(id);

            if (customer != null)
            {
                return Ok(customer);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.NotFound, "Customer Not Found");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong inside the CustomerController GetById action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "Error occurred while executing Get Customer By Id");
        }
    }

    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    public ActionResult<CustomerApiModel> Post([FromBody] CustomerApiModel input)
    {
        try
        {
            if (input == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, "Given Customer is null");
            }
            else
            {
                return Ok(_chinookSupervisor.AddCustomer(input));
            }
        }
        catch (ValidationException ex)
        {
            _logger.LogError($"Something went wrong inside the CustomerController Add Customer action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "Error occurred while executing Add Customers");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong inside the CustomerController Add Customer action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "Error occurred while executing Add Customers");
        }
    }

    [HttpPut("{id}")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public ActionResult<CustomerApiModel> Put(int id, [FromBody] CustomerApiModel input)
    {
        try
        {
            if (input == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, "Given Customer is null");
            }
            else
            {
                return Ok(_chinookSupervisor.UpdateCustomer(input));
            }
        }
        catch (ValidationException ex)
        {
            _logger.LogError($"Something went wrong inside the CustomerController Update Customer action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "Error occurred while executing Update Customers");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong inside the CustomerController Add Customer action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "Error occurred while executing Update Customers");
        }
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        try
        {
            return Ok(_chinookSupervisor.DeleteCustomer(id));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong inside the CustomerController Delete action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "Error occurred while executing Delete Customer");
        }
    }

    [HttpGet("supportrep/{id}")]
    [Produces("application/json")]
    public ActionResult<EmployeeApiModel> GetBySupportRepId(int id)
    {
        try
        {
            var employee = _chinookSupervisor.GetEmployeeById(id);

            if (employee != null)
            {
                return Ok(employee);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.NotFound, "No Support Rep Could Be Found");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong inside the CustomerController GetBySupportRepId action: {ex}");
            return StatusCode((int)HttpStatusCode.InternalServerError,
                "Error occurred while executing GetBySupportRepId");
        }
    }
}