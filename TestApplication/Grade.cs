using System.ComponentModel.DataAnnotations;

namespace TestApplication;

public class Grade
{
    [Key]
    public int Id { get; set; }
    public int Score { get; set; }
}