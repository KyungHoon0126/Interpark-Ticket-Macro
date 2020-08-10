using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TicketingMacro
{
    internal struct WINDOWPLACEMENT
    {
        public int length;
        public int flags;
        public SHOW_WINDOW_COMMANDS showcCmd;
        public Point minPosition;
        public Point maxPosition;
        public Rectangle normalPosition;
    }
}
