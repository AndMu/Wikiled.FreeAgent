﻿using System;
using System.Net;
using RestSharp;

namespace Wikiled.FreeAgent.Exceptions
{
    public class FreeAgentException : Exception
    {
        public string Errors = "";

        public FreeAgentException()
        {
        }

        public FreeAgentException(string message)
            : base(message)
        {
        }

        public FreeAgentException(IRestResponse r)
        {
            Response = r;
            StatusCode = r.StatusCode;

            try
            {
                var json = Response.Content;
                if (json.Contains("\"errors\""))
                {
                    Errors = json;
                }
            }
            catch
            {
                //do nothing
            }
        }

        /// <summary>
        ///     The response of the error call (for Debugging use)
        /// </summary>
        public IRestResponse Response { get; }

        public HttpStatusCode StatusCode { get; set; }

        public override string ToString()
        {
            return string.Format("[FreeAgentException: StatusCode={0}, Response={1}, Content={2}]", StatusCode, Response, Response.Content);
        }
    }
}
