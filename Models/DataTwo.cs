#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace iTerminal.Models;

public class DataTwo{
    public Unit? Unit {get;set;}

    public List<Linja>? Routes {get;set;}

    public Linja? Route {get;set;}

    public Association? Association {get;set;}


    
}