using System.ComponentModel.DataAnnotations;

namespace Company.Function;

public class Vote
{
    [Required]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public DateTime DateSubmitted { get; set; }
    public string? SubmittedBy { get; set; }
    public string? UserId { get; set; }
    public string? Email { get; set; }
}
