using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace KenzanCSharp.JWT
{
    public class Header
    {
        public String alg;
    };

    public class Payload
    {
        public String iss;
        public DateTime exp;
        public DateTime atIssued;
        public String username;
        public List<String> roles;
    };

    public class JWTToken
    {
        private Header header { get; }
        private Payload payload { get; }
        public String username { get { return (payload == null ? null : payload.username);  } }
        public List<String> roles { get { return (payload == null ? null : payload.roles);  } }

        private String _passed_token;

        public String token { get {
                if (_passed_token != null) return _passed_token;
                string header = Convert.ToBase64String((new System.Text.ASCIIEncoding()).GetBytes(JsonConvert.SerializeObject(this.header, Formatting.None)));
                string payload = Convert.ToBase64String((new System.Text.ASCIIEncoding()).GetBytes(JsonConvert.SerializeObject(this.payload, Formatting.None)));

                HMACSHA256 hmacsha256 = new HMACSHA256((new System.Text.ASCIIEncoding()).GetBytes("signing key"));
                byte[] computedSignature = hmacsha256.ComputeHash((new System.Text.ASCIIEncoding()).GetBytes(header + "." + payload));
                string signature = Convert.ToBase64String(computedSignature);
                return "Bearer " + header + "." + payload + "." + signature;
            } }

        public Boolean valid { get; }

        public JWTToken(String token)
        {
            _passed_token = token;
            valid = false;
            String[] pieces = token.Split('.');

            if (pieces == null || pieces.Length != 3)
                return;

            byte[] header = Convert.FromBase64String(pieces[0]);
            byte[] payload = Convert.FromBase64String(pieces[1]);
            byte[] signature = Convert.FromBase64String(pieces[2]);
            this.header = JsonConvert.DeserializeObject<Header>(Encoding.UTF8.GetString(header));
            this.payload = JsonConvert.DeserializeObject<Payload>(Encoding.UTF8.GetString(payload));

            HMACSHA256 hmacsha256 = new HMACSHA256((new System.Text.ASCIIEncoding()).GetBytes("signing key"));

            byte[] computedSignature = hmacsha256.ComputeHash((new System.Text.ASCIIEncoding()).GetBytes(pieces[0] + "." + pieces[1]));
            valid = computedSignature.SequenceEqual(signature);
        }

        public JWTToken(Employee employee)
        {
            header = new Header() { alg = "HS256" };
            payload = new Payload() { atIssued = DateTime.Now, exp = DateTime.Now.AddMinutes(60), iss = "Get issuer", username = employee.username, roles = new List<string>() };
            foreach (EmployeeRole r in employee.EmployeeRoles) payload.roles.Add(r.role);
            _passed_token = null;
            valid = true;
        }
    }
}