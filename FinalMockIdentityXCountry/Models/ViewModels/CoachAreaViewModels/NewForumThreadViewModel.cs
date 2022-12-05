using System.ComponentModel.DataAnnotations;

namespace FinalMockIdentityXCountry.Models.ViewModels.CoachAreaViewModels
{
    public class NewForumThreadViewModel
    {
        [Required(AllowEmptyStrings =false,ErrorMessage ="This field is required")] 
        public string? ThreadTitle { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field is required")] 
        public string? ThreadBody { get; set; }
    }
}
