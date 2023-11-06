using System.Collections.Generic;
using System.Diagnostics;

namespace ClassScheduling
{
    public class GeneticAlgorithm
    {
        //private List<Schedule> population;
        private int populationSize;
        private double mutationRate;
        InitialData InitialData;

        // Constructor
        public GeneticAlgorithm(int populationSize, double mutationRate, InitialData initialData)
        {
            this.populationSize = populationSize;
            this.mutationRate = mutationRate;
            this.InitialData = initialData;
        }

        // Initialize the population with random schedules
        public List<Schedule> GenerateSchedules()
        {
            var population = new List<Schedule>();
            Random random = new Random();

            for (int i = 0; i < populationSize; i++)
            {
                Schedule schedule = new Schedule();

                // Generate random schedule for each class and assign instructors
                foreach (var activity in InitialData.Activities) // Replace with your list of class IDs
                {
                    var room = InitialData.Rooms.RandomElement(random);

                    var timeSlot = InitialData.AvailableTimes.RandomElement(random);

                    var instructor = InitialData.Instructors.RandomElement(random);

                    schedule.ClassSchedule.Add(activity, new Tuple<Room, TimeOnly, Instructor>(room, timeSlot, instructor));
                }

                population.Add(schedule);
            }
            return population;
        }

        // Evaluate the fitness function for a schedule
        private List<Schedule> EvaluateFitness(List<Schedule> schedules)
        {
            foreach (var schedule in schedules)
            {
                var fitness = 0.0;

                var existingActivityTimesAndRooms = new List<Tuple<Room, TimeOnly>>();
                var existingInstructorTimeSlots = new List<Tuple<Instructor, TimeOnly, Activity>>();

                //Implement your fitness function here based on the variables provided, including instructor preferences
                foreach (var activity in schedule.ClassSchedule.Keys)
                {
                    var room = schedule.ClassSchedule[activity].Item1;
                    var timeSlot = schedule.ClassSchedule[activity].Item2;
                    var instructor = schedule.ClassSchedule[activity].Item3;

                    if (existingActivityTimesAndRooms.Exists(x => x.Item1.Equals(room) && x.Item2.Equals(timeSlot)))
                    {
                        fitness += -0.5;
                    }
                    else
                    {
                        existingActivityTimesAndRooms.Add(new Tuple<Room, TimeOnly>(room, timeSlot));
                    }
                    //Room size logic
                    if (room.Capacity < activity.Enrollment) fitness += -0.5;
                    else if (room.Capacity > 6 * activity.Enrollment) fitness += -0.2;
                    else if (room.Capacity > 3 * activity.Enrollment) fitness += -0.4;
                    else fitness += 0.3;

                    //Listed instructor
                    if (activity.PreferredInstructors.Contains(instructor)) fitness += 0.5;
                    else if (activity.OtherInstructors.Contains(instructor)) fitness += 0.2;
                    else fitness += -0.2;

                    //Instructor load
                    existingInstructorTimeSlots.Add(new Tuple<Instructor, TimeOnly, Activity>(instructor, timeSlot, activity));
                    // Update fitness based on the above considerations
                }

                fitness += CheckSLACourses(schedule);

                fitness += CheckInstuctorAssignments(existingInstructorTimeSlots);


                // Update schedule.Fitness property with the calculated fitness value
                schedule.Fitness = fitness;

            }
            return schedules;
        }

        private double CheckInstuctorAssignments(List<Tuple<Instructor, TimeOnly, Activity>> existingInstructorTimeSlots)
        {
            var fitness = 0.0;
            //Get list of lists of classes grouped by instructor
            var groupedInstructorLists = existingInstructorTimeSlots.GroupBy(x => x.Item1)
                .Select(grp => grp.OrderBy(y => y.Item2).ToList()).ToList();

            foreach (var activities in groupedInstructorLists)
            {
                if (activities.Count > 4) fitness += -0.5;
                if ((activities.Count.Equals(1) || activities.Count.Equals(2))
                    && !activities.First().Item1.Name.Equals("Tyler"))
                {
                    fitness += -0.4;
                }
                var overlappingActivities = 0;

                for (int i = 0; i < activities.Count - 1; i++)
                {
                    if (activities[i].Equals(activities[i + 1]))
                        overlappingActivities++;
                    if (activities[i].Item2.AddHours(1).Equals(activities[i + 1]))
                    {
                        fitness += 0.5;
                        if (activities[i].Item3.Name.Equals("Roman") || activities[i].Item3.Name.Equals("Beach")
                            && (activities[i].Item3.Name.Equals("Roman") || activities[i].Item3.Name.Equals("Beach")))
                        {
                            fitness += -0.4;
                        }
                    }
                }
                if (overlappingActivities > 0) { fitness += -0.2; }
                else { fitness += 0.2; }
            }

            return fitness;
        }

