using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassScheduling
{
    public class InitialData
    {
        public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<Room> Rooms { get; set; }
        public IEnumerable<Activity> Activities { get; set; }
        public IEnumerable<TimeOnly> AvailableTimes { get; set; }

    }
}
