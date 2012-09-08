using System;
using System.Collections.Generic;

namespace Calendar
{
    public class ResolveAppointmentsEventArgs : EventArgs
    {
        public ResolveAppointmentsEventArgs(DateTime start, DateTime end)
        {
            StartDate = start;
            EndDate = end;
            Appointments = new List<Appointment>();
        }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public List<Appointment> Appointments { get; set; }
    }
}
