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
        Done,
        Absent,
        Reported
    }
    public enum RoomStatus
    {
        Open,
        Closed
    }

}
