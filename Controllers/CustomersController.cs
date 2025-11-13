using FlowTap.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CustomerDto = FlowTap.Api.Services.CustomerDto;
using CreateCustomerRequest = FlowTap.Api.Services.CreateCustomerRequest;
using UpdateCustomerRequest = FlowTap.Api.Services.UpdateCustomerRequest;
using CustomerHistoryDto = FlowTap.Api.Services.CustomerHistoryDto;

namespace FlowTap.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CustomerDto>>> GetCustomers([FromQuery] string? search)
    {
        var customers = await _customerService.GetCustomersAsync(search);
        return Ok(customers);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> CreateCustomer([FromBody] CreateCustomerRequest request)
    {
        try
        {
            var customer = await _customerService.CreateCustomerAsync(request);
            return Ok(new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                Email = customer.Email,
                StoreId = customer.StoreId,
                LoyaltyPoints = customer.LoyaltyPoints,
                TotalSpent = customer.TotalSpent,
                CreatedAt = customer.CreatedAt
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CustomerDto>> UpdateCustomer(Guid id, [FromBody] UpdateCustomerRequest request)
    {
        try
        {
            var customer = await _customerService.UpdateCustomerAsync(id, request);
            return Ok(new CustomerDto
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                Email = customer.Email,
                StoreId = customer.StoreId,
                LoyaltyPoints = customer.LoyaltyPoints,
                TotalSpent = customer.TotalSpent,
                CreatedAt = customer.CreatedAt
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}/history")]
    public async Task<ActionResult<List<CustomerHistoryDto>>> GetCustomerHistory(Guid id)
    {
        var history = await _customerService.GetCustomerHistoryAsync(id);
        return Ok(history);
    }
}

