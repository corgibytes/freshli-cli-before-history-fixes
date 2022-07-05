﻿using Corgibytes.Freshli.Cli.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace Corgibytes.Freshli.Cli.Test;

public class Startup
{
    public virtual void ConfigureServices(IServiceCollection services) =>
        new FreshliServiceBuilder(services).Register();
    //var runnerMock = new Mock<Runner>();
    //runnerMock.Setup(r => r.Run(It.IsAny<string>())).Returns(new List<MetricsResult>());
    //services.AddScoped<Runner>(provider => runnerMock.Object);
}
