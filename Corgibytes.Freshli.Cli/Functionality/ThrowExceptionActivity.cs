using System;
using System.Threading;
using System.Threading.Tasks;
using Corgibytes.Freshli.Cli.Functionality.Engine;

namespace Corgibytes.Freshli.Cli.Functionality;

public class ThrowExceptionActivity : IApplicationActivity
{
    public ValueTask Handle(IApplicationEventEngine eventClient, CancellationToken cancellationToken) =>
        throw new Exception("Simulating failure from an activity");
}
