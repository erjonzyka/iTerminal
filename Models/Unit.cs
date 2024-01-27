using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
#pragma warning disable CS8618

namespace iTerminal.Models;

public class Unit
{
    [Key]
    public int UnitId { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Price is required")]
    public int Price { get; set; }

    [Required(ErrorMessage = "Seat number is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Value must be greater than or equal to 1")]
    public int Seats {get;set;}
    public string? Description { get; set; }
    public DateTime Nisja { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public int? CreatorId {get;set;}
    public int? RouteId { get; set; }

    public Company? Creator {get;set;}

    public Linja? route {get;set;}

    public string? ImageFileName { get; set; } 
        public byte[]? ImageData { get; set; } 

        [Display(Name = "Upload Image")] 
        [NotMapped]
        public IFormFile? ImageFile { get; set; } 
  
    
    public List<Association>? AllAssociations {get;set;}
    
    
}


public class UniqueProductAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {

        if (value == null)
        {

            return new ValidationResult("Name is required!");
        }


        MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));

        if (_context.Units.Any(e => e.Name == value.ToString()))
        {

            return new ValidationResult("This Unit exists!");
        }
        else
        {

            return ValidationResult.Success;
        }
    }
}




