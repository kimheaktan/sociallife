using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MeetUp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MeetUp.Controllers
{
    public class HomeController : Controller
    {
        private MUContext _db; 

        public HomeController(MUContext context) 
        { 
            _db = context; 
        }

        private int? _uid
        {
            get
            {
                return HttpContext.Session.GetInt32("UserId");
            }
        }
        private bool _isLoggedIn
        {
            get
            {
                int? uid = _uid;

                if (uid != null)
                {
                    User loggedInUser =
                        _db.Users.FirstOrDefault(u => u.UserId == uid);

                    HttpContext.Session
                        .SetString("FullName", loggedInUser.FullName());
                }
                return uid != null;
            }
        }
        public IActionResult Index()
        {
            int? _uid = HttpContext.Session.GetInt32("UserId");
            if(_uid != null)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            int? _uid = HttpContext.Session.GetInt32("UserId");
            if(_uid == null)
            {
                return RedirectToAction("Index", "Home");
            }

            User CurrUser = _db.Users.Include(u => u.Memberships).ThenInclude(m => m.Group).Include(u => u.Events).ThenInclude(r => r.GroupEvent).FirstOrDefault(u => u.UserId == _uid);

            List<RSVP> thisUserRSVPs = _db.RSVPs
            .Include(r => r.GroupEvent)
            .ThenInclude(ge => ge.Creator)
            .Include(r => r.GroupEvent)
            .ThenInclude(ge => ge.Participants)
            .ThenInclude(r => r.User)
            .Where(r  => r.UserId == _uid)
            .OrderBy(r => r.GroupEvent.GroupEventDate).ToList();
            
            List<Membership> groupsOfUser = _db.Memberships.Include(m => m.Group).ThenInclude(g => g.Creator).Include(m => m.Group).ThenInclude(g => g.Members)
            .Where(m => m.UserId == _uid).ToList();

            // List<Membership> AllMembers = _db.Memberships.ToList();

            DashView info = new DashView{
                User = CurrUser,
                RSVPsOfThisUser = thisUserRSVPs,
                UserGroups = groupsOfUser,
                // AllMemberships = AllMembers
            };

            return View(info);
        }
        
        [HttpGet("register")]
        public IActionResult _Register()
        {
            if(_isLoggedIn)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User newUser)
        {
            if(_isLoggedIn)
            {
                return RedirectToAction("Dashboard");
            }
            if(ModelState.IsValid)
            {
                bool isEmailTaken = _db.Users.Any(u => u.Email == newUser.Email);//Any() will return True if there's a match 
                if(isEmailTaken){// means if it's True
                    ModelState.AddModelError("Email", "Email is Taken");
                    return View("Index");
                }
                else{
                    PasswordHasher<User> hasher = new PasswordHasher<User>();
                        newUser.Password = hasher.HashPassword(newUser,newUser.Password);

                        _db.Users.Add(newUser);
                        _db.SaveChanges();

                        HttpContext.Session.SetInt32("UserId", newUser.UserId);

                        HttpContext.Session.SetString("UserName", newUser.FirstName);
                }
            }
            else
            {
                return View("_Register");
            }
            return RedirectToAction("Dashboard");
        }

        [HttpGet("login")]
        public IActionResult ViewLogin()
        {
            int? _uid = HttpContext.Session.GetInt32("UserId");
            if(_uid != null)
            {
                return RedirectToAction("Dashboard");
            }
            return View("Login");
        }

        [HttpPost("login/user")]
        public IActionResult Login(LoginUser loginUser)
        {
            if(_isLoggedIn)
            {
                return RedirectToAction("Dashboard");
            }
            if(ModelState.IsValid)
            {
                User dbUser = _db.Users.FirstOrDefault(u => u.Email == loginUser.LoginEmail);
                if(dbUser == null)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Credentials");
                }
                else
                {
                //instatiate a User and assigning loginUser's props to User props so that loginUser and dbUser will match during VerifyHashedPassword method
                    User loggingInUser = new User{
                        Email = loginUser.LoginEmail, Password = loginUser.LoginPassword
                    };
                    PasswordHasher<User> hasher = new PasswordHasher<User>();

                    PasswordVerificationResult result = hasher.VerifyHashedPassword(loggingInUser, dbUser.Password, loggingInUser.Password);

                    if(result == 0)//not matched password/failed
                    {
                        ModelState.AddModelError("LoginEmail", "Invalid Credentials,pw");
                    }
                    else{
                        HttpContext.Session.SetInt32("UserId", dbUser.UserId);
                        HttpContext.Session.SetString("UserName", dbUser.FirstName);

                    }
                }
            }
            if(!ModelState.IsValid) // or Modelstate.IsValid == flase
            {
                return View("Login");
            }

            return RedirectToAction("Dashboard");

        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
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
}
