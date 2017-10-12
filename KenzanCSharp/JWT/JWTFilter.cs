using KenzanCSharp.Controllers;
using KenzanCSharp.JWT;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace KenzanCSharp.App_Start
{
    public class JWTFilter : IAuthenticationFilter
    {
        public bool AllowMultiple => throw new NotImplementedException();

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
           {
               HttpRequestMessage request = context.Request;
               AuthenticationHeaderValue authorization = request.Headers.Authorization;
               JsonResult<ErrorResponse> bad = new JsonResult<ErrorResponse>(new ErrorResponse() { error = "Unauthorized(1)" }, new JsonSerializerSettings(), Encoding.ASCII, context.Request);

            if (authorization == null || authorization.Scheme != "Bearer")
                return;

            if(String.IsNullOrEmpty(authorization.Parameter))
               {
                   context.ErrorResult = bad;
                   return;
               }

               JWTToken token = new JWTToken(authorization.Parameter);

               IPrincipal principal = new JWTUser(token);

               if (token.valid && !principal.Identity.IsAuthenticated)
                   context.ErrorResult = bad;
               else
                   context.Principal = principal;
               return;
           });
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            //TODO: Flush this out
            //throw new NotImplementedException();
            return Task.Run(() => { return; });
        }
    }
}