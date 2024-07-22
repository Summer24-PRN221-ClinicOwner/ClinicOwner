using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public static class EntityStatus
    {
        
    }
    public enum AppointmentStatus
    {
        Waiting,
        Canceled,
        ReScheduled,
        Checkin,
        Absent,
        Reported,
        LateCanceled
           
    }
    public enum RoomStatus
    {
        Closed,
        Open
    }
    public enum ServiceStatus
    {
        Unavailable,
        Available
    }
    public enum UserStatus
    {
        Disabled,
        Active
    }
    public static class PaymentStatus
    {
        public const string PAID= "Paid";
        public const string CHECKOUT= "Checkout";
        public const string REFUNDED= "Refunded";
        public const string CHECKOUT_REFUNDED= "Checkout_Refunded";
    }
}
