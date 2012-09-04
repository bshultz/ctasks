using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Calendar;
using System.Runtime.InteropServices;
using Microsoft.Win32.TaskScheduler;

namespace CalendarTest
{
    public partial class Form1 : Form
    {
       
        List<Calendar.Appointment> m_Appointments;

        public Form1()

        {
            
            InitializeComponent();

			this.cmbbxInterval.DataSource = Enum.GetValues(typeof(Calendar.AppointmentSlotDuration));

            string user = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            //bool preWin7 = true;




            #region " Add Some Appointments "
            m_Appointments = new List<Appointment>();

            DateTime m_Date = DateTime.Now; //test datetime            
            String dateShortString = m_Date.ToShortDateString();  //!!Used to set today as the context from which to show tasks

            m_Date = m_Date.AddHours(10 - m_Date.Hour); //test datetime
            m_Date = m_Date.AddMinutes(-m_Date.Minute); //test datetime

            //Appointment m_Appointment = new Appointment(); //moved

         //   m_Appointment.StartDate = m_Date;
          //  m_Appointment.EndDate = m_Date.AddMinutes(10);
          //  m_Appointment.Subject = "test1\r\nmultiline";

            //m_Appointments.Add(m_Appointment);  //moved

            

            using (TaskService ts = new TaskService())
            {
               // DateTime t_Date; //trigger datetime

                TaskFolder tf = ts.RootFolder;
                 Version ver = ts.HighestSupportedVersion;
                 

                
                
                bool newVer = (ver >= new Version(1, 2));
                
                       foreach (Task t in tf.Tasks)
                        try         {
                            //m_Appointment = new Appointment();
                            //m_Appointment.StartDate = m_Date.AddHours(2);
                            //m_Appointment.EndDate = m_Date.AddHours(3);

                            String trig = t.Definition.Triggers.ToString();                            

                           // Console.WriteLine("+ {0}, {1} ({2})", t.Name, t.Definition.RegistrationInfo.Author, t.State);

                            

                            foreach (Trigger trg in t.Definition.Triggers)
                            {

                                for (int i = 0; i < t.Definition.Triggers.Count; i++)
                                {
                                    Appointment m_Appointment = new Appointment();

                                    DateTime[] dateTimes = new DateTime[t.Definition.Triggers.Count];
                                    dateTimes[i] = Convert.ToDateTime(dateShortString + " " + trg.StartBoundary.TimeOfDay);
                                    //dateTimes[i] = trg.StartBoundary.Date.ToLocalTime();



                                   // t_Date = Convert.ToDateTime(trg.ToString());

                                   // t_Date = Convert.ToDateTime("7/15/2012 " + trg.StartBoundary.TimeOfDay);

                                      m_Appointment.StartDate = dateTimes[i];
                                      m_Appointment.EndDate = dateTimes[i].AddMinutes(10);
                                      m_Appointment.Subject = t.Name; // "test1\r\nmultiline";


                                    m_Appointments.Add(m_Appointment);
                                }

                            }

                           foreach (Microsoft.Win32.TaskScheduler.Action act in t.Definition.Actions)
                                   Console.WriteLine(" = {0}", act);
                                     }
                        catch { }
                                   
        }

/*
            m_Appointments.Add(m_Appointment);

            m_Date = m_Date.AddDays(1);

            m_Appointment = new Appointment();
            m_Appointment.StartDate = m_Date.AddHours(2);
            m_Appointment.EndDate = m_Date.AddHours(3);
            m_Appointment.Subject = "test2\r\n locked one";
            m_Appointment.Color = System.Drawing.Color.LightBlue;
            m_Appointment.Locked = true;

            m_Appointments.Add(m_Appointment);

            m_Date = m_Date.AddDays(-1);

            m_Appointment = new Appointment();
            m_Appointment.StartDate = m_Date;
            m_Appointment.EndDate = m_Date.AddHours(4);
            m_Appointment.EndDate = m_Appointment.EndDate.AddMinutes(15);
            m_Appointment.Color = System.Drawing.Color.Yellow;
			m_Appointment.Subject = "test3\r\n some numbers 123456 and unicode chars (Russian) –усский текст and (Turkish) рьёЁцз÷зiЁ";

            m_Appointments.Add(m_Appointment);

            m_Appointment = new Appointment();
            m_Appointment.StartDate = m_Date;
            m_Appointment.EndDate = m_Date.AddDays(2);
			m_Appointment.Subject = "More than one day";
            m_Appointment.AllDayEvent = true;
            m_Appointment.Color = System.Drawing.Color.Red;

            m_Appointments.Add(m_Appointment);

            m_Appointment = new Appointment();
            m_Appointment.StartDate = m_Date.AddDays(2);
            m_Appointment.EndDate = m_Date.AddDays(4);
			m_Appointment.Subject = "More than one day (2)";
            m_Appointment.AllDayEvent = true;
            m_Appointment.Color = System.Drawing.Color.Coral;

            m_Appointments.Add(m_Appointment);

            m_Appointment = new Appointment();
            m_Appointment.StartDate = m_Date;
            m_Appointment.EndDate = m_Date.AddDays(4);
			m_Appointment.Subject = "More than one day (3)";
            m_Appointment.AllDayEvent = true;
            m_Appointment.Color = System.Drawing.Color.Red;

            m_Appointments.Add(m_Appointment);
            */
			#endregion

			dayView1.StartDate = DateTime.Now;
			dayView1.OnNewAppointment += new EventHandler<NewAppointmentEventArgs>(dayView1_NewAppointment);
            dayView1.OnSelectionChanged += new EventHandler<EventArgs>(dayView1_SelectionChanged);
			dayView1.OnResolveAppointments += new EventHandler<ResolveAppointmentsEventArgs>(this.dayView1_ResolveAppointments);

            dayView1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dayView1_MouseMove);

