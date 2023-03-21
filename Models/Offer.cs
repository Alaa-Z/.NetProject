using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Project.Models;

namespace Project.Models
{
    public class Offer
    {
        //Propreties 
        // Primary key
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string? Name { get; set; }


        [Required]
        [Display(Name = "Content")]
        public string? Details { get; set; }

        [Required]
        [Display(Name = "Price")]
        public int Price { get; set; }


        [Display(Name = "Image")]
        public string? ImagePath { get; set; } = "https://localhost:7014/api/ServiceApi/uploads/";

        [NotMapped]
        [Display(Name = "Upload image file")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "AltText")]
        public string? AltText { get; set; }

        // foreign key
        [Display(Name = "Select a service")]
        public int ServiceId { get; set; }

        [JsonIgnore]
        public Service? Service { get; set; }


    }
}
