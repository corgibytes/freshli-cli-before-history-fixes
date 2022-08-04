using Corgibytes.Freshli.Cli.Functionality.Analysis;

namespace Corgibytes.Freshli.Cli.Test.Functionality.Analysis;

[UnitTest]
public class StartAnalysisActivityTest : StartAnalysisActivityTestBase<StartAnalysisActivity, CacheWasNotPreparedEvent>
{
    protected override StartAnalysisActivity Activity => new(_cacheManager.Object, _intervalParser.Object)
    {
        CacheDirectory = "example",
        RepositoryUrl = "http://git.example.com",
        RepositoryBranch = "main",
        HistoryInterval = "1m"
    };
}
