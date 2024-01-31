using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using iTerminal.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
namespace iTerminal.Controllers;

public class AdminController : Controller
{    
    private readonly ILogger<AdminController> _logger;
    // Add a private variable of type MyContext (or whatever you named your context file)
    private MyContext _context;         
    // Here we can "inject" our context service into the constructor 
    // The "logger" was something that was already in our code, we're just adding around it   
    private readonly IWebHostEnvironment _environment;
    // Here we can "inject" our context service into the constructor 
    // The "logger" was something that was already in our code, we're just adding around it   
    public AdminController(ILogger<AdminController> logger, MyContext context, IWebHostEnvironment environment)
    {
        _logger = logger;
        // When our HomeController is instantiated, it will fill in _context with context
        // Remember that when context is initialized, it brings in everything we need from DbContext
        // which comes from Entity Framework Core
        _context = context;
        _environment = environment;

    }

    [SessionCheck]
[AdminCheck]
public IActionResult Index()
{
    return View();   
}

[AdminCheck]
[HttpGet("createunit")]
public IActionResult CreateUnit(){
    DataTwo DataTwo = new DataTwo();
    DataTwo.Routes= _context.Routes.ToList();
    return View(DataTwo);
    
}


[AdminCheck]
    [HttpPost("registerunit")]
    public async Task<IActionResult> RegisterUnit(Unit unit)
{
    if (ModelState.IsValid)
    {
        if (unit.ImageFile != null && unit.ImageFile.Length > 0)
        {
            Console.WriteLine("U ekzekutua");
            // Process the uploaded file
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + unit.ImageFile.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await unit.ImageFile.CopyToAsync(fileStream);
            }

            // Update the model properties with the file details
            unit.ImageFileName = uniqueFileName;
            unit.ImageData = System.IO.File.ReadAllBytes(filePath);
        }
            _context.Units.Add(unit);
            _context.SaveChanges();
            Association association = new Association();
             association.UnitId = unit.UnitId;
            association.RouteId = unit.RouteId;
            _context.Associations.Add(association);
            _context.SaveChanges();
            return RedirectToAction("Index");
           }
   
        DataTwo DataTwo = new DataTwo();
        DataTwo.Routes= _context.Routes.ToList();
        return View("CreateUnit", DataTwo);

}


    [AdminCheck]
    [HttpPost("registerroute")]
    public IActionResult RegisterRoute(Linja route)
    {
        if (ModelState.IsValid)
        {
            if(_context.Routes.Any(e=> e.PointA == route.PointA && e.PointB == route.PointB) || route.PointA == route.PointB){
                ModelState.AddModelError("PointA", "Vlerat nuk jane te vlefshme");
                DataTwo data = new DataTwo();
                 data.Routes= _context.Routes.ToList();
                return View("CreateUnit", data);
            }
            _context.Add(route);
            _context.SaveChanges();
            return RedirectToAction("CreateUnit");
        }
        DataTwo DataTwo = new DataTwo();
        DataTwo.Routes= _context.Routes.ToList();
        return View("CreateUnit", DataTwo);
    }

    [AdminCheck]
    [HttpGet("myunitspast")]
    public IActionResult MyUnitsPast(){

        return RedirectToAction("MyUnits", new { nisja = 1 });
    }

    [AdminCheck]
    [HttpGet("myunits")]
    public IActionResult MyUnits(int page = 1, int nisja = 0)
{
    int pageSize = 10; // Number of items to display per page

    IQueryable<Unit> unitsQuery = _context.Units.AsQueryable();;

    // Retrieve the products with associated categories
    switch(nisja){
        case 0 :  unitsQuery = _context.Units.Include(e => e.Creator).Include(e=> e.route).Where(e=> e.CreatorId == HttpContext.Session.GetInt32("AdminId") && e.Nisja > DateTime.Now);;
        break;
        case  1:
         unitsQuery = _context.Units.Include(e => e.Creator).Include(e=> e.route).Where(e=> e.CreatorId == HttpContext.Session.GetInt32("AdminId") && e.Nisja < DateTime.Now);;
        break;
    }
   

    

    // Pagination
    var totalProducts = unitsQuery.Count();
    var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

    // Ensure the requested page is within the valid range
    page = Math.Max(1, Math.Min(page, totalPages));

    // Apply pagination to the query
    var units = unitsQuery.Skip((page - 1) * pageSize).Take(pageSize).ToList();

    // Pass the paginated and sorted data to the view
    var viewModel = new PaginatedProductViewModel
    {
        Units = units,
        PageNumber = page,
        TotalPages = totalPages,
    };

    return View(viewModel);
}


}





public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        int? userId = context.HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            context.Result = new RedirectToActionResult("Index", "Login", null);
        }
    }
}

public class AdminCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        int? userId = context.HttpContext.Session.GetInt32("AdminId");
        if (userId == null)
        {
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}













