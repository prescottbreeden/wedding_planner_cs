using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using EventPlanner.Models;

namespace EventPlanner.Controllers
{
    public class HomeController : Controller
    {
        private EventPlannerContext dbContext;
     
        public HomeController(EventPlannerContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("userlogin")]
        public IActionResult UserLogin(LoginUser userSubmission)
        {
            if(ModelState.IsValid)
            {
                // If inital ModelState is valid, query for a user with provided email
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == userSubmission.Email);
                // If no user exists with provided email
                if(userInDb == null)
                {
                    // Add an error to ModelState and return to View!
                    ModelState.AddModelError("Email", "Email not found");
                    return View("Index");
                }
                else
                {
                    // Initialize hasher object
                    var pwhasher = new PasswordHasher<LoginUser>();
                    if (userSubmission.Password == null)
                    {
                        ModelState.AddModelError("Password", "Password not found");
                        return View("Index");
                    }
                    else
                    {
                        // var pwhasher = new PasswordHasher<LoginUser>();
                        if (pwhasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password) == 
                            PasswordVerificationResult.Success)
                            {
                                HttpContext.Session.SetInt32("userId", userInDb.UserId);
                                HttpContext.Session.SetString("userName", userInDb.FirstName);
                                    return RedirectToAction("WeddingList");
                            }
                        else
                        {
                            ModelState.AddModelError("Password", "Email or Password combination is not valid");
                            return View("Index");
                        }
                    }
                }
            }
            HttpContext.Session.Clear();
            return View("Index");
        }

        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            // Check initial ModelState
            if(ModelState.IsValid)
            {
                // If a User exists with provided email
                if(dbContext.Users.Any(user => user.Email == newUser.Email))
                {
                    // Manually add a ModelState error to the Email field, with provided
                    // error message
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                    // You may consider returning to the View at this point
                }
                // Initializing a PasswordHasher object, providing our User class as its
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                //Save your user object to the databas

                HttpContext.Session.SetInt32("userId", newUser.UserId);
                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();
                return RedirectToAction("WeddingList");
            }
            return View("Index");
        }

        [HttpGet("weddingview")]
        public IActionResult WeddingList()
        {
            List<Wedding> allWeddings = dbContext.Weddings
                .Include(x => x.Wedders).ToList();
            return View(allWeddings);
        }


        [HttpGet("{weddingId}")]
        public IActionResult OneWedding(int weddingId)
        {
            Wedding wedding = dbContext.Weddings
                .Include(w => w.Wedders)
                .ThenInclude(a => a.Guest)
                .FirstOrDefault(w => w.WeddingId == weddingId);
            return View(wedding);
        }

        [HttpGet("newwedding")]
        public IActionResult AddWedding()
        {
            return View("AddWedding");
        }

        [HttpPost("createwedding")]
        public IActionResult AddWedding(Wedding newWedding)
        {
            dbContext.Add(newWedding);
            // OR dbContext.Users.Add(newUser);
            dbContext.SaveChanges();
            // Other code
            return RedirectToAction("WeddingList");
        }


        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

    }
}
