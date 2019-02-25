using System;
using System.Collections.Generic;
using RestSharp;
using Wikiled.FreeAgent.Extensions;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.Client
{
    public class TimeslipClient : ResourceClient<TimeslipWrapper, TimeslipsWrapper, Timeslip>
    {
        public TimeslipClient(FreeAgentClient client)
            : base(client)
        {
        }

        //need to add in the GET to have a parameter for the date filter

        public override string ResourceName => "timeslips";

        public override List<Timeslip> ListFromWrapper(TimeslipsWrapper wrapper)
        {
            return wrapper.timeslips;
        }

        public override Timeslip SingleFromWrapper(TimeslipWrapper wrapper)
        {
            return wrapper.timeslip;
        }

        public override TimeslipWrapper WrapperFromSingle(Timeslip single)
        {
            return new TimeslipWrapper {timeslip = single};
        }

        public List<Timeslip> All(string from_date, string to_date)
        {
            return All(
                r =>
                {
                    r.AddParameter("from_date", from_date, ParameterType.GetOrPost);
                    r.AddParameter("to_date", to_date, ParameterType.GetOrPost);
                });
        }

        public List<Timeslip> AllRecent()
        {
            DateTime now = DateTime.Now;

            return All(
                r =>
                {
                    r.AddParameter("from_date", now.AddDays(-20).ModelDate(), ParameterType.GetOrPost);
                    r.AddParameter("to_date", now.ModelDate(), ParameterType.GetOrPost);
                });
        }
    }
}
