using System.ComponentModel.DataAnnotations;

namespace Company.Function.Models;

public class Vote
{
    [Required]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    [Required]
    public string JokeId { get; set; } = Guid.NewGuid().ToString();
    public DateTime DateSubmitted { get; set; }
    public string? SubmittedBy { get; set; }
    public string? UserId { get; set; }
    public string? Email { get; set; }
}
