using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data;

namespace Calendar
{
	public class Appointment : iAppointment
    {

		#region " Private Fields "
		private int layer;
		private string group;
		private DateTime startDate;
		private DateTime endDate;
		private bool locked;
		private Color color = Color.White;
		private Color textColor = Color.Black;
		private int appointmentId = 0;
		private Color borderColor = Color.Blue;
		private bool drawBorder = false;
		private string subject = string.Empty;
		private bool allDayEvent = false;
		private bool reccuringEvent = false;
		internal int conflictCount;
		#endregion

        public Appointment()
        {
			color = Color.White;
			borderColor = Color.Blue;
			Subject = "New Appointment";
		}

		#region " Public Properties "
		public int Layer
        {
            get { return layer; }
            set { layer = value; }
        }

        public string Group
        {
            get { return group; }
            set { group = value; }
        }
       
        [System.ComponentModel.DefaultValue(false)]
        public bool Locked
        {
            get
            {
                return locked;
            }
            set
            {
                locked = value;
                OnLockedChanged();
            }
        }

        public Color Color
		{
			get
			{
				return color;
			}
			set
			{
				color = value;
				OnColorChanged();
			}
		}

        public Color TextColor
        {
            get
            {
                return textColor;
            }
            set
            {
                textColor = value;
                OnTextColorChanged();
            }
        }

		public Color BorderColor
		{
			get
			{
				return borderColor;
			}
			set
			{
				borderColor = value;
				OnBorderColorChanged();
			}
		}
        
		public bool DrawBorder
        {
            get
            {
                return drawBorder;
            }
            set
            {
                drawBorder = value;
            }
        }

        public bool AllDayEvent
        {
            get
            {
                return allDayEvent;
            }
            set
            {
                allDayEvent = value;
                OnAllDayEventChanged();
            }
        }

		public bool Recurring
		{
			get
			{
				return reccuringEvent;
			}
			set
			{
				reccuringEvent = value;
			}
		}
		#endregion

		#region " Events "
		protected virtual void OnStartDateChanged()
		{
		}
		protected virtual void OnEndDateChanged()
		{
		}
		protected virtual void OnLockedChanged()
		{
		}
		protected virtual void OnColorChanged()
		{
		}
		protected virtual void OnTextColorChanged()
		{
		}
		protected virtual void OnBorderColorChanged()
		{
		}
		protected virtual void OnTitleChanged()
		{
		}
		protected virtual void OnAllDayEventChanged()
		{
		}
		#endregion

		#region iAppointment Members
		public DateTime StartDate
		{
			get
			{
				return startDate;
			}
			set
			{
				startDate = value;
				OnStartDateChanged();

			}
		}

		public DateTime EndDate
		{
			get
			{
				return endDate;
			}
			set
			{
				endDate = value;
				OnEndDateChanged();
			}
		}

		public int AppointmentId
		{
			get
			{
				return appointmentId;
			}
			set
			{
				appointmentId = value;
			}
		}

		public string Subject
		{
			get
			{
				return subject;
			}
			set
			{
				subject = value;
			}
		}

		public string Location { get; set; }

		public string Note { get; set; }
		#endregion
	}
}
