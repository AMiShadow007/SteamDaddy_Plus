using System.Diagnostics;
using System.Reflection;

var asm = Assembly.GetExecutingAssembly();
var resourceName = "SteamDaddy.install_b.ps1";
var scriptName = "SteamDaddy_Install.ps1";

var resource = asm.GetManifestResourceNames()
    .FirstOrDefault(x => x.EndsWith("install_b.ps1", StringComparison.OrdinalIgnoreCase));

if (resource is null)
{
    Console.Error.WriteLine("[SteamDaddy] Embedded installer script not found.");
    Environment.Exit(1);
}

var tempDir = Path.Combine(Path.GetTempPath(), "SteamDaddy");
Directory.CreateDirectory(tempDir);

var scriptPath = Path.Combine(tempDir, scriptName);

using (var stream = asm.GetManifestResourceStream(resource))
{
    if (stream is null)
    {
        Console.Error.WriteLine("[SteamDaddy] Failed to read embedded installer script.");
        Environment.Exit(1);
    }

    using var reader = new StreamReader(stream);
    var content = reader.ReadToEnd();
    File.WriteAllText(scriptPath, content);
}

var startInfo = new ProcessStartInfo
{
    FileName = "powershell.exe",
    Arguments = $"-NoProfile -ExecutionPolicy Bypass -File \"{scriptPath}\"",
    WorkingDirectory = tempDir,
    UseShellExecute = false,
    CreateNoWindow = false
};

try
{
    using var process = Process.Start(startInfo);
    process?.WaitForExit();
    Environment.Exit(process?.ExitCode ?? 0);
}
catch (Exception ex)
{
    Console.Error.WriteLine($"[SteamDaddy] Failed to launch installer: {ex.Message}");
    Environment.Exit(1);
}
