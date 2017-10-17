using KenzanCSharp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KenzanCSharp.Content
{
    public class Login
    {
        public String username { get; set; }
        public String password { get; set; }
    }

    public class LoginResponse
    {
        public LoginResponse() { errorcode = ErrorNumber.NONE;  }
        public String error { get; set; }
        public ErrorNumber errorcode { get; set; }
        public String jwt { get; set; }
    }
}