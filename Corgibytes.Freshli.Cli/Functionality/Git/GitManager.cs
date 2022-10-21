using System;
using Newtonsoft.Json;

namespace Corgibytes.Freshli.Cli.Functionality.Git;

public class GitManager : IGitManager
{
    [JsonProperty] private readonly IConfiguration _configuration;
    [JsonProperty] private readonly GitArchive _gitArchive;
    [JsonProperty] private readonly ICommandInvoker _commandInvoker;

    public GitManager(ICommandInvoker commandInvoker, GitArchive gitArchive, IConfiguration configuration)
    {
        _commandInvoker = commandInvoker;
        _gitArchive = gitArchive;
        _configuration = configuration;
    }

    public string CreateArchive(
        string repositoryId, GitCommitIdentifier gitCommitIdentifier) =>
        _gitArchive.CreateArchive(repositoryId, gitCommitIdentifier);

    public bool IsGitRepositoryInitialized(string repositoryLocation)
    {
        try
        {
            _commandInvoker.Run(_configuration.GitPath, "status", repositoryLocation);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public GitCommitIdentifier ParseCommitId(string commitId) => new(commitId);
}
