using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Calendar
{
    public class Office12Renderer : AbstractRenderer
    {
        protected override void Dispose(bool mainThread)
        {
            base.Dispose(mainThread);

            if (baseFont != null)
                baseFont.Dispose();
        }

        Font baseFont;

        public override Font BaseFont
        {
            get
            {
                if (baseFont == null)
                {
                    baseFont = new Font("Segoe UI", 8, FontStyle.Regular);
                }

                return baseFont;
            }
        }

        public override Color HourColor
        {
            get
            {
                return System.Drawing.Color.FromArgb(230, 237, 247);
            }
        }

        public override Color HalfHourSeperatorColor
        {
            get
            {
                return System.Drawing.Color.FromArgb(165, 191, 225);
            }
        }

        public override Color HourSeperatorColor
        {
            get
            {
                return System.Drawing.Color.FromArgb(213, 215, 241);
            }
        }

        public override Color WorkingHourColor
        {
            get
            {
                return System.Drawing.Color.FromArgb(255, 255, 255);
            }
        }

        public override System.Drawing.Color BackColor
        {
            get
            {
                return Color.FromArgb(213, 228, 242);
            }
        }

        public override Color SelectionColor
        {
            get
            {
                return System.Drawing.Color.FromArgb(41, 76, 122);
            }
        }

		public override void DrawHourLabel(System.Drawing.Graphics g, System.Drawing.Rectangle rect, int hour, bool ampm)
		{
			DrawHourLabel(g, rect, hour, ampm, false);
		}

		public override void DrawHourLabel(Graphics g, Rectangle rect, int hour, bool ampm, bool timeindicator)
		{
			if (g == null)
				throw new ArgumentNullException("g");

			int orgHour = hour;
			Color color = Color.FromArgb(101, 147, 207);

			//Draw Divider Line
			using (Pen pen = new Pen(color))
				g.DrawLine(pen, rect.Left, rect.Y, rect.Width, rect.Y);

			string ampmtime;
			if (ampm)
			{
				ampmtime = (hour < 12) ? "AM" : "PM";

				if (hour != 12)
					hour = hour % 12;
			}
			else
				ampmtime = "00";
			
			using (SolidBrush brush = new SolidBrush(color))
			{
				g.DrawString(hour.ToString("##00", System.Globalization.CultureInfo.InvariantCulture), HourFont, brush, rect);
				rect.X += 27;
				g.DrawString(ampmtime, MinuteFont, brush, rect);
			}

			DateTime dtnow = DateTime.Now;
			if (timeindicator && orgHour == dtnow.Hour)
			{
				int tioffset = 5;
				Rectangle rectIndicator = rect;
				rectIndicator.Width = rect.Width - tioffset;
				rectIndicator.Height = tioffset;
				rectIndicator.Y = (rect.Height / 60) * dtnow.Minute;
				rectIndicator.X = tioffset;
				using (LinearGradientBrush lgb = new LinearGradientBrush(rectIndicator, Color.FromArgb(247, 207, 114), Color.FromArgb(251, 230, 148), LinearGradientMode.Vertical))
					g.FillRectangle(lgb, rectIndicator);
			}
		}

        public override void DrawDayHeader(System.Drawing.Graphics g, System.Drawing.Rectangle rect, DateTime date)
        {
            if (g == null)
                throw new ArgumentNullException("g");

            using (StringFormat format = new StringFormat())
            {
                format.Alignment = StringAlignment.Center;
                format.FormatFlags = StringFormatFlags.NoWrap;
                format.LineAlignment = StringAlignment.Center;

                using (StringFormat formatdd = new StringFormat())
                {
                    formatdd.Alignment = StringAlignment.Near;
                    formatdd.FormatFlags = StringFormatFlags.NoWrap;
                    formatdd.LineAlignment = StringAlignment.Center;

                    using (SolidBrush brush = new SolidBrush(this.BackColor))
                        g.FillRectangle(brush, rect);

                    using (Pen aPen = new Pen(Color.FromArgb(205, 219, 238)))
                        g.DrawLine(aPen, rect.Left, rect.Top + (int)rect.Height / 2, rect.Right, rect.Top + (int)rect.Height / 2);

                    using (Pen aPen = new Pen(Color.FromArgb(141, 174, 217)))
                        g.DrawRectangle(aPen, rect);

                    Rectangle topPart = new Rectangle(rect.Left + 1, rect.Top + 1, rect.Width - 2, (int)(rect.Height / 2) - 1);
                    Rectangle lowPart = new Rectangle(rect.Left + 1, rect.Top + (int)(rect.Height / 2) + 1, rect.Width - 1, (int)(rect.Height / 2) - 1);

                    using (LinearGradientBrush aGB = new LinearGradientBrush(topPart, Color.FromArgb(228, 236, 246), Color.FromArgb(214, 226, 241), LinearGradientMode.Vertical))
                        g.FillRectangle(aGB, topPart);

                    using (LinearGradientBrush aGB = new LinearGradientBrush(lowPart, Color.FromArgb(194, 212, 235), Color.FromArgb(208, 222, 239), LinearGradientMode.Vertical))
                        g.FillRectangle(aGB, lowPart);

                    if (date.Date.Equals(DateTime.Now.Date))
                    {
                        topPart.Inflate((int)(-topPart.Width / 4 + 1), 1); //top left orange area
                        topPart.Offset(rect.Left - topPart.Left + 1, 1);
                        topPart.Inflate(1, 0);
                        using (LinearGradientBrush aGB = new LinearGradientBrush(topPart, Color.FromArgb(247, 207, 114), Color.FromArgb(251, 230, 148), LinearGradientMode.Horizontal))
                        {
                            topPart.Inflate(-1, 0);
                            g.FillRectangle(aGB, topPart);
                        }

                        topPart.Offset(rect.Right - topPart.Right, 0);        //top right orange
                        topPart.Inflate(1, 0);
                        using (LinearGradientBrush aGB = new LinearGradientBrush(topPart, Color.FromArgb(251, 230, 148), Color.FromArgb(247, 207, 114), LinearGradientMode.Horizontal))
                        {
                            topPart.Inflate(-1, 0);
                            g.FillRectangle(aGB, topPart);
                        }

                        using (Pen aPen = new Pen(Color.FromArgb(128, 240, 154, 30))) //center line
                            g.DrawLine(aPen, rect.Left, topPart.Bottom - 1, rect.Right, topPart.Bottom - 1);

                        topPart.Inflate(0, -1);
                        topPart.Offset(0, topPart.Height + 1); //lower right
                        using (LinearGradientBrush aGB = new LinearGradientBrush(topPart, Color.FromArgb(240, 157, 33), Color.FromArgb(250, 226, 142), LinearGradientMode.BackwardDiagonal))
                            g.FillRectangle(aGB, topPart);

                        topPart.Offset(rect.Left - topPart.Left + 1, 0); //lower left
                        using (LinearGradientBrush aGB = new LinearGradientBrush(topPart, Color.FromArgb(240, 157, 33), Color.FromArgb(250, 226, 142), LinearGradientMode.ForwardDiagonal))
                            g.FillRectangle(aGB, topPart);
                        using (Pen aPen = new Pen(Color.FromArgb(238, 147, 17)))
                            g.DrawRectangle(aPen, rect);
                    }

                    g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                    //get short dayabbr. if narrow dayrect
                    string sTodaysName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(date.DayOfWeek);
                    if (rect.Width < 105)
                        sTodaysName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedDayName(date.DayOfWeek);

                    rect.Offset(2, 1);

                    using (Font fntDay = new Font("Segoe UI", 8))
                        g.DrawString(sTodaysName, fntDay, SystemBrushes.WindowText, rect, format);

                    rect.Offset(-2, -1);

                    using (Font fntDayDate = new Font("Segoe UI", 9, FontStyle.Bold))
                        g.DrawString(date.ToString(" d"), fntDayDate, SystemBrushes.WindowText, rect, formatdd);
                }
            }
        }

        public override void DrawDayBackground(System.Drawing.Graphics g, System.Drawing.Rectangle rect)
        {

        }

		public override void DrawAppointment(System.Drawing.Graphics g, System.Drawing.Rectangle rect, Appointment appointment, bool isSelected, System.Drawing.Rectangle gripRect)
		{
			DrawAppointment(g, rect, appointment, isSelected, gripRect, true, false);
		}
        
		public override void DrawAppointment(System.Drawing.Graphics g, System.Drawing.Rectangle rect, Appointment appointment, bool isSelected, System.Drawing.Rectangle gripRect, bool enableShadows, bool useroundedCorners)
        {
            if (appointment == null)
                throw new ArgumentNullException("appointment");

            if (g == null)
                throw new ArgumentNullException("g");

			if (rect.Width == 0 || rect.Height == 0)
				return;

			float _ShadowDistance = 6f;

			#region " Adjust Appointment Rectangle based on the Size of the Grip Rectangle "
			rect.X += gripRect.Width;
			rect.Width -= gripRect.Width;
			gripRect.X += gripRect.Width;
			#endregion

			#region " Gradient Colors "
			Color start = InterpolateColors(appointment.Color, Color.White, 0.4f);
			Color end = InterpolateColors(appointment.Color, Color.FromArgb(191, 210, 234), 0.7f);
			//start = Color.FromArgb(230, start);
			//end = Color.FromArgb(180, end);
			#endregion

			GraphicsPath gp = null;
			try
			{
				#region " Create Graphics Path "
				if (useroundedCorners)
				{
					gp = CreateRoundRectangle(rect);
				}
				else
				{
					gp = new GraphicsPath();
					gp.AddRectangle(rect);
				}
				#endregion

				#region " Shadows "
				if (enableShadows)
				{
					Matrix _Matrix = new Matrix();
					_Matrix.Translate(_ShadowDistance, _ShadowDistance);
					gp.Transform(_Matrix);
					using (PathGradientBrush _Brush = new PathGradientBrush(gp))
					{
						// set the wrapmode so that the colors will layer themselves
						// from the outer edge in
						_Brush.WrapMode = WrapMode.Clamp;
						
						// Create a color blend to manage our colors and positions and
						// since we need 3 colors set the default length to 3
						ColorBlend _ColorBlend = new ColorBlend(3);

						// here is the important part of the shadow making process, remember
						// the clamp mode on the colorblend object layers the colors from
						// the outside to the center so we want our transparent color first
						// followed by the actual shadow color. Set the shadow color to a 
						// slightly transparent DimGray, I find that it works best.
						_ColorBlend.Colors = new Color[] { Color.Transparent, Color.FromArgb(180, Color.DimGray), Color.FromArgb(180, Color.DimGray) };

						// our color blend will control the distance of each color layer
						// we want to set our transparent color to 0 indicating that the 
						// transparent color should be the outer most color drawn, then
						// our Dimgray color at about 10% of the distance from the edge
						_ColorBlend.Positions = new float[] { 0f, .1f, 1f };
						
						// assign the color blend to the pathgradientbrush
						_Brush.InterpolationColors = _ColorBlend;
						
						// fill the shadow with our pathgradientbrush
						g.FillPath(_Brush, gp);
					}

					// Draw shadow lines
					//int xLeft = rect.X + 6;
					//int xRight = rect.Right + 1;
					//int yTop = rect.Y + 1;
					//int yButton = rect.Bottom + 1;

					//for (int i = 0; i < 5; i++)
					//{
					//    using (Pen shadow_Pen = new Pen(Color.FromArgb(70 - 12 * i, Color.Black)))
					//    {
					//        g.DrawLine(shadow_Pen, xLeft + i, yButton + i, xRight + i - 1, yButton + i); //horisontal lines
					//        g.DrawLine(shadow_Pen, xRight + i, yTop + i, xRight + i, yButton + i); //vertical
					//    }
					//}

					//Move the GraphicsPath Back to the original Location
					_Matrix.Translate(_ShadowDistance * -2, _ShadowDistance * -2);
					gp.Transform(_Matrix);
				}
				#endregion

				#region " Draw Background "
					using (LinearGradientBrush aGB = new LinearGradientBrush(rect, start, end, LinearGradientMode.Vertical))
						g.FillPath(aGB, gp);
				#endregion

				#region " Selection Border "
				if (isSelected)
				{
					using (Pen m_Pen = new Pen(appointment.BorderColor, 2))
						g.DrawPath(m_Pen, gp);
				}
				#endregion

				#region " Appointment Grip "
				gripRect.Width += 1;

				GraphicsPath grippath = null;
				try
				{
					if (useroundedCorners)
					{
						grippath = CreateGripRectangle(gripRect);
					}
					else
					{
						grippath = new GraphicsPath();
						grippath.AddRectangle(gripRect);
					}
					using (SolidBrush aGB = new SolidBrush(appointment.BorderColor))
						g.FillPath(aGB, grippath);
				}
				finally
				{
					grippath.Dispose();
				}

				#endregion

				#region " Appointment Border
				if (appointment.DrawBorder)
					using (Pen m_Pen = new Pen(appointment.BorderColor, 1))
						g.DrawPath(m_Pen, gp);
				#endregion

				#region " Draw Recurring Icon "
				if (appointment.Recurring || appointment.Locked)
				{
					Rectangle iconrec = rect;
					iconrec.Width = 16;
					iconrec.Height = 16;

					iconrec.X = rect.Right - 18;
					iconrec.Y = rect.Bottom - 18;
					Image icon = (appointment.Locked) ? Properties.CalendarResources.LockedAppointment : Properties.CalendarResources.recurring;
					g.DrawImage(icon, iconrec);
					icon.Dispose();
				}
				#endregion

			}
			finally
			{
				gp.Dispose();
			}
			
			#region " Draw Text "
			using (StringFormat format = new StringFormat())
			{
				format.Alignment = StringAlignment.Near;
				format.LineAlignment = StringAlignment.Near;
				// draw appointment text

				rect.X += gripRect.Width;
				g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
				g.DrawString(appointment.Subject, this.BaseFont, SystemBrushes.WindowText, rect, format);
				g.TextRenderingHint = TextRenderingHint.SystemDefault;
			}
			#endregion
		}
    }
}
