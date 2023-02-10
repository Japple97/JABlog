using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JABlog.Models
{
    public class BlogUser : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} and at the most {1} characters", MinimumLength = 2)]
        public string? FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string? LastName { get; set;}

        [NotMapped]
        [Display(Name = "Full Name")]
        public string FullName { get { return $"{FirstName } {LastName}"; } }
    }
}
