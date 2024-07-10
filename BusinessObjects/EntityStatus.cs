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
        Reported
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
}
