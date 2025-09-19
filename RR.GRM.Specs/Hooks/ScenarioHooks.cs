using Reqnroll;
using RR.GRM.Models.Configurations;
using RR.GRM.Repository.DataSources;
using RR.GRM.Business;
using RR.GRM.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace RR.GRM.Specs.Hooks
{
    [Binding]
    public class ScenarioHooks
    {
        private static ScenarioContext _scenarioContext = null!;

        [BeforeScenario]
        public static void Setup(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;

            var services = new ServiceCollection();
            services.Configure<FilesLocationConfiguration>(config => 
            {
                config.MusicContractPath = Path.Combine(AppContext.BaseDirectory, "TestData", "MusicContracts.csv");
                config.DistributionPartnerUsagePath = Path.Combine(AppContext.BaseDirectory, "TestData", "DistributionPartnerContracts.csv");
            });
            services.AddSingleton<IFileOperations, FileOperations>();
            services.AddSingleton<IMusicContractRepository, MusicContractFileRepository>();
            services.AddSingleton<IDistributionPartnerUsageRepository, DistributionPartnerUsageFileRepository>();
            services.AddSingleton<IContractService, ContractService>();

            var provider = services.BuildServiceProvider();

            _scenarioContext.Set(provider.GetService<IContractService>(), "ContractService");
        }
    }
}