        private double CheckSLACourses(Schedule schedule)
        {
            var fitness = 0.0;
            var sla100A = schedule.ClassSchedule[schedule.ClassSchedule.Keys.Where(x => x.Name.Equals("SLA100A")).First()];
            var sla100B = schedule.ClassSchedule[schedule.ClassSchedule.Keys.Where(x => x.Name.Equals("SLA100B")).First()];
            var sla191A = schedule.ClassSchedule[schedule.ClassSchedule.Keys.Where(x => x.Name.Equals("SLA191A")).First()];
            var sla191B = schedule.ClassSchedule[schedule.ClassSchedule.Keys.Where(x => x.Name.Equals("SLA191B")).First()];

            //SLA sections of the same class are at the same time
            if (CheckForDifferenceEqualsTimeSpan(sla100A.Item2, sla100B.Item2, new TimeSpan(0, 0, 0)))
            {
                fitness += -0.5;
            }
            if (CheckForDifferenceEqualsTimeSpan(sla191A.Item2, sla191B.Item2, new TimeSpan(0, 0, 0)))
            {
                fitness += -0.5;
            }
            //SLA sections of the same class are more than four hours apart
            if (CheckForDifferenceGreaterThanTimeSpan(sla100A.Item2, sla100B.Item2, new TimeSpan(4, 0, 0)))
            {
                fitness += -0.5;
            }
            if (CheckForDifferenceGreaterThanTimeSpan(sla191A.Item2, sla191B.Item2, new TimeSpan(4, 0, 0)))
            {
                fitness += -0.5;
            }
            //SLA sections of different classes are two hours apart
            if (CheckForDifferenceEqualsTimeSpan(sla100A.Item2, sla191A.Item2, new TimeSpan(2, 0, 0)))
            {
                fitness += 0.25;
            }
            if (CheckForDifferenceEqualsTimeSpan(sla100A.Item2, sla191B.Item2, new TimeSpan(2, 0, 0)))
            {
                fitness += 0.25;
            }
            if (CheckForDifferenceEqualsTimeSpan(sla100B.Item2, sla191A.Item2, new TimeSpan(2, 0, 0)))
            {
                fitness += 0.25;
            }
            if (CheckForDifferenceEqualsTimeSpan(sla100B.Item2, sla191B.Item2, new TimeSpan(2, 0, 0)))
            {
                fitness += 0.25;
            }
            //SLA sections of different classes are at the same time
            if (CheckForDifferenceEqualsTimeSpan(sla100A.Item2, sla191A.Item2, new TimeSpan(0, 0, 0)))
            {
                fitness += -0.25;
            }
            if (CheckForDifferenceEqualsTimeSpan(sla100A.Item2, sla191B.Item2, new TimeSpan(0, 0, 0)))
            {
                fitness += -0.25;
            }
            if (CheckForDifferenceEqualsTimeSpan(sla100B.Item2, sla191A.Item2, new TimeSpan(0, 0, 0)))
            {
                fitness += -0.25;
            }
            if (CheckForDifferenceEqualsTimeSpan(sla100B.Item2, sla191B.Item2, new TimeSpan(0, 0, 0)))
            {
                fitness += -0.25;
            }
            //SLA sections of different classes are consecutive. Also checks if they are near each other
            if (CheckForDifferenceEqualsTimeSpan(sla100A.Item2, sla191A.Item2, new TimeSpan(1, 0, 0)))
            {
                fitness += 0.5;
                if (CheckForDisperateRooms(sla100A.Item1, sla191A.Item1))
                {
                    fitness += -0.4;
                };
            }
            if (CheckForDifferenceEqualsTimeSpan(sla100A.Item2, sla191B.Item2, new TimeSpan(1, 0, 0)))
            {
                fitness += 0.5;
                if (CheckForDisperateRooms(sla100A.Item1, sla191B.Item1))
                {
                    fitness += -0.4;
                };
            }
            if (CheckForDifferenceEqualsTimeSpan(sla100B.Item2, sla191A.Item2, new TimeSpan(1, 0, 0)))
            {
                fitness += 0.5;
                if (CheckForDisperateRooms(sla100B.Item1,sla191A.Item1))
                {
                    fitness += -0.4;
                }
            }
            if (CheckForDifferenceEqualsTimeSpan(sla100B.Item2, sla191B.Item2, new TimeSpan(1, 0, 0)))
            {
                fitness += 0.5;
                if (CheckForDisperateRooms(sla100B.Item1, sla191B.Item1))
                {
                    fitness += -0.4;
                }
            }

            return fitness;
        }

        private static bool CheckForDisperateRooms(Room firstRoom, Room secondRoom)
        {
            return (firstRoom.Name.Contains("Roman") && !(secondRoom.Name.Contains("Roman") || secondRoom.Name.Contains("Beach")))
                || (firstRoom.Name.Contains("Beach") && !(secondRoom.Name.Contains("Roman") || secondRoom.Name.Contains("Beach")))
                || (secondRoom.Name.Contains("Roman") && !(firstRoom.Name.Contains("Roman") || firstRoom.Name.Contains("Beach")))
                || (secondRoom.Name.Contains("Beach") && !(firstRoom.Name.Contains("Roman") || firstRoom.Name.Contains("Beach")));
        }

