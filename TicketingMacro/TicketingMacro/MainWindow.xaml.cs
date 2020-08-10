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

namespace TicketingMacro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Properties
        private const int MOUSEEVENT_LEFTDOWN = 0x02;
        private const int MOUSEEVENT_LEFTUP = 0x04;
        private const int MOUSEEVENT_RIGHTDOWN = 0x08;
        private const int MOUSEENVENT_RIGHTUP = 0x10;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            Initial();
        }

        private void btnMacroExecute_Click(object sender, RoutedEventArgs e)
        {
            
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
    }
}
