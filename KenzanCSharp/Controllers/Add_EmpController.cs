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
        public ErrorResponse Post([FromBody] JSONEmployee json_employee)
        {

            ErrorResponse err;

            if (!ModelState.IsValid || json_employee._id != null)
            {
                return new ErrorResponse(ErrorNumber.CANNOT_INSERT_MISSING_FIELDS, "No records added");
            }

            if(json_employee.ContainsExtra)
            {
                return new ErrorResponse(ErrorNumber.CANNOT_INSERT_UNKNOWN_FIELDS, "Extra fields in json");
            }

            Employee employee = new Employee(json_employee);

            kenzanEntities ke = new kenzanEntities();
            ke.Employees.Add(employee);

            try
            {
                if (ke.SaveChanges() == 0)
                    err = new ErrorResponse(ErrorNumber.UNKNOWN_ERROR, "No records added");
                else
                    err = new ErrorResponse(employee.id);
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                if(e.Message.Contains("Duplicate"))
                    err = new ErrorResponse(ErrorNumber.DUPLICATE_RECORD, e.Message);
                else
                    err = new ErrorResponse(ErrorNumber.CANNOT_INSERT_MISSING_FIELDS, e.Message);
            }

            return err;
        }
    }
}