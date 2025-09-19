using Moq;
using RR.GRM.Models.Models;
using RR.GRM.Repository;

namespace RR.GRM.Business.Test
{
    [TestClass]
    public sealed class A_ContractServiceTests
    {
        private ContractService _contractService = null!;

        private readonly Mock<IMusicContractRepository> _musicContractRepositoryMock = new();
        private readonly Mock<IDistributionPartnerUsageRepository> _distributionPartnerUsageRepository = new();
        private List<MusicContract> _musicContracts = null!;
        

        [TestInitialize]
        public void TestInit()
        {
            _musicContracts = [
                new MusicContract {
                    Artist = "Tinie Tempah",
                    Title = "Miami 2 Ibiza",
                    Usages = ["digital download"],
                    StartDate = DateTime.Parse("02-01-2012")
                },
                new MusicContract {
                    Artist = "Monkey Claw",
                    Title = "Christmas Special",
                    Usages = ["streaming"],
                    StartDate = DateTime.Parse("12-25-2012"),
                    EndDate = DateTime.Parse("12-31-2012")
                },
                new MusicContract {
                    Artist = "Monkey Claw",
                    Title = "Iron Horse",
                    Usages = ["streaming"],
                    StartDate = DateTime.Parse("06-01-2012")
                },
                new MusicContract {
                    Artist = "Monkey Claw",
                    Title = "Christmas Special",
                    Usages = ["mixed"],
                    StartDate = DateTime.Parse("12-25-2012"),
                },
                new MusicContract {
                    Artist = "Monkey Claw",
                    Title = "Iron Horse",
                    Usages = ["mixed"],
                    StartDate = DateTime.Parse("06-01-2012")
                }
            ];
            //_musicContractRepositoryMock
            //    .Setup(mc => mc.GetMusicContractsByUsageAndEffectvieDate(It.IsAny<string>(), It.IsAny<DateTime>()))
            //    .Returns(_musicContracts);

            //_distributionPartnerUsages = [
            //    new DistributionPartnerUsage { 
            //        Partner = "ITunes", 
            //        Usage = "digital download" 
            //    },
            //    new DistributionPartnerUsage {
            //        Partner = "YouTube",
            //        Usage = "streaming"
            //    },
            //    new DistributionPartnerUsage {
            //        Partner = "Spotify",
            //        Usage = "mixed"
            //    }
            //];

            //_distributionPartnerUsageRepository
            //    .Setup(dpu => dpu.GetDistributionPartnerUsageByPartnerName(It.IsAny<string>()))
            //    .Returns(_distributionPartnerUsages);

            _contractService = new ContractService(_musicContractRepositoryMock.Object, _distributionPartnerUsageRepository.Object);
        }

        [TestMethod]
        public void GetPartnerApplicableContracts_GetsDistributionPartnerUsage_FromRepository()
        {
            _musicContractRepositoryMock
                .Setup(mc => mc.GetMusicContractsByUsageAndEffectvieDate("digital download", DateTime.Parse("02-01-2012")))
                .Returns([
                    new MusicContract {
                        Artist = "Tinie Tempah",
                        Title = "Miami 2 Ibiza",
                        Usages = ["digital download"],
                        StartDate = DateTime.Parse("02-01-2012")
                    }
                ]);

            _distributionPartnerUsageRepository
                .Setup(dpu => dpu.GetDistributionPartnerUsageByPartnerName("ITunes"))
                .Returns(new DistributionPartnerUsage
                {
                    Partner = "ITunes",
                    Usage = "digital download"
                });

            var result = _contractService.GetPartnerApplicableContracts("ITunes", DateTime.Parse("02-01-2012"));

            _distributionPartnerUsageRepository
                .VerifyAll();
        }

        [TestMethod]
        public void GetPartnerApplicableContracts_GetsMusicContracts_FromRepository()
        {
            _musicContractRepositoryMock
                .Setup(mc => mc.GetMusicContractsByUsageAndEffectvieDate("digital download", DateTime.Parse("02-01-2012")))
                .Returns([
                    new MusicContract {
                        Artist = "Tinie Tempah",
                        Title = "Miami 2 Ibiza",
                        Usages = ["digital download"],
                        StartDate = DateTime.Parse("02-01-2012")
                    } 
                ]);

            _distributionPartnerUsageRepository
                .Setup(dpu => dpu.GetDistributionPartnerUsageByPartnerName("ITunes"))
                .Returns(new DistributionPartnerUsage
                {
                    Partner = "ITunes",
                    Usage = "digital download"
                });

            var result = _contractService.GetPartnerApplicableContracts("ITunes", DateTime.Parse("02-01-2012"));

            _musicContractRepositoryMock
                .VerifyAll();
        }
        
        [TestMethod]
        public void GetPartnerApplicableContracts_Returns_Results_Order_By_StartDate_Descendant()
        {
            _musicContractRepositoryMock
                .Setup(mc => mc.GetMusicContractsByUsageAndEffectvieDate(It.IsAny<string>(), It.IsAny<DateTime>()))
                    .Returns([
                        new MusicContract {
                            Artist = "Monkey Claw",
                            Title = "Iron Horse",
                            Usages = ["streaming"],
                            StartDate = DateTime.Parse("06-01-2012")
                        },
                        new MusicContract {
                            Artist = "Monkey Claw",
                            Title = "Christmas Special",
                            Usages = ["streaming"],
                            StartDate = DateTime.Parse("12-25-2012"),
                            EndDate = DateTime.Parse("12-31-2012")
                        }
                    ]);

            _distributionPartnerUsageRepository
                .Setup(dpu => dpu.GetDistributionPartnerUsageByPartnerName("Spotify"))
                .Returns(new DistributionPartnerUsage
                {
                    Partner = "Spotify",
                    Usage = "mixed"
                });

            var result = _contractService.GetPartnerApplicableContracts("Spotify", DateTime.Parse("01-01-2013"));

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(DateTime.Parse("12-25-2012"), result[0].StartDate);
            Assert.AreEqual(DateTime.Parse("06-01-2012"), result[1].StartDate);
        }
    }
}
