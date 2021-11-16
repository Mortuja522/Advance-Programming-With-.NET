using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Repository;

namespace WebApplication1.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomerAccess : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var flag = base.AuthorizeCore(httpContext);
            if (flag)
            {
                var u = CustomerRepository.GetUserType(Int32.Parse(httpContext.User.Identity.Name));
                if (u.Type == 3)
                {
                    return true;
                }
            }
            return false;
        }
    }
}