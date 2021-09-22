using System;
using System.Collections.Generic;

namespace Library.TaskAppointmentManager
{
    public class Appointment : Item
    {
        public Appointment() : base()
        {
        }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<string> Attendees { get; set; }

        
        public override string ToString()
        {
            return $"ID: {Id} - TYPE: Appointment - NAME: {Name} - " +
                $"DESCRIPTION: {Description} - START DATE: {Start} - " +
                $"END DATE: {End} - ATTENDEES: {Attendees}";
        }
    }
}
