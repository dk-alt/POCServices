using POCLogger;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace POCServices.Filters
{
    public class AuthorizationFilter : AuthorizeAttribute
    {


        readonly bool sEnv = Convert.ToBoolean(ConfigurationManager.AppSettings["Secure"]);
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (!sEnv)
            {
                //by pass authorization 
                return true;
            }
            else
            {
                IPrincipal incomingPrincipal = actionContext.RequestContext.Principal;
                Logger.Info(string.Format("User {0} authenticated: {1} ", incomingPrincipal.Identity.Name, incomingPrincipal.Identity.IsAuthenticated));
                return base.IsAuthorized(actionContext);
            }

        }
    }
}