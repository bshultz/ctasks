using System;
using System.Collections.Generic;
using System.Text;

namespace Calendar
{
	interface iAppointment
	{
		int AppointmentId { get; set; }
		DateTime StartDate { get; set; }
		DateTime EndDate { get; set; }
		string Subject { get; set; }
		string Location { get; set; }
		string Note { get; set; }
	}
}
