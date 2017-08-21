using System;
using System.Linq;
using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Providers.ScreenScrapping.Abstraction;
using Sitecore.DataExchange.Providers.ScreenScrapping.Extensions;
using Sitecore.DataExchange.Providers.ScreenScrapping.Models;
using Sitecore.DataExchange.Providers.ScreenScrapping.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Converters.PipelineSteps
{
    public class ImportMediaStepConverter : BasePipelineStepConverter
    {
        private static readonly Guid TemplateId = Guid.Parse(Constants.Templates.PipelineSteps.ImportMediaStepTemplateId);
        public ImportMediaStepConverter(IItemModelRepository repository) : base(repository)
        {
            this.SupportedTemplateIds.Add(TemplateId);
        }
        protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
        {
            var settings = new ImportMediaSettings();
            settings.Extensions = base.GetStringValue(source, "Extensions");
            if (!string.IsNullOrEmpty(settings.Extensions))
            {
                settings.Extensions = "," + settings.Extensions.Trim().Trim(',') + ",";
            }

            settings.ParentItem = base.GetStringValue(source, "ParentItem");
            settings.BaseUrl = base.GetStringValue(source, "BaseUrl");
            settings.DownloadFolderPath = base.GetStringValue(source, "DownloadFolderPath");
            settings.Fields = base.ConvertReferencesToModels<Field>(source, "Fields").ToList();
            pipelineStep.Plugins.Add(settings);
        }
    }
}