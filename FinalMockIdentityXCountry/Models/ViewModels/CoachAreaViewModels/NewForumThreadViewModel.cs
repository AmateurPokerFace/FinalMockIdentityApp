using Microsoft.Build.Framework;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels
{
    public class NewForumThreadViewModel
    {
        [Required]
        public string? ThreadTitle { get; set; }
        [Required]
        public string? ThreadBody { get; set; }
    }
}
