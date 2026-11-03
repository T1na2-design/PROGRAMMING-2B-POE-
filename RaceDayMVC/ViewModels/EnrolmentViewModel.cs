namespace RaceDayMVC.ViewModels
{
    public class EnrolmentViewModel
    {
        public int EnrolmentID { get; set; }
        public string? EventTitle { get; set; }
        public string? CategoryName { get; set; }
        public DateTime EnrolmentDate { get; set; }
        public string? Status { get; set; }
        public string? BibNumber { get; set; }
    }
}
