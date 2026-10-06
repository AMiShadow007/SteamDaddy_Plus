using System.Diagnostics;

var scriptPath = Path.Combine(AppContext.BaseDirectory, "install_b.ps1");
if (!File.Exists(scriptPath))
{
    Console.Error.WriteLine($"[SteamDaddy] Error: Missing script at {scriptPath}");
    Environment.Exit(1);
}

var startInfo = new ProcessStartInfo
{
    FileName = "powershell.exe",
    Arguments = $"-NoProfile -ExecutionPolicy Bypass -File \"{scriptPath}\"",
    WorkingDirectory = AppContext.BaseDirectory,
    UseShellExecute = true
};

try
{
    using var process = Process.Start(startInfo);
    process?.WaitForExit();
    Environment.Exit(process?.ExitCode ?? 0);
}
catch (Exception ex)
{
    Console.Error.WriteLine($"[SteamDaddy] Error: Failed to launch installer: {ex.Message}");
    Environment.Exit(1);
}
