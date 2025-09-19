using Microsoft.Extensions.Options;
using RR.GRM.Models.Configurations;
using RR.GRM.Models.Models;
using RR.GRM.Repository.DataSources;

namespace RR.GRM.Repository
{
    public class DistributionPartnerUsageFileRepository: IDistributionPartnerUsageRepository
    {
        private IFileOperations _fileOperations;
        private FilesLocationConfiguration _filesLocationConfiguration;
        private List<DistributionPartnerUsage> _cachedDistributionPartnerUsages;

        public DistributionPartnerUsageFileRepository(IFileOperations fileOperations, IOptions<FilesLocationConfiguration> filesLocationConfiguration)
        {
            _fileOperations = fileOperations;
            _filesLocationConfiguration = filesLocationConfiguration.Value;
            _cachedDistributionPartnerUsages = new List<DistributionPartnerUsage>();
        }

        public DistributionPartnerUsage? GetDistributionPartnerUsageByPartnerName(string partnerName)
        {
            if (_cachedDistributionPartnerUsages.Count == 0)
            {
                var fileLines = _fileOperations
                .GetFileLines(_filesLocationConfiguration.DistributionPartnerUsagePath)
                .Skip(1)
                .Select(ParseTextLineToDistributionPartnerUsage)
                .ToList();

                _cachedDistributionPartnerUsages = fileLines;
            }

            return _cachedDistributionPartnerUsages
                .FirstOrDefault(p => p.Partner.Equals(partnerName, StringComparison.OrdinalIgnoreCase));
        }

        private DistributionPartnerUsage ParseTextLineToDistributionPartnerUsage(string fileLine)
        {
            var parts = fileLine.Split('|');
            return new DistributionPartnerUsage
            {
                Partner = parts[0],
                Usage = parts[1]                
            };
        }
    }
}