        private bool CheckForDifferenceEqualsTimeSpan(TimeOnly timeOne, TimeOnly timeTwo, TimeSpan timeSpan)
        {
            return (timeOne > timeTwo && (timeOne - timeTwo).Equals(timeSpan)
            || (timeTwo > timeOne && (timeTwo - timeOne).Equals(timeSpan)));
        }

        private bool CheckForDifferenceGreaterThanTimeSpan(TimeOnly timeOne, TimeOnly timeTwo, TimeSpan timeSpan)
        {
            return (timeOne > timeTwo && (timeOne - timeTwo) > timeSpan)
            || (timeTwo > timeOne && timeTwo - timeOne > timeSpan);
        }

        // Perform crossover operation using parents selected via Fitness Proportionate Selection
        private Schedule Crossover(Schedule parent1, Schedule parent2)
        {
            Schedule offspring = new Schedule();
            Random random = new Random();

            foreach (var activity in parent1.ClassSchedule.Keys)
            {
                // Select genes (room and time slot) from either parent with equal probability
                if (random.NextDouble() < 0.5)
                {
                    offspring.ClassSchedule[activity] = parent1.ClassSchedule[activity];
                }
                else
                {
                    offspring.ClassSchedule[activity] = parent2.ClassSchedule[activity];
                }
            }

            return offspring;
        }

        // Perform mutation operation on a schedule
        private void Mutate(Schedule schedule)
        {
            // Implement mutation logic to modify the schedule
            // You can randomly change room or time slot for a class based on the mutation rate
            Random random = new Random();

            var randomActivity = random.Next(schedule.ClassSchedule.Count);
            var activity = schedule.ClassSchedule.Skip(randomActivity).First().Key;

            var randomRoomIndex = random.Next(InitialData.Rooms.Count());
            var randomRoom = InitialData.Rooms.Skip(randomRoomIndex).First();

            var randomTimeIndex = random.Next(InitialData.AvailableTimes.Count());
            var randomTime = InitialData.AvailableTimes.Skip(randomTimeIndex).First();

            var randomInstructorIndex = random.Next(InitialData.Instructors.Count());
            var randomInstructor = InitialData.Instructors.Skip(randomInstructorIndex).First();

            var mutatedActivityKey = new Tuple<Room, TimeOnly, Instructor>(randomRoom, randomTime, randomInstructor);

            schedule.ClassSchedule.Remove(activity);
            schedule.ClassSchedule.Add(activity, mutatedActivityKey);            
        }

        // Run the genetic algorithm for a specified number of generations
        public Schedule Run(int generations, List<Schedule> population)
        {
            for (int generation = 0; generation < generations; generation++)
            {
                // Evaluate fitness for each schedule in the population
                EvaluateFitness(population);
                
                // Sort the population by fitness in descending order
                population.Sort((a, b) => b.Fitness.CompareTo(a.Fitness));
                Console.WriteLine($"Top fitness value: {population[0].Fitness}");
                // Perform selection, crossover, and mutation to create the next generation
                List<Schedule> newPopulation = new List<Schedule>();

                for (int i = 0; i < population.Count; i++)
                {
                    var parents = SelectParents(population);
                    Schedule parent1 = parents.Item1;
                    Schedule parent2 = parents.Item2;

                    Schedule offspring = Crossover(parent1, parent2);

                    // Apply mutation based on mutation rate
                    if (mutationRate > 0 && new Random().NextDouble() < mutationRate)
                    {
                        Mutate(offspring);
                    }

                    newPopulation.Add(offspring);
                }

                population = newPopulation;
            }

            EvaluateFitness(population);
            // Return the best schedule after running the genetic algorithm
            return population.OrderByDescending(x=>x.Fitness).First();
        }
        // Select parents using Fitness Proportionate Selection
        private Tuple<Schedule, Schedule> SelectParents(List<Schedule> population)
        {
            double totalFitness = population.Sum(schedule => schedule.Fitness);
            double randomValue1 = new Random().NextDouble() * totalFitness;
            double randomValue2 = new Random().NextDouble() * totalFitness;

            double sumFitness = 0;
            Schedule parent1 = null;
            Schedule parent2 = null;

            foreach (var schedule in population)
            {
                sumFitness += schedule.Fitness;
                if (sumFitness >= randomValue1 && parent1 == null)
                {
                    parent1 = schedule;
                }
                if (sumFitness >= randomValue2 && parent2 == null)
                {
                    parent2 = schedule;
                }
                if (parent1 != null && parent2 != null)
                {
                    break;
                }
            }

            return new Tuple<Schedule, Schedule>(parent1, parent2);
        }
    }
}