            comboBox1.SelectedIndex = 1;

			this.cmbbxInterval.SelectedItem = dayView1.AppointmentDuration;

			// get process
			m_processHandle = GetCurrentProcess();

			// update status
			OnTimer_Tick(this, null);

			//m_timer = new System.Timers.Timer(500);
			//m_timer.Elapsed += new System.Timers.ElapsedEventHandler(m_timer_Elapsed);
        }

        void dayView1_NewAppointment(object sender, NewAppointmentEventArgs args)
        {
            Appointment m_Appointment = new Appointment();

            m_Appointment.StartDate = args.StartDate;
            m_Appointment.EndDate = args.EndDate;
			m_Appointment.Subject = args.Title;
            m_Appointment.Group = "2";

            m_Appointments.Add(m_Appointment);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //dayView1.DaysToShow = int.Parse( textBox1.Text );
        }

        private void dayView1_MouseMove(object sender, MouseEventArgs e)
        {
            label2.Text = dayView1.GetTimeAt(e.X, e.Y).ToString();
        }

        private void dayView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dayView1.Selection == SelectionType.DateRange)
            {
                label3.Text = dayView1.SelectionStart.ToString() + ":" + dayView1.SelectionEnd.ToString();
            }
            else if (dayView1.Selection == SelectionType.Appointment)
            {
                label3.Text = dayView1.SelectedAppointment.StartDate.ToString() + ":" + dayView1.SelectedAppointment.EndDate.ToString();
            }
        }

        private void dayView1_ResolveAppointments(object sender, ResolveAppointmentsEventArgs args)
        {
            List<Appointment> m_Apps = new List<Appointment>();

            foreach (Appointment m_App in m_Appointments)
                if ((m_App.StartDate >= args.StartDate) &&
                    (m_App.StartDate <= args.EndDate))
                    m_Apps.Add(m_App);

            args.Appointments = m_Apps;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Appointment m_App = new Appointment();
            m_App.StartDate = dayView1.SelectionStart;
            m_App.EndDate = dayView1.SelectionEnd;
            m_App.BorderColor = Color.Red;

            m_Appointments.Add(m_App);

            dayView1.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            dayView1.DaysToShow = 1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dayView1.DaysToShow = 3;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dayView1.DaysToShow = 5;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dayView1.DaysToShow = 7;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() == "Office 11")
            {
                dayView1.Renderer = new Office11Renderer();
            }
            else
            {
                dayView1.Renderer = new Office12Renderer();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (dayView1.SelectedAppointment != null)
            {
                colorDialog1.Color = dayView1.SelectedAppointment.Color;

                if (colorDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    dayView1.SelectedAppointment.Color = colorDialog1.Color;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (dayView1.SelectedAppointment != null)
            {
                colorDialog1.Color = dayView1.SelectedAppointment.BorderColor;

                if (colorDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    dayView1.SelectedAppointment.BorderColor = colorDialog1.Color;
                }
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            dayView1.AppointmentSlotHeight = trackBar1.Value;
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            dayView1.StartDate = monthCalendar1.SelectionStart;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            dayView1.AllowScroll = !dayView1.AllowScroll;
        }

		private void chkbxEnableShadows_CheckedChanged(object sender, EventArgs e)
		{
			dayView1.EnableShadows = chkbxEnableShadows.Checked;
		}

		private void chkbxUseRoundedCorners_CheckedChanged(object sender, EventArgs e)
		{
			dayView1.EnableRoundedCorners = chkbxUseRoundedCorners.Checked;
		}

		private void cmbbxInterval_SelectedIndexChanged(object sender, EventArgs e)
		{
			dayView1.AppointmentDuration = (Calendar.AppointmentSlotDuration)cmbbxInterval.SelectedItem;
		}

		private void OnStartStop_Click(object sender, EventArgs e)
		{
			// invert state
			m_isRunning = !m_isRunning;

			// disbale if running
			m_btnStartStop.Text = (m_isRunning) ? "Stop loop" : "Start create / dispose loop";

			// running?
			if (m_isRunning)
			{
				// reset
				m_lblSummary.Text = "";
				m_controlcount = 0;
				m_initialGdiCount = GetGuiResources(m_processHandle, (uint)ResourceType.Gdi);

				// loop
				while (m_isRunning)
				{
					CreateDisposeControl(typeof(DayView));
					++m_controlcount;
				}

				// how many GDI objects per Control?
				uint gdiObjects = GetGuiResources(m_processHandle, (uint)ResourceType.Gdi) - m_initialGdiCount;
				m_lblSummary.Text = string.Format("{0} Calendars were created and disposed with a {1:0.0} GDI objects leak per Calendar",
					m_controlcount,
					gdiObjects / (float)m_controlcount);
			}
		}

		private void OnTimer_Tick(object sender, EventArgs e)
		{
			// update status bar
			if (m_processHandle != IntPtr.Zero)
			{
				// memory
				PROCESS_MEMORY_COUNTERS counters = new PROCESS_MEMORY_COUNTERS();
				GetProcessMemoryInfo(m_processHandle, out counters, 40);
				m_sslMemory.Text = string.Format("Memory usage: {0:#,##0} KBytes", counters.WorkingSetSize / 1024);
				m_sslMemory.Invalidate();

				// gdi
				m_sslGdi.Text = string.Format("GDI Objects: {0}", GetGuiResources(m_processHandle, (uint)ResourceType.Gdi));
				m_sslGdi.Invalidate();

				// objects
				m_sslObjectCount.Text = string.Format("Calendars Created: {0}", m_controlcount);
				m_sslObjectCount.Invalidate();
			}
		}
    }
}