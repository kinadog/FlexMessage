using System.ComponentModel.DataAnnotations;

namespace Demo2.Models;

public class Logs
{
    [Key]
    public int Id { get; set; }
    public string? Message { get; set; }
    public DateTime Writedates { get; set; }
}