using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class Bugs
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Projects { get; set; }

        [Required]
        [MaxLength(50)]
        public string Tickets { get; set; }

        [Required]
        public string Description { get; set; }

        public string Resolved { get; set; }

        [Required]
        public string Priority { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime DueDate { get; set; }
        public string ClosedDate { get; set; }
       
        public string Creator { get; set; }


        public string TimeCreated { get; set; }

        public Bugs()
        {
        }

    
        
    }
}
