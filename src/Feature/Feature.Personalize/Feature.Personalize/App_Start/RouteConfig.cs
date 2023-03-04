using System.Web.Mvc;
using System.Web.Routing;

namespace Feature.Personalize.App_Start
{
    public class RouteConfig
    {
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