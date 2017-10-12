using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace KenzanCSharp.Controllers
{
    [System.Web.Http.Authorize(Roles = "ROLE_ADD_EMP")]
    public class Add_EmpController : ApiController
    {
        // POST: rest/add_emp
        public ErrorResponse Post([FromBody] Employee employee)
        {
            kenzanEntities ke = new kenzanEntities();
            ke.Employees.Add(employee);
            int updated = 0;
            ErrorResponse err = new ErrorResponse();

            try
            {
                updated = ke.SaveChanges();
                if (updated == 0)
                    err.error = "No records updated";
                else
                {
                    err.id = employee.id;
                    err.error = "ok";
                }
            } catch(Exception e)
            {
                updated = 0;
                err.error = "Add exception: " + e.Message;
            }

            return err;
        }
    }
}