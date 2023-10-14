using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Domain.Entities;

public class Dog
{
    [Required(ErrorMessage = "name is required.")]
    [MaxLength(50, ErrorMessage = "name must be at most 50 characters.")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "color is required.")]
    [MaxLength(50, ErrorMessage = "color must be at most 50 characters.")]
    public string Color { get; set; } = "";

    [Required(ErrorMessage = "tail_length is required.")]
    [Range(0.01, 1000, ErrorMessage = "tail_length must be a number between 0.01 and 1000.")]
    [JsonProperty("tail_length")] 
    public double TailLength { get; set; }

    [Required(ErrorMessage = "weight is required.")]
    [Range(0.01, 1000, ErrorMessage = "weight must be a number between 0.01 and 1000.")]
    public double Weight { get; set; }
}
