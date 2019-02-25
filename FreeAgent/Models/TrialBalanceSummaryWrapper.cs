using System.Collections.Generic;

namespace Wikiled.FreeAgent.Models
{
    public class TrialBalanceSummaryWrapper
    {
        public TrialBalanceSummaryWrapper()
        {
            trial_balance_summary = new List<TrialBalanceSummary>();
        }

        public List<TrialBalanceSummary> trial_balance_summary { get; set; }
    }
}