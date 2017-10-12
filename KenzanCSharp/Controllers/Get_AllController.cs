using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace KenzanCSharp.Controllers
{
    [System.Web.Http.Authorize]
    public class Get_AllController : ApiController
    {
        // GET: rest/get_all
        public List<Employee> Get_All()
        {
            kenzanEntities ke = new kenzanEntities();
            ke.Configuration.ProxyCreationEnabled = false;
            List<Employee> empList = ke.Employees
                .Where<Employee>(e => e.bStatus == Status.ACTIVE)
                .ToList<Employee>();
            return empList;
        }
    }
}