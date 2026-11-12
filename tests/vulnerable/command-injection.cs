using System.Diagnostics;

public class PingService
{
    public string Ping(string host)
    {
        var process = new Process();
        process.StartInfo.FileName = "/bin/bash";
        process.StartInfo.Arguments = "-c \"ping -c 1 " + host + "\"";
        process.Start();
        return process.StandardOutput.ReadToEnd();
    }
}
