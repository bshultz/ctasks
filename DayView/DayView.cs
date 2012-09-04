using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Calendar
{
    public class DayView : Control
    {
        public DayView()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Selectable, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.UserPaint, true);

            scrollbar = new VScrollBar();
            scrollbar.SmallChange = appointmentSlotHeight;
            scrollbar.LargeChange = appointmentSlotHeight * 2;
            scrollbar.Dock = DockStyle.Right;
            scrollbar.Visible = allowScroll;
            scrollbar.Scroll += new ScrollEventHandler(scrollbar_Scroll);
            AdjustScrollbar();
            scrollbar.Value = (startHour * 2 * appointmentSlotHeight);

            this.Controls.Add(scrollbar);

            editbox = new TextBox();
            editbox.Multiline = true;
            editbox.Visible = false;
            editbox.BorderStyle = BorderStyle.None;
            editbox.KeyUp += new KeyEventHandler(editbox_KeyUp);
            editbox.Margin = Padding.Empty;

            this.Controls.Add(editbox);

            drawTool = new DrawTool();
            drawTool.DayView = this;

            selectionTool = new SelectionTool();
            selectionTool.DayView = this;
            selectionTool.Complete += new EventHandler(selectionTool_Complete);

            activeTool = drawTool;

            UpdateWorkingHours();

            this.Renderer = new Office12Renderer();
        }

		#region " Private Fields "
		private TextBox editbox;
		private VScrollBar scrollbar;
		private DrawTool drawTool;
		private SelectionTool selectionTool;
		private int allDayEventsHeaderHeight = 20;

		private DateTime workStart;
		private DateTime workEnd;

		private int hourLabelWidth = 50;
		private int hourLabelIndent = 2;
		private int dayHeadersHeight = 20;
		private int appointmentGripWidth = 5;
		private int dayGripWidth = 5;
		private int horizontalAppointmentHeight = 20;

		private AppHeightDrawMode appHeightMode = AppHeightDrawMode.TrueHeightAll;
		private int appointmentSlotHeight = 24;
		private AbstractRenderer renderer;
		private bool ampmdisplay = false;
		private bool drawAllAppBorder = false;
		private bool minHalfHourApp = false;
		private int daysToShow = 1;
		private SelectionType selection;
		private DateTime startDate;
		private int startHour = 8;
		private Appointment selectedAppointment;
		private DateTime selectionStart;
		private DateTime selectionEnd;
		private ITool activeTool;
		private int workingHourStart = 8;
		private int workingMinuteStart = 30;
		private int workingHourEnd = 18;
		private int workingMinuteEnd = 30;
		private bool selectedAppointmentIsNew;
		private bool allowScroll = true;
		private bool allowInplaceEditing = true;
		private bool allowNew = true;
		private bool enableShadows = true;
		private bool enableRoundCorners = false;
		private bool enableTimeIndicator = false;
		private bool enableDurationDisplay = false;
		private AppointmentSlotDuration appointmentSlotDuration = AppointmentSlotDuration.ThirtyMinutes;

		internal System.Collections.Hashtable cachedAppointments = new System.Collections.Hashtable();
		internal Dictionary<Appointment, AppointmentView> appointmentViews = new Dictionary<Appointment, AppointmentView>();
		internal Dictionary<Appointment, AppointmentView> longappointmentViews = new Dictionary<Appointment, AppointmentView>();
		#endregion

		#region Properties
		[Category("DailyView")]
		[System.ComponentModel.DefaultValue(5)]
		public int DayGripWidth
		{
			get
			{
				return dayGripWidth;
			}
			set
			{
				if (dayGripWidth == value)
					return;

				dayGripWidth = value;
				this.Invalidate();
			}
		}

		[Category("DailyView")]
		[System.ComponentModel.DefaultValue(20)]
		public int DayHeadersHeight
		{
			get
			{
				return dayHeadersHeight;
			}
			set
			{
				if (dayHeadersHeight == value)
					return;

				dayHeadersHeight = value;
				this.Invalidate();
			}
		}
		
		[Category("DailyView")]
		public bool AmPmDisplay
        {
            get
            {
                return ampmdisplay;
            }
            set
            {
				if (ampmdisplay == value)
					return;
                ampmdisplay = value;
                this.Invalidate();
            }
        }

		[Category("DailyView")]
		public bool MinHalfHourApp
        {
            get
            {
                return minHalfHourApp;
            }
            set
            {
				if (minHalfHourApp == value)
					return;

                minHalfHourApp = value;
                this.Invalidate();
            }
        }

		[Category("DailyView")]
        [System.ComponentModel.DefaultValue(1)]
        public int DaysToShow
        {
            get
            {
                return daysToShow;
            }
            set
            {
				if (daysToShow == value)
					return;
                daysToShow = value;
				
				if (this.CurrentlyEditing)
					FinishEditing(true);

				this.Invalidate();
            }
        }

		[Category("DailyView")]
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
				if (startDate == value)
					return;
                startDate = value;

				startDate = startDate.Date;

				selectedAppointment = null;
				selectedAppointmentIsNew = false;
				selection = SelectionType.DateRange;

				this.Invalidate();
            }
        }

		[Category("DailyView")]
        [System.ComponentModel.DefaultValue(8)]
        public int StartHour
        {
            get
            {
                return startHour;
            }
            set
            {
                startHour = value;
                OnStartHourChanged();
            }
        }

		[Category("DailyView")]
        [System.ComponentModel.DefaultValue(8)]
        public int WorkingHourStart
        {
            get
            {
                return workingHourStart;
            }
            set
            {
                workingHourStart = value;
                UpdateWorkingHours();
            }
        }

		[Category("DailyView")]
        [System.ComponentModel.DefaultValue(30)]
        public int WorkingMinuteStart
        {
            get
            {
                return workingMinuteStart;
            }
            set
            {
                workingMinuteStart = value;
                UpdateWorkingHours();
            }
        }

		[Category("DailyView")]
        [System.ComponentModel.DefaultValue(18)]
        public int WorkingHourEnd
        {
            get
            {
                return workingHourEnd;
            }
            set
            {
                workingHourEnd = value;
                UpdateWorkingHours();
            }
        }

		[Category("DailyView")]
        [System.ComponentModel.DefaultValue(30)]
        public int WorkingMinuteEnd
        {
            get { return workingMinuteEnd; }
            set
            {
                workingMinuteEnd = value;
                UpdateWorkingHours();
            }
        }

		[Category("DailyView")]
        [DefaultValue(true)]
        public bool AllowScroll
        {
            get
            {
                return allowScroll;
            }
            set
            {
				if (allowScroll == value)
					return;

                allowScroll = value;

				this.scrollbar.Visible = this.AllowScroll;
				this.Invalidate();
            }
        }

		[Category("DailyView")]
        [DefaultValue(true)]
        public bool AllowInplaceEditing
        {
            get
            {
                return allowInplaceEditing;
            }
            set
            {
                allowInplaceEditing = value;
            }
        }

		[Category("DailyView.Appointment")]
		[System.ComponentModel.DefaultValue(24)]
		public int AppointmentSlotHeight
		{
			get
			{
				return appointmentSlotHeight;
			}
			set
			{
				if (appointmentSlotHeight == value)
					return;

				appointmentSlotHeight = value;
				AdjustScrollbar();
				this.Invalidate();
			}
		}

		[Category("DailyView.Appointment")]
		[System.ComponentModel.DefaultValue(5)]
		public int AppointmentGripWidth
		{
			get
			{
				return appointmentGripWidth;
			}
			set
			{
				if (appointmentGripWidth == value)
					return;

				appointmentGripWidth = value;
				this.Invalidate();
			}
		}

		[Category("DailyView.Appointment")]
		public bool DrawAllAppBorder
		{
			get
			{
				return drawAllAppBorder;
			}
			set
			{
				if (drawAllAppBorder == value)
					return;
				drawAllAppBorder = value;
				this.Invalidate();
			}
		}

		[Category("DailyView.Appointment")]
		[System.ComponentModel.DefaultValue(20)]
		public int HorizontalAppointmentHeight
		{
			get
			{
				return horizontalAppointmentHeight;
			}
			set
			{
				if (horizontalAppointmentHeight == value)
					return;

				horizontalAppointmentHeight = value;
				this.Invalidate();
			}
		}

		[Category("DailyView.Appointment")]
		public AppHeightDrawMode AppointmentHeightMode
		{
			get { return appHeightMode; }
			set { appHeightMode = value; }
		}
		
		[Category("DailyView.Appointment")]
        [DefaultValue(true)]
        public bool AllowNew
        {
            get
            {
                return allowNew;
            }
            set
            {
                allowNew = value;
            }
        }

		[Category("DailyView.Appointment")]
		public bool EnableShadows
		{
			get
			{
				return enableShadows;
			}
			set
			{
				if (enableShadows == value)
					return;
				enableShadows = value;
				this.Invalidate();
			}
		}

		[Category("DailyView.Appointment")]
		public bool EnableRoundedCorners
		{
			get
			{
				return enableRoundCorners;
			}
			set
			{
				if (enableRoundCorners == value)
					return;
				enableRoundCorners = value;
				this.Invalidate();
			}
		}

		[Category("DailyView.Appointment")]
		public bool EnableTimeIndicator
		{
			get
			{
				return enableTimeIndicator;
			}
			set
			{
				if (value == enableTimeIndicator)
					return;
				enableTimeIndicator = value;
				this.Invalidate();
			}
		}

		[Category("DailyView.Appointment")]
		public bool EnableDurationDisplay
		{
			get
			{
				return enableDurationDisplay;
			}
			set
			{
				if (value == enableDurationDisplay)
					return;
				enableDurationDisplay = value;
				this.Invalidate();
			}
		}

		[Category("DailyView.Appointment")]
		[System.ComponentModel.DefaultValue(AppointmentSlotDuration.ThirtyMinutes)]
		public AppointmentSlotDuration AppointmentDuration
		{
			get
			{
				return appointmentSlotDuration;
			}
			set
			{
				if (appointmentSlotDuration == value)
					return;
				appointmentSlotDuration = value;
				this.Invalidate();
			}
		}
		#endregion

		#region " Property Overrides "
		[System.ComponentModel.Browsable(false)]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}
		[System.ComponentModel.Browsable(false)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}
		[System.ComponentModel.Browsable(false)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}
		[System.ComponentModel.Browsable(false)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
			set
			{
				base.BackgroundImageLayout = value;
			}
		}
		[System.ComponentModel.Browsable(false)]
		public override System.Drawing.Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
			}
		}
		[System.ComponentModel.Browsable(false)]
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
			}
		}
		[System.ComponentModel.Browsable(false)]
		public override RightToLeft RightToLeft
		{
			get
			{
				return base.RightToLeft;
			}
			set
			{
				base.RightToLeft = value;
			}
		}
		#endregion

		#region " Private Properties "
		[System.ComponentModel.Browsable(false)]
		public DateTime SelectionStart
		{
			get { return selectionStart; }
			set { selectionStart = value; }
		}

		[System.ComponentModel.Browsable(false)]
		public DateTime SelectionEnd
		{
			get { return selectionEnd; }
			set { selectionEnd = value; }
		}
		
		[System.ComponentModel.Browsable(false)]
		public bool SelectedAppointmentIsNew
		{
			get
			{
				return selectedAppointmentIsNew;
			}
		}

		[System.ComponentModel.Browsable(false)]
		public int VScrollBarWith
		{
			get
			{
				if (scrollbar.Visible)
					return scrollbar.Width;
				else
					return 0;
			}
		}

		[System.ComponentModel.Browsable(false)]
		public ITool ActiveTool
		{
			get { return activeTool; }
			set { activeTool = value; }
		}

		[System.ComponentModel.Browsable(false)]
		public bool CurrentlyEditing
		{
			get
			{
				return editbox.Visible;
			}
		}

		[System.ComponentModel.Browsable(false)]
		public Appointment SelectedAppointment
		{
			get { return selectedAppointment; }
		}

		[System.ComponentModel.Browsable(false)]
		public SelectionType Selection
		{
			get
			{
				return selection;
			}
		}

		[System.ComponentModel.Browsable(false)]
		[System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public AbstractRenderer Renderer
		{
			get
			{
				return renderer;
			}
			set
			{
				if (renderer == value)
					return;

				renderer = value;
				this.Font = renderer.BaseFont;
				this.Invalidate();
			}
		}
		
		[System.ComponentModel.Browsable(false)]
		private int HeaderHeight
		{
			get
			{
				return dayHeadersHeight + allDayEventsHeaderHeight;
			}
		}
		#endregion

		#region Event Handlers
		private void editbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
                FinishEditing(true);
            }
            else if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                FinishEditing(false);
            }
        }

		private void selectionTool_Complete(object sender, EventArgs e)
        {
            if (selectedAppointment != null)
            {
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(EnterEditMode));
            }
        }

		private void scrollbar_Scroll(object sender, ScrollEventArgs e)
        {
            Invalidate();
			
			//scroll text box too
            if (editbox.Visible)
                editbox.Top += e.OldValue - e.NewValue;
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, width, height, specified);
            AdjustScrollbar();
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // Flicker free
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            // Capture focus
            this.Focus();

            if (CurrentlyEditing)
            {
                FinishEditing(false);
            }

            if (selectedAppointmentIsNew)
            {
                RaiseNewAppointment();
            }

            ITool newTool = null;

            Appointment appointment = GetAppointmentAt(e.X, e.Y);

            if (e.Y < HeaderHeight && e.Y > dayHeadersHeight && appointment == null)
            {
                if (selectedAppointment != null)
                {
                    selectedAppointment = null;
                    Invalidate();
                }

                newTool = drawTool;
                selection = SelectionType.None;

                base.OnMouseDown(e);
                return;
            }

            if (appointment == null)
            {
                if (selectedAppointment != null)
                {
                    selectedAppointment = null;
                    Invalidate();
                }

                newTool = drawTool;
                selection = SelectionType.DateRange;
            }
            else
            {
                newTool = selectionTool;
                selectedAppointment = appointment;
                selection = SelectionType.Appointment;

                Invalidate();
            }

            if (activeTool != null)
            {
                activeTool.MouseDown(e);
            }

            if ((activeTool != newTool) && (newTool != null))
            {
                newTool.Reset();
                newTool.MouseDown(e);
            }

            activeTool = newTool;

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (activeTool != null)
                activeTool.MouseMove(e);

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (activeTool != null)
                activeTool.MouseUp(e);

            base.OnMouseUp(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (e.Delta < 0)
            {//mouse wheel scroll down
                ScrollMe(true);
            }
            else
            {//mouse wheel scroll up
                ScrollMe(false);
            }
        }

		protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if ((allowNew) && char.IsLetterOrDigit(e.KeyChar))
            {
                if ((this.Selection == SelectionType.DateRange))
                {
                    if (!selectedAppointmentIsNew)
                        EnterNewAppointmentMode(e.KeyChar);
                }
            }
        }

		protected virtual void ResolveAppointments(ResolveAppointmentsEventArgs args)
		{
			System.Diagnostics.Debug.WriteLine("Resolve app");

			if (OnResolveAppointments != null)
				OnResolveAppointments(this, args);

			this.allDayEventsHeaderHeight = 0;

			// cache resolved appointments in hashtable by days.
			cachedAppointments.Clear();

			if ((selectedAppointmentIsNew) && (selectedAppointment != null))
			{
				if ((selectedAppointment.StartDate > args.StartDate) && (selectedAppointment.StartDate < args.EndDate))
				{
					args.Appointments.Add(selectedAppointment);
				}
			}

			foreach (Appointment appointment in args.Appointments)
			{
				int key = -1;
				AppointmentList list;

				if (appointment.StartDate.Day == appointment.EndDate.Day && appointment.AllDayEvent == false)
				{
					key = appointment.StartDate.Day;
				}
				else
				{
					key = -1;
				}

				list = (AppointmentList)cachedAppointments[key];

				if (list == null)
				{
					list = new AppointmentList();
					cachedAppointments[key] = list;
				}

				list.Add(appointment);
			}
		}
		#endregion

        #region " Public Methods "

        public void ScrollMe(bool down)
        {
			if (this.AllowScroll == false)
				return;

			int newScrollValue;

            if (down)
            {//mouse wheel scroll down
                newScrollValue = this.scrollbar.Value + this.scrollbar.SmallChange;

                if (newScrollValue < this.scrollbar.Maximum)
                    this.scrollbar.Value = newScrollValue;
                else
                    this.scrollbar.Value = this.scrollbar.Maximum;
            }
            else
            {//mouse wheel scroll up
                newScrollValue = this.scrollbar.Value - this.scrollbar.SmallChange;

                if (newScrollValue > this.scrollbar.Minimum)
                    this.scrollbar.Value = newScrollValue;
                else
                    this.scrollbar.Value = this.scrollbar.Minimum;
            }

			this.Invalidate();
        }

        public Rectangle GetTrueRectangle()
        {
            Rectangle truerect;
            truerect = this.ClientRectangle;
            truerect.X += hourLabelWidth + hourLabelIndent;
			truerect.Width -= VScrollBarWith + hourLabelWidth + hourLabelIndent;
            truerect.Y += this.HeaderHeight;
            truerect.Height -= this.HeaderHeight;

            return truerect;
        }

        public Rectangle GetFullDayApptsRectangle()
        {
            Rectangle fulldayrect;
            fulldayrect = this.ClientRectangle;
            fulldayrect.Height = this.HeaderHeight - dayHeadersHeight;
            fulldayrect.Y += dayHeadersHeight;
			fulldayrect.Width -= (hourLabelWidth + hourLabelIndent + this.VScrollBarWith);
            fulldayrect.X += hourLabelWidth + hourLabelIndent;

            return fulldayrect;
        }
        
        public void StartEditing()
        {
            if (!selectedAppointment.Locked && appointmentViews.ContainsKey(selectedAppointment))
            {
                Rectangle editBounds = appointmentViews[selectedAppointment].Rectangle;

                editBounds.Inflate(-3, -3);
                editBounds.X += appointmentGripWidth - 2;
                editBounds.Width -= appointmentGripWidth - 5;

                editbox.Bounds = editBounds;
                editbox.Text = selectedAppointment.Subject;
                editbox.Visible = true;
                editbox.SelectionStart = editbox.Text.Length;
                editbox.SelectionLength = 0;

                editbox.Focus();
            }
        }

        public void FinishEditing(bool cancel)
        {
            editbox.Visible = false;

            if (!cancel)
            {
                if (selectedAppointment != null)
                    selectedAppointment.Subject = editbox.Text;
            }
            else
            {
                if (selectedAppointmentIsNew)
                {
                    selectedAppointment = null;
                    selectedAppointmentIsNew = false;
                }
            }

            Invalidate();
            this.Focus();
        }

        public DateTime GetTimeAt(int x, int y)
        {
			int dayWidth = (this.Width - (VScrollBarWith + hourLabelWidth + hourLabelIndent)) / daysToShow;

            int hour = (y - this.HeaderHeight + scrollbar.Value) / appointmentSlotHeight;
            x -= hourLabelWidth;

            DateTime date = startDate;

            date = date.Date;
            date = date.AddDays(x / dayWidth);

            if ((hour > 0) && (hour < 24 * 2))
                date = date.AddMinutes((hour * 30));

            return date;
        }

        public Appointment GetAppointmentAt(int x, int y)
        {
            foreach (AppointmentView view in appointmentViews.Values)
                if (view.Rectangle.Contains(x, y))
                    return view.Appointment;

            foreach (AppointmentView view in longappointmentViews.Values)
                if (view.Rectangle.Contains(x, y))
                    return view.Appointment;

            return null;
		}

		private Rectangle GetHourRangeRectangle(DateTime start, DateTime end, Rectangle baseRectangle)
		{
			Rectangle rect = baseRectangle;

			int startY;
			int endY;

			startY = (start.Hour * appointmentSlotHeight * 2) + ((start.Minute * appointmentSlotHeight) / 30);
			endY = (end.Hour * appointmentSlotHeight * 2) + ((end.Minute * appointmentSlotHeight) / 30);

			rect.Y = startY - scrollbar.Value + this.HeaderHeight;

			rect.Height = System.Math.Max(1, endY - startY);

			return rect;
		}
		#endregion
		
		#region " Private Methods "
		private void AdjustScrollbar()
		{
			scrollbar.Maximum = (2 * appointmentSlotHeight * 25) - this.Height + this.HeaderHeight;
			scrollbar.Minimum = 0;
		}
		protected virtual void OnStartHourChanged()
		{
			// Fix : http://www.codeproject.com/cs/miscctrl/Calendardayview.asp?forumid=232232&select=1901930&df=100#xx1901930xx

			if ((startHour * 2 * appointmentSlotHeight) > scrollbar.Maximum) //maximum is lower on larger forms
			{
				scrollbar.Value = scrollbar.Maximum;
			}
			else
			{
				scrollbar.Value = (startHour * 2 * appointmentSlotHeight);
			}

			Invalidate();
		}
		private void UpdateWorkingHours()
		{
			workStart = new DateTime(1, 1, 1, workingHourStart, workingMinuteStart, 0);
			workEnd = new DateTime(1, 1, 1, workingHourEnd, workingMinuteEnd, 0);

			Invalidate();
		}
		private void EnterNewAppointmentMode(char key)
		{
			Appointment appointment = new Appointment();
			appointment.StartDate = selectionStart;
			appointment.EndDate = selectionEnd;
			appointment.Subject = key.ToString();

			selectedAppointment = appointment;
			selectedAppointmentIsNew = true;

			activeTool = selectionTool;

			Invalidate();

			System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(EnterEditMode));
		}

		private delegate void StartEditModeDelegate(object state);
		private void EnterEditMode(object state)
		{
			if (!allowInplaceEditing)
				return;

			if (this.InvokeRequired)
			{
				Appointment selectedApp = selectedAppointment;

				System.Threading.Thread.Sleep(200);

				if (selectedApp == selectedAppointment)
					this.Invoke(new StartEditModeDelegate(EnterEditMode), state);
			}
			else
			{
				StartEditing();
			}
		}
		#endregion

		#region " Method Overrides "
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				editbox.Dispose();
				scrollbar.Dispose();
			}
			base.Dispose(disposing);
		}
		#endregion

		#region " Drawing Methods "
		protected override void OnPaint(PaintEventArgs e)
        {
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			// resolve appointments on visible date range.
			ResolveAppointmentsEventArgs args = new ResolveAppointmentsEventArgs(this.StartDate, this.StartDate.AddDays(daysToShow));
			ResolveAppointments(args);

			using (SolidBrush backBrush = new SolidBrush(renderer.BackColor))
				e.Graphics.FillRectangle(backBrush, this.ClientRectangle);

			// Visible Rectangle
			Rectangle rectangle = new Rectangle(0, 0, this.Width - VScrollBarWith, this.Height);

			DrawDays(ref e, rectangle);

			DrawHourLabels(ref e, rectangle);

			DrawDayHeaders(ref e, rectangle);
        }

		private void DrawDays(ref PaintEventArgs e, Rectangle rectangle)
		{
			Rectangle rect = rectangle;
			rect.X += hourLabelWidth + hourLabelIndent;
			rect.Y += this.HeaderHeight;
			rect.Width -= (hourLabelWidth + hourLabelIndent);


			if (e.ClipRectangle.IntersectsWith(rect) == false)
				return;

			int dayWidth = rect.Width / daysToShow;

			#region " Multi Day Appointments "
			AppointmentList longAppointments = (AppointmentList)cachedAppointments[-1];

			AppointmentList drawnLongApps = new AppointmentList();

			AppointmentView view;

			int y = dayHeadersHeight;
			bool intersect = false;

			List<int> layers = new List<int>();
			if (longAppointments != null)
			{

				foreach (Appointment appointment in longAppointments)
				{
					appointment.Layer = 0;

					if (drawnLongApps.Count != 0)
					{
						foreach (Appointment app in drawnLongApps)
							if (!layers.Contains(app.Layer))
								layers.Add(app.Layer);

						foreach (int lay in layers)
						{
							foreach (Appointment app in drawnLongApps)
							{
								if (app.Layer == lay)
									if (appointment.StartDate.Date >= app.EndDate.Date || appointment.EndDate.Date <= app.StartDate.Date)
										intersect = false;
									else
									{
										intersect = true;
										break;
									}
								appointment.Layer = lay;
							}

							if (!intersect)
								break;
						}

						if (intersect)
							appointment.Layer = layers.Count;
					}

					drawnLongApps.Add(appointment); // changed by Gimlei
				}

				foreach (Appointment app in drawnLongApps)
					if (!layers.Contains(app.Layer))
						layers.Add(app.Layer);

				allDayEventsHeaderHeight = layers.Count * (horizontalAppointmentHeight + 5) + 5;

				Rectangle backRectangle = rect;
				backRectangle.Y = y;
				backRectangle.Height = allDayEventsHeaderHeight;

				renderer.DrawAllDayBackground(e.Graphics, backRectangle);

				foreach (Appointment appointment in longAppointments)
				{
					Rectangle appointmenRect = rect;
					int spanDays = appointment.EndDate.Subtract(appointment.StartDate).Days;

					if (appointment.EndDate.Day != appointment.StartDate.Day && appointment.EndDate.TimeOfDay < appointment.StartDate.TimeOfDay)
						spanDays += 1;

					appointmenRect.Width = dayWidth * spanDays - 5;
					appointmenRect.Height = horizontalAppointmentHeight;
					appointmenRect.X += (appointment.StartDate.Subtract(startDate).Days) * dayWidth; // changed by Gimlei
					appointmenRect.Y = y + appointment.Layer * (horizontalAppointmentHeight + 5) + 5; // changed by Gimlei

					view = new AppointmentView();
					view.Rectangle = appointmenRect;
					view.Appointment = appointment;

					longappointmentViews[appointment] = view;

					Rectangle gripRect = appointmenRect;
					gripRect.Width = appointmentGripWidth;

					renderer.DrawAppointment(e.Graphics, appointmenRect, appointment, appointment == selectedAppointment, gripRect, EnableShadows, EnableRoundedCorners);

				}
			}
			#endregion

			DateTime time = startDate;
			Rectangle rectangle2 = rect;
			rectangle2.Width = dayWidth;
			rectangle2.Y += allDayEventsHeaderHeight;
			rectangle2.Height -= allDayEventsHeaderHeight;

			appointmentViews.Clear();
			layers.Clear();

			for (int day = 0; day < daysToShow; day++)
			{
				DrawDay(ref e, rectangle2, time);

				rectangle2.X += dayWidth;

				time = time.AddDays(1);
			}
		}

		private void DrawHourLabels(ref PaintEventArgs e, Rectangle rectangle)
        {
			Rectangle rect = rectangle;
			rect.Y += this.HeaderHeight;

            e.Graphics.SetClip(rect);

			int hourlabelheight = (int)AppointmentDuration * AppointmentSlotHeight;

            for (int m_Hour = 0; m_Hour < 24; m_Hour++)
            {
                Rectangle hourRectangle = rect;

                hourRectangle.Y = rect.Y + (m_Hour * 2 * appointmentSlotHeight) - scrollbar.Value;
                hourRectangle.X += hourLabelIndent;
                hourRectangle.Width = hourLabelWidth;
				hourRectangle.Height = hourlabelheight;

                if (hourRectangle.Y > this.HeaderHeight / 2)
                    renderer.DrawHourLabel(e.Graphics, hourRectangle, m_Hour, this.AmPmDisplay, EnableTimeIndicator);
            }

            e.Graphics.ResetClip();
        }

		private void DrawDayHeaders(ref PaintEventArgs e, Rectangle rectangle)
        {
			Rectangle rect = rectangle;
			rect.X += hourLabelWidth + hourLabelIndent;
			rect.Width -= (hourLabelWidth + hourLabelIndent);
			rect.Height = dayHeadersHeight;

			if (e.ClipRectangle.IntersectsWith(rect) == false)
				return;

            int dayWidth = rect.Width / daysToShow;

            //one day header rectangle
            Rectangle dayHeaderRectangle = new Rectangle(rect.Left, rect.Top, dayWidth, rect.Height);
            DateTime headerDate = startDate;

            for (int day = 0; day < daysToShow; day++)
            {
                renderer.DrawDayHeader(e.Graphics, dayHeaderRectangle, headerDate);

                dayHeaderRectangle.X += dayWidth;
                headerDate = headerDate.AddDays(1);
            }
			Rectangle scrollrect = rectangle;
			if (this.AllowScroll == false)
			{
				scrollrect.X = rect.Width + hourLabelWidth + hourLabelIndent;
				scrollrect.Width = VScrollBarWith;
				using (SolidBrush backBrush = new SolidBrush(renderer.BackColor))
					e.Graphics.FillRectangle(backBrush, scrollrect);
			}
        }

        private void DrawDay(ref PaintEventArgs e, Rectangle rect, DateTime time)
        {
            Rectangle workingHoursRectangle = GetHourRangeRectangle(workStart, workEnd, rect);

            if (workingHoursRectangle.Y < this.HeaderHeight)
            {
                // Fix : http://www.codeproject.com/cs/miscctrl/Calendardayview.asp?forumid=232232&select=1904152&df=100#xx1904152xx
                
                workingHoursRectangle.Height -= this.HeaderHeight - workingHoursRectangle.Y;
                workingHoursRectangle.Y = this.HeaderHeight;
            }

            if (!((time.DayOfWeek == DayOfWeek.Saturday) || (time.DayOfWeek == DayOfWeek.Sunday))) //weekends off -> no working hours
                renderer.DrawHourRange(e.Graphics, workingHoursRectangle, false, false);

            if ((selection == SelectionType.DateRange) && (time.Day == selectionStart.Day))
            {
                Rectangle selectionRectangle = GetHourRangeRectangle(selectionStart, selectionEnd, rect);
                if (selectionRectangle.Top + 1 > this.HeaderHeight)
                renderer.DrawHourRange(e.Graphics, selectionRectangle, false, true);
            }

            e.Graphics.SetClip(rect);

            for (int hour = 0; hour < 24 * 2; hour++)
            {
                int y = rect.Top + (hour * appointmentSlotHeight) - scrollbar.Value;

				using (Pen pen = new Pen(((hour % 2) == 0 ? renderer.HalfHourSeperatorColor : renderer.HourSeperatorColor)))
                    e.Graphics.DrawLine(pen, rect.Left, y, rect.Right, y);

                if (y > rect.Bottom)
                    break;
            }

            renderer.DrawDayGripper(e.Graphics, rect, dayGripWidth);

            e.Graphics.ResetClip();

            AppointmentList appointments = (AppointmentList)cachedAppointments[time.Day];

            if (appointments != null)
            {
                List<string> groups = new List<string>();

                foreach (Appointment app in appointments)
                    if (!groups.Contains(app.Group))
                        groups.Add(app.Group);

                Rectangle rect2 = rect;
                rect2.Width = rect2.Width / groups.Count;

                //groups.Sort();

                foreach (string group in groups)
                {
                    DrawAppointments(ref e, rect2, time, group);

                    rect2.X += rect2.Width;
                }
            }
        }

        private void DrawAppointments(ref PaintEventArgs e, Rectangle rect, DateTime time, string group)
        {
            DateTime timeStart = time.Date;
            DateTime timeEnd = timeStart.AddHours(24);
            timeEnd = timeEnd.AddSeconds(-1);

            AppointmentList appointments = (AppointmentList)cachedAppointments[time.Day];

            if (appointments != null)
            {
                HalfHourLayout[] layout = GetMaxParalelAppointments(appointments);
                
                List<Appointment> drawnItems = new List<Appointment>();

                for (int halfHour = 0; halfHour < 24 * 2; halfHour++)
                {
                    HalfHourLayout hourLayout = layout[halfHour];

                    if ((hourLayout != null) && (hourLayout.Count > 0))
                    {
                        for (int appIndex = 0; appIndex < hourLayout.Count; appIndex++)
                        {
                            Appointment appointment = hourLayout.Appointments[appIndex];

                            if (appointment.Group != group)
                                continue;

                            if (drawnItems.IndexOf(appointment) < 0)
                            {
                                Rectangle appRect = rect;
                                int appointmentWidth;
                                AppointmentView view;

                                appointmentWidth = rect.Width / appointment.conflictCount;

								int lastX = 0;

                                foreach (Appointment app in hourLayout.Appointments)
                                {
                                    if ((app != null) && (app.Group == appointment.Group) && (appointmentViews.ContainsKey(app)))
                                    {
                                        view = appointmentViews[app];

                                        if (lastX < view.Rectangle.X)
                                            lastX = view.Rectangle.X;
                                    }
                                }

                                if ((lastX + (appointmentWidth * 2)) > (rect.X + rect.Width))
                                    lastX = 0;

                                appRect.Width = appointmentWidth - 5;

                                if (lastX > 0)
                                    appRect.X = lastX + appointmentWidth;

                                DateTime appstart = appointment.StartDate;
                                DateTime append = appointment.EndDate;

                                // Draw the appts boxes depending on the height display mode                           
                                // If small appts are to be drawn in half-hour blocks
                                if (this.AppointmentHeightMode == AppHeightDrawMode.FullHalfHourBlocksShort && appointment.EndDate.Subtract(appointment.StartDate).TotalMinutes < 30)
                                {
                                    // Round the start/end time to the last/next halfhour
                                    appstart = appointment.StartDate.AddMinutes(-appointment.StartDate.Minute);
                                    append = appointment.EndDate.AddMinutes(30 - appointment.EndDate.Minute);
                                    
                                    // Make sure we've rounded it to the correct halfhour :)
                                    if (appointment.StartDate.Minute >= 30)
                                        appstart = appstart.AddMinutes(30);
                                    if (appointment.EndDate.Minute > 30)
                                        append = append.AddMinutes(30);
                                }

                                // This is basically the same as previous mode, but for all appts
                                else if (this.AppointmentHeightMode == AppHeightDrawMode.FullHalfHourBlocksAll)
                                {
                                    appstart = appointment.StartDate.AddMinutes(-appointment.StartDate.Minute);
                                    if (appointment.EndDate.Minute != 0 && appointment.EndDate.Minute != 30)
                                        append = appointment.EndDate.AddMinutes(30 - appointment.EndDate.Minute);
                                    else
                                        append = appointment.EndDate;

                                    if (appointment.StartDate.Minute >= 30)
                                        appstart = appstart.AddMinutes(30);
                                    if (appointment.EndDate.Minute > 30)
                                        append = append.AddMinutes(30);
                                }

                                // Based on previous code
                                else if (this.AppointmentHeightMode == AppHeightDrawMode.EndHalfHourBlocksShort && appointment.EndDate.Subtract(appointment.StartDate).TotalMinutes < 30)
                                {
                                    // Round the end time to the next halfhour
                                    append = appointment.EndDate.AddMinutes(30 - appointment.EndDate.Minute);

                                    // Make sure we've rounded it to the correct halfhour :)
                                    if (appointment.EndDate.Minute > 30)
                                        append = append.AddMinutes(30);
                                }

                                else if (this.AppointmentHeightMode == AppHeightDrawMode.EndHalfHourBlocksAll)
                                {
                                    // Round the end time to the next halfhour
                                    if (appointment.EndDate.Minute != 0 && appointment.EndDate.Minute != 30)
                                        append = appointment.EndDate.AddMinutes(30 - appointment.EndDate.Minute);
                                    else
                                        append = appointment.EndDate;
                                    // Make sure we've rounded it to the correct halfhour :)
                                    if (appointment.EndDate.Minute > 30)
                                        append = append.AddMinutes(30);
                                }

                                appRect = GetHourRangeRectangle(appstart, append, appRect);

                                view = new AppointmentView();
                                view.Rectangle = appRect;
                                view.Appointment = appointment;

                                appointmentViews[appointment] = view;

                                e.Graphics.SetClip(rect);

                                if (this.DrawAllAppBorder)
                                    appointment.DrawBorder = true;

                                // Procedure for gripper rectangle is always the same
								Rectangle gripRect = appRect;
                                gripRect.Width = appointmentGripWidth;
                                
                                renderer.DrawAppointment(e.Graphics, appRect, appointment, appointment == selectedAppointment, gripRect, this.EnableShadows, this.enableRoundCorners);

                                e.Graphics.ResetClip();

                                drawnItems.Add(appointment);
                            }
                        }
                    }
                }
            }
        }

        private static HalfHourLayout[] GetMaxParalelAppointments(List<Appointment> appointments)
        {
            HalfHourLayout[] appLayouts = new HalfHourLayout[24 * 2];

            foreach (Appointment appointment in appointments)
            {
                appointment.conflictCount = 1;
            }

            foreach (Appointment appointment in appointments)
            {
                int firstHalfHour = appointment.StartDate.Hour * 2 + (appointment.StartDate.Minute / 30);
                int lastHalfHour = appointment.EndDate.Hour * 2 + (appointment.EndDate.Minute / 30);

                // Added to allow small parts been displayed
                if (lastHalfHour == firstHalfHour)
                {
                    if (lastHalfHour < 24 * 2)
                        lastHalfHour++;
                    else
                        firstHalfHour--;
                }

                for (int halfHour = firstHalfHour; halfHour < lastHalfHour; halfHour++)
                {
                    HalfHourLayout layout = appLayouts[halfHour];

                    if (layout == null)
                    {
                        layout = new HalfHourLayout();
                        layout.Appointments = new Appointment[20];
                        appLayouts[halfHour] = layout;
                    }

                    layout.Appointments[layout.Count] = appointment;

                    layout.Count++;

                    List<string> groups = new List<string>();

                    foreach (Appointment app2 in layout.Appointments)
                    {
                        if ((app2 != null) && (!groups.Contains(app2.Group)))
                            groups.Add(app2.Group);
                    }

                    layout.Groups = groups;

                    // update conflicts
                    foreach (Appointment app2 in layout.Appointments)
                    {
                        if ((app2 != null) && (app2.Group == appointment.Group))
                            if (app2.conflictCount < layout.Count)
                                app2.conflictCount = layout.Count - (layout.Groups.Count - 1);
                    }
                }
            }

            return appLayouts;
        }
        #endregion

        #region " Internal Utility Classes "

        internal class HalfHourLayout
        {
            public int Count;
            public List<string> Groups;
            public Appointment[] Appointments;
        }

        internal class AppointmentView
        {
            public Appointment Appointment;
            public Rectangle Rectangle;
        }

        internal class AppointmentList : List<Appointment>
        {
        }

        #endregion

        #region " Events "
        public event EventHandler<EventArgs> OnSelectionChanged;
		public event EventHandler<ResolveAppointmentsEventArgs> OnResolveAppointments;
		public event EventHandler<NewAppointmentEventArgs> OnNewAppointment;
        public event EventHandler<AppointmentEventArgs> OnAppointmentMove;

		internal void RaiseNewAppointment()
		{
			NewAppointmentEventArgs args = new NewAppointmentEventArgs(selectedAppointment.Subject, selectedAppointment.StartDate, selectedAppointment.EndDate);

			if (OnNewAppointment != null)
			{
				OnNewAppointment(this, args);
			}

			selectedAppointment = null;
			selectedAppointmentIsNew = false;

			Invalidate();
		}

		internal void RaiseSelectionChanged(EventArgs e)
		{
			if (OnSelectionChanged != null)
				OnSelectionChanged(this, e);
		}

		internal void RaiseAppointmentMove(AppointmentEventArgs e)
		{
			if (OnAppointmentMove != null)
				OnAppointmentMove(this, e);
		}
        #endregion
    }
}
