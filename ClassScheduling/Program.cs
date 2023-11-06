using ClassScheduling;
using System;
using System.Collections.Generic;

namespace ClassScheduling
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set your population size and mutation rate
            int populationSize = 1500;
            double mutationRate = 0.01;
            var initialDataFactory = new InitialDataFactory();
            var initialData = initialDataFactory.GetInitialData();

            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(populationSize, mutationRate, initialData);

            var schedules = geneticAlgorithm.GenerateSchedules();
            // Set the number of generations
            int generations = 100;

            // Run the genetic algorithm
            Schedule bestSchedule = geneticAlgorithm.Run(generations, schedules);

            // Print the best schedule and its fitness value
            Console.WriteLine("Best Schedule:");
            foreach (var classId in bestSchedule.ClassSchedule.Keys)
            {
                var room = bestSchedule.ClassSchedule[classId].Item1;
                var timeSlot = bestSchedule.ClassSchedule[classId].Item2;
                var instructor = bestSchedule.ClassSchedule[classId].Item3;
                Console.WriteLine($"Class {classId.Name}: Room {room.Name}, Time Slot {timeSlot}, Instructor {instructor.Name}");
            }
            var instructorCounts = new Dictionary<string, int>();
            foreach (var scheduleTuples in bestSchedule.ClassSchedule.Values)
            {
                if (!instructorCounts.TryAdd(scheduleTuples.Item3.Name, 1))
                {
                    instructorCounts[scheduleTuples.Item3.Name]++;
                }
            }
            foreach (var i in instructorCounts.Keys)
            {
                Console.WriteLine($"Instructor: {i} Number of Classes: {instructorCounts[i]}");
            }
            Console.WriteLine($"Fitness: {bestSchedule.Fitness}");
        }
    }
}