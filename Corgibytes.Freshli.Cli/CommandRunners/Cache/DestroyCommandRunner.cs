using System;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using Corgibytes.Freshli.Cli.CommandOptions;
using Corgibytes.Freshli.Cli.Extensions;
using Corgibytes.Freshli.Cli.Functionality;
using Corgibytes.Freshli.Cli.Resources;
using Corgibytes.Freshli.Lib;

namespace Corgibytes.Freshli.Cli.CommandRunners.Cache;

public class CacheDestroyCommandRunner : CommandRunner<CacheDestroyCommandOptions>
{
    public CacheDestroyCommandRunner(IServiceProvider serviceProvider, Runner runner)
        : base(serviceProvider, runner)
    {
    }

    public override int Run(CacheDestroyCommandOptions options, InvocationContext context)
    {
        // Unless the --force flag is passed, prompt the user whether they want to destroy the cache
        if (!options.Force && !Confirm(
                string.Format(CliOutput.CacheDestroyCommandRunner_Run_Prompt, options.CacheDir.FullName),
                context
            ))
        {
            context.Console.Out.WriteLine(CliOutput.CacheDestroyCommandRunner_Run_Abort);
            return true.ToExitCode();
        }

        // Destroy the cache
        context.Console.Out.WriteLine(string.Format(CliOutput.CacheDestroyCommandRunner_Run_Destroying, options.CacheDir));
        try
        {
            return Functionality.Cache.Destroy(options.CacheDir).ToExitCode();
        }
        // Catch errors
        catch (CacheException error)
        {
            context.Console.Error.WriteLine(error.Message);
            return error.IsWarning.ToExitCode();
        }
    }
}
