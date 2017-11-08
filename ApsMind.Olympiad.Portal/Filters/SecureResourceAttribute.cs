using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using ApsMind.Olympiad.Framework.Entity;
using ApsMind.Olympiad.Framework;

namespace Apsmind.FREZTOPLAN.Portal.Filters
{
    
    public class SecureResourceAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var authorizeHeader = actionContext.Request.Headers.Authorization;
            // && authorizeHeader.Scheme.Equals("bearer ", StringComparison.OrdinalIgnoreCase)
            if (authorizeHeader != null && String.IsNullOrEmpty(authorizeHeader.Parameter) == false)
            {
                DatabaseContext objDB = new DatabaseContext();
                var existingToken = objDB.Tokens.Where(x => x.TokenCode == authorizeHeader.Parameter).FirstOrDefault();
                if (existingToken != null)
                {

                    var principal = new GenericPrincipal((new GenericIdentity(existingToken.UserId.ToString())),
                                                                    (new[] { existingToken.RoleId.ToString() }));
                        Thread.CurrentPrincipal = principal;
                        if (HttpContext.Current != null)
                            HttpContext.Current.User = principal;

                        return;
                    
                }
            }
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode
                                                                                   .Unauthorized);

            actionContext.Response.Content = new StringContent("Username and password are missings or invalid");
        }
    } 
}