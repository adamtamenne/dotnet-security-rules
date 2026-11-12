using System.Diagnostics;
using System.Text.RegularExpressions;

public class PingService
{
    private static readonly Regex HostPattern = new(@"^[a-zA-Z0-9.\-]+$");

    public string Ping(string host)
    {
        if (!HostPattern.IsMatch(host))
            throw new ArgumentException("Invalid hostname");
        var process = new Process();
        process.StartInfo.FileName = "/usr/bin/ping";
        process.StartInfo.ArgumentList.Add("-c");
        process.StartInfo.ArgumentList.Add("1");
        process.StartInfo.ArgumentList.Add(host);
        process.Start();
        return process.StandardOutput.ReadToEnd();
    }
}
