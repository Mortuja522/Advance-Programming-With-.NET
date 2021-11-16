using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class CustomerRepository
    {
        static HomeRentEntities db;

        static CustomerRepository()
        {
            db = new HomeRentEntities();
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