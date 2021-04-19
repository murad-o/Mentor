﻿using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Lesson
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
