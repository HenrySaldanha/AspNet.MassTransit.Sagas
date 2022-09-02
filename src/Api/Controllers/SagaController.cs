using Microsoft.AspNetCore.Mvc;
using Serilog;
using MassTransit.Visualizer;
using PurchaseSaga;
using MassTransit.SagaStateMachine;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SagaController : ControllerBase
{
    private readonly Serilog.ILogger _logger;

    public SagaController() =>
        _logger = Log.ForContext<PurchaseController>();

    [HttpGet("url-viewer")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IActionResult GenerateUrlViewer()
    {
        _logger.Information("Request received; Method {method}", nameof(GenerateUrlViewer));

        var graph = new OrderStateMachine().GetGraph();
        var dotFile = new StateMachineGraphvizGenerator(graph).CreateDotFile();
        var quickChart = $"https://quickchart.io/graphviz?graph={dotFile.Replace("\n","").Replace("\r", "")}";
        return Ok(quickChart);
    }

    [HttpGet("dot-file")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IActionResult GenerateGraph()
    {
        _logger.Information("Request received; Method {method}", nameof(GenerateGraph));

        var graph = new OrderStateMachine().GetGraph();
        var dotFile = new StateMachineGraphvizGenerator(graph).CreateDotFile();
        return Ok(dotFile);
    }
}