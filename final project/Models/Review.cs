using System;

namespace final_project.Models;

public class Review
{
    public Guid Id { get; set; }
    public Guid FilmId { get; set; }
    public string? Author { get; set; }
    public int? Rating { get; set; }
    public string? Content { get; set; }
    public DateTime? CreatedAt { get; set; }
}