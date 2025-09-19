using System.Globalization;
using System.Text;
using NUnit.Framework;
using Reqnroll;
using RR.GRM.Business;
using RR.GRM.Models.DTO;

namespace RR.GRM.Specs.StepDefinitions
{
    [Binding]
    public class MusicContractsSearchSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private string _actualOutput = "";

        public MusicContractsSearchSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given("the supplied reference data")]
        public void GivenTheSuppliedReferenceData()
        {
        }

        [When("user perform search by (.*) (.*)")]
        public void WhenUserPerformSearchByITunes(string partnerName, string effectiveDateStr)
        {
            var contractService = _scenarioContext.Get<IContractService>("ContractService");            
            var effectiveDate = DateTime.ParseExact(effectiveDateStr, "MM-dd-yyyy", CultureInfo.InvariantCulture);
                        
            var results = contractService.GetPartnerApplicableContracts(partnerName, effectiveDate);

            _actualOutput = FormatTable(results);
        }

        [Then("the output should be")]
        public void ThenTheOutputShouldBe(DataTable expectedTable)
        {
            var expectedOutput = FormatTableFromGherkin(expectedTable);            
            Assert.That(expectedOutput == _actualOutput);
        }

        private static string FormatTable(List<PartnerApplicableContract> contracts)
        {
            var sb = new StringBuilder();
            sb.AppendLine("| Artist | Title | Usages | StartDate | EndDate |");
            foreach (var c in contracts.OrderBy(c => c.Artist))
            {
                sb.AppendLine($"| {c.Artist} | {c.Title} | {c.Usage} | {c.StartDate:MM-dd-yyyy} | {(c.EndDate?.ToString("MM-dd-yyyy") ?? "")} |");
            }
            return sb.ToString();
        }

        private static string FormatTableFromGherkin(Table table)
        {
            var sb = new StringBuilder();
            sb.AppendLine("| Artist | Title | Usages | StartDate | EndDate |");
            foreach (var row in table.Rows)
            {
                sb.AppendLine($"| {row["Artist"]} | {row["Title"]} | {row["Usages"]} | {row["StartDate"]} | {row["EndDate"]} |");
            }
            return sb.ToString();
        }
    }
}
