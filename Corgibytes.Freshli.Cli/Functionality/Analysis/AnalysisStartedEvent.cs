using System;
using Corgibytes.Freshli.Cli.Functionality.Engine;
using Corgibytes.Freshli.Cli.Functionality.Git;

namespace Corgibytes.Freshli.Cli.Functionality.Analysis;

public class AnalysisStartedEvent : IApplicationEvent
{
    private readonly IServiceProvider _serviceProvider = null!;

    public Guid AnalysisId { get; init; }

    public string CacheDir { get; init; } = null!;

    public string GitPath { get; init; } = null!;

    public void Handle(IApplicationActivityEngine eventClient) =>
        eventClient.Dispatch(new CloneGitRepositoryActivity(_serviceProvider, AnalysisId, CacheDir, GitPath));
}
