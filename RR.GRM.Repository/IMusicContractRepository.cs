using RR.GRM.Models.Models;

namespace RR.GRM.Repository
{
    public interface IMusicContractRepository
    {
        IList<MusicContract> GetMusicContractsByUsageAndEffectvieDate(string usage, DateTime effectiveDate);
    }
}