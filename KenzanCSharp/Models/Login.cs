using KenzanCSharp.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KenzanCSharp.Content
{
    public class Login
    {
        [Required]
        public String username { get; set; }
        [Required]
        public String password { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JToken> AdditionalProperties { get; set; } = new Dictionary<string, JToken>();
        public bool ContainsExtra { get { return AdditionalProperties.Count != 0; } }

    }

    public class LoginResponse
    {
        public LoginResponse() { errorcode = ErrorNumber.NONE;  }
        public String error { get; set; }
        public ErrorNumber errorcode { get; set; }
        public String jwt { get; set; }
    }
}