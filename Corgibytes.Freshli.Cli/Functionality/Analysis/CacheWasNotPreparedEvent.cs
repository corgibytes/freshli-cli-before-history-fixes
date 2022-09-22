using Corgibytes.Freshli.Cli.CommandRunners.Cache;
using Corgibytes.Freshli.Cli.Functionality.Engine;

namespace Corgibytes.Freshli.Cli.Functionality.Analysis;

public class CacheWasNotPreparedEvent : ErrorEvent
{
    public string GitPath { get; init; } = null!;
    public string CacheDirectory { get; init; } = null!;
    public string RepositoryUrl { get; init; } = null!;
    public string? RepositoryBranch { get; init; }
    public string HistoryInterval { get; init; } = null!;
    public CommitHistory UseCommitHistory { get; init; }
    public LatestOnly LatestOnly { get; init; }

    public override void Handle(IApplicationActivityEngine eventClient) =>
        eventClient.Dispatch(
            new PrepareCacheActivity(CacheDirectory, RepositoryUrl, RepositoryBranch, HistoryInterval, UseCommitHistory)
            {
                GitPath = GitPath,
                CacheDirectory = CacheDirectory,
                RepositoryUrl = RepositoryUrl,
                RepositoryBranch = RepositoryBranch,
                HistoryInterval = HistoryInterval,
                UseCommitHistory = UseCommitHistory,
                LatestOnly = LatestOnly
            });
}
