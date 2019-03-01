using RestSharp;
using System;
using System.Collections.Generic;
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

        public IObservable<Timeslip> All(string fromDate, string toDate)
        {
            return All(
                r =>
                {
                    r.AddParameter("from_date", fromDate, ParameterType.GetOrPost);
                    r.AddParameter("to_date", toDate, ParameterType.GetOrPost);
                });
        }

        public IObservable<Timeslip> AllRecent()
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
