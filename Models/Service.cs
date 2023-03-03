using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Project.Models
{
    public class Service
    {
        //Propreties 
        // Primary key
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name of the service")]
        public string? Name { get; set; }


        [Required]
        [Display(Name = "Content")]
        public string? Details { get; set; }


        //[Display(Name = "Image")]
        //public string? ImagePath { get; set; }

        [Display(Name = "Image")]
        public string? ImagePath { get; set; } = "https://localhost:7014/api/ServiceApi/uploads/";


        [NotMapped]
        [Display(Name = "Upload image file")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "AltText")]
        public string? AltText { get; set; }

        public ICollection<Offer>? Offer { get; set; }


    }
}