
using System.ComponentModel.DataAnnotations;

namespace JABlog.Models
{
    public class EmailData
    {
        [Required]
        public string? EmailAddress { get; set; }
        [Required]
        public string? EmailSubject { get; set; }
        [Required]
        public string? EmailBody { get; set; }


        public string? FullName { get; set; }

    }
}
