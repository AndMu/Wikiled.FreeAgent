namespace Wikiled.FreeAgent.Models
{
    public class InvoiceViewFilter
    {
        public static string Draft = "draft";

        public static string OpenOrOverdue = "open_or_overdue";

        public static string RecentOpenOrOverdue = "recent_open_or_overdue";

        public static string ReminderEmails = "reminder_emails";

        public static string ScheduledToEmail = "scheduled_to_email";

        public static string ThankYouEmails = "thank_you_emails";

        private static readonly string lastNMonths = "last_{0}_months";

        public static string LastNMonths(int months = 1)
        {
            return string.Format(lastNMonths, months);
        }
    }
}