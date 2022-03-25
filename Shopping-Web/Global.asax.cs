using System;
using System.Web.Http;
using System.Web.Routing;

namespace Shopping_Web
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            // Add Routes.
            RegisterCustomRoutes(RouteTable.Routes);
        }

        void RegisterCustomRoutes(RouteCollection routes)
        {
            routes.MapPageRoute(
                "index",
                "*",
                "~/Index.html"
            );
            RouteTable.Routes.MapHttpRoute(
                name: "AgentApi",
                routeTemplate: "api/{controller}/{action}"
            );
        }
    }
}