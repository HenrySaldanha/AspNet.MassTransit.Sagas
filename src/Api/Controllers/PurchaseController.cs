using Microsoft.AspNetCore.Mvc;
using MassTransit;
using Domain;
using Serilog;
using Api.Models;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PurchaseController : ControllerBase
{
    private readonly IBus _bus;
    private readonly Serilog.ILogger _logger;

    public PurchaseController(IBus bus)
    {
        _bus = bus;
        _logger = Log.ForContext<PurchaseController>();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateOrderAsync(OrderRequest orderRequest, CancellationToken cancellationToken)
    {
        _logger.Information("Request received; Method {method}; Request: {@request}",
                nameof(CreateOrderAsync), @orderRequest);

        var command = new CreateOrderCommand(orderRequest);
        await _bus.Publish(command, cancellationToken);

        _logger.Information("Published event: {event}; Request: {@request}",
                nameof(CreateOrderCommand), @command);

        return Ok();
    }
}