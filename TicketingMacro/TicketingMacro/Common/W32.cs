using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace TicketingMacro
{
    public class W32
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(String className, String WindowName);

        [DllImport("user32.dll")]
        internal static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT placeMenet);

        [DllImport("user32.dll")]
        internal static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBlt, int nFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void MouseEvent(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtralInfo);
    }
}
