using FinalMockIdentityXCountry.Models.ViewModelHelperClasses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.Delete
{
    public class RecordWorkoutsViewModel
    {
        public RecordWorkoutsViewModel()
        {
            WorkoutSelections = new List<WorkoutSelectionInformation>();
        }

        public List<WorkoutSelectionInformation> WorkoutSelections { get; set; }
        //public string WorkoutName { get; set; }
        public string RunnersName { get; set; }
        public string RunnerId { get; set; }

    }
}
