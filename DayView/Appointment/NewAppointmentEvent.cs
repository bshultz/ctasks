using System;

namespace Calendar
{
    public class NewAppointmentEventArgs : EventArgs
    {
        public NewAppointmentEventArgs(string title, DateTime start, DateTime end)
        {
            Title = title;
            StartDate = start;
            EndDate = end;
        }

        public string Title { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }
    }
}
