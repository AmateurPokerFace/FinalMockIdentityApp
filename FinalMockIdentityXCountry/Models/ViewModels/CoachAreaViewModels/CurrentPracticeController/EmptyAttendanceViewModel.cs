namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels.CurrentPracticeController
{
    public class EmptyAttendanceViewModel
    {
        public EmptyAttendanceViewModel(int practiceId)
        {
            PracticeId = practiceId;
        }

        public int PracticeId { get; set; }
    }
}
