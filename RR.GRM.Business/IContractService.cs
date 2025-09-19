using RR.GRM.Models.DTO;

namespace RR.GRM.Business
{
    public interface IContractService
    {
        List<PartnerApplicableContract> GetPartnerApplicableContracts(string partnerName, DateTime effectiveDate);
    }
}