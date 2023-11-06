using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassScheduling
{
    public class InitialDataFactory
    {
        public InitialData GetInitialData() 
        { 
            var initialData = new InitialData();
            
            initialData.Rooms = GetRooms();
            initialData.Instructors = GetInstructors();
            initialData.Activities = GetActivities(initialData.Instructors);
            initialData.AvailableTimes = GetAvailableTimes();

            return initialData;
        }
        private IEnumerable<TimeOnly> GetAvailableTimes() 
        {
            return new List<TimeOnly>()
            {
                new TimeOnly(11,0),
                new TimeOnly(12,0),
                new TimeOnly(13,0),
                new TimeOnly(14,0),
                new TimeOnly(15,0)
            };
        }
        private IEnumerable<Instructor> GetInstructors()
        {
            return new List<Instructor>()
            {
                new Instructor("Lock"),
                new Instructor("Glen"),
                new Instructor("Banks"),
                new Instructor("Richards"),
                new Instructor("Shaw"),
                new Instructor("Singer"),
                new Instructor("Uther"),
                new Instructor("Tyler"),
                new Instructor("Numen"),
                new Instructor("Zeldin")
            }.OrderBy(i => i.Name);
        }

        public IEnumerable<Room> GetRooms()
        {
            return new List<Room>()
            {
                new Room("Slater 003", 45),
                new Room("Roman 216", 30),
                new Room("Loft 206", 75),
                new Room("Roman 201", 50),
                new Room("Loft 310", 108),
                new Room("Beach 201", 60),
                new Room("Beach 301", 75),
                new Room("Logos 325", 450),
                new Room("Frank 119", 60)
            }.OrderBy(r => r.Name);
        }

        private IEnumerable<Activity> GetActivities(IEnumerable<Instructor> instructors)
        {

            return new List<Activity>()
            {
                new Activity("SLA100A", 50,
                instructors.Where(i => i.Name.Equals("Glen")
                    || i.Name.Equals("Lock")
                    || i.Name.Equals("Banks")
                    || i.Name.Equals("Zeldin")),
                instructors.Where(i => i.Name.Equals("Numen")
                    || i.Name.Equals("Richards"))),
                new Activity("SLA100B", 50,
                instructors.Where(i => i.Name.Equals("Glen")
                    || i.Name.Equals("Lock")
                    || i.Name.Equals("Banks")
                    || i.Name.Equals("Zeldin")),
                instructors.Where(i => i.Name.Equals("Numen")
                    || i.Name.Equals("Richards"))),
                new Activity("SLA191A", 50,
                instructors.Where(i => i.Name.Equals("Glen")
                    || i.Name.Equals("Lock")
                    || i.Name.Equals("Banks")
                    || i.Name.Equals("Zeldin")),
                instructors.Where(i => i.Name.Equals("Numen")
                    || i.Name.Equals("Richards"))),
                new Activity("SLA191B", 50,
                instructors.Where(i => i.Name.Equals("Glen")
                    || i.Name.Equals("Lock")
                    || i.Name.Equals("Banks")
                    || i.Name.Equals("Zeldin")),
                instructors.Where(i => i.Name.Equals("Numen")
                    || i.Name.Equals("Richards"))),
                new Activity("SLA201", 50,
                instructors.Where(i => i.Name.Equals("Glen")
                    || i.Name.Equals("Banks")
                    || i.Name.Equals("Zeldin")
                    || i.Name.Equals("Shaw")),
                instructors.Where(i => i.Name.Equals("Numen")
                    || i.Name.Equals("Richards")
                    || i.Name.Equals("Singer"))),
                new Activity("SLA291", 50,
                instructors.Where(i => i.Name.Equals("Lock")
                    || i.Name.Equals("Banks")
                    || i.Name.Equals("Zeldin")
                    || i.Name.Equals("Singer")),
                instructors.Where(i => i.Name.Equals("Numen")
                    || i.Name.Equals("Richards")
                    || i.Name.Equals("Shaw")
                    || i.Name.Equals("Tyler"))),
                new Activity("SLA303", 60,
                instructors.Where(i => i.Name.Equals("Glen")
                    || i.Name.Equals("Banks")
                    || i.Name.Equals("Zeldin")),
                instructors.Where(i => i.Name.Equals("Numen")
                    || i.Name.Equals("Singer")
                    || i.Name.Equals("Shaw"))),
                new Activity("SLA304", 25,
                instructors.Where(i => i.Name.Equals("Glen")
                    || i.Name.Equals("Banks")
                    || i.Name.Equals("Tyler")),
                instructors.Where(i => i.Name.Equals("Numen")
                    || i.Name.Equals("Singer")
                    || i.Name.Equals("Shaw")
                    || i.Name.Equals("Richards")
                    || i.Name.Equals("Uther")
                    || i.Name.Equals("Zeldin"))),
                new Activity("SLA394", 20,
                instructors.Where(i => i.Name.Equals("Tyler")
                    || i.Name.Equals("Singer")),
                instructors.Where(i => i.Name.Equals("Richards")
                    || i.Name.Equals("Zeldin"))),
                new Activity("SLA449", 60,
                instructors.Where(i => i.Name.Equals("Tyler")
                    || i.Name.Equals("Singer")
                    || i.Name.Equals("Shaw")),
                instructors.Where(i => i.Name.Equals("Zuldin")
                    || i.Name.Equals("Uther"))),
                new Activity("SLA451", 100,
                instructors.Where(i => i.Name.Equals("Tyler")
                    || i.Name.Equals("Singer")
                    || i.Name.Equals("Shaw")),
                instructors.Where(i => i.Name.Equals("Zuldin")
                    || i.Name.Equals("Uther")
                    || i.Name.Equals("Richards")
                    || i.Name.Equals("Banks")))
            };
        }
    }
}
