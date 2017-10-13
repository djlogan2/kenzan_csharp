using KenzanCSharp.Content;
using KenzanCSharp.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using crypto = BCrypt.Net;

namespace KenzanCSharp.Controllers
{
    [System.Web.Http.Authorize]
    public class Set_PasswordController : ApiController
    {
        // POST rest/login
        public ErrorResponse Post([FromBody]Login login)
        {
            if (User.Identity.Name == login.username || User.IsInRole("ROLE_SET_PASSWORD"))
            {
                kenzanEntities ke = new kenzanEntities();
                Employee emp = ke.Employees
                    .Where<Employee>(e => e.username == login.username && e.bStatus == Status.ACTIVE)
                    .FirstOrDefault<Employee>();

                if (emp == null) return new ErrorResponse() { error = "No user found" };

                emp.password = crypto.BCrypt.HashPassword(login.password);

                if (ke.SaveChanges() != 1)
                    return new ErrorResponse() { error = "Unable to set new password" };
                else
                    return new ErrorResponse() { error = "ok" };
            }
            else
                return new ErrorResponse() { error = "Not authorized to set password" };
        }
    }
}