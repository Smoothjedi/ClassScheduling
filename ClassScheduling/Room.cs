using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassScheduling
{
    public class Room
    {
        public Room(string name, int capacity)
        {
            Name = name;
            Capacity = capacity;
        }

        public string Name { get; }
        public int Capacity { get; }
    }
}
