﻿using KenzanCSharp.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace KenzanCSharp.Controllers
{
    [RESTAuthorize(Roles = "ROLE_UPDATE_EMP")]
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
                return new ErrorResponse(ErrorNumber.CANNOT_UPDATE_NONEXISTENT_RECORD, "Nonexistant record");

            emp.dateOfBirth = employee.dateOfBirth;
            emp.dateOfEmployment = employee.dateOfEmployment;
            emp.firstName = employee.firstName;
            emp.lastName = employee.lastName;
            emp.middleInitial = employee.middleInitial;
            emp.bStatus = employee.bStatus;
            emp.username = employee.username;
            if (ke.SaveChanges() != 1)
                return new ErrorResponse(emp.id, ErrorNumber.DUPLICATE_RECORD, "Duplicate record");
            else
                return new ErrorResponse();
        }
    }
}