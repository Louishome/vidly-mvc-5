using System;
using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Date released")]
        [Required]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Date added")]
        [Required]
        public DateTime DateAdded { get; set; }

        [Display(Name = "Number in stock")]
        [Required]
        public int NumberInStock { get; set; }

        [Required]
        public int GenreId { get; set; }

        public Genre GenreType { get; set; }
    }
}