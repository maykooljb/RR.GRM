namespace RR.GRM.Models.DTO
{
    public class PartnerApplicableContract
    {
        public string Artist { get; set; } = "";
        public string Title { get; set; } = "";
        public string Usage { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
