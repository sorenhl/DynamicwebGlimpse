using System;
using System.Linq;
using Glimpse.AspNet.Extensions;
using Glimpse.Core.Extensibility;
using System.Web;

namespace GlimpseDemo
{
    public class OurSecurityPolicy:IRuntimePolicy
    {
        public RuntimePolicy Execute(IRuntimePolicyContext policyContext)
        {
            // Get HttpContext
            var httpContext = policyContext.GetHttpContext();

            // Don't show glimpse in the backend as this desturbs the view
            if(httpContext.Request.Path.StartsWith("/admin/", StringComparison.InvariantCultureIgnoreCase))
                return RuntimePolicy.Off;

            // Local rule
            if (httpContext.Request != null && string.Equals(httpContext.Request.UserHostAddress, "::1"))
                return RuntimePolicy.On;

            // Session not loaded, assume we want to show it
            if (httpContext.Session == null)
                return RuntimePolicy.On;
                
            // Session is loaded, so now we verify if admin is logged in
            if(Dynamicweb.ExecutingContext.IsAdminLoggedIn())
                return RuntimePolicy.On;

            // Oups, no admin, get out of here!
            return RuntimePolicy.Off;
        }

        public RuntimeEvent ExecuteOn
        {
            get { return RuntimeEvent.BeginSessionAccess; }
        }
    }
}
