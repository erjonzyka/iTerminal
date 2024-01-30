#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace iTerminal.Models;

public class PaginatedProductViewModel
{
    public List<Unit>? Units { get; set; }
    public List<UserReg>? Users {get;set;}
    public int? PageNumber { get; set; }
    public int? TotalPages { get; set; }

    public UserReg? User {get; set;}
    public Unit? Unit {get; set; }
    public Trip? Trip {get;set;}
}