using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace KenzanCSharp.JWT
{
    public class JWTUser : IPrincipal, IIdentity
    {
        public JWTUser(JWTToken token)
        {
            Name = token.username;
            AuthenticationType = "JWT";
            IsAuthenticated = token.valid;
            roles = token.roles;
        }
        private List<String> roles;

        public string Name { get; set; }

        public string AuthenticationType { get; set; }

        public bool IsAuthenticated { get; set; }

        public IIdentity Identity { get { return this; } }

        public bool IsInRole(string role)
        {
            return roles.Contains(role);
        }
    }
}