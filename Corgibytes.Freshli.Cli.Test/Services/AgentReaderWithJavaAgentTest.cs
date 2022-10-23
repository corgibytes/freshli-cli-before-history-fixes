using System;
using System.Collections.Generic;
using System.IO;
using Corgibytes.Freshli.Cli.Functionality;
using Corgibytes.Freshli.Cli.Services;
using Xunit;
using Environment = Corgibytes.Freshli.Cli.Functionality.Environment;

namespace Corgibytes.Freshli.Cli.Test.Services;

[IntegrationTest]
public class AgentReaderWithJavaAgentTest
{
    [Fact]
    public void DetectManifestsUsingProtobuf()
    {
        SetupDirectory(out var repositoryLocation, out var reader, out var checkoutDirectory);

        var actualManifests = reader.DetectManifests(repositoryLocation);

        var expectedManifests = new List<string>
        {
            "java/pom.xml",
            "java/protoc/pom.xml",
            "ruby/pom.xml"
        };
        Assert.Equal(expectedManifests, actualManifests);

        // delete cloned files
        checkoutDirectory.Delete(true);
    }

    [Fact]
    public void GenerateBillOfMaterialsUsingProtobuf()
    {
        SetupDirectory(out var repositoryLocation, out var reader, out var checkoutDirectory);

        // java/pom.xml is detected by detect manifest, see also DetectManifestsUsingProtobuf()
        var billOfMaterialsPath =
            reader.ProcessManifest(Path.Combine(repositoryLocation, "java", "pom.xml"), DateTime.Now);

        Assert.Equal(Path.Combine(repositoryLocation, "java", "target", "bom.json"), billOfMaterialsPath);

        // delete cloned files
        checkoutDirectory.Delete(true);
    }

    [Fact]
    public void AgentReaderReturnsEmptyListWhenNoManifestsFound()
    {
        var checkoutLocation = CreateCheckoutLocation(out var checkoutDirectory);
        var reader = new AgentReader(new CacheManager(new Configuration(new Environment())), new CommandInvoker(),
            "freshli-agent-java");
        var repositoryLocation = Path.Combine(checkoutLocation, "invalid_repository");

        var actualManifests = reader.DetectManifests(repositoryLocation);
        Assert.Empty(actualManifests);
        checkoutDirectory.Delete();
    }

    private static void SetupDirectory(out string repositoryLocation, out AgentReader reader,
        out DirectoryInfo checkoutDirectory)
    {
        var checkoutLocation = CreateCheckoutLocation(out checkoutDirectory);

        // clone https://github.com/protocolbuffers/protobuf to a temp location
        new CommandInvoker().Run("git", "clone https://github.com/protocolbuffers/protobuf", checkoutLocation);

        repositoryLocation = Path.Combine(checkoutLocation, "protobuf");

        reader = new AgentReader(new CacheManager(new Configuration(new Environment())), new CommandInvoker(),
            "freshli-agent-java");
    }

    private static string CreateCheckoutLocation(out DirectoryInfo checkoutDirectory)
    {
        var checkoutLocation = Path.Combine(Path.GetTempPath(), "repositories");

        checkoutDirectory = new DirectoryInfo(checkoutLocation);
        if (checkoutDirectory.Exists)
        {
            checkoutDirectory.Delete(true);
        }

        checkoutDirectory.Create();
        return checkoutLocation;
    }
}
