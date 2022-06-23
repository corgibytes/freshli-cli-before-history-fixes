using System;
using Corgibytes.Freshli.Cli.DependencyManagers;

namespace Corgibytes.Freshli.Cli.Test.DependencyManagers;

public class MockNuGetDependencyManagerRepository : IDependencyManagerRepository
{
    public DateTimeOffset GetReleaseDate(string name, string version) => (name, version) switch
    {
        ("Newtonsoft.Json", "3.22.2021") => new(2021, 3, 22, 0, 0, 0, TimeSpan.Zero),
        ("Newtonsoft.Json", "8.3.2014") => new(2014, 8, 3, 0, 0, 0, TimeSpan.Zero),
        ("DifferentTimezone", "3.22.2021") => new(2021, 3, 22, 0, 0, 0, TimeSpan.FromHours(-10)),
        ("DifferentTimezone", "8.3.2014") => new(2014, 8, 3, 0, 0, 0, TimeSpan.FromHours(-10)),
        ("calculatron", "21.3") => new(2022, 6, 16, 0, 0, 0, TimeSpan.Zero),
        ("calculatron", "14.6") => new(2019, 12, 31, 0, 0, 0, TimeSpan.Zero),
        ("flyswatter", "1.1.0") => new(1990, 1, 29, 0, 0, 0, TimeSpan.Zero),
        ("auto-cup-of-tea", "112.0") => new(2004, 11, 11, 0, 0, 0, TimeSpan.Zero),
        ("auto-cup-of-tea", "256.0") => new(2011, 10, 26, 0, 0, 0, TimeSpan.Zero),
        _ => throw new ArgumentException("Mock date could not be returned. Forgot to add it?")
    };

    public string GetLatestVersion(string name) => name switch
    {
        "calculatron" => "21.3",
        "flyswatter" => "1.1.0",
        "auto-cup-of-tea" => "256.0",
        _ => throw new ArgumentException("Mock date could not be returned. Forgot to add it?")
    };

    public SupportedDependencyManagers Supports() => SupportedDependencyManagers.NuGet();
}

