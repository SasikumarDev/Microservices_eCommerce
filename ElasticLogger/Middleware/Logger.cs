using System.Reflection;
using System.Text.Json;
using ElasticLogger.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System.Diagnostics;

namespace ElasticLogger.Middleware;

internal class Logger
{
    private RequestDelegate _next = null!;
    public Logger(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        LogModel logData = new LogModel
        {
            Date = DateTime.Now
        };
        var elasticConn = httpContext.RequestServices.GetRequiredService<ConnectionSettings>(); //.DefaultIndex("todos");
        var elastiClient = new ElasticClient(elasticConn);
        var stopWatch = new Stopwatch();
        try
        {
            if (!elastiClient.Indices.Exists(Constants.Constants.LOGDOCUMENTNAME).Exists)
            {
                await CreateIndices(elastiClient);
            }


            logData.Method = httpContext.Request.Method;
            logData.LogType = "Info";
            logData.ApplicationName = Assembly.GetEntryAssembly()?.GetName().Name ?? "";
            logData.Route = httpContext.Request.Path;
            logData.RequestHeaders = JsonSerializer.Serialize(httpContext.Request.Headers.ToDictionary(x => Convert.ToString(x.Key), x => Convert.ToString(x.Value)));



            stopWatch.Start();
            await _next.Invoke(httpContext);
            stopWatch.Stop();

            logData.ElapsedTime = stopWatch.ElapsedMilliseconds;
            await LogData(logData, elastiClient);
        }
        catch (Exception ex)
        {
            logData.LogType = "Error";
            stopWatch.Stop();
            logData.ElapsedTime = stopWatch.ElapsedMilliseconds;
            logData.ErrorMessage = ex.Message;
            string TicketID = await LogData(logData, elastiClient);
            httpContext.Response.StatusCode = 400;
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(new{TicketID,Message="Please raise ticket"}));
        }
        // finally
        // {
        //     await LogData(logData, elastiClient);
        // }
    }

    private async Task<string> LogData(LogModel logModel, ElasticClient client)
    {
        var insertedData = await client.IndexAsync(logModel, s => s.Index(Constants.Constants.LOGDOCUMENTNAME).Id(DateTime.UtcNow.ToFileTime()));
        return insertedData.Id;
    }
    private async Task CreateIndices(ElasticClient client)
    {
        var createResponse = await client.Indices.CreateAsync(Constants.Constants.LOGDOCUMENTNAME, c => c.Map<LogModel>(m =>
                       m.Properties(p => p.Text(s => s.Name(n => n.Method)))
                       .Properties(p => p.Text(s => s.Name(n => n.Route)))
                       .Properties(p => p.Text(s => s.Name(n => n.RequestBody)))
                       .Properties(p => p.Text(s => s.Name(n => n.RequestHeaders)))
                       .Properties(p => p.Text(s => s.Name(n => n.ResponseBody)))
                       .Properties(p => p.Number(s => s.Name(n => n.ElapsedTime)))
                       .Properties(p => p.Text(s => s.Name(n => n.ApplicationName)))
                       .Properties(p => p.Text(s => s.Name(n => n.LogType)))
                       .Properties(p => p.Text(s => s.Name(n => n.ErrorMessage)))
                       .Properties(p=>p.Date(s=>s.Name(n=>n.Date)))
                       ));
        if (!createResponse.IsValid)
        {
            throw new Exception($"Elastic Logger Unable to index for logging, {createResponse.OriginalException}");
        }
    }
}