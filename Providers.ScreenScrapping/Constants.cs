namespace Sitecore.DataExchange.Providers.ScreenScrapping
{
    public static class Constants
    {
        public static class Templates
        {
            public static class Endpoints
            {
                public const string ExcelFileEndpointTemplateId = "{542DB787-7473-4F5D-AAD8-D621096093D2}";
                public const string SitemapEndpointTemplateId = "{D5736639-0318-4617-872B-75AE0E055304}";
                public const string IndexNameEndpointTemplateId = "{81F81B5E-520F-44F3-99F6-ABAEAA60B04D}";
                public const string SitecoreRepositoryEndpointTemplateId = "{3E61FD58-DAD5-44EB-AE71-2D7CB1E4240A}";
            }

            public static class Pipelines
            {
                public const string PageTypePipelineTemplateId = "{6B864353-0894-494E-A7CC-3B219BCB8B3D}";
            }

            public static class Custom
            {
                public const string FieldTemplateId = "{AA746618-42A5-407D-B8ED-1B9C5CFEA8C4}";
            }

            public static class PipelineSteps
            {
                public const string ReadExcelFilePipelineStepTemplateId = "{7E574187-CB7B-4A2D-9B06-75F81E645E3A}";
                public const string ReadSitemapPipelineStepTemplateId = "{DA26D688-3281-44B8-9562-1FDF63B3C2F2}";
                public const string IterateMeatadataPipelineStepTemplateId = "{BAB718F4-96EC-460A-B4FE-CCBB35189C79}";
                public const string ResolveTagPipelineStepTemplateId = "{A2103635-6451-4AA2-AF03-274F6D1DDED2}";
                public const string SaveItemStepTemplateId = "{19736BC2-D14F-45AD-BA38-41FB55B96506}";
                public const string PageReadPipelineStepTemplateId = "{1D933415-DE8E-49EE-BC7A-FF9AC7A53A3B}";
                public const string DetermineParentItemPipelineStepTemplateId = "{7174460F-B40A-4795-A3B1-E992C0999D92}";
                public const string DetermineTemplatePipelineStepTemplateId = "{6610C731-6A9B-4E54-B0CB-923C5F2C1714}";
                public const string ImportMediaStepTemplateId = "{9876322F-8C17-4E1A-81ED-459954741A2D}";
            }

            public const string DatarowValueAccessorTemplateId = "";
        }

        public static class Fields
        {
            public const string Path = "Path";
            public const string SheetName = "SheetName";
            public const string PageType = "PageType";
            public const string Url = "Url";

            public const string Endpoint = "Endpoint";
            public const string FieldName = "FieldName";
            public const string Template = "Template";
            public const string ParentItem = "Parent Item";

            public const string IndexName = "IndexName";
        }

        public static class Properties
        {
            public const string Url = "Url";
            public const string ItemId = "ItemId";
            public const string TemplateId = "TemplateId";
            public const string ParentItemId = "ParentItemId";
            public const string ItemName = "ItemName";
            public const string DatabaseName = "DatabaseName";
            public const string LanguageName = "LanguageName";
            public const string Action = "Action";
        }
    }
}
