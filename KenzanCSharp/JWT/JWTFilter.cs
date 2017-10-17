using KenzanCSharp.Controllers;
using KenzanCSharp.JWT;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace KenzanCSharp.App_Start
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RESTAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            ErrorNumber ret;

            if (actionContext.ControllerContext.RequestContext.Principal is JWTUser)
                ret = ((JWTUser)actionContext.ControllerContext.RequestContext.Principal).token.errorcode;
            else
                ret = ErrorNumber.NO_AUTHORIZATION_TOKEN;

            if (ret == ErrorNumber.NONE)
                ret = ErrorNumber.NOT_AUTHORIZED_FOR_OPERATION;

            actionContext.Response = actionContext.Request.CreateResponse(
                        HttpStatusCode.Forbidden,
                        new ErrorResponse(ret, "Authorization failed"),
                        actionContext.ControllerContext.Configuration.Formatters.JsonFormatter
                    );
        }
    }

    public class JWTFilter : IAuthenticationFilter
    {
        public bool AllowMultiple => throw new NotImplementedException();

        private JsonResult<ErrorResponse> retError(ErrorNumber en, String em, HttpRequestMessage request) { return new JsonResult<ErrorResponse>(new ErrorResponse(en, em), new JsonSerializerSettings(), Encoding.ASCII, request);}
        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
           {
               HttpRequestMessage request = context.Request;
               AuthenticationHeaderValue authorization = request.Headers.Authorization;
               //JsonResult<ErrorResponse> bad = new JsonResult<ErrorResponse>(new ErrorResponse(ErrorNumber.NOT_AUTHORIZED_FOR_OPERATION, "Generic not authorized"), new JsonSerializerSettings(), Encoding.ASCII, context.Request);

            if (authorization == null || authorization.Scheme != "Bearer")
                return;

            if(String.IsNullOrEmpty(authorization.Parameter))
               {
                   context.ErrorResult = retError(ErrorNumber.INVALID_AUTHORIZATION_TOKEN_NO_BEARER, "No Bearer", request);
                   return;
               }

               JWTToken token = new JWTToken(authorization.Parameter);

               IPrincipal principal = new JWTUser(token);

               if (token.valid && !principal.Identity.IsAuthenticated)
                   context.ErrorResult = retError(ErrorNumber.UNKNOWN_ERROR, "Not sure what this is", request);
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