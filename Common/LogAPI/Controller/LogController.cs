using LogAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace LogAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class LogController : ControllerBase
{
    private readonly ConnectionSettings _connectionSettings;
    public LogController(ConnectionSettings connectionSettings)
    {
        _connectionSettings = connectionSettings;
    }

    [HttpGet]
    [Route("[action]/{fromDate:datetime}/{toDate:datetime}")]
    public async Task<IActionResult> getLogs([FromRoute] DateTime? fromDate,DateTime? toDate)
    {
        var elasticClient = new ElasticClient(_connectionSettings.DefaultIndex("requestlogs"));
        var result = await elasticClient.SearchAsync<LogModel>(s => s.Query(q => q.MatchAll()).Scroll("5m").Size(2000));
        var datefilte = await elasticClient.SearchAsync<LogModel>(s => s.Query(q =>
        q.Bool(c =>
        c.Filter(f =>
        f.DateRange(dr =>
        dr.Field(fld => fld.Date)
        .GreaterThan(DateTime.Now.AddDays(-40))
        .LessThan(DateTime.Now)))))
        .Size(600));
        return Ok(datefilte.Documents);
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> getLogMappings()
    {
        var elasticClient = new ElasticClient(_connectionSettings.DefaultIndex("requestlogs"));
        var mappings = await elasticClient.Indices.GetMappingAsync<object>(x => x.Index("requestlogs"));
        var mappingResul = mappings.Indices.Select((x) => new
        {
            x.Key,
            x.Value
        }).ToList().Select((data) => new
        {
            data = data.Value.Mappings.Properties.Select((x) => new
            {
                x.Key.Name,
                x.Value.Type
            })
        }).SelectMany(x => x.data);
        return Ok(mappingResul);
    }
}