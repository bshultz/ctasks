using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Windows.Forms;
using Calendar;
using Calendar.Utility;
using Microsoft.Win32.TaskScheduler;

namespace CalendarTest
{
    public partial class Form1 : Form
    {
        private readonly List<Appointment> _appointments = new List<Appointment>();

        public Form1()
        {

            InitializeComponent();

            var bindableAppointmentSlotDurations = Enum.GetValues(typeof (AppointmentSlotDuration)).Cast<AppointmentSlotDuration>()
                .Select(a => new BindableAppointmentSlotDuration(a))
                .OrderBy(b => b.Order).ToList();
            this.cmbbxInterval.DataSource = bindableAppointmentSlotDurations;
            var selected = bindableAppointmentSlotDurations.First(b => b.Duration == dayView1.AppointmentDuration);
            this.cmbbxInterval.SelectedItem = selected;

            string user = WindowsIdentity.GetCurrent().Name;
            //bool preWin7 = true;




            #region " Add Some Appointments "


            DateTime now = DateTime.Now; //test datetime            
            String dateShortString = now.ToShortDateString();  //!!Used to set today as the context from which to show tasks

            now = now.AddHours(10 - now.Hour); //test datetime
            now = now.AddMinutes(-now.Minute); //test datetime



            using (var ts = new TaskService())
            {
                // DateTime t_Date; //trigger datetime

                TaskFolder tf = ts.RootFolder;
                Version ver = ts.HighestSupportedVersion;




                bool newVer = (ver >= new Version(1, 2));

                foreach (var t in tf.Tasks)
                {
                    try
                    {
                        TriggerCollection triggerCollection = t.Definition.Triggers;

                        foreach (Trigger trg in triggerCollection)
                        {
                            var dateTimes = new DateTime[triggerCollection.Count];

                            for (int i = 0; i < triggerCollection.Count; i++)
                            {
                                dateTimes[i] = Convert.ToDateTime(dateShortString + " " + trg.StartBoundary.TimeOfDay);
                                var appointment = new Appointment {StartDate = dateTimes[i], EndDate = dateTimes[i].AddMinutes(10), Subject = t.Name};
                                _appointments.Add(appointment);
                            }
                        }

                        foreach (var act in t.Definition.Actions)
                        {
                            Console.WriteLine(" = {0}", act);
                        }
                    }
                    catch { }
                }

            }

            /*
                        _appointments.Add(m_Appointment);

                        m_Date = m_Date.AddDays(1);

                        m_Appointment = new Appointment();
                        m_Appointment.StartDate = m_Date.AddHours(2);
                        m_Appointment.EndDate = m_Date.AddHours(3);
                        m_Appointment.Subject = "test2\r\n locked one";
                        m_Appointment.Color = System.Drawing.Color.LightBlue;
                        m_Appointment.Locked = true;

                        _appointments.Add(m_Appointment);

                        m_Date = m_Date.AddDays(-1);

                        m_Appointment = new Appointment();
                        m_Appointment.StartDate = m_Date;
                        m_Appointment.EndDate = m_Date.AddHours(4);
                        m_Appointment.EndDate = m_Appointment.EndDate.AddMinutes(15);
                        m_Appointment.Color = System.Drawing.Color.Yellow;
                        m_Appointment.Subject = "test3\r\n some numbers 123456 and unicode chars (Russian) –усский текст and (Turkish) рьёЁцз÷зiЁ";

                        _appointments.Add(m_Appointment);

                        m_Appointment = new Appointment();
                        m_Appointment.StartDate = m_Date;
                        m_Appointment.EndDate = m_Date.AddDays(2);
                        m_Appointment.Subject = "More than one day";
                        m_Appointment.AllDayEvent = true;
                        m_Appointment.Color = System.Drawing.Color.Red;

                        _appointments.Add(m_Appointment);

                        m_Appointment = new Appointment();
                        m_Appointment.StartDate = m_Date.AddDays(2);
                        m_Appointment.EndDate = m_Date.AddDays(4);
                        m_Appointment.Subject = "More than one day (2)";
                        m_Appointment.AllDayEvent = true;
                        m_Appointment.Color = System.Drawing.Color.Coral;

                        _appointments.Add(m_Appointment);

                        m_Appointment = new Appointment();
                        m_Appointment.StartDate = m_Date;
                        m_Appointment.EndDate = m_Date.AddDays(4);
                        m_Appointment.Subject = "More than one day (3)";
                        m_Appointment.AllDayEvent = true;
                        m_Appointment.Color = System.Drawing.Color.Red;

                        _appointments.Add(m_Appointment);
                        */
            #endregion

            dayView1.StartDate = DateTime.Now;
            dayView1.OnNewAppointment += dayView1_NewAppointment;
            dayView1.OnSelectionChanged += dayView1_SelectionChanged;
            dayView1.OnResolveAppointments += this.dayView1_ResolveAppointments;

            dayView1.MouseMove += dayView1_MouseMove;

            comboBox1.SelectedIndex = 1;

            // get process
            m_processHandle = GetCurrentProcess();

            // update status
            OnTimer_Tick(this, null);

            //m_timer = new System.Timers.Timer(500);
            //m_timer.Elapsed += new System.Timers.ElapsedEventHandler(m_timer_Elapsed);
        }

        void dayView1_NewAppointment(object sender, NewAppointmentEventArgs args)
        {
            var newAppointment = new Appointment {StartDate = args.StartDate, EndDate = args.EndDate, Subject = args.Title, Group = "2"};
            _appointments.Add(newAppointment);
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
            List<Appointment> appointments = _appointments.Where(m_App => (m_App.StartDate >= args.StartDate) && (m_App.StartDate <= args.EndDate)).ToList();
            args.Appointments = appointments;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var appointment = new Appointment {StartDate = dayView1.SelectionStart, EndDate = dayView1.SelectionEnd, BorderColor = Color.Red};
            _appointments.Add(appointment);
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
            dayView1.AppointmentDuration = ((BindableAppointmentSlotDuration)cmbbxInterval.SelectedItem).Duration;
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
                var counters = new PROCESS_MEMORY_COUNTERS();
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