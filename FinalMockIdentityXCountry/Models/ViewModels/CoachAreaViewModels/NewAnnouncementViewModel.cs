using Microsoft.Build.Framework;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels
{
    public class NewAnnouncementViewModel
    {
        [Required]
        public string? AnnouncementTitle { get; set; }
        [Required]
        public string? AnnouncementBody { get; set; }
    }
}
