using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Providers.ScreenScrapping.Abstraction;
using Sitecore.DataExchange.Providers.ScreenScrapping.Extensions;
using Sitecore.DataExchange.Providers.ScreenScrapping.Models;
using Sitecore.DataExchange.Providers.ScreenScrapping.Plugins;
using Sitecore.DataExchange.Providers.ScreenScrapping.Repositories;
using Sitecore.Services.Core.Diagnostics;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Processors.PipelineSteps
{
    [RequiredPipelineStepPlugins(typeof(ImportMediaSettings))]
    public class ImportMediaStepProcessor : BasePipelineStepProcessor
    {
        public override void Process(PipelineStep pipelineStep, PipelineContext pipelineContext)
        {
            ILogger logger = pipelineContext.PipelineBatchContext.Logger;
            if (!this.CanProcess(pipelineStep, pipelineContext))
            {
                logger.Error(
                    "Pipeline step processing will abort because the pipeline step cannot be processed. (pipeline step: {0})",
                    (object)pipelineStep.Name);
            }
            else
            {
                var importMediaSettings = pipelineStep.GetImportMediaSettings();
                if (importMediaSettings == null)
                {
                    logger.Error(
                        "Pipeline step processing will abort because the pipeline step has no import media settings assigned. (pipeline step: {0})",
                        (object)pipelineStep.Name);
                }
                else
                {
                    if (!pipelineContext.HasItemMetadataSettings())
                    {
                        logger.Error(
                            "Pipeline step processing will abort because the required item metadata not found. (pipeline step: {0})",
                            (object)pipelineStep.Name);
                    }
                    else
                    {
                        var itemMetadataSettings = pipelineContext.GetItemMetadataSettings();

                        if (itemMetadataSettings.ItemDetail.Fields != null &&
                            itemMetadataSettings.ItemDetail.Fields.Any())
                        {
                            foreach (var field in importMediaSettings.Fields)
                            {
                                if (itemMetadataSettings.ItemDetail.Fields.Any(
                                    f => f.Key.Equals(field.Name, StringComparison.OrdinalIgnoreCase)))
                                {
                                    var value = itemMetadataSettings.ItemDetail.Fields[field.Name];
                                    HtmlDocument htmlDoc = new HtmlDocument();
                                    htmlDoc.OptionFixNestedTags = true;
                                    try
                                    {
                                        htmlDoc.LoadHtml(value.ToString());
                                        if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Any())
                                        {
                                            logger.Error("Html Parse error for field " + field.Name);
                                        }
                                        else
                                        {
                                            if (htmlDoc.DocumentNode == null)
                                            {
                                                logger.Error("Html Parse error for field " + field.Name);
                                            }
                                            else
                                            {
                                                ReadAllNodes(htmlDoc, "//img | //a", itemMetadataSettings.ItemDetail.DatatabaseName, itemMetadataSettings.ItemDetail.LanguageName, importMediaSettings);
                                                itemMetadataSettings.ItemDetail.Fields[field.Name] = htmlDoc.DocumentNode.OuterHtml;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        logger.Error("agilitypack" + ex.Message, new Exception());
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ReadAllNodes(HtmlDocument htmlDocument, string xPath, string databaseName,
            string languageName, ImportMediaSettings settings)
        {

            HtmlNodeCollection nodes = htmlDocument.DocumentNode.SelectNodes(xPath);
            foreach (HtmlNode node in nodes)
            {
                if (node != null)
                {
                    CreateUpdateMediaInfo mediaInfo = new CreateUpdateMediaInfo
                    {
                        DatabaseName = databaseName,
                        LanguageName = languageName
                    };

                    Import(node, settings, mediaInfo);
                }
            }
        }

        protected virtual void Import(HtmlNode node, ImportMediaSettings settings, CreateUpdateMediaInfo mediaInfo)
        {
            if (settings != null)
            {
                var urlInfo = GetUrl(node, "href", "src");

                if (urlInfo != null && !string.IsNullOrEmpty(urlInfo.Item1) && !string.IsNullOrEmpty(urlInfo.Item2))
                {
                    var attr = urlInfo.Item1;
                    var url = urlInfo.Item2;

                    var urlArray = url.Split('/');
                    
                    var nameWithExtension = urlArray[urlArray.Length - 1];
                    var extension = "," + nameWithExtension.GetExtension().ToLower().Trim().Trim(',') + ",";
                    if (settings.Extensions != null &&
                        settings.Extensions.ToLower().Contains(extension))
                    {
                        if (!string.IsNullOrEmpty(url))
                        {
                            if (!url.StartsWith("http:") && !url.StartsWith("https:"))
                            {
                                url = settings.BaseUrl.Trim().TrimEnd('/') + "/" + url.Trim().TrimStart('/');
                            }
                            
                            var downloadPath = Path.Combine(settings.DownloadFolderPath, nameWithExtension);

                            using (var fs = new FileStream( downloadPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                            {
                                // Download image
                                var isDownload = new DownloadRepository().DownloadContent(url, fs);
                                if (isDownload)
                                {
                                    mediaInfo.ParentId = settings.ParentItem;
                                    mediaInfo.MediaName = nameWithExtension.GetNameWithoutExtension();

                                    CustomContext.Repository.SaveMedia(mediaInfo, nameWithExtension, fs);

                                    if (!string.IsNullOrEmpty(mediaInfo.MediaId))
                                    {
                                        node.SetAttributeValue(attr,
                                            $"-/media/{mediaInfo.MediaId.ConvertToAssetId()}.ashx");
                                    }
                                }
                            }

                            File.Delete(downloadPath);
                        }
                    }
                }
            }
        }

        private Tuple<string, string> GetUrl(HtmlNode node, params string[] attrs)
        {
            if (attrs != null && attrs.Any())
            {
                foreach (var attr in attrs)
                {
                    if (node.Attributes[attr] != null && !string.IsNullOrEmpty(node.Attributes[attr].Value))
                    {
                        return new Tuple<string, string>(attr, node.Attributes[attr].Value.ToLower());
                    }
                }
            }

            return null;
        }
    }
}