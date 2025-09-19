namespace RR.GRM.Models.Models
{
    public class MusicContract
    {
        public string Artist { get; set; } = "";
        public string Title { get; set; } = "";
        public List<string> Usages { get; set; } = [];
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
