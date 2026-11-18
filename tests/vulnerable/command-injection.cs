using System.Diagnostics;

public class PingService
{
    public string Ping(string host)
    {
        var process = new Process();
        process.StartInfo.FileName = "/bin/bash";
        // ruleid: dotnet-cmdi-process-start
        process.StartInfo.Arguments = "-c \"ping -c 1 " + host + "\"";
        process.Start();
        return process.StandardOutput.ReadToEnd();
    }

    public void RunTool(string toolPath)
    {
        var process = new Process();
        // ruleid: dotnet-cmdi-process-start
        process.StartInfo.FileName = toolPath;
        process.Start();
    }

    public void Convert(string file)
    {
        // ruleid: dotnet-cmdi-process-start
        Process.Start("convert", $"{file} out.png");
    }
}
