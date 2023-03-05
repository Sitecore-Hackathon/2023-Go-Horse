using System.Web.Routing;
using Sitecore.Pipelines;
using XmCloudSXAStarter.App_Start;

namespace XmCloudSXAStarter.Processors
{
    public class LoadRoutes
    {
        public void Process(PipelineArgs args)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}