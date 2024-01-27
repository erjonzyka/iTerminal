using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
#pragma warning disable CS8618

namespace iTerminal.Models;

public class Linja
{
    [Key]
    public int RouteId { get; set; }
    [Required(ErrorMessage ="Vendi i nisjes nuk mund te jete bosh.")]
    public string PointA {get;set;}

    [Required(ErrorMessage ="Destinacioni nuk mund te jete bosh")]
    public string PointB {get;set;}

    public List<Association>? AllAssociations {get;set;}
}






