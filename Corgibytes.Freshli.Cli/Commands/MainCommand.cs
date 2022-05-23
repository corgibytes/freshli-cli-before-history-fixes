﻿using System.CommandLine;
using System.IO;
using Corgibytes.Freshli.Cli.Functionality;
using Corgibytes.Freshli.Cli.Resources;

namespace Corgibytes.Freshli.Cli.Commands;

public class MainCommand : RootCommand
{
    public MainCommand() : base($"{CliOutput.Help_MainCommand_Description}")
    {
        Option<DirectoryInfo> cacheDirOption = new(
            new[] { "--cache-dir" },
            description: $"{CliOutput.Help_Option_CacheDir_Description}",
            getDefaultValue: () => CacheContext.DefaultCacheDir)
        {
            Arity = ArgumentArity.ExactlyOne
        };

        AddGlobalOption(cacheDirOption);
    }
}
