using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace CalendarTest
{
    public partial class Form1
    {
		private IntPtr m_processHandle = IntPtr.Zero;
		private bool m_isRunning = false;
		private int m_controlcount = 0;
		private uint m_initialGdiCount = 0;

        private void CreateDisposeControl(Type ControlType)
        {
			// create
			//Calendar.DayView tvw = new Calendar.DayView();
			Control tvw = (Control)System.Activator.CreateInstance(ControlType);
			tvw.Location = new Point(0, 0);
			tvw.Dock = DockStyle.Fill;
            m_pnlContainer.Controls.Add(tvw);

            // do some gui processing
            Application.DoEvents();

            // dispose
            m_pnlContainer.Controls.Remove(tvw);
            tvw.Dispose();
            tvw = null;

            // do some gui processing
            Application.DoEvents();
        }

        #region Win32 API functions

        /// uiFlags: 0 - Count of GDI objects
        /// uiFlags: 1 - Count of USER objects
        /// - Win32 GDI objects (pens, brushes, fonts, palettes, regions, device contexts, bitmap headers)
        /// - Win32 USER objects:
        ///      - WIN32 resources (accelerator tables, bitmap resources, dialog box templates, font resources, menu resources, raw data resources, string table entries, message table entries, cursors/icons)
        /// - Other USER objects (windows, menus)
        [DllImport("user32.dll")]
        private static extern uint GetGuiResources(IntPtr hProcess, uint uiFlags);

        private enum ResourceType
        {
            Gdi = 0,
            User = 1
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentProcess();

        [DllImport("psapi.dll")]
        private static extern int GetProcessMemoryInfo(IntPtr hProcess, out PROCESS_MEMORY_COUNTERS counters, int size);

        [StructLayout(LayoutKind.Sequential, Size=40)]
        private struct PROCESS_MEMORY_COUNTERS
        {
            public int cb;
            public int PageFaultCount;
            public int PeakWorkingSetSize;
            public int WorkingSetSize;
            public int QuotaPeakPagedPoolUsage;
            public int QuotaPagedPoolUsage;
            public int QuotaPeakNonPagedPoolUsage;
            public int QuotaNonPagedPoolUsage;
            public int PagefileUsage;
            public int PeakPagefileUsage;
        }

        #endregion
    }
}