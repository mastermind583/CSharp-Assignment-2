using System;

namespace Library.TaskAppointmentManager
{
    public class Task
    {
        private static int currentId = 1;

        public Task()
        {
            Id = currentId++;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsCompleted { get; set; }
        public override string ToString()
        {
            return $"ID: {Id} - DEADLINE: {Deadline} - NAME: {Name} - DESCRIPTION: {Description} - COMPLETED: {IsCompleted}";
        }
    }
}