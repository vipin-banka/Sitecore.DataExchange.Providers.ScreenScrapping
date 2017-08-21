using System;
using System.Linq;
using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Providers.ScreenScrapping.Extensions;
using Sitecore.DataExchange.Providers.ScreenScrapping.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Converters.PipelineSteps
{
    public class ResolveTagStepConverter : BasePipelineStepConverter
    {
        private static readonly Guid TemplateId = Guid.Parse(Constants.Templates.PipelineSteps.ResolveTagPipelineStepTemplateId);

        public ResolveTagStepConverter(IItemModelRepository repository) : base(repository)
        {
            this.SupportedTemplateIds.Add(TemplateId);
        }

        protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
        {
            AddEndpointSettings(source, pipelineStep);
        }

        private void AddEndpointSettings(ItemModel source, PipelineStep pipelineStep)
        {
            var settings = new FieldValueResolverSettings();
            settings.Key = base.GetStringValue(source, "Key");
            settings.ParentItems = base.GetStringValue(source, "Paths").Split('|').ToList();
            settings.Templates = base.GetStringValue(source, "Templates").Split('|').ToList();
            settings.SearchByName = base.GetBoolValue(source, "SearchByName");
            settings.TypeFullName = base.GetStringValue(source, "SearchType");
            pipelineStep.Plugins.Add(settings);

            var indexEndpoint = base.ConvertReferenceToModel<Endpoint>(source, "IndexEndpoint");
            settings.IndexName = indexEndpoint.GetIndexNameSettings().IndexName;

            var endpointSettings = new EndpointSettings();
            endpointSettings.EndpointFrom = base.ConvertReferenceToModel<Endpoint>(source, "RepositoryEndpoint");
            pipelineStep.Plugins.Add(endpointSettings);
        }
    }
}