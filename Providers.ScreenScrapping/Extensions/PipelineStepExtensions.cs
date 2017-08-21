using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.ScreenScrapping.Plugins;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Extensions
{
    public static class PipelineStepExtensions
    {
        public static WebPageReaderSettings GetWebPageReaderSettings(this PipelineStep pipelineStep)
        {
            return pipelineStep.GetPlugin<WebPageReaderSettings>();
        }

        public static bool HasWebPageReaderSettings(this PipelineStep pipelineStep)
        {
            return (GetWebPageReaderSettings(pipelineStep) != null);
        }
        public static FieldValueResolverSettings GetFieldValueResolverSettings(this PipelineStep pipelineStep)
        {
            return pipelineStep.GetPlugin<FieldValueResolverSettings>();
        }

        public static bool HasFieldValueResolverSettings(this PipelineStep pipelineStep)
        {
            return (GetFieldValueResolverSettings(pipelineStep) != null);
        }

        public static AssetsSettings GetAssetsSettings(this PipelineStep pipelineStep)
        {
            return pipelineStep.GetPlugin<AssetsSettings>();
        }

        public static bool HasAssetsSettings(this PipelineStep pipelineStep)
        {
            return (GetAssetsSettings(pipelineStep) != null);
        }

        public static ImportMediaSettings GetImportMediaSettings(this PipelineStep pipelineStep)
        {
            return pipelineStep.GetPlugin<ImportMediaSettings>();
        }

        public static bool HasImportMediaSettings(this PipelineStep pipelineStep)
        {
            return (GetImportMediaSettings(pipelineStep) != null);
        }

        public static ParentItemSettings GetParentItemSettings(this PipelineStep pipelineStep)
        {
            return pipelineStep.GetPlugin<ParentItemSettings>();
        }

        public static bool HasParentItemSettings(this PipelineStep pipelineStep)
        {
            return (GetParentItemSettings(pipelineStep) != null);
        }

        public static TemplateSettings GetTemplateSettings(this PipelineStep pipelineStep)
        {
            return pipelineStep.GetPlugin<TemplateSettings>();
        }

        public static bool HasTemplateSettings(this PipelineStep pipelineStep)
        {
            return (GetTemplateSettings(pipelineStep) != null);
        }
    }
}