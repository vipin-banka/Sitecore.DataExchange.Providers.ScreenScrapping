using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Sitecore.DataExchange.Providers.ScreenScrapping;
using Sitecore.DataExchange.Providers.ScreenScrapping.Remote.Repository;
using Sitecore.DataExchange.Providers.ScreenScrapping.Repositories;
using Sitecore.DataExchange.Remote.Http;
using Sitecore.DataExchange.Remote.Repositories;
using Sitecore.DataExchange.Remote.Runners;
using Sitecore.DataExchange.Repositories;
using Sitecore.DataExchange.Repositories.Tenants;
using System.IO;

namespace Providers.ScreenScrapping.DemoApp
{
    class Program
    {
        private static ConnectionSettings GetConnectionSettings()
        {
            var cxSettings = new ConnectionSettings
            {
                Host = System.Configuration.ConfigurationManager.AppSettings["Host"],
                UserName = System.Configuration.ConfigurationManager.AppSettings["Username"],
                Password = System.Configuration.ConfigurationManager.AppSettings["Password"],
            };

            return cxSettings;
        }

        private static IItemModelRepository GetItemModelRepository()
        {
            return new WebApiItemModelRepository(System.Configuration.ConfigurationManager.AppSettings["Database"], GetConnectionSettings());
        }

        private static IContentRepository GetModelRepository()
        {
            return new WebApiContentRepository(System.Configuration.ConfigurationManager.AppSettings["Database"], GetConnectionSettings());
        }

        private static void RunPipelineBatch()
        {
            Sitecore.DataExchange.Context.ItemModelRepository = GetItemModelRepository();
            CustomContext.Repository = GetModelRepository();

            var repo = new SitecoreTenantRepository
            {
                ItemModelRepository = GetItemModelRepository()
            };
            //
            // Only read the tenant you are interested in.
            var tenant = repo.GetTenants().FirstOrDefault(t => t.Enabled);
            if (tenant == null)
            {
                //
                // The tenant you specified does not exist, or is not enabled.
                return;
            }
            //
            // Only read the pipeline batch you are interested in.
            var batches = repo.GetPipelineBatches(tenant.ID, true);
            var batch = batches.FirstOrDefault(b => b.Enabled);
            if (batch == null)
            {
                Console.WriteLine("Pipeline batch not found.");
                //
                // The pipeline batch you specified does not exist, or is not enabled.
                return;
            }
            //
            // Instantiate an object that runs pipeline batches.
            var runner = new RemotePipelineBatchRunner();
            if (!runner.Run(batch))
            {
                Console.WriteLine("Stopped.");
                //
                // The runner was not started or an error occurred and the
                // pipeline batch was aborted.
                return;
            }
        }

        static void Main(string[] args)
        {
            RunPipelineBatch();
            Console.Read();
        }
    }
}
