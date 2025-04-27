using System.IO;
using Serilog;

namespace iAmModlist_Launcher.Launcher.Logging;

public static class LauncherLogger
{
    public static void CreateLogger()
    {
        if (!Directory.Exists("logs"))
        {
            Directory.CreateDirectory("logs");
        }

        LogRotator.RotateLogs();
        
        var logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("logs\\launcher.current.log",
                outputTemplate: $"[{{Level}}] [{{Timestamp}}]: {{Message}}{{NewLine}}")
            .CreateLogger();

        Log.Logger = logger;
        Log.Information("### iAmModlist Launcher ###");
    }
}

public static class LogRotator
{
    private const string LogDirectory = "logs";
    private const string CurrLogName = "launcher.current.log";
    private const string RotatingLogPattern = "launcher.{0:D2}.log";
    private const int MaxLogFiles = 4;

    public static void RotateLogs()
    {
        // Get list of log files
        var logFiles = Directory.GetFiles(LogDirectory, "launcher.*.log")
            .Where(f => Path.GetFileName(f) != CurrLogName)
            .Select(f => new FileInfo(f))
            .OrderBy(f => f.Name)
            .Reverse()
            .ToList();

        // Delete oldest log file
        if (logFiles.Count >= MaxLogFiles)
        {
            var filesToDelete = logFiles.Take(logFiles.Count - (MaxLogFiles - 1));

            foreach (var file in filesToDelete)
            {
                Log.Information("Deleting old log {FileName}", file.Name);
                file.Delete();
            }

            // Refresh list of log files after deletion
            logFiles = [.. Directory.GetFiles(LogDirectory, "launcher.*.log")
                .Where(f => Path.GetFileName(f) != CurrLogName)
                .Select(f => new FileInfo(f))
                .OrderBy(f => f.Name)
                .Reverse()];
        }

        // Shift existing logs up
        for (var i = logFiles.Count; i >= 1; i--)
        {
            var src = Path.Combine(LogDirectory, string.Format(RotatingLogPattern, i));
            var dst = Path.Combine(LogDirectory, string.Format(RotatingLogPattern, i + 1));

            if (File.Exists(src))
            {
                Log.Information("Renaming {Src} -> {Dst}", src, dst);
                File.Move(src, dst);
            }
        }

        var currentLogPath = Path.Combine(LogDirectory, CurrLogName);
        if (File.Exists(currentLogPath))
        {
            var newLogPath = Path.Combine(LogDirectory, string.Format(RotatingLogPattern, 1));
            File.Move(currentLogPath, newLogPath);
        }
    }
}