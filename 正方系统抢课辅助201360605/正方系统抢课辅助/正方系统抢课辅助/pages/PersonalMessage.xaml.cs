using System;
using System.Collections.Generic;
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
    /// PersonalMessage.xaml 的交互逻辑
    /// </summary>
    public partial class PersonalMessage : Page
    {
        public PersonalMessage()
        {
            InitializeComponent();
            Student student = new Student();
            grid_StudentMsg.DataContext = student;
        }
    }
}
