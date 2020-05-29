﻿using System.ComponentModel.DataAnnotations;

namespace VoteApplication.Repositories.Models
{
    public class Candidate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }
    }
}