using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

    /// <summary>
    /// Makes glimpse work and allows override of methods
    /// </summary>
    public class GlimpseHttpApplication : HttpApplication
    {
        private static readonly Regex _axdRegex = new Regex(@"^/(?<axd>[a-z]+.axd)($|\?|/)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        protected virtual void Application_Start(object sender, EventArgs e)
        {
            Dynamicweb.Frontend.GlobalAsaxHandler.Application_Start(sender, e);
        }

        protected virtual void Application_End(object sender, EventArgs e)
        {
            Dynamicweb.Frontend.GlobalAsaxHandler.Application_End(sender, e);
        }

        protected virtual void Application_OnPreRequestHandlerExecute(object sender, EventArgs e)
        {
            if (IsCustomPath()) return;
            Dynamicweb.Frontend.GlobalAsaxHandler.Application_OnPreRequestHandlerExecute(sender, e);
        }

        protected virtual void Session_Start(object sender, EventArgs e)
        {
            Dynamicweb.Frontend.GlobalAsaxHandler.Session_Start(sender, e);
        }

        protected virtual void Application_BeginRequest(object sender, EventArgs e)
        {
            if (IsCustomPath()) return;
            Dynamicweb.Frontend.GlobalAsaxHandler.Application_BeginRequest(sender, e);
        }

        protected virtual void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (IsCustomPath()) return;
            Dynamicweb.Frontend.GlobalAsaxHandler.Application_AuthenticateRequest(sender, e);
        }

        protected virtual void Application_Error(object sender, EventArgs e)
        {
            Dynamicweb.Frontend.GlobalAsaxHandler.Application_Error(sender, e);
        }

        protected virtual void Session_End(object sender, EventArgs e)
        {
            Dynamicweb.Frontend.GlobalAsaxHandler.Session_End(sender, e);
        }



        /// <summary>
        /// True if custom path (/Glimpse.axd). You may override this to make other custom paths
        /// </summary>
        /// <returns>Check if glimpse</returns>
        public virtual bool IsCustomPath()
        {
            if (HttpContext.Current == null)
                return false;

            var match = _axdRegex.Match(HttpContext.Current.Request.Url.AbsolutePath);
            return match.Success
		}
    }
