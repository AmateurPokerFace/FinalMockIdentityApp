using System.ComponentModel.DataAnnotations;

namespace FinalMockIdentityXCountry.Models.ViewModels.RunnerAreaViewModels
{
    public class LogDataViewModel
    {
        public int WorkoutInformationId { get; set; }
        public int PracticeId { get; set; }
        public string? RunnerId { get; set; }
        public DateTime PracticeStartDateTime { get; set; }
        public string? PracticeLocation { get; set; }
        public string? WorkoutName { get; set; }
        public int Hours { get; set; }
        [Range(0,60)]
        public int Minutes { get; set; }
        [Range(0, 60)]
        public int Seconds { get; set; }
        public double Distance { get; set; } 
    }
}
