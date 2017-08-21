using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.DataExchange.Providers.ScreenScrapping.Abstraction;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Models
{
    public abstract class BaseHasPlugins : IHasPlugins
    {
        public ICollection<IPlugin> Plugins { get; private set; }

        protected BaseHasPlugins()
        {
            this.Plugins = new List<IPlugin>();
        }

        public T GetPlugin<T>() where T : IPlugin
        {
            return this.Plugins.OfType<T>().FirstOrDefault<T>();
        }

        public object GetPlugin(Type type)
        {
            return (object)this.Plugins.FirstOrDefault<IPlugin>((Func<IPlugin, bool>)(x => type.IsInstanceOfType((object)x)));
        }
    }
}
