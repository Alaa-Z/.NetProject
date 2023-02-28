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


        //[Required]
        [Display(Name = "Content")]
        public string? Details { get; set; }

        //[Required]
        [Display(Name = "Name of the Image")]
        public string? ImageName { get; set; }

        [NotMapped]
        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }

        public ICollection<Offer>? Offer { get; set; }


    }
}