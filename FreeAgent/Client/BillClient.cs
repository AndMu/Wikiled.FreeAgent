using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using Wikiled.FreeAgent.Models;

namespace Wikiled.FreeAgent.Client
{
    public class BillClient : ResourceClient<BillWrapper, BillsWrapper, Bill>
    {
        public BillClient(FreeAgentClient client)
            : base(client)
        {
        }

        public override string ResourceName => "bills";

        public override List<Bill> ListFromWrapper(BillsWrapper wrapper)
        {
            return wrapper.bills;
        }

        public override Bill SingleFromWrapper(BillWrapper wrapper)
        {
            return wrapper.bill;
        }

        public override BillWrapper WrapperFromSingle(Bill single)
        {
            return new BillWrapper {bill = single};
        }

        /// <summary>
        ///     All the specified view, from_date and to_date.
        /// </summary>
        /// <param name='view'>
        ///     View. - recent or recurring
        /// </param>
        /// <param name='fromDate'>
        ///     From_date.
        /// </param>
        /// <param name='toDate'>
        ///     To_date.
        /// </param>
        public IObservable<Bill> All(string fromDate = "", string toDate = "")
        {
            return All(
                r =>
                {
                    if (!string.IsNullOrEmpty(fromDate))
                    {
                        r.AddParameter("from_date", fromDate, ParameterType.GetOrPost);
                    }

                    if (!string.IsNullOrEmpty(toDate))
                    {
                        r.AddParameter("to_date", toDate, ParameterType.GetOrPost);
                    }
                });
        }
    }
}
