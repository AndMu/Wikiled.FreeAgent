using System;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.Extensions
{
    public static class ModelExtensions
    {
        public static DateTime FromModelDate(this string modeldate)
        {
            return DateTime.Parse(modeldate);
        }

        public static string Id(this BaseModel model)
        {
            return StringId(model.url);
        }

        public static int LocalId(this BaseModel model)
        {
            int val = -1;
            if (int.TryParse(model.Id(), out val))
            {
                return val;
            }

            return -1;
        }

        public static int LocalId(this string idstring)
        {
            int val = -1;
            if (int.TryParse(StringId(idstring), out val))
            {
                return val;
            }

            return -1;
        }

        public static string ModelDate(this DateTime currentdate)
        {
            return currentdate.ToString("yyyy-MM-dd");
        }

        public static string ModelDateTime(this DateTime currentdate)
        {
            return currentdate.ToString("s");
        }

        public static UserPermission PermissionLevel(this User user)
        {
            return (UserPermission)user.permission_level;
        }

        public static string StringId(string url)
        {
            if (string.IsNullOrEmpty(url)) return "";

            string[] elements = url.Split('/');
            return elements[elements.Length - 1];
        }

        public static string UrlId(this BaseModel model)
        {
            if (string.IsNullOrEmpty(model.url)) return "";
            try
            {
                string[] elements = model.url.Split('/');
                return "/v2/{0}/{1}".Fmt(elements[elements.Length - 2], elements[elements.Length - 1]);
            }
            catch
            {
                return "";
            }
        }

        public static string UrlId(this string id, string resourcetype)
        {
            return "/v2/{0}/{1}".Fmt(resourcetype, id);
        }

        public static string UrlId(this int id, string resourcetype)
        {
            return "/v2/{0}/{1}".Fmt(resourcetype, id.ToString());
        }
    }
}
