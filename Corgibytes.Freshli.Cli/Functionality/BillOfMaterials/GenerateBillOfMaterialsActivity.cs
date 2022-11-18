using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Corgibytes.Freshli.Cli.Functionality.Engine;
using Corgibytes.Freshli.Cli.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Corgibytes.Freshli.Cli.Functionality.BillOfMaterials;

public class GenerateBillOfMaterialsActivity : IApplicationActivity, IMutexed
{
    private static readonly ConcurrentDictionary<string, Mutex> s_historyPointMutexes = new();
    public readonly string AgentExecutablePath;
    public readonly Guid AnalysisId;
    public readonly int HistoryStopPointId;
    public readonly string ManifestPath;

    public GenerateBillOfMaterialsActivity(Guid analysisId, string agentExecutablePath,
        int historyStopPointId, string manifestPath)
    {
        AnalysisId = analysisId;
        AgentExecutablePath = agentExecutablePath;
        HistoryStopPointId = historyStopPointId;
        ManifestPath = manifestPath;
    }

    public async ValueTask Handle(IApplicationEventEngine eventClient)
    {
        var agentManager = eventClient.ServiceProvider.GetRequiredService<IAgentManager>();
        var agentReader = agentManager.GetReader(AgentExecutablePath);

        var cacheManager = eventClient.ServiceProvider.GetRequiredService<ICacheManager>();
        var cacheDb = cacheManager.GetCacheDb();

        var historyStopPoint = await cacheDb.RetrieveHistoryStopPoint(HistoryStopPointId);
        _ = historyStopPoint ?? throw new Exception($"Failed to retrieve history stop point {HistoryStopPointId}");

        var historyPointPath = historyStopPoint.LocalPath;
        var asOfDateTime = historyStopPoint.AsOfDateTime;

        var fullManifestPath = Path.Combine(historyPointPath, ManifestPath);
        var bomFilePath = await agentReader.ProcessManifest(fullManifestPath, asOfDateTime);
        var cachedBomFilePath = await cacheManager.StoreBomInCache(bomFilePath, AnalysisId, asOfDateTime);

        await eventClient.Fire(new BillOfMaterialsGeneratedEvent(
            AnalysisId, HistoryStopPointId, cachedBomFilePath, AgentExecutablePath));
    }

    public async ValueTask<Mutex> GetMutex(IServiceProvider provider)
    {
        var cacheManager = provider.GetRequiredService<ICacheManager>();
        var cacheDb = cacheManager.GetCacheDb();

        var historyStopPoint = await cacheDb.RetrieveHistoryStopPoint(HistoryStopPointId);
        // TODO create an exception class for this exception and write a test to cover it getting generated
        _ = historyStopPoint ?? throw new Exception($"Failed to retrieve history stop point {HistoryStopPointId}");

        var historyPointPath = historyStopPoint.LocalPath;
        EnsureHistoryPointMutexExists(historyPointPath);
        return s_historyPointMutexes[historyPointPath];
    }

    private static void EnsureHistoryPointMutexExists(string path)
    {
        if (!s_historyPointMutexes.ContainsKey(path))
        {
            s_historyPointMutexes[path] = new Mutex();
        }
    }
}
