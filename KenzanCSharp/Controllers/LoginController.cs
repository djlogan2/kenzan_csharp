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
    [System.Web.Http.AllowAnonymous]
    public class LoginController : ApiController
    {
        // POST rest/login
        public LoginResponse Post([FromBody]Login login)
        {
            kenzanEntities ke = new kenzanEntities();
            Employee employee = ke.Employees
                .Include("EmployeeRoles")
                .Where<Employee>(e => e.username == login.username && e.bStatus == Status.ACTIVE)
                .FirstOrDefault<Employee>();

            if (employee != null && employee.password != null && crypto.BCrypt.Verify(login.password, employee.password))
            {
                JWTToken token = new JWTToken(employee);
                return new LoginResponse() { jwt = token.token };
            } else
                return new LoginResponse() { errorcode = ErrorNumber.INVALID_USERNAME_OR_PASSWORD, error = "Login failed", jwt = null };
        }
    }
}