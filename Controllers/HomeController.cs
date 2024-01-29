using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using iTerminal.Models;
using Microsoft.EntityFrameworkCore;

namespace iTerminal.Controllers;

public class HomeController : Controller
{    
    private readonly ILogger<HomeController> _logger;
    // Add a private variable of type MyContext (or whatever you named your context file)
    private MyContext _context;         
    // Here we can "inject" our context service into the constructor 
    // The "logger" was something that was already in our code, we're just adding around it   
    public HomeController(ILogger<HomeController> logger, MyContext context)    
    {        
        _logger = logger;
        // When our HomeController is instantiated, it will fill in _context with context
        // Remember that when context is initialized, it brings in everything we need from DbContext
        // which comes from Entity Framework Core
        _context = context;    
    }  

    public IActionResult Index()
    {
        return View();
    }


    [HttpPost("Show")]
    public IActionResult Show(Linja searched)
    {
        if(ModelState.IsValid){
            List<Unit> AllUnits = _context.Units.Include(e=> e.route).Include(e=> e.Creator).Where(e=> e.route.PointA == searched.PointA && e.route.PointB== searched.PointB).ToList();
            return RedirectToAction("Shfaq", AllUnits);
        }
        return View("Index");


    }

    [HttpGet("shop")]
    public IActionResult Shfaq(List<Linja>AllUnits)
    {
        return View(AllUnits);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
