using KenzanCSharp.App_Start;
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
    [RESTAuthorize]
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

                if (emp == null) return new ErrorResponse(ErrorNumber.INVALID_USERNAME_OR_PASSWORD, "No user found");

                emp.password = crypto.BCrypt.HashPassword(login.password);

                if (ke.SaveChanges() != 1)
                    return new ErrorResponse(ErrorNumber.UNKNOWN_ERROR, "Unable to save password");
                else
                    return new ErrorResponse();
            }
            else
                return new ErrorResponse(ErrorNumber.NOT_AUTHORIZED_FOR_OPERATION, "Not authorized");
        }
    }
}