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
        [Display(Name = "Name of the Image")]
        public string? ImageName { get; set; }

        [NotMapped]
        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }


        // foreign key
        public int ServiceID { get; set; }

        public Service? Service { get; set; }


    }
}
