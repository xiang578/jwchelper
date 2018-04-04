using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace 正方系统抢课辅助
{
    /// <summary>
    /// SchedulePage.xaml 的交互逻辑
    /// </summary>
    public partial class SchedulePage : Page
    {
        public SchedulePage()
        {
            InitializeComponent();
        }
        private void Schedule_Loaded(object sender, RoutedEventArgs e)
        {
            Init("", "");
            progressBar.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// 初始化课程表
        /// </summary>
        public void Init(string schoolYear, string term)
        {
            
            Course course;
            if(schoolYear=="")
             course = new Course();
            else
             course=new Course(schoolYear,term);
            //动态为每一个课程建立一个textBlock,加上border边框，并存进Grid中

            grid_schedule.Children.RemoveRange(13, grid_schedule.Children.Count-13);
            for (int col = 1; col <8; col++)
                for (int row = 1; row < 6; row++)
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.TextWrapping = TextWrapping.Wrap;
                    textBlock.Text = course.weekCourse[col - 1][row - 1].ToString().Replace("<br>", "\n");
                    Border border = new Border();
                    Grid.SetColumn(border, col);
                    Grid.SetRow(border,row);
                    border.Child = textBlock;
                    grid_schedule.Children.Add(border);
                    
                }
            //学年，学期，combox数据绑定
            cmb_shoolYear.ItemsSource = course.CourseShoolYear_list;
            cmb_term.ItemsSource = course.CourseTerm_list;
        }
        /// <summary>
        /// 当选择改变时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmb_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (cmb_shoolYear.SelectedIndex >= 0&&cmb_term.SelectedIndex>=0)
            {
                 Init(cmb_shoolYear.SelectedValue.ToString(), cmb_term.SelectedValue.ToString());
            }
        }


    }
        
        
        
}
