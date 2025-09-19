using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RR.GRM.Business;
using RR.GRM.Models.Configurations;
using RR.GRM.Repository;
using RR.GRM.Repository.DataSources;

class Program
{
    static void Main(string[] args)
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        
        var provider = services.BuildServiceProvider();       
        var contractService = provider.GetRequiredService<IContractService>();

        string partnerName = args[0];
        DateTime effectiveDate = DateTime.Parse(args[1]);
        var results = contractService.GetPartnerApplicableContracts(partnerName, effectiveDate);

        Console.WriteLine("| Artist | Title | Usages | StartDate | EndDate |");
        foreach (var r in results)
        {
            Console.WriteLine($"| {r.Artist} | {r.Title} | {r.Usage} | {r.StartDate:MM-dd-yyyy} | {(r.EndDate?.ToString("MM-dd-yyyy") ?? "")} |");
        }
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        services.Configure<FilesLocationConfiguration>(config => {
            var fileSetting = configuration.GetSection("FilesLocationConfiguration").Get<FilesLocationConfiguration>();
            config.DistributionPartnerUsagePath = fileSetting.DistributionPartnerUsagePath;
            config.MusicContractPath = fileSetting.MusicContractPath;
        });
        services.AddSingleton<IFileOperations, FileOperations>();
        services.AddSingleton<IMusicContractRepository, MusicContractFileRepository>();
        services.AddSingleton<IDistributionPartnerUsageRepository, DistributionPartnerUsageFileRepository>();
        services.AddSingleton<IContractService, ContractService>();
    }
}