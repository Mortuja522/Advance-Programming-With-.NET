using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private object formsAuthentication;

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(User u)
        {
            if (ModelState.IsValid)
            {
                HomeRentEntities db = new HomeRentEntities();
                u.active = 1;
                u.Image = "null";
                u.Phone = 0;
                u.Address = "null";
                db.Users.Add(u);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View();


        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(User ur)
        {


            var u = UserRepository.Authenction(ur.Email, ur.Password);

            if (u != null)
            {
                if (u.active == 1)
                {
                    FormsAuthentication.SetAuthCookie(u.UserId.ToString(), true);
                    Session["password"] = u.Password;
                    Session["Username"] = u.Username;
                    Session["id"] = u.UserId;

                    if (u.Type == 1)
                    {
                        return RedirectToAction("AdminDashboard", "Admin");
                    }
                    else if (u.Type == 2)
                    {
                        return RedirectToAction("Dashboard","Owner");
                    }
                    else if (u.Type == 3)
                    {
                        return RedirectToAction("Dashboard", "Customer");
                    }
                }
                ViewBag.Message = "Your account is block!";
                return View();
            }
            ViewBag.Message = "Invalid User Email or password";
            return View();

        }


    }

}
