using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
#pragma warning disable CS8618

namespace iTerminal.Models;

public class Message
{
    [Key]
    public int MessageId { get; set; }
    [Required]
    public string Content {get;set;}
    public bool Seen {get;set;} = false;
    public int? CompanyId { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreatedAt {get;set;} = DateTime.Now;

    public Company? Company { get; set; }
    public UserReg? User { get; set; }
}






