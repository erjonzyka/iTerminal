using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
#pragma warning disable CS8618

namespace iTerminal.Models;

public class Trip
{
    [Key]
    public int TripId { get; set; }
    [FutureDate(ErrorMessage = "Koha nuk mund te jete ne te pasmen")]
    public DateTime Start {get;set;}
    [Required(ErrorMessage ="Numri i vendeve nuk mund te jete bosh")]
    [Range(1, int.MaxValue, ErrorMessage = "Vlera duhet te jete me e madhe se 1")]
    public int Seats {get;set;}
    public int? RouteId { get; set; }

    public int? UserId { get; set; }

    public Route? Route { get; set; }
    public UserReg? Company { get; set; }
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



