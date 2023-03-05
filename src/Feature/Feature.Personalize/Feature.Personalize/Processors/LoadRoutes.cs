using System.Web.Routing;
using Feature.Personalize.App_Start;
using Sitecore.Pipelines;

namespace Feature.Personalize.Processors
{
    public class LoadRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}