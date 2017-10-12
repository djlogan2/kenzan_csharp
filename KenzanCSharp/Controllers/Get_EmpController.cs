using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace KenzanCSharp.Controllers
{
    //[System.Web.Http.Authorize]
    [System.Web.Http.AllowAnonymous]
    public class Get_EmpController : ApiController
    {
        // GET: rest/get_emp
        public Employee Get_Emp([FromUri] int id)
        {
            kenzanEntities ke = new kenzanEntities();
            ke.Configuration.ProxyCreationEnabled = false;
            Employee emp = ke.Employees
                .Where(e => e.id == id && e.bStatus == Status.ACTIVE)
                .FirstOrDefault<Employee>();
            return emp;
        }
    }
}