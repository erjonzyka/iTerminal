using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
#pragma warning disable CS8618

namespace iTerminal.Models;

public class Association
{
    [Key]
    public int AssociationId { get; set; }

    public int? UnitId { get; set; }

    public int? RouteId { get; set; }

    public Unit? unit { get; set; }
    public Linja? route { get; set; }
}






