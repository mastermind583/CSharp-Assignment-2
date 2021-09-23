using System;
using System.Collections.Generic;

namespace Library.TaskAppointmentManager
{
    public class Appointment : Item
    {
        public Appointment() : base()
        {
            Attendees = new List<string>();
        }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<string> Attendees { get; set; }

        
        public override string ToString()
        {
            string attendees = null;
            for (int i = 0; i < Attendees.Count; i++)
            {
                attendees += Attendees[i];
                if (i != Attendees.Count - 1)
                    attendees += ", ";
            }
            return $"ID: {Id} - TYPE: Appointment - NAME: {Name} - " +
                $"DESCRIPTION: {Description} - START DATE: {Start} - " +
                $"END DATE: {End} - ATTENDEES: {attendees}";
        }
    }
}
