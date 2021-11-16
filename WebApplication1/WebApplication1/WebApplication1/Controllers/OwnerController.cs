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
    [OwnerAccess]
    public class OwnerController : Controller
    {
        // GET: Owner
        public ActionResult Dashboard()
        {
            return View();
        }


        [HttpGet]
        public ActionResult AddFlat()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddFlat(Flat f)
        {
            if (ModelState.IsValid)
            {
                int id = Int32.Parse(Session["id"].ToString());
                string fileName = Path.GetFileNameWithoutExtension(f.ImageFile.FileName);
                string extension = Path.GetExtension(f.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                f.FlatImage = "~/Image/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                f.ImageFile.SaveAs(fileName);

                HomeRentEntities db = new HomeRentEntities();
                f.UserId = Int32.Parse(Session["id"].ToString());
                db.Flats.Add(f);
                db.SaveChanges();

                return RedirectToAction("FlatList");
            }
            return View();
        }
        public ActionResult FlatList()
        {
            int id = Int32.Parse(Session["id"].ToString());

            HomeRentEntities db = new HomeRentEntities();
            var flat = (from f in db.Flats
                        where f.UserId == id
                        select f);

            return View(flat.ToList());

        }

        public ActionResult ViewDetails(int id)
        {
            var flat = OwnerRepository.GetFlatById(id);
            return View(flat);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var flat = OwnerRepository.GetFlatById(id);
            return View(flat);
        }

        [HttpPost]
        public ActionResult Edit(Flat f, int id)
        {
            if (ModelState.IsValid)
            {
                HomeRentEntities db = new HomeRentEntities();
                var flat = (from fs in db.Flats
                            where fs.FlatId == id
                            select fs).First();

                flat.FlatSize = f.FlatSize;
                flat.FlatDetails = f.FlatDetails;
                flat.Rent = f.Rent;
                flat.Location = f.Location;

                db.SaveChanges();
                return RedirectToAction("FlatList");
            }

            return View();
        }

        public ActionResult Delete(int id)
        {
            OwnerRepository.DeleteFlat(id);
            return RedirectToAction("FlatList");

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
                return RedirectToAction("EditProfile");
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
    }
}