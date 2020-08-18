using OpenCvSharp;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;

namespace TicketingMacro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        #region Properties
        #region MouseEvents
        private const int MOUSEEVENT_LEFTDOWN = 0x02;
        private const int MOUSEEVENT_LEFTUP = 0x04;
        private const int MOUSEEVENT_RIGHTDOWN = 0x08;
        private const int MOUSEENVENT_RIGHTUP = 0x10;
        #endregion

        #region TicketGradeColors
        public Color PurpleColor = Color.FromArgb(255, 123, 104, 238);
        public Color DarkGreenColor = Color.FromArgb(255, 28, 168, 20);
        public Color SkyBlueColor = Color.FromArgb(255, 23, 179, 255);
        public Color OrangeColor = Color.FromArgb(255, 251, 126, 78);
        public Color LightGreenColor = Color.FromArgb(255, 160, 213, 63);
        #endregion
        #endregion

        internal struct LpPos
        {
            public int DlgX;
            public int DlgY;
        }

        IntPtr FindHwnd;
        Bitmap ConfirmImg = null;
        LpPos pPos = new LpPos();

        public MainWindow()
        {
            InitializeComponent();
            Initial();
            ConfirmImg = new Bitmap(@"img\Confirm.PNG");
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

        private void btnMacroExecute_Click(object sender, RoutedEventArgs e)
        {
            String tmp = "";
            Bitmap screenBmp;
            int ticketCnt = 0; // Ticket Cnt Check
            int ticketGrade = 0; // Ticket Grade Check

            tmp = "인터파크 티켓 - Chrome";
            FindHwnd = W32.FindWindow(null, tmp);

            ticketCnt = cbTicketCnt.SelectedIndex + 1;
            ticketGrade = cbTicketGrade.SelectedIndex;

            if(FindHandle(FindHwnd) && ticketCnt > 0 && ticketGrade >= 0)
            {
                GetHandlePos(FindHwnd);
                System.Windows.Forms.Cursor.Position = new System.Drawing.Point(pPos.DlgX, pPos.DlgY);
                screenBmp = GetScreen(FindHwnd);

                if (GetPixels(FindHwnd, SelectColor(), screenBmp, pPos.DlgX, pPos.DlgY, ticketCnt))
                {
                    SearchImg(screenBmp, ConfirmImg, pPos.DlgX, pPos.DlgY);
                }
            }
            else if (ticketCnt <= 0)
            {
                System.Windows.Forms.MessageBox.Show("티켓 장수를 선택해 주세요.");
            }
            else if (ticketGrade <= 0)
            {
                System.Windows.Forms.MessageBox.Show("티켓 색상(등급)을 선택해 주세요.");
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("창을 찾을 수 없습니다.");
            }
        }

        #region Win32
        private Boolean FindHandle(IntPtr hWnd)
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
            Rectangle rectangle = Rectangle.Round(graphicsData.VisibleClipBounds);
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

        public void lnClick(int x, int y)
        {
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(x, y);
            W32.mouse_event(MOUSEEVENT_LEFTDOWN | MOUSEEVENT_LEFTUP, (uint)System.Windows.Forms.Cursor.Position.X, (uint)System.Windows.Forms.Cursor.Position.Y, 0, 0);
        }
#endregion

        private void cbTicketGrade_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Color color = new Color();
            Rectangle rectangle = e.Bounds;

            if(e.Index >= 0)
            {
                string selectedGradeColor = ((ComboBox)sender).Items[e.Index].ToString();
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

                Brush brush = new SolidBrush(color);
                graphics.FillRectangle(brush, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
                graphics.DrawString(selectedGradeColor, font, Brushes.Black, rectangle.X, rectangle.Top);
            }
        }

        private bool GetPixels(IntPtr hWnd, Color selectedColor, Bitmap screenImg, int nx, int ny, int index)
        {
            Color findColor;
            int nCx = 0, nCy = 0;

            for (int y = 0; y < (screenImg.Height / 8); y++)
            {
                for (int x = 0; x < (screenImg.Width / 8); x++)
                {
                    nCx = x * 8;
                    nCy = y * 8;

                    findColor = screenImg.GetPixel(nCx, nCy);

                    if (findColor == selectedColor)
                    {
                        if(index > 0)
                        {
                            lnClick(nCx + nx, nCy + ny);
                            index--;
                        }
                        else if (index == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private Color SelectColor()
        {
            string str = cbTicketGrade.SelectedItem.ToString();
            Color color = new Color();

            switch(str)
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

            return color;
        }

        private bool SearchImg(Bitmap screenImg, Bitmap findImg, int posX, int posY)
        {
            using (Mat screenMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(screenImg))
            using (Mat findMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(findImg))
            using (Mat res = screenMat.MatchTemplate(findMat, TemplateMatchModes.CCoeffNormed))
            {
                double minVal = 0, maxVal = 0;
                OpenCvSharp.Point minLoc, maxLoc;
                Cv2.MinMaxLoc(res, out minVal, out maxVal, out minLoc, out maxLoc);
                Debug.WriteLine("Similarity Degree : " + maxVal);

                if(maxVal >= 0.8)
                {
                    lnClick(maxLoc.X + posX + (findMat.Width / 2), maxLoc.Y + posY + (findMat.Height / 2));
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}