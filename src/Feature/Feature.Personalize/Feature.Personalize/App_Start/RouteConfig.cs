using System.Web.Mvc;
using System.Web.Routing;

namespace Feature.Personalize.App_Start
{
    /// <summary>
    /// Register MVC routes
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Method that will register routes
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "personalizeConnect",
                url: "PersonalizeConnect/{action}",
                defaults: new { controller = "PersonalizeConnect", action = "Index" }
            );
        }
    }
}