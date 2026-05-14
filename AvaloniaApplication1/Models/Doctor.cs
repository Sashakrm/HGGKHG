using System.ComponentModel.DataAnnotations;
namespace AvaloniaApplication1.Models;

public class Doctor
{
    [Key]
    public int Id { get; set; }
    
    [Required, StringLength(150)]
    public string Full_Name { get; set; }
    
    [StringLength(100)]
    public string Specialization {get; set;}=string.Empty;
    
    [StringLength(200)]
    public string Schedle { get; set; } = string.Empty;
}