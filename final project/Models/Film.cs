using System;

namespace final_project.Models;

public class Film
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Director { get; set; }
    public int? Year { get; set; }
    public string? Genre { get; set; }
    public Guid StatusId { get; set; }
    public string? StatusName { get; set; }
}