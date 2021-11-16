using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Auth;
using WebApplication1.Classes;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
    [CustomerAccess]
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Dashboard()
        {
            int id = Int32.Parse(Session["id"].ToString());

            using (HomeRentEntities db = new HomeRentEntities())
            {
                var booking = (from b in db.Bookings
                               where b.UserId == id
                               select b);
                return View(booking.ToList());
            }
        }

        public ActionResult HouseList()
        {
            HomeRentEntities db = new HomeRentEntities();
            var flat = (from f in db.Flats select f);
            return View(flat.ToList());
        }

        public ActionResult bookingDetails(int id)
        {
            using (HomeRentEntities db = new HomeRentEntities())
            {
                var flat = (from f in db.Flats
                            where f.FlatId == id
                            select f).FirstOrDefault();

                var user = (from f in db.Flats
                            where f.FlatId == id
                            select f).FirstOrDefault();

                return View(flat);

            }
        }

        public ActionResult FlatDetails(int id)
        {
            using (HomeRentEntities db = new HomeRentEntities())
            {
                var flat = (from f in db.Flats
                            where f.FlatId == id
                            select f).FirstOrDefault();
                return View(flat);

            }
        }
        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string Password, string New_Password, string con_Password)
        {
            int id = Int32.Parse(Session["id"].ToString());
            if (ModelState.IsValid)
            {
                if (Session["password"].ToString() != Password)
                {
                    ViewBag.connPass = "Current Password Doesn't match!";
                    return View();
                }
                else if (New_Password != con_Password)
                {
                    ViewBag.newpass = "New password and Confirm password Doesn't match!";
                    return View();
                }
                else
                {
                    using (HomeRentEntities db = new HomeRentEntities())
                    {
                        var pass = (from pa in db.Users
                                    where pa.UserId == id
                                    select pa).FirstOrDefault();
                        pass.Password = New_Password;
                        db.SaveChanges();
                        Session["password"] = New_Password;
                        ViewBag.Message = "Password change Successfully";
                        return View();
                    }
                }
            }
            return View();
        }


        [HttpGet]
        public ActionResult EditProfile()
        {
            int id = Int32.Parse(Session["id"].ToString());

            HomeRentEntities db = new HomeRentEntities();
            var user = (from us in db.Users
                        where us.UserId == id
                        select us).First();
            return View(user);
        }

        [HttpPost]
        public ActionResult EditProfile(UserClass u)
        {
            int id = Int32.Parse(Session["id"].ToString());

            if (ModelState.IsValid)
            {
                var user = UserRepository.EditProfile(u, id);

                ViewBag.success = "profile Edit Successfullt";
                return RedirectToAction("EditProfile"); ;
            }
            return View();

        }

        public ActionResult ProfilePicture(User u)
        {
            int id = Int32.Parse(Session["id"].ToString());
            string fileName = Path.GetFileNameWithoutExtension(u.ImageFile.FileName);
            string extension = Path.GetExtension(u.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            u.Image = "~/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            u.ImageFile.SaveAs(fileName);

            using (HomeRentEntities db = new HomeRentEntities())
            {
                var us = (from ur in db.Users
                          where ur.UserId == id
                          select ur).FirstOrDefault();
                us.Image = u.Image;
                db.SaveChanges();
            }
            ModelState.Clear();
            return RedirectToAction("EditProfile");
        }

        public ActionResult Booking(int id)
        {
            int userId = Int32.Parse(Session["id"].ToString());

            using (HomeRentEntities db = new HomeRentEntities())
            {
                var booking = (from bk in db.Bookings
                               where bk.FlatId == id && bk.UserId == userId
                               select bk).FirstOrDefault();
                if (booking == null)
                {
                    var flat = (from f in db.Flats
                                where f.FlatId == id
                                select f).FirstOrDefault();

                    Booking b = new Booking();
                    b.UserId = userId;
                    b.FlatId = id;
                    b.Status = "Processing";
                    b.Amount = flat.Rent;

                    db.Bookings.Add(b);
                    db.SaveChanges();
                    return RedirectToAction("Dashboard");
                }

                Session["booked"] = "You already booked this flat!";
                return RedirectToAction("HouseList");

            }

            
        }
    }
}