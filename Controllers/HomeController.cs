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
    public HomeController(EventPlannerContext context) { dbContext = context; }

    [HttpGet("")]
    public IActionResult Index()
    {
      return View();
    }

    [HttpPost("userlogin")]
    public IActionResult UserLogin(LoginUser userSubmission)
    {
      if (ModelState.IsValid)
      {
        var userInDb = dbContext.Users
            .FirstOrDefault(u => u.Email == userSubmission.Email);

        if (userInDb is null)
        {
          ModelState.AddModelError("Email", "Email not found");
          return View("Index");
        }
        var pwhasher = new PasswordHasher<LoginUser>();
        if (pwhasher.VerifyHashedPassword( 
          userSubmission, 
          userInDb.Password, 
          userSubmission.Password
          ) == PasswordVerificationResult.Success)
        {
          HttpContext.Session.SetInt32("userId", userInDb.UserId);
          HttpContext.Session.SetString("userName", userInDb.FirstName);
          return RedirectToAction("WeddingList", "Home");
        }
        else
        {
          ModelState.AddModelError("Password", "Email or Password combination is not valid");
          return View("Index");
        }
      }
      return View("Index");
    }

    [HttpPost("register")]
    public IActionResult Register(User newUser)
    {
      if (ModelState.IsValid)
      {
        if (dbContext.Users.Any(user => user.Email == newUser.Email))
        {
          ModelState.AddModelError("Email", "Email already in use!");
          return View("Index");
        }
        dbContext.createUser(newUser, HttpContext);
        return RedirectToAction("WeddingList");
      }
      return View("Index");
    }

    [HttpGet("weddingview")]
    public IActionResult WeddingList()
    {
      int? id = HttpContext.Session.GetInt32("userId");
      User user = dbContext.Users.FirstOrDefault(u => u.UserId == id);
      List<Wedding> allWeddings = dbContext.Weddings
          .Include(x => x.Wedders).ToList();
      return View(new Dashboard(allWeddings, user));
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
      dbContext.SaveChanges();
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
