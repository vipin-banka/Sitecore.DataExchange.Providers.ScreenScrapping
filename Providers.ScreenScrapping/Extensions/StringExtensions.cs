using System;

namespace Sitecore.DataExchange.Providers.ScreenScrapping.Extensions
{
    public static class StringExtensions
    {
        public static string GetLastPart(this string value)
        {
            var index = value.LastIndexOf("/", StringComparison.InvariantCulture);
            if (index >= 0)
            {
                return value.Substring(index+1);
            }

            return value;
        }

        public static string ConvertToAssetId(this string id)
        {
            return id.ToLower().Replace("{", string.Empty)
                .Replace("}", string.Empty)
                .Replace("-", string.Empty);
        }

        public static string GetNameWithoutExtension(this string name)
        {
            var pos = name.IndexOf(".");
            if (pos > 0)
            {
                name = name.Substring(0, pos);
            }

            return name;
        }

        public static string GetExtension(this string name)
        {
            var pos = name.LastIndexOf(".");
            if (pos > 0)
            {
                return name.Substring(pos);
            }

            return name;
        }
    }
}
