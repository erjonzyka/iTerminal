#pragma warning disable CS8618
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace iTerminal.Models;

public class Company
{

    [Key]
    public int CompanyId { get; set; }
    [Required(ErrorMessage ="Name is required")]
    [MinLength(2, ErrorMessage ="First Name must be at least 2 characters")]

    [UniqueName]
    public string Name { get; set; }
    
    [UniqueEmail]    
    public string Email { get; set; }
    [Required]
    [MinLength(8, ErrorMessage ="Password must be at least 8 characters")]
    public string Password { get; set; }
    [NotMapped]
    [Required]
    [MinLength(8)]
    [Compare("Password",ErrorMessage ="Passwords must match!")]
    public string Pwconfirm { get; set; }


    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public List<Unit>? AllUnits  {get; set;}

    
}


public class UniqueNameAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {

        if (value == null)
        {

            return new ValidationResult("Name is required!");
        }


        MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));

        if (_context.Companies.Any(e => e.Name == value.ToString()))
        {

            return new ValidationResult("Name must be unique!");
        }
        else
        {

            return ValidationResult.Success;
        }
    }
}





