using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using iTerminal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
namespace iTerminal.Controllers;

public class LoginController : Controller
{    
    private readonly ILogger<LoginController> _logger;
    // Add a private variable of type MyContext (or whatever you named your context file)
    private MyContext _context;         
    // Here we can "inject" our context service into the constructor 
    // The "logger" was something that was already in our code, we're just adding around it   
    public LoginController(ILogger<LoginController> logger, MyContext context)    
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

    [HttpGet("company/login")]
    public IActionResult CompanyLogin(){
        return View();
    }




   [HttpPost("login")]
    public IActionResult Login(UserLogin user){
        if(ModelState.IsValid){
            UserReg? CurrentUser = _context.Users.FirstOrDefault(e => e.Email == user.LEmail);
            if(CurrentUser == null){
                ModelState.AddModelError("LEmail", "Invalid Username/Password");
                return View("Index");
            }
            PasswordHasher<UserLogin> hasher = new PasswordHasher<UserLogin> ();
            var result = hasher.VerifyHashedPassword(user, CurrentUser.Password, user.LPassword);
            if(result == 0){
                ModelState.AddModelError("LPassword", "Password invalid");
                return View("Index");
            }
            if(CurrentUser.Role == 1){
                HttpContext.Session.SetInt32("AdminId", CurrentUser.id);
                HttpContext.Session.SetInt32("UserId", CurrentUser.id);
                HttpContext.Session.SetString("UserName", CurrentUser.FirstName);
                return  RedirectToAction("Index", "Admin");
            }
            HttpContext.Session.SetInt32("UserId", CurrentUser.id);
            HttpContext.Session.SetString("UserName", CurrentUser.FirstName);

            return  RedirectToAction("Index", "Home");
        }
        else{
            return View("Index");
        }
    }

    [HttpPost("company/login")]
    public IActionResult CompanyLogin(CompanyLogin company){
        if(ModelState.IsValid){
            Company? CurrentCompany = _context.Companies.FirstOrDefault(e => e.Email == company.LEmail);
            if(CurrentCompany == null){
                ModelState.AddModelError("LEmail", "Invalid Email/Password");
                return View("CompanyLogin");
            }
            PasswordHasher<CompanyLogin> hasher = new PasswordHasher<CompanyLogin> ();
            var result = hasher.VerifyHashedPassword(company, CurrentCompany.Password, company.LPassword);
            if(result == 0){
                ModelState.AddModelError("LPassword", "Password invalid");
                return View("CompanyLogin");
            }
            HttpContext.Session.SetInt32("UserId", CurrentCompany.CompanyId);
            HttpContext.Session.SetInt32("AdminId", CurrentCompany.CompanyId);
            HttpContext.Session.SetString("UserName", CurrentCompany.Name);

            return  RedirectToAction("Index", "Admin");
        }
        else{
            return View("CompanyLogin");
        }
    }





    [HttpGet("reg")]
    public IActionResult ClientRegister(){
        return View();
    }

        [HttpPost("register")]
    public IActionResult Register(UserReg user){
        if(ModelState.IsValid){
            PasswordHasher<UserReg> Hasher = new PasswordHasher<UserReg>();
            user.Password = Hasher.HashPassword(user, user.Password);  
            _context.Add(user);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("UserId", user.id);
            HttpContext.Session.SetInt32("CartNo", 0);
            HttpContext.Session.SetString("UserName", user.FirstName);
            return RedirectToAction("Index", "Home");
        }
        else{
            return View("ClientRegister");
        }
    }

    [HttpGet("company/reg")]
    public IActionResult CompReg(){
        return View();
    }


    [HttpPost("company/register")]
    public IActionResult CompanyRegister(Company company){
        if(ModelState.IsValid){
            PasswordHasher<Company> Hasher = new PasswordHasher<Company>();
            company.Password = Hasher.HashPassword(company, company.Password);  
            _context.Add(company);
            _context.SaveChanges();
            HttpContext.Session.SetInt32("AdminId", company.CompanyId);
            HttpContext.Session.SetInt32("UserId", company.CompanyId);
            HttpContext.Session.SetString("Name", company.Name);
            return RedirectToAction("Index", "Admin");
        }
        else{
            return View("CompReg");
        }
    }

    [SessionCheck]
    [HttpGet("logout")]
    public IActionResult LogOut()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    
}