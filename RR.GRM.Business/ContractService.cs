using RR.GRM.Models.DTO;
using RR.GRM.Repository;

namespace RR.GRM.Business
{
    public class ContractService : IContractService
    {
        private IMusicContractRepository _musicContractRepository;
        private IDistributionPartnerUsageRepository _distributionPartnerUsageRepository;

        public ContractService(IMusicContractRepository musicContractRepository, IDistributionPartnerUsageRepository distributionPartnerUsageRepository)
        {
            _musicContractRepository = musicContractRepository;
            _distributionPartnerUsageRepository = distributionPartnerUsageRepository;
        }

        public List<PartnerApplicableContract> GetPartnerApplicableContracts(string partnerName, DateTime effectiveDate)
        {
            var allowedUsage = _distributionPartnerUsageRepository
                .GetDistributionPartnerUsageByPartnerName(partnerName);                

            if(allowedUsage == null)
            {
                return new List<PartnerApplicableContract>();
            }

            return _musicContractRepository
                .GetMusicContractsByUsageAndEffectvieDate(allowedUsage.Usage, effectiveDate)
                .Select(mc => new PartnerApplicableContract
                {
                    Artist = mc.Artist,
                    Title = mc.Title,
                    Usage = allowedUsage.Usage,
                    StartDate = mc.StartDate,
                    EndDate = mc.EndDate
                })
                .OrderByDescending(c => c.StartDate)
                .ToList();
        }
    }
}
