﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamGeneratorModel.Model
{
    public class Student : Entity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Surname")]
        public string SurName { get; set; }
        public string Email { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        [Required]
        public string Owner { get; set; }
    }
}
