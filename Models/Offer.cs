using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Project.Models;

namespace Project.Models
{
    public class Offer
    {
        //Propreties 
        // Primary key
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name of the offer")]
        public string? Name { get; set; }


        [Required]
        [Display(Name = "Content")]
        public string? Details { get; set; }

        [Required]
        [Display(Name = "Price")]
        public int Price { get; set; }


        //[Display(Name = "FileName - image")]
        //public string? ImageName { get; set; }

        //[NotMapped]
        //[Display(Name = "Image")]
        //public IFormFile ImageFile { get; set; }

        //[Required]
        //[Display(Name = "AltText")]
        //public string? AltText { get; set; }

        [Display(Name = "Image")]
        public string? ImagePath { get; set; }

        [NotMapped]
        [Display(Name = "Upload image file")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "AltText")]
        public string? AltText { get; set; }

        // foreign key
        [Display(Name = "Select a service")]
        public int ServiceId { get; set; }

        public Service? Service { get; set; }


    }
}
