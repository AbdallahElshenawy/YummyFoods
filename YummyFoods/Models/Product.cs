﻿using System.ComponentModel.DataAnnotations;

namespace YummyFoods.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } 
        public string? Description { get; set; }
        [Range(10, 10000)]
        public decimal Price { get; set; }
        public string? SpecialTag { get; set; }
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
