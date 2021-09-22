using System;

namespace Library.TaskAppointmentManager
{
    public class Task : Item
    {
        public Task() : base()
        {
        }

        public DateTime Deadline { get; set; }
        public bool IsCompleted { get; set; }
        public override string ToString()
        {
            return $"ID: {Id} - DEADLINE: {Deadline} - NAME: {Name} - DESCRIPTION: {Description} - COMPLETED: {IsCompleted}";
        }
    }
}