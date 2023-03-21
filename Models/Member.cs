using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
	public class Member
	{
        //Propreties 
        // Primary key
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string? Name { get; set; }


        [Required]
        [Display(Name = "JobTitle")]
        public string? JobTitle { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string? Email { get; set; }


        [Display(Name = "Image")]
        public string? ImagePath { get; set; } = "https://localhost:7014/api/MemeberApi/uploads/";


        [NotMapped]
        [Display(Name = "Upload image file")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "AltText")]
        public string? AltText { get; set; }

    }
}

