using System;
using System.Collections.Generic;
using Corgibytes.Freshli.Cli.DataModel;
using Corgibytes.Freshli.Cli.Functionality.Analysis;
using Corgibytes.Freshli.Cli.Functionality.Engine;
using Corgibytes.Freshli.Cli.Functionality.Git;
using Microsoft.Extensions.DependencyInjection;

namespace Corgibytes.Freshli.Cli.Functionality.History;

public class ComputeHistoryActivity : IApplicationActivity
{
    public readonly IHistoryStopData HistoryStopData;

    public Guid AnalysisId;

    public ComputeHistoryActivity(Guid analysisId, IHistoryStopData historyStopData)
    {
        AnalysisId = analysisId;
        HistoryStopData = historyStopData;
    }

    public void Handle(IApplicationEventEngine eventClient)
    {
        var computeHistoryService = eventClient.ServiceProvider.GetRequiredService<IComputeHistory>();
        var cacheManager = eventClient.ServiceProvider.GetRequiredService<ICacheManager>();
        var cacheDb = cacheManager.GetCacheDb();
        var cachedAnalysis = cacheDb.RetrieveAnalysis(AnalysisId);

        if (cachedAnalysis == null)
        {
            eventClient.Fire(new AnalysisIdNotFoundEvent());
            return;
        }

        IEnumerable<HistoryIntervalStop> historyIntervalStops;

        if (cachedAnalysis.RevisionHistoryMode.Equals(RevisionHistoryMode.OnlyLatestRevision))
        {
            historyIntervalStops = computeHistoryService.ComputeLatestOnly(HistoryStopData);
        }
        else if (cachedAnalysis.UseCommitHistory.Equals(CommitHistory.AtInterval))
        {
            try
            {
                historyIntervalStops = computeHistoryService
                    .ComputeWithHistoryInterval(HistoryStopData, cachedAnalysis.HistoryInterval, DateTimeOffset.Now);
            }
            catch (InvalidHistoryIntervalException exception)
            {
                eventClient.Fire(new InvalidHistoryIntervalEvent { ErrorMessage = exception.Message });
                return;
            }
        }
        else
        {
            historyIntervalStops = computeHistoryService.ComputeCommitHistory(HistoryStopData);
        }

        foreach (var historyIntervalStop in historyIntervalStops)
        {
            var historyStopPointId = cacheDb.AddHistoryStopPoint(
                new CachedHistoryStopPoint {
                    CachedAnalysisId = AnalysisId,
                    RepositoryId = HistoryStopData.RepositoryId,
                    LocalPath = HistoryStopData.Path,
                    GitCommitId = historyIntervalStop.GitCommitIdentifier,
                    AsOfDateTime = historyIntervalStop.AsOfDateTime
                });

            eventClient.Fire(new HistoryIntervalStopFoundEvent(AnalysisId, historyStopPointId));
        }
    }
}
