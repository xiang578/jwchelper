using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace 正方系统抢课辅助
{
    /// <summary>
    /// MainPages.xaml 的交互逻辑
    /// </summary>
    public partial class MainPages : Page
    {
        public MainPages()
        {
            InitializeComponent();
        }
        private void btn_schedule_Click_1(object sender, RoutedEventArgs e)
        {
            frame_navigate.Source = new Uri("pages/SchedulePage.xaml", UriKind.Relative);
        }

        private void btn_Score_Click(object sender, RoutedEventArgs e)
        {
            frame_navigate.Source = new Uri("pages/GradePage.xaml", UriKind.Relative);
        }

        private void btn_Personalmessage_Click(object sender, RoutedEventArgs e)
        {
            frame_navigate.Source = new Uri("pages/PersonalMessage.xaml", UriKind.Relative);
        }

        private void btn_ProfessionChoose_Click_1(object sender, RoutedEventArgs e)
        {
            frame_navigate.Source = new Uri("pages/ProfessionChoose.xaml", UriKind.Relative);
        }


        private void btn_Evaluate_Click_1(object sender, RoutedEventArgs e)
        {
            frame_navigate.Source = new Uri("pages/StudentEvaluate.xaml", UriKind.Relative);
        }
        private void btn_CET46_Click_1(object sender, RoutedEventArgs e)
        {
            frame_navigate.Source = new Uri("pages/ChooseCET46.xaml", UriKind.Relative);
        }

        private void hyperlink0_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("http://" + SimilateSendPostDate.host + "/(" + SimilateSendPostDate.TagCode + ")/xs_main.aspx?xh=" + Student.StuNum);
        }

        private void btn_PublicCourseChoose_Click_1(object sender, RoutedEventArgs e)
        {
            frame_navigate.Source = new Uri("pages/PublicCourseChoose.xaml", UriKind.Relative);
        }
    }
}
