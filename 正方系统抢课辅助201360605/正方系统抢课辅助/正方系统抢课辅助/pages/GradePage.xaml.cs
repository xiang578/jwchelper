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
    /// GradePage.xaml 的交互逻辑
    /// </summary>
    public partial class GradePage : Page
    {
        public GradePage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 窗口Load函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            //实例化Grade类，激发提交数据提取Combox里内容选取，
            //绑定数据
            //初始化选择框
            //默认首先显示今年最新一门成绩
            Grade grade = new Grade();
            Cob_CourseProperties.ItemsSource = grade.CourseProperties_list;
            Cob_SchoolYear.ItemsSource = grade.SchoolYear_list;
            Cob_Term.ItemsSource = grade.Term_list;
            Cob_SchoolYear.SelectedIndex = Cob_SchoolYear.Items.Count - 1;
            Cob_Term.SelectedIndex = 0;
            Cob_CourseProperties.SelectedIndex = 0;
            SearchGrade(Cob_SchoolYear.SelectedValue.ToString(), Cob_Term.SelectedValue.ToString(), Cob_CourseProperties.SelectedIndex.ToString());

        }
        /// <summary>
        /// 提交查询的函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Sub_Click(object sender, RoutedEventArgs e)
        {
            SearchGrade(Cob_SchoolYear.SelectedValue.ToString(), Cob_Term.SelectedValue.ToString(), Cob_CourseProperties.SelectedIndex.ToString());
        }
        /// <summary>
        /// 根据条件查找适合的
        /// </summary>
        /// <param name="schoolYear">学年</param>
        /// <param name="term">学期</param>
        /// <param name="courseProperties">课程属性</param>
        private void SearchGrade(string schoolYear, string term, string courseProperties)
        {
            //课程性质的提交方式上有点特别，不是提交课程性质，而是序列号，以01 ，02，03，因此在课程性质上加了个0，并选择的是他的序号，当为00是默认提交的是空字符串
            Grade grade = new Grade(schoolYear, term, courseProperties == "0" ? "" : "0" + Cob_CourseProperties.SelectedIndex.ToString());
            dataGrid_Grade.ItemsSource = grade.GradeMessage_list;
        }
    }
}
