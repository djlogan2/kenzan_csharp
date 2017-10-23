using KenzanCSharp.Controllers;
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
        public DateTime? exp;
        public DateTime? atIssued;
        public String username;
        public List<String> roles;
    };

    public class JWTToken
    {
        private static string SIGNING_KEY = "Kenzan Signing Key";
        private static string ISSUER = "Kenzan";
        private static int    EXPIRATION_MINUTES = 60;
        private Header header { get; }
        private Payload payload { get; }
        public String username { get { return (payload == null ? null : payload.username);  } }
        public List<String> roles { get { return (payload == null ? null : payload.roles);  } }
        public ErrorNumber errorcode;

        private String _passed_token;

        public String token { get {
                if (_passed_token != null) return _passed_token;
                string header = Convert.ToBase64String((new System.Text.ASCIIEncoding()).GetBytes(JsonConvert.SerializeObject(this.header, Formatting.None)));
                this.payload.iss = ISSUER;
                this.payload.atIssued = DateTime.Now;
                this.payload.exp = this.payload.atIssued.Value.AddMinutes(EXPIRATION_MINUTES);
                string payload = Convert.ToBase64String((new System.Text.ASCIIEncoding()).GetBytes(JsonConvert.SerializeObject(this.payload, Formatting.None)));

                HMACSHA256 hmacsha256 = new HMACSHA256((new System.Text.ASCIIEncoding()).GetBytes(SIGNING_KEY));
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
            { errorcode = ErrorNumber.INVALID_AUTHORIZATION_TOKEN_PARSE_ERROR; return; }

            byte[] header = Convert.FromBase64String(pieces[0]);
            byte[] payload = Convert.FromBase64String(pieces[1]);
            byte[] signature = Convert.FromBase64String(pieces[2]);

            try
            {
                this.header = JsonConvert.DeserializeObject<Header>(Encoding.UTF8.GetString(header));
                this.payload = JsonConvert.DeserializeObject<Payload>(Encoding.UTF8.GetString(payload));
            } catch(/*JsonSerialization*/Exception e)
            {
                if(e is JsonReaderException)
                {
                    JsonReaderException jre = (JsonReaderException)e;
                    if(jre.Path == "atIssued")
                        { errorcode = ErrorNumber.INVALID_AUTHORIZATION_PAYLOAD_INVALID_ISSUED; return; }
                    else if (jre.Path == "exp")
                        { errorcode = ErrorNumber.INVALID_AUTHORIZATION_PAYLOAD_INVALID_EXPIRATION; return; }
                    else
                        { errorcode = ErrorNumber.INVALID_AUTHORIZATION_TOKEN_PARSE_ERROR; return; }
                }
                errorcode = ErrorNumber.INVALID_AUTHORIZATION_TOKEN_PARSE_ERROR;
                return;
            }

            if(this.header== null || this.payload == null)
            { errorcode = ErrorNumber.INVALID_AUTHORIZATION_TOKEN_PARSE_ERROR; return; }

            if(this.header.alg == null || this.header.alg != "HS256")
            { errorcode = ErrorNumber.INVALID_AUTHORIZATION_HEADER_INVALID_ALGORITHM; return; }

            if(this.payload.iss == null)
            { errorcode = ErrorNumber.INVALID_AUTHORIZATION_PAYLOAD_NO_ISSUER; return; }

            if(this.payload.iss != ISSUER)
            { errorcode = ErrorNumber.INVALID_AUTHORIZATION_PAYLOAD_INVALID_ISSUER; return; }

            if(this.payload.atIssued == null)
            { errorcode = ErrorNumber.INVALID_AUTHORIZATION_PAYLOAD_NO_ISSUED; return; }

            if(this.payload.exp == null)
            { errorcode = ErrorNumber.INVALID_AUTHORIZATION_PAYLOAD_NO_EXPIRATION; return; }

            if(this.payload.atIssued.Value.ToLocalTime().CompareTo(this.payload.exp.Value.ToLocalTime()) >= 0 || this.payload.atIssued.Value.ToLocalTime().CompareTo(DateTime.Now) >= 0)
            { errorcode = ErrorNumber.INVALID_AUTHORIZATION_PAYLOAD_INVALID_ISSUED; return; }

            if (this.payload.exp.Value.ToLocalTime().CompareTo(DateTime.Now) <= 0)
            { errorcode = ErrorNumber.INVALID_AUTHORIZATION_TOKEN_EXPIRED; return; }

            HMACSHA256 hmacsha256 = new HMACSHA256((new System.Text.ASCIIEncoding()).GetBytes(SIGNING_KEY));

            byte[] computedSignature = hmacsha256.ComputeHash((new System.Text.ASCIIEncoding()).GetBytes(pieces[0] + "." + pieces[1]));
            valid = computedSignature.SequenceEqual(signature);
            if(!valid) { errorcode = ErrorNumber.INVALID_AUTHORIZATION_TOKEN_INVALID_SIGNATURE; return; }

        }

        public JWTToken(Employee employee)
        {
            header = new Header() { alg = "HS256" };
            payload = new Payload() { iss = ISSUER, username = employee.username, roles = new List<string>() };
            foreach (EmployeeRole r in employee.EmployeeRoles) payload.roles.Add(r.role);
            _passed_token = null;
            valid = true;
        }
    }
}