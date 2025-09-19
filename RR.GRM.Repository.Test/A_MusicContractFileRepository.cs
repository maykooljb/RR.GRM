using Microsoft.Extensions.Options;
using Moq;
using RR.GRM.Models.Configurations;
using RR.GRM.Repository.DataSources;

namespace RR.GRM.Repository.Test
{
    [TestClass]
    public sealed class A_MusicContractFileRepository
    {
        // SUT
        private MusicContractFileRepository _musicContractRepository = null!;


        private Mock<IFileOperations> _fileOperationsMock = new();
        private FilesLocationConfiguration _filesLocationConfiguration = new()
        {
            MusicContractPath = "dummyPath"
        };                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          

        [TestInitialize]
        public void TestInit()
        {
            _fileOperationsMock
                .Setup(fo => fo.GetFileLines(It.IsAny<string>()))
                .Returns(
                [
                    "Artist|Title|Usages|StartDate",
                    "Tinie Tempah|Miami 2 Ibiza|digital download|02-01-2012|",
                    "Tinie Tempah|Miami 2 Ibiza|digital download|02-01-2012|",                                                                                                                  
                    "Tinie Tempah|Till I'm Gone|digital download|08-01-2012|",
                    "Monkey Claw|Black Mountain|digital download|02-01-2012|",
                    "Monkey Claw|Iron Horse|digital download, streaming|06-01-2012|",
                    "Monkey Claw|Motor Mouth|digital download, streaming|03-01-2011|",
                    "Monkey Claw|Christmas Special|streaming|12-25-2012|12-31-2012"
                ]);            

            _musicContractRepository = new MusicContractFileRepository(_fileOperationsMock.Object, Options.Create(_filesLocationConfiguration));
        }

        [TestMethod]
        public void GetMusicContractsByUsageAndEffectvieDate_ReturnsMatchingContracts_ByUsage()
        {
            var contracts = _musicContractRepository.GetMusicContractsByUsageAndEffectvieDate("streaming", DateTime.Parse("12-25-2012"));
            Assert.AreEqual(3, contracts.Count);
        }

        [TestMethod]
        public void GetMusicContractsByUsageAndEffectvieDate_ReturnsMatchingContracts_ByDate()
        {
            var contracts = _musicContractRepository.GetMusicContractsByUsageAndEffectvieDate("streaming", DateTime.Parse("06-01-2012"));
            Assert.AreEqual(2, contracts.Count);
        }

        [TestMethod]
        public void GetMusicContractsByUsageAndEffectvieDate_ContractsDataIsParsedAsExpected()
        {
            var contracts = _musicContractRepository.GetMusicContractsByUsageAndEffectvieDate("digital download", DateTime.Parse("06-01-2012"));

            var contract = contracts.FirstOrDefault();
            Assert.AreEqual("Tinie Tempah", contract?.Artist);
            Assert.AreEqual("Miami 2 Ibiza", contract?.Title);
            Assert.AreEqual("digital download", contract?.Usages.FirstOrDefault());
            Assert.AreEqual("02-01-2012", contract?.StartDate.ToString("MM-dd-yyyy"));
        }
    }
}
