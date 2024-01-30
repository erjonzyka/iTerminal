using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
#pragma warning disable CS8618

namespace iTerminal.Models;

public class Trip
{
    [Key]
    public int TripId { get; set; }
    [Required(ErrorMessage ="Numri i vendeve nuk mund te jete bosh")]
    [Range(1, int.MaxValue, ErrorMessage = "Vlera duhet te jete me e madhe se 1")]
    public int Seats {get;set;}

    public int? Total {get;set;}
    public int? UnitId { get; set; }

    public int? UserId { get; set; }

    public Unit? Unit { get; set; }
    public UserReg? User { get; set; }
}


public class FutureDateAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is DateTime date)
        {
            return date > DateTime.Now;
        }
        return false;
    }
}



