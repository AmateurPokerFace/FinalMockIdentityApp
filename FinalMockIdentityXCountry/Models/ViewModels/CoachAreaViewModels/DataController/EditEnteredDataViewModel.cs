using System.ComponentModel.DataAnnotations;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.DataController
{
    public class EditEnteredDataViewModel
    {
        public string? RunnerName { get; set; }
        public int Hours { get; set; }
        [Range(0, 60)]
        public int Minutes { get; set; }
        [Range(0, 60)]
        public int Seconds { get; set; }
        public double Distance { get; set; }
        public string? WorkoutName { get; set; }
        public int WorkoutInformationId { get; set; }
    }
}
