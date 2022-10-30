using System;
using System.CommandLine;
using Corgibytes.Freshli.Cli.CommandOptions;
using Corgibytes.Freshli.Cli.Commands;
using Corgibytes.Freshli.Cli.Functionality;
using Corgibytes.Freshli.Cli.Functionality.Engine;
using Corgibytes.Freshli.Lib;

namespace Corgibytes.Freshli.Cli.CommandRunners;

public class FailCommandRunner : CommandRunner<FailCommand, EmptyCommandOptions>
{
    public FailCommandRunner(IServiceProvider serviceProvider, IRunner runner,
        IApplicationActivityEngine activityEngine)
        : base(serviceProvider, runner) =>
        ActivityEngine = activityEngine;

    private IApplicationActivityEngine ActivityEngine { get; }

    public override int Run(EmptyCommandOptions options, IConsole console)
    {
        ActivityEngine.Dispatch(new ThrowExceptionActivity());
        ActivityEngine.Wait();

        return 0;
    }
}
