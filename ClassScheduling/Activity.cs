using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassScheduling
{
    public class Activity
    {
        public Activity(string name, int enrollment, 
            IEnumerable<Instructor> preferredInstructors, 
            IEnumerable<Instructor> availableInstructors)
        {
            Name = name;
            Enrollment = enrollment;
            PreferredInstructors = preferredInstructors;
            OtherInstructors = availableInstructors;
        }

        public string Name { get;}
        public int Enrollment {  get; }
        public IEnumerable<Instructor> OtherInstructors { get; }
        public IEnumerable<Instructor> PreferredInstructors { get; }
        public IEnumerable<Instructor> AvailableInstructors 
        {
            get
            {
                return Enumerable.Concat(OtherInstructors,PreferredInstructors);
            }
        }
    }
}
