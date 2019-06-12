using System;
using System.Globalization;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.Extensions
{
    public static class ModelExtensions
    {
        public static DateTime FromModelDate(this string modelDate)
        {
            if (!DateTime.TryParseExact(modelDate , "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out var dateTime))
            {
                throw new ArgumentOutOfRangeException(nameof(modelDate));
            }

            return dateTime;
        }

        public static string Id(this BaseModel model)
        {
            return StringId(model.Url);
        }

        public static int LocalId(this BaseModel model)
        {
            if (int.TryParse(model.Id(), out var val))
            {
                return val;
            }

            return -1;
        }

        public static int LocalId(this string idString)
        {
            if (int.TryParse(StringId(idString), out var val))
            {
                return val;
            }

            return -1;
        }

        public static string ModelDate(this DateTime currentDate)
        {
            return currentDate.ToString("yyyy-MM-dd");
        }

        public static string ModelDateTime(this DateTime currentDate)
        {
            return currentDate.ToString("s");
        }

        public static UserPermission PermissionLevel(this User user)
        {
            return (UserPermission)user.permission_level;
        }

        public static string StringId(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }

            string[] elements = url.Split('/');
            return elements[elements.Length - 1];
        }

        public static string UrlId(this BaseModel model)
        {
            if (string.IsNullOrEmpty(model.Url))
            {
                return string.Empty;
            }

            try
            {
                string[] elements = model.Url.Split('/');
                return "/v2/{0}/{1}".Fmt(elements[elements.Length - 2], elements[elements.Length - 1]);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string UrlId(this string id, string resourceType)
        {
            return "/v2/{0}/{1}".Fmt(resourceType, id);
        }

        public static string UrlId(this int id, string resourceType)
        {
            return "/v2/{0}/{1}".Fmt(resourceType, id.ToString());
        }
    }
}
