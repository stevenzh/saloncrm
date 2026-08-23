using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace SalonCRM.Web
{
   public  class GenSessionID : IHttpModule

    {
        private string m_RootDomain = string.Empty;

        #region IHttpModule Members

        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            m_RootDomain = ConfigurationManager.AppSettings["RootDomain"];
           // m_RootDomain = "localhost";

            Type stateServerSessionProvider = typeof(HttpSessionState).Assembly.GetType("System.Web.SessionState.OutOfProcSessionStateStore");
            FieldInfo uriField = stateServerSessionProvider.GetField("s_uribase", BindingFlags.Static | BindingFlags.NonPublic);

            if (uriField == null)
                throw new ArgumentException("UriField was not found");

            uriField.SetValue(null, m_RootDomain);
 
            context.EndRequest += new System.EventHandler(context_EndRequest);


        }

        void context_EndRequest(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            for (int i = 0; i < app.Context.Response.Cookies.Count; i++)
            {
                if (app.Context.Response.Cookies[i].Name == "ASP.NET_SessionId")
                {
                    app.Context.Response.Cookies[i].Domain = m_RootDomain;
                }
            }
        }

        #endregion

    }
}
