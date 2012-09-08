using System;

namespace Calendar
{
    public class AppointmentEventArgs : EventArgs
    {
        public AppointmentEventArgs( Appointment appointment )
        {
            Appointment = appointment;
        }

        public Appointment Appointment { get; private set; }
    }
}
