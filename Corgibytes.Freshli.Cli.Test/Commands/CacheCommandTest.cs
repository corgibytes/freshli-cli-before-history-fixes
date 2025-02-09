using System.CommandLine;
using Corgibytes.Freshli.Cli.Commands;
using Corgibytes.Freshli.Cli.Commands.Cache;
using Corgibytes.Freshli.Cli.Test.Common;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Corgibytes.Freshli.Cli.Test.Commands;

[UnitTest]
public class CacheCommandTest : FreshliTest
{
    public CacheCommandTest(ITestOutputHelper output) : base(output)
    {
    }

    [Fact]
    public void Verify_no_cache_handler_configuration()
    {
        var cacheCommand = new CacheCommand();
        cacheCommand.Handler.Should().BeNull();
    }

    [Fact]
    public void Verify_prepare_handler_configuration()
    {
        var cachePrepareCommand = new CachePrepareCommand();
        cachePrepareCommand.Handler.Should().NotBeNull();
    }

    [Fact]
    public void Verify_destroy_handler_configuration()
    {
        var cacheDestroyCommand = new CacheDestroyCommand();
        cacheDestroyCommand.Handler.Should().NotBeNull();
    }

    [Theory]
    [InlineData("--force")]
    public void Verify_force_option_configuration(string alias) =>
        TestHelpers.VerifyAlias<CacheDestroyCommand>(alias, ArgumentArity.ZeroOrOne, false);
}
