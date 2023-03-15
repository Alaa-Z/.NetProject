using System;
using System.ComponentModel.DataAnnotations;
using Project.Models;

namespace Project.Models
{
	public class Message
	{
        //Propreties 
        // Primary key
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Msg { get; set; }

    }
}

