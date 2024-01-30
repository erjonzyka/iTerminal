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
            List<Unit> AllUnits = _context.Units.Include(e=> e.route).Include(e=> e.Creator).Where(e=> e.route.PointA.ToLower() == searched.PointA.ToLower() && e.route.PointB.ToLower() == searched.PointB.ToLower() && e.Nisja > DateTime.Now && e.Seats > 0).ToList();
            return View("Shfaq", AllUnits);
        }
        return View("Index");
    }

    [HttpGet("present")]
    public IActionResult Shfaq(List<Unit> AllUnits)
    {
        return View(AllUnits);
    }

    [SessionCheck]
    [HttpGet("rezervo/{id}")]
    public IActionResult Rezervo(int id){
        Unit? requestedUnit = _context.Units.Include(e=>e.Creator).FirstOrDefault(e=> e.UnitId == id);
        UserReg? loggedUser = _context.Users.FirstOrDefault(e=> e.id == HttpContext.Session.GetInt32("UserId"));
        PaginatedProductViewModel data = new PaginatedProductViewModel ();
        data.Unit = requestedUnit;
        data.User = loggedUser;
        return View(data);
    }

    [SessionCheck]
    [HttpPost("confirm")]
    public IActionResult Confirm(Trip trip){
        Unit? unitToUpdate = _context.Units.FirstOrDefault(e=> e.UnitId == trip.UnitId);
        unitToUpdate.Seats -= trip.Seats;
        trip.Total = trip.Seats * unitToUpdate.Price;
        _context.Add(trip);
        _context.SaveChanges();
        return RedirectToAction("UpcomingTrips");
    }

    [SessionCheck]
    [HttpGet("upcoming")]
    public IActionResult UpcomingTrips(){
        List<Trip> upcomingTrips = _context.Trips.Include(e=> e.Unit).ThenInclude(e=> e.route).Include(e=> e.Unit.Creator).Include(e=> e.User).Where(e=> e.UserId == HttpContext.Session.GetInt32("UserId") && e.Unit.Nisja > DateTime.Now).OrderByDescending(e=> e.Unit.Nisja).ToList();
        return View(upcomingTrips);
    }

    [SessionCheck]
    [HttpGet("anullo/{id}")]
    public IActionResult Anullo(int id){
        Trip? tripToRemove = _context.Trips.FirstOrDefault(e=> e.TripId == id);
        Unit? unitToUpdate = _context.Units.FirstOrDefault(e=> e.UnitId == tripToRemove.UnitId);
        unitToUpdate.Seats+= tripToRemove.Seats;
        _context.Remove(tripToRemove);
        _context.SaveChanges();
        return RedirectToAction ("UpcomingTrips");
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
