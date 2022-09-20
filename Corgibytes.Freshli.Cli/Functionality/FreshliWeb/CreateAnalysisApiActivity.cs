using System;
using Corgibytes.Freshli.Cli.Functionality.Engine;
using Microsoft.Extensions.DependencyInjection;

namespace Corgibytes.Freshli.Cli.Functionality.FreshliWeb;

public class CreateAnalysisApiActivity : IApplicationActivity
{
    public Guid CachedAnalysisId { get; }
    public string Url { get; }
    public string Branch { get; }
    public string CacheDir { get; }
    public string GitPath { get; }

    public CreateAnalysisApiActivity(Guid cachedAnalysisId, string url, string branch, string cacheDir, string gitPath)
    {
        CachedAnalysisId = cachedAnalysisId;
        Url = url;
        Branch = branch;
        CacheDir = cacheDir;
        GitPath = gitPath;
    }

    public void Handle(IApplicationEventEngine eventClient)
    {
        var apiService = eventClient.ServiceProvider.GetRequiredService<IResultsApi>();
        var apiAnalysisId = apiService.CreateAnalysis(Url);
        eventClient.Fire(new AnalysisApiCreatedEvent()
        {
            CachedAnalysisId = CachedAnalysisId,
            Url = Url,
            Branch = Branch,
            CacheDir = CacheDir,
            GitPath = GitPath,
            ApiAnalysisId = apiAnalysisId
        });
    }
}
