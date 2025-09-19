using Microsoft.Extensions.Options;
using RR.GRM.Models.Configurations;
using RR.GRM.Models.Models;
using RR.GRM.Repository.DataSources;

namespace RR.GRM.Repository
{
    public class MusicContractFileRepository: IMusicContractRepository
    {
        private IFileOperations _fileOperations;
        private FilesLocationConfiguration _filesLocationConfiguration;
        private List<MusicContract> _cachedMusicContracts;

        public MusicContractFileRepository(IFileOperations fileOperations, IOptions<FilesLocationConfiguration> filesLocationConfiguration)
        {
            _fileOperations = fileOperations;
            _filesLocationConfiguration = filesLocationConfiguration.Value;
            _cachedMusicContracts = new List<MusicContract>();
        }

        public IList<MusicContract> GetMusicContractsByUsageAndEffectvieDate(string usage, DateTime effectiveDate)
        {
            if(_cachedMusicContracts.Count == 0)
            {
                var fileLines = _fileOperations
                .GetFileLines(_filesLocationConfiguration.MusicContractPath)
                .Skip(1)
                .Select(ParseTextLineToMusicContract)
                .ToList();

                _cachedMusicContracts = fileLines;
            }

            return _cachedMusicContracts
                .Where(c =>
                    c.Usages.Any(u => u == usage)
                    && c.StartDate <= effectiveDate
                    && (!c.EndDate.HasValue || c.EndDate.Value >= effectiveDate))
                .ToList();
        }

        private MusicContract ParseTextLineToMusicContract(string fileLine)
        {
            var parts = fileLine.Split('|');
            return new MusicContract
            {
                Artist = parts[0],
                Title = parts[1],
                Usages = parts[2].Split(',').Select(u => u.Trim()).ToList(),
                StartDate = DateTime.Parse(parts[3]),
                EndDate = string.IsNullOrWhiteSpace(parts[4]) ? null : DateTime.Parse(parts[4])
            };
        }
    }
}
