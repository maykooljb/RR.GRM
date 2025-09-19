using RR.GRM.Models.Models;

namespace RR.GRM.Repository
{
    public interface IDistributionPartnerUsageRepository
    {
        DistributionPartnerUsage? GetDistributionPartnerUsageByPartnerName(string partnerName);
    }
}