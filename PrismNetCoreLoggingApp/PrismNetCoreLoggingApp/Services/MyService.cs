// ------------------------------------------------------------------------------------------------------------------—
namespace PrismNetCoreLoggingApp.Services
{
    using System.Diagnostics;

    using Microsoft.Extensions.Logging;

    using PrismNetCoreLoggingApp.Interfaces;

    public class MyService:IMyService
    {
        private readonly ILogger<MyService> logger;

        public MyService(ILogger<MyService> logger)
        {
            this.logger = logger;
        }

        public void DoSomething(int counter)
        {
            //Debug.WriteLine($"Doing something... Counter is {counter}");
            this.logger.LogInformation($"Doing something... Counter is {counter}");
        }
    }
}