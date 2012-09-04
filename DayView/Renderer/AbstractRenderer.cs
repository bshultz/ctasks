using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace Calendar
{
    public abstract class AbstractRenderer : IDisposable
    {
        ~AbstractRenderer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool mainThread)
        {
            if (hourFont != null)
                hourFont.Dispose();

            if (minuteFont != null)
                minuteFont.Dispose();
        }

        public virtual Color AllDayEventsBackColor
        {
            get
            {
                return InterpolateColors(this.BackColor, Color.Black, 0.5f);
            }
        }

        public virtual Font BaseFont
        {
            get
            {
                return Control.DefaultFont;
            }
        }

        public virtual Color HourSeperatorColor
        {
            get
            {
                return System.Drawing.Color.FromArgb(234, 208, 152);
            }
        }

        public virtual Color HalfHourSeperatorColor
        {
            get
            {
                return System.Drawing.Color.FromArgb(243, 228, 177);
            }
        }

        public virtual Color HourColor
        {
            get
            {
                return System.Drawing.Color.FromArgb(255, 244, 188);
            }
        }

        public virtual Color WorkingHourColor
        {
            get
            {
                return System.Drawing.Color.FromArgb(255, 255, 213);
            }
        }

        public virtual Color BackColor
        {
            get
            {
                return SystemColors.Control;
            }
        }

        public virtual Color SelectionColor
        {
            get
            {
                return SystemColors.Highlight;
            }
        }

        private Font hourFont;

        public virtual Font HourFont
        {
            get
            {
                if (hourFont == null)
                {
                    hourFont = new Font(BaseFont.FontFamily, 14, FontStyle.Regular);
                }

                return hourFont;
            }
        }

        private Font minuteFont;

        public virtual Font MinuteFont
        {
            get
            {
                if (minuteFont == null)
                {
                    minuteFont = new Font(BaseFont.FontFamily, 8, FontStyle.Regular);
                }

                return minuteFont;
            }
        }

        public abstract void DrawHourLabel(Graphics g, Rectangle rect, int hour, bool ampm);

		public abstract void DrawHourLabel(Graphics g, Rectangle rect, int hour, bool ampm, bool timeindicator);

        public abstract void DrawDayHeader(Graphics g, Rectangle rect, DateTime date);

        public abstract void DrawDayBackground(Graphics g, Rectangle rect);

        public virtual void DrawHourRange(Graphics g, Rectangle rect, bool drawBorder, bool hilight)
        {
            if (g == null)
                throw new ArgumentNullException("g");

            using (SolidBrush brush = new SolidBrush(hilight ? this.SelectionColor : this.WorkingHourColor))
            {
                g.FillRectangle(brush, rect);
            }

            if (drawBorder)
                g.DrawRectangle(SystemPens.WindowFrame, rect);
        }

        public virtual void DrawDayGripper(Graphics g, Rectangle rect, int gripWidth)
        {
            if (g == null)
                throw new ArgumentNullException("g");

            using (Brush m_Brush = new SolidBrush(Color.White))
                g.FillRectangle(m_Brush, rect.Left, rect.Top - 1, gripWidth, rect.Height);

            using (Pen m_Pen = new Pen(Color.Black))
                g.DrawRectangle(m_Pen, rect.Left, rect.Top - 1, gripWidth, rect.Height);
        }

        public abstract void DrawAppointment(Graphics g, Rectangle rect, Appointment appointment, bool isSelected, Rectangle gripRect);

		public abstract void DrawAppointment(Graphics g, Rectangle rect, Appointment appointment, bool isSelected, Rectangle gripRect, bool enableShadows, bool useroundedCorners);

        public void DrawAllDayBackground(Graphics g, Rectangle rect)
        {
            if (g == null)
                throw new ArgumentNullException("g");

            using (Brush brush = new SolidBrush(InterpolateColors(this.BackColor, Color.Black, 0.5f)))
                g.FillRectangle(brush, rect);
        }

        public static Color InterpolateColors(Color color1, Color color2, float percentage)
        {
            int num1 = ((int)color1.R);
            int num2 = ((int)color1.G);
            int num3 = ((int)color1.B);
            int num4 = ((int)color2.R);
            int num5 = ((int)color2.G);
            int num6 = ((int)color2.B);
            byte num7 = Convert.ToByte(((float)(((float)num1) + (((float)(num4 - num1)) * percentage))));
            byte num8 = Convert.ToByte(((float)(((float)num2) + (((float)(num5 - num2)) * percentage))));
            byte num9 = Convert.ToByte(((float)(((float)num3) + (((float)(num6 - num3)) * percentage))));
            return Color.FromArgb(num7, num8, num9);
        }

		public static GraphicsPath CreateRoundRectangle(Rectangle rectangle)
		{
			int radius = 8;

			GraphicsPath path = new GraphicsPath();
			int l = rectangle.Left;
			int t = rectangle.Top;
			int w = rectangle.Width;
			int h = rectangle.Height;
			int d = radius << 1;
			path.AddArc(l, t, d, d, 180, 90); // topleft
			path.AddLine(l + radius, t, l + w - radius, t); // top
			path.AddArc(l + w - d, t, d, d, 270, 90); // topright
			path.AddLine(l + w, t + radius, l + w, t + h - radius); // right
			path.AddArc(l + w - d, t + h - d, d, d, 0, 90); // bottomright
			path.AddLine(l + w - radius, t + h, l + radius, t + h); // bottom
			path.AddArc(l, t + h - d, d, d, 90, 90); // bottomleft
			path.AddLine(l, t + h - radius, l, t + radius); // left
			path.CloseFigure();
			return path;
		}

		public static GraphicsPath CreateGripRectangle(Rectangle rectangle)
		{
			int radius = rectangle.Width;

			GraphicsPath path = new GraphicsPath();
			int l = rectangle.Left;
			int t = rectangle.Top;
			int w = rectangle.Width;
			int h = rectangle.Height;
			int d = radius << 1;
			path.AddArc(l, t, d, d, 180, 90); // topleft
			path.AddLine(l + w, t + radius, l + w, t + h - radius); // right
			path.AddArc(l, t + h - d, d, d, 90, 90); // bottomleft
			path.AddLine(l, t + h - radius, l, t + radius); // left
			path.CloseFigure();
			return path;
		}
    }
}
