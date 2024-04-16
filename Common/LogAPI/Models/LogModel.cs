namespace LogAPI.Models;

public class LogModel
{
    public string Method { get; set; } = null!;
    public string Route { get; set; } = null!;
    public string RequestBody { get; set; } = null!;
    public string RequestHeaders { get; set; } = null!;
    public string ResponseBody { get; set; } = null!;
    public long ElapsedTime { get; set; }
    public string ApplicationName { get; set; } = null!;
    public string LogType { get; set; } = null!;
    public string ErrorMessage { get; set; } = null!;
    public DateTime Date { get; set; }
}