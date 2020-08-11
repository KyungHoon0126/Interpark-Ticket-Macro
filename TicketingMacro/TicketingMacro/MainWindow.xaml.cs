using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TicketingMacro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties
        #region MouseEvents
        private const int MOUSEEVENT_LEFTDOWN = 0x02;
        private const int MOUSEEVENT_LEFTUP = 0x04;
        private const int MOUSEEVENT_RIGHTDOWN = 0x08;
        private const int MOUSEENVENT_RIGHTUP = 0x10;
        #endregion

        #region TicketGradeColors
        public System.Drawing.Color PurpleColor = System.Drawing.Color.FromArgb(255, 123, 104, 238);
        public System.Drawing.Color DarkGreenColor = System.Drawing.Color.FromArgb(255, 28, 168, 20);
        public System.Drawing.Color SkyBlueColor = System.Drawing.Color.FromArgb(255, 23, 179, 255);
        public System.Drawing.Color OrangeColor = System.Drawing.Color.FromArgb(255, 251, 126, 78);
        public System.Drawing.Color LightGreenColor = System.Drawing.Color.FromArgb(255, 160, 213, 63);
        #endregion

        internal struct LpPos
        {
            public int DlgX;
            public int DlgY;
        }
        #endregion

        IntPtr FindHwnd;
        Bitmap ConfirmImg = null;
        LpPos pPos = new LpPos();

        public MainWindow()
        {
            InitializeComponent();
            Initial();
            ConfirmImg = new Bitmap(@"img\Confirm.PNG");
        }

        private void btnMacroExecute_Click(object sender, RoutedEventArgs e)
        {
            String tmp = "";
            Bitmap screenBmp;

            tmp = "인터파크 티켓 - Chrome";
            FindHwnd = W32.FindWindow(null, tmp);

            if(findHandle(FindHwnd) && cbTicketGrade.SelectedIndex >= 0)
            {
                GetHandlePos(FindHwnd);
                System.Windows.Forms.Cursor.Position = new System.Drawing.Point(pPos.DlgX, pPos.DlgY);
                screenBmp = GetScreen(FindHwnd);

                if(true)
                {

                }
            }
        }

        private Boolean findHandle(IntPtr hWnd)
        {
            if(hWnd != IntPtr.Zero)
            {
                Debug.WriteLine("Find Handle : " + hWnd.ToString());
                System.Windows.Forms.MessageBox.Show("Handle을 찾았습니다.");
                return true;
            }
            else
            {
                Debug.WriteLine("Not Find Handle");
                System.Windows.Forms.MessageBox.Show("Handle을 못 찾았습니다.");
                return false;
            }
        }

        private void GetHandlePos(IntPtr hWnd)
        {
            System.Drawing.Point point = new System.Drawing.Point();
            System.Drawing.Size size = new System.Drawing.Size();

            GetWindowPos(hWnd, ref point, ref size);

            pPos.DlgX = point.X;
            pPos.DlgY = point.Y;
        }

        private void GetWindowPos(IntPtr hWnd, ref System.Drawing.Point point, ref System.Drawing.Size size)
        {
            WINDOWPLACEMENT placeMent = new WINDOWPLACEMENT();
            placeMent.length = Marshal.SizeOf(placeMent);

            W32.GetWindowPlacement(hWnd, ref placeMent);

            size = new System.Drawing.Size(placeMent.normalPosition.Right - (placeMent.normalPosition.Left * 2),
                                           placeMent.normalPosition.Bottom - (placeMent.normalPosition.Top * 2));
            point = new System.Drawing.Point(placeMent.normalPosition.Left, placeMent.normalPosition.Top);
        }

        private Bitmap GetScreen(IntPtr hWnd)
        {
            Graphics graphicsData = Graphics.FromHwnd(hWnd);
            System.Drawing.Rectangle rectangle = System.Drawing.Rectangle.Round(graphicsData.VisibleClipBounds);
            Bitmap bmp = new Bitmap(rectangle.Width, rectangle.Height);

            using (Graphics graphic = Graphics.FromImage(bmp))
            {
                IntPtr hdc = graphic.GetHdc();
                W32.PrintWindow(hWnd, hdc, 0x2);
                graphic.ReleaseHdc(hdc);
            }

            pbCaptureImg.Image = bmp;
            return bmp;
        }

        private void Initial()
        {
            cbTicketCnt.Items.Add("1장");
            cbTicketCnt.Items.Add("2장");
            cbTicketCnt.Items.Add("3장");
            cbTicketCnt.Items.Add("4장");

            cbTicketGrade.Items.Add("보라색");
            cbTicketGrade.Items.Add("진 초록색");
            cbTicketGrade.Items.Add("하늘색");
            cbTicketGrade.Items.Add("오렌지색");
            cbTicketGrade.Items.Add("연두색");
        }

        private void cbTicketGrade_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics graphics = e.Graphics;
            System.Drawing.Color color = new System.Drawing.Color();
            System.Drawing.Rectangle rectangle = e.Bounds;

            if(e.Index >= 0)
            {
                string selectedGradeColor = ((System.Windows.Forms.ComboBox)sender).Items[e.Index].ToString();
                Font font = new Font("Arial", 10, System.Drawing.FontStyle.Regular);

                switch(selectedGradeColor)
                {
                    case "보라색":
                        color = PurpleColor;
                        break;
                    case "진 초록색":
                        color = DarkGreenColor;
                        break;
                    case "하늘색":
                        color = SkyBlueColor;
                        break;
                    case "오렌지색":
                        color = OrangeColor;
                        break;
                    case "연두색":
                        color = LightGreenColor;
                        break;
                }

                System.Drawing.Brush brush = new SolidBrush(color);
                graphics.FillRectangle(brush, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
                graphics.DrawString(selectedGradeColor, font, System.Drawing.Brushes.Black, rectangle.X, rectangle.Top);
            }
        }
    }
}
