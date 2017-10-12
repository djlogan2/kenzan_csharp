using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace KenzanCSharp.Controllers
{
    [System.Web.Http.Authorize(Roles ="ROLE_DELETE_EMP")]
    public class Delete_EmpController : ApiController
    {
        // GET: rest/delete_emp
        [System.Web.Http.HttpGet] public ErrorResponse Delete_Emp([FromUri] int id)
        {
            kenzanEntities ke = new kenzanEntities();
            Employee emp = ke.Employees
                .Where(e => e.id == id && e.bStatus == Status.ACTIVE)
                .FirstOrDefault<Employee>();

            if(emp == null)
                return new ErrorResponse() { error = "No record found to update" };

            emp.bStatus = Status.INACTIVE;
            if (ke.SaveChanges() != 1)
                return new ErrorResponse() { error = "Unable to update record" };
            else
                return new ErrorResponse() { error = "ok" };
        }
    }
}