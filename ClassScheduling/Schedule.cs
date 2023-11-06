using ClassScheduling;

public class Schedule
{
    // Representation of the schedule (room, time slot) for each class
    public Dictionary<Activity, Tuple<Room,TimeOnly, Instructor>> ClassSchedule { get; set; }
    public double Fitness { get; set; }

    // Constructor to initialize the class schedule
    public Schedule()
    {
        ClassSchedule = new Dictionary<Activity, Tuple<Room, TimeOnly, Instructor>>();
    }
}
