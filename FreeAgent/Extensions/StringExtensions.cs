using System;
using System.Text;

namespace Wikiled.FreeAgent.Extensions
{
    public static class StringExtensions
    {
        public static string Fmt(this string str, params string[] p)
        {
            return string.Format(str, p);
        }


        public static string UrlEncode(this string value)
        {
            value = Uri.EscapeDataString(value);

            StringBuilder builder = new StringBuilder();
            foreach (char ch in value)
            {
                if ("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~%".IndexOf(ch) != -1)
                {
                    builder.Append(ch);
                }
                else
                {
                    builder.Append('%' + string.Format("{0:X2}", (int)ch));
                }
            }

            return builder.ToString();
        }
    }
}
