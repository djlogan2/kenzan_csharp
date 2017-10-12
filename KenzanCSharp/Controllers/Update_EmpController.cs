using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace KenzanCSharp.Controllers
{
    [System.Web.Http.Authorize(Roles = "ROLE_UPDATE_EMP")]
    public class Update_EmpController : ApiController
    {
        // POST: rest/upd_emp
        public ErrorResponse Post([FromBody] Employee employee)
        {
            kenzanEntities ke = new kenzanEntities();
            ke.Configuration.ProxyCreationEnabled = false;
            Employee emp = ke.Employees
                .Where(e => e.id == employee.id && e.bStatus == Status.ACTIVE)
                .FirstOrDefault<Employee>();

            if (emp == null)
                return new ErrorResponse() { error = "No record found to update" };

            emp.dateOfBirth = employee.dateOfBirth;
            emp.dateOfEmployment = employee.dateOfEmployment;
            emp.firstName = employee.firstName;
            emp.lastName = employee.lastName;
            emp.middleInitial = employee.middleInitial;
            emp.bStatus = employee.bStatus;
            emp.username = employee.username;
            if (ke.SaveChanges() != 1)
                return new ErrorResponse() { error = "Not yet implemented" };
            else
                return new ErrorResponse() { error = "ok" };
        }
    }
}