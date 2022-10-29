namespace FinalMockIdentityXCountry.Models.ViewModels.RunnerAreaViewModels
{
    public class SelectAPracticeViewModel
    {
        public int WorkoutInformationId { get; set; }
        public int PracticeId { get; set; }
        public string? RunnerId { get; set; }
        public bool DataWasLogged { get; set; }
        public DateTime PracticeStartTimeAndDate { get; set; }
        public string? PracticeLocation { get; set; }
        public string? WorkoutName { get; set; }
    }
}
