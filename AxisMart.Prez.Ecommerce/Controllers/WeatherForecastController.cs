using AxisMart.Application.Ecommerce.User.Customer.Register;
using AxisMart.Application.Shared.Messaging.Command;
using AxisMart.Core.Ecommerce.User.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace AxisMart.Prez.Ecommerce.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}


[ApiController]
[Route("[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public sealed record RegisterCustomer(string FirstName, string LastName, string Email, string Phone, string Password);

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCustomer register, CancellationToken cancellationToken)
    {
        var command = new RegisterCustomerCommand(new FirstName(register.FirstName), new LastName(register.LastName), new Email(register.Email), new Phone(register.Phone), register.Password);
        var result = await _mediator.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
