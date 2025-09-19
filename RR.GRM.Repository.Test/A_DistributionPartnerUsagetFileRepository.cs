using Microsoft.Extensions.Options;
using Moq;
using RR.GRM.Models.Configurations;
using RR.GRM.Repository.DataSources;

namespace RR.GRM.Repository.Test
{
    [TestClass]
    public sealed class A_DistributionPartnerUsageFileRepository
    {
        // SUT
        private DistributionPartnerUsageFileRepository _distributionPartnerUsageFileRepository = null!;


        private Mock<IFileOperations> _fileOperationsMock = new();
        private FilesLocationConfiguration _filesLocationConfiguration = new()
        {
            DistributionPartnerUsagePath = "dummyPath"
        };                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          

        [TestInitialize]
        public void TestInit()
        {
            _fileOperationsMock
                .Setup(fo => fo.GetFileLines(It.IsAny<string>()))
                .Returns(
                [
                    "Partner|Usage",
                    "ITunes|digital download",
                    "YouTube|streaming",
                ]);            

            _distributionPartnerUsageFileRepository = new DistributionPartnerUsageFileRepository(_fileOperationsMock.Object, Options.Create(_filesLocationConfiguration));
        }

        [TestMethod]
        public void GetDistributionPartnerUsageByPartnerName_ReturnsMatchingPartner()
        {
            var partnerUsage = _distributionPartnerUsageFileRepository.GetDistributionPartnerUsageByPartnerName("ITunes");
            Assert.IsNotNull(partnerUsage);
            Assert.AreEqual("ITunes", partnerUsage.Partner);

        }

        [TestMethod]
        public void GetDistributionPartnerUsages_DataIsParsedAsExpected()
        {
            var contract = _distributionPartnerUsageFileRepository.GetDistributionPartnerUsageByPartnerName("ITunes");
            Assert.AreEqual("ITunes", contract?.Partner);            
            Assert.AreEqual("digital download", contract?.Usage);
        }
    }
}
