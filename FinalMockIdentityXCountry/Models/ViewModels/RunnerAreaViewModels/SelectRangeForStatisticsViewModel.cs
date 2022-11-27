using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering; 
using System.ComponentModel.DataAnnotations;

namespace FinalMockIdentityXCountry.Models.ViewModels.RunnerAreaViewModels
{
    public class SelectRangeForStatisticsViewModel
    {
        public SelectRangeForStatisticsViewModel()
        {
            QueryFilters = new List<SelectListItem>();
        }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? InitialDate { get; set; }
        public string? RunnerId { get; set; } = null;
         
        public string? SelectedQueryFilter { get; set; }

        [ValidateNever]
        public List<SelectListItem>? QueryFilters { get; set; }
    }
}
