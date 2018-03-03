using System.Web.Http;
using Sitecore.Pipelines;

namespace Hackathon.RedStaffy.XConnectBI.Pipelines
{
    public class RegisterHttpRoutes
    {
        public void Process(PipelineArgs args)
        {
            GlobalConfiguration.Configure(Configure);
        }

        /// <summary>
        /// Register API router.
        /// </summary>
        /// <param name="configuration"></param>
        protected void Configure(HttpConfiguration configuration)
        {
            var routes = configuration.Routes;

            routes.MapHttpRoute("RedStaffyAPIs", "RedStaffy/api/{controller}", new
            {
                controller = "AsyncContacts",
                action = "Get"
            });
        }
    }
}