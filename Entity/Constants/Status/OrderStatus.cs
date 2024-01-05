using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Constants.Status
{
    public static class OrderStatus
    {
        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusInProcess = "Processing";
        public const string StatusShipped = "Shipped";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";
        public const string StatusCompleted = "Completed";
        public static bool CanBeCancelled(this string status) => status == StatusPending || status == StatusApproved || status == StatusInProcess;
        public static readonly string[] Collection =
        {
            StatusPending,
            StatusApproved,
            StatusInProcess,
            StatusShipped,
            StatusCompleted,
            StatusCancelled,
        };
    }
}
