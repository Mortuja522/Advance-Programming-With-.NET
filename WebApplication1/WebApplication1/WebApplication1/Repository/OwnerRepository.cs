using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class OwnerRepository
    {
        static HomeRentEntities db;

        static OwnerRepository()
        {
            db = new HomeRentEntities();
        }

        public static void Create(Flat f) { }

        public static Flat GetFlatById(int id)
        {
            var flat = (from f in db.Flats
                        where f.FlatId == id
                        select f).FirstOrDefault();
            return flat;
        }

        public static void DeleteFlat(int id)
        {
            Flat flat = (from f in db.Flats
                        where f.FlatId == id
                        select f).FirstOrDefault();
            db.Flats.Remove(flat);
            db.SaveChanges();
        }
        public static User GetUserType(int id)
        {
            var u = (from us in db.Users
                     where us.UserId == id
                     select us).FirstOrDefault();
            return u;
        }
    }
}