using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LoginReg.Models;
using Microsoft.AspNetCore.Identity;

namespace LoginReg.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context{get;set;}
        public HomeController(MyContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Create")]
    public IActionResult Create(User user)
    {
        if(ModelState.IsValid)
        {
            // If a User exists with provided email
            if(_context.Users.Any(u => u.Email == user.Email))
            {
                ModelState.AddModelError("Email", "Email already in use!");
                return View("Index");
            }
            else
            {
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            user.Password = Hasher.HashPassword(user, user.Password);
            _context.Users.Add(user);
            _context.SaveChanges();
            Console.WriteLine("hello");
            return RedirectToAction("Login");
            }
        }
        return View("Index");
    } 
    [HttpGet("Login")]
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost("Logged")]
    public IActionResult Logged(LoginUser userSubmission)
    {
        if(ModelState.IsValid)
        {
            // If inital ModelState is valid, query for a user with provided email
            var userInDb = _context.Users.FirstOrDefault(u => u.Email == userSubmission.Email);
            // If no user exists with provided email
            if(userInDb == null)
            {
                // Add an error to ModelState and return to View!
                ModelState.AddModelError("Email", "Invalid Email/Password");
                return View("Login");
            }
            
            
            var hasher = new PasswordHasher<LoginUser>();
            
            // verify provided password against hash stored in db
            var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);
            
            // result can be compared to 0 for failure
            if(result == 0)
            {
            ModelState.AddModelError("Password", "Invalid Email/Password");
            return View("Login");
            }
            else
            {
                return View("Success");
            }
        }
        return View("Login");
    }
}
}
