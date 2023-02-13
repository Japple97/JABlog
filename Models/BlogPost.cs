﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JABlog.Models
{
    public class BlogPost
    {
        //Primary Key
        public int Id { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} and at the most {1} characters", MinimumLength = 2)]
        public string? Title { get; set; }

        [StringLength(600, ErrorMessage = "The {0} must be at least {2} and at the most {1} characters", MinimumLength = 2)]
        public string? Abstract { get; set; }

        [Required]
        public string? Content { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Updated { get; set; }


        // TODO: make this required
        public string? Slug { get; set; }

        [Display(Name = "Deleted?")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Published?")]
        public bool IsPublished { get; set; }

        //image properties
        public byte[]? ImageData { get; set; }
        public string? ImageType { get; set; }

        [NotMapped]
        public virtual IFormFile? ImageFile { get; set; }


       
        //Foreign Key 1-to-1
        public int CategoryId { get; set; }

        // navigation properties
        public virtual Category? Category { get; set; }

        // Many-to-Many
        public virtual ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();

        // 1-to-Many 
        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();


    }
}
