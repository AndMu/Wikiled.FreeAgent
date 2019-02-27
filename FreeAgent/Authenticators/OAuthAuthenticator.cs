using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using RestSharp;
using RestSharp.Authenticators;
using Wikiled.FreeAgent.Extensions;

namespace Wikiled.FreeAgent.Authenticators
{
    public class OAuthAuthenticator : IAuthenticator
    {
        private const string ConsumerKeyKey = "oauth_consumer_key";

        private const string NonceKey = "oauth_nonce";

        private const string SignatureKey = "oauth_signature";

        private const string SignatureMethodDefault = "PLAINTEXT";

        private const string SignatureMethodKey = "oauth_signature_method";

        private const string TimestampKey = "oauth_timestamp";

        private const string TokenKey = "oauth_token";

        private const string UnreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

        private const string Version = "1.0";

        private const string VersionKey = "oauth_version";

        // Fields
        private readonly string baseUrl;

        private readonly string consumerKey;

        private readonly string consumerSecret;

        private readonly string token;

        private readonly string tokenSecret;

        private static readonly Random Random = new Random();

        // Methods
        public OAuthAuthenticator(string baseUrl, string consumerKey, string consumerSecret)
            : this(baseUrl, consumerKey, consumerSecret, string.Empty, string.Empty)
        {
        }

        public string SignatureMethod { get; set; } = SignatureMethodDefault;

        public OAuthAuthenticator(string baseUrl, string consumerKey, string consumerSecret, string token, string tokenSecret)
        {
            this.baseUrl = baseUrl;
            this.consumerKey = consumerKey;
            this.consumerSecret = consumerSecret;
            this.token = token;
            this.tokenSecret = tokenSecret;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            if (request.Method == Method.PUT)
            {
                //Do the parameters as URL segments for PUT
                request.AddParameter("oauth_consumer_key", consumerKey, ParameterType.UrlSegment);
                request.AddParameter("oauth_nonce", GenerateNonce(), ParameterType.UrlSegment);
                if (!string.IsNullOrEmpty(token))
                {
                    request.AddParameter("oauth_token", token, ParameterType.UrlSegment);
                }

                request.AddParameter("oauth_timestamp", GenerateTimeStamp(), ParameterType.UrlSegment);
                request.AddParameter("oauth_signature_method", SignatureMethod, ParameterType.UrlSegment);
                request.AddParameter("oauth_version", "1.0", ParameterType.UrlSegment);
                request.Parameters.Sort(new QueryParameterComparer());
                request.AddParameter("oauth_signature", GenerateSignature(request), ParameterType.UrlSegment);
            }
            else
            {
                request.AddParameter("oauth_version", "1.0");
                request.AddParameter("oauth_nonce", GenerateNonce());
                request.AddParameter("oauth_timestamp", GenerateTimeStamp());
                request.AddParameter("oauth_signature_method", SignatureMethod);
                request.AddParameter("oauth_consumer_key", consumerKey);
                if (!string.IsNullOrEmpty(token))
                {
                    request.AddParameter("oauth_token", token);
                }

                request.Parameters.Sort(new QueryParameterComparer());
                request.AddParameter("oauth_signature", GenerateSignature(request));
            }
        }

        public string GenerateNonce()
        {
            return Random.Next(0x1e208, 0x98967f).ToString();
        }

        public string GenerateTimeStamp()
        {
            TimeSpan span = DateTime.UtcNow - new DateTime(0x7b2, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(span.TotalSeconds).ToString();
        }

        private static string ComputeHash(HashAlgorithm hashAlgorithm, string data)
        {
            if (hashAlgorithm == null)
            {
                throw new ArgumentNullException("hashAlgorithm");
            }

            if (string.IsNullOrEmpty(data))
            {
                throw new ArgumentNullException("data");
            }

            byte[] bytes = Encoding.UTF8.GetBytes(data);
            return Convert.ToBase64String(hashAlgorithm.ComputeHash(bytes));
        }

        private static string NormalizeRequestParameters(IEnumerable<Parameter> parameters)
        {
            var builder = new StringBuilder();
            List<Parameter> list = parameters.Where(
                p =>
                {
                    //Hackity hack, don't come back...
                    return p.Type == ParameterType.GetOrPost || p.Name == "file" || p.Name.StartsWith("oauth_");
                }).ToList();

            for (int i = 0; i < list.Count; i++)
            {
                Parameter parameter = list[i];
                builder.AppendFormat("{0}={1}", parameter.Name, parameter.Value.ToString().UrlEncode());
                if (i < list.Count - 1)
                {
                    builder.Append("&");
                }
            }

            return builder.ToString();
        }

        private Uri BuildUri(IRestRequest request)
        {
            string resource = request.Resource;
            resource = request.Parameters.Where(delegate(Parameter p) { return p.Type == ParameterType.UrlSegment; })
                .Aggregate(
                    resource,
                    (current, p) => current.Replace("{" + p.Name + "}", p.Value.ToString().UrlEncode()));
            return new Uri($"{baseUrl}/{resource}");
        }

        private string GenerateSignature(IRestRequest request)
        {
            if (SignatureMethod == "PLAINTEXT")
            {
                return consumerSecret + "&" + tokenSecret;
            }

            Uri uri = BuildUri(request);
            string str = string.Format("{0}://{1}", uri.Scheme, uri.Host);
            if ((uri.Scheme != "http" || uri.Port != 80) && (uri.Scheme != "https" || uri.Port != 0x1bb))
            {
                str = str + ":" + uri.Port;
            }

            str = str + uri.AbsolutePath;
            string str2 = NormalizeRequestParameters(request.Parameters);

            var builder = new StringBuilder();
            builder.AppendFormat("{0}&", request.Method.ToString().ToUpper());
            builder.AppendFormat("{0}&", str.UrlEncode());
            builder.AppendFormat("{0}", str2.UrlEncode());

            string data = builder.ToString();
            var hashAlgorithm = new HMACSHA1
                                {
                                    Key = Encoding.UTF8.GetBytes($"{consumerSecret.UrlEncode()}&{(string.IsNullOrEmpty(tokenSecret) ? string.Empty : tokenSecret.UrlEncode())}")
                                };
            return ComputeHash(hashAlgorithm, data);
        }

        // Nested Types
        private class QueryParameterComparer : IComparer<Parameter>
        {
            // Methods
            public int Compare(Parameter x, Parameter y)
            {
                return x.Name == y.Name ? string.Compare(x.Value.ToString(), y.Value.ToString()) : string.Compare(x.Name, y.Name);
            }
        }
    }
}
