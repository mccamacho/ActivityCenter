using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DojoActivity.Models;

namespace DojoActivity.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;
        public HomeController(MyContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        

        [HttpPost]
        [Route("register")]
        public IActionResult Register(User model)
        {
            User CheckUserEmail = _context.Users.SingleOrDefault(User=>User.Email == model.Email);
            if(CheckUserEmail !=null)
            {
                ViewBag.Err = "Email has already been registered";
            }
             else if(ModelState.IsValid){
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                User NewUser = new User();
                NewUser.FirstName = model.FirstName;
                NewUser.LastName = model.LastName;
                NewUser.Email = model.Email;
                NewUser.Password= Hasher.HashPassword(NewUser ,model.Password);
                NewUser.CreatedAt = DateTime.Now;
                NewUser.UpdatedAt = DateTime.Now;
                _context.Users.Add(NewUser);
                _context.SaveChanges();
                User LoggedUser = _context.Users.SingleOrDefault(user=>user.Email == model.Email);
                HttpContext.Session.SetInt32("UserId", LoggedUser.UserID);
                return RedirectToAction("Dashboard");
             }
            return View("Index");
        }
        [HttpPost]
        [Route("login")]
        public IActionResult LogIn(string email, string Password)
        {
            User CheckUser = _context.Users.SingleOrDefault(User=>User.Email == email);
            if(CheckUser !=null)
            {
                var Hasher = new PasswordHasher<User>();
                if(0 != Hasher.VerifyHashedPassword(CheckUser,CheckUser.Password,Password))
                {
                    HttpContext.Session.SetInt32("UserId", CheckUser.UserID);
                    return RedirectToAction("Dashboard");
                    };
                }
                ViewBag.Err= "Email and/or Password are incorect";
                return View("Index");

        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            int? userid = HttpContext.Session.GetInt32("UserId");
            User userInDb = _context.Users.FirstOrDefault(u => u.UserID == userid);
            Console.WriteLine(userid);
            if (userid == null)
            {
                return Redirect("/");
            }
            List<Activity> Activities=_context.Activities.Include(c => c.JoiningUser).OrderBy(x => x.Date).ToList();
            
            ViewBag.Activities = Activities;
            ViewBag.User = userInDb;
            
            
            return View("Dashboard");

        }

        [Route("AddActivity")]
        public IActionResult AddActivity()
        {
            
            return View();
        }

        
        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

        [HttpPost]
        [Route("CreateActivity")]
        public IActionResult CreateActivity(Activity newActivity)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                
                return RedirectToAction("Index","Home");
            }
            User userInDb = _context.Users.FirstOrDefault(u => u.UserID == HttpContext.Session.GetInt32("UserId"));

            int userSession = HttpContext.Session.GetInt32("UserId")?? default(int);
            if(newActivity.Date<DateTime.Now){
                ViewBag.Err= "Event has to be in the future!";
                return View("AddActivity");
            }
            if(ModelState.IsValid)
            {
                Activity activity = new Activity
            {
                Title = newActivity.Title,
                Description = newActivity.Description,
                Date = newActivity.Date,
                Time = newActivity.Time,
                Duration = newActivity.Duration,
                Coordinator = userSession,
                CoordinatorName = userInDb.FirstName
                
            }; 
            _context.Activities.Add(activity);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
            }
            return View("AddActivity");
        }

        [HttpGet]
        [Route("delete/{ActivityId}")]
        public IActionResult Delete(int ActivityId)
        {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return Redirect("/");
            }
            // delete wedding by id
            Activity a = _context.Activities
                            .Where(w => w.ActivityId == ActivityId).SingleOrDefault();
            _context.Activities.Remove(a);
            _context.SaveChanges();
            return Redirect("/dashboard");
        }

        [HttpGet("join/{ActivityId}")]
        public IActionResult Join(int ActivityId)
        {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return Redirect("/");
            }
            int? userid = HttpContext.Session.GetInt32("UserId");
            User userInDb = _context.Users.FirstOrDefault(u => u.UserID == userid);
            UserActivity n = new UserActivity();
            n.ActivityId = ActivityId;
            n.UserId  = (int)userid;
            _context.UserActivities.Add(n);
            _context.SaveChanges();
            return Redirect("/dashboard");

            
        }

        [HttpGet("unjoin/{ActivityId}")]
        public IActionResult Unjoin(int ActivityId)
        {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return Redirect("/");
            }
            int? userid = HttpContext.Session.GetInt32("UserId");
            var x = _context.UserActivities.Where(u => u.UserId == userid).FirstOrDefault(w => w.ActivityId == ActivityId);
            _context.UserActivities.Remove(x);
            _context.SaveChanges();
            return Redirect("/dashboard");

        }

        [HttpGet("activity/{ActivityId}")]
        public IActionResult View(int ActivityId)
        {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return Redirect("/");
            }
            Activity a = _context.Activities.FirstOrDefault(u => u.ActivityId == ActivityId);
            ViewBag.Title = a.Title;
            ViewBag.CoordinatorName = a.CoordinatorName;
            ViewBag.Description = a.Description;
            

            return View("View");


        }

    
        
    }
}
