using KenzanCSharp.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace KenzanCSharp.Controllers
{
    [RESTAuthorize(Roles = "ROLE_ADD_EMP")]
    public class Add_EmpController : ApiController
    {
        // POST: rest/add_emp
        public ErrorResponse Post([FromBody] Employee employee)
        {
            kenzanEntities ke = new kenzanEntities();
            ke.Employees.Add(employee);

            ErrorResponse err;

            try
            {
                if (ke.SaveChanges() == 0)
                    err = new ErrorResponse(ErrorNumber.DUPLICATE_RECORD, "No records updated");
                else
                    err = new ErrorResponse(employee.id);
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                err = new ErrorResponse(ErrorNumber.CANNOT_INSERT_MISSING_FIELDS, e.Message);
            }

            return err;
        }
    }
}