using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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


namespace 正方系统抢课辅助.Pages
{
    /// <summary>
    /// ProfessionChoose.xaml 的交互逻辑
    /// </summary>
    public partial class ProfessionChoose : Page
    {
        private string status = "已经成功进入系统！\n";
        private string requestUriString;
        private string viewState;
        Dictionary<string, string> dic_Course = new Dictionary<string, string>() { };
        public ProfessionChoose()
        {
            InitializeComponent();
            
            this.Title = "欢迎你：" + Student.StuName + "同学！";
            string refer_str = "http://" + SimilateSendPostDate.host + "/(" + SimilateSendPostDate.TagCode + ")/xs_main.aspx?xh=" + Student.StuNum;
            requestUriString = "http://" + SimilateSendPostDate.host + "/(" + SimilateSendPostDate.TagCode + ")/xf_xsyxxxk.aspx?xh=" + Student.StuNum + "&xm=" + System.Web.HttpUtility.UrlEncode(Student.StuNum, Encoding.GetEncoding("GB2312")) + "&gnmkdm=N121110";
            //post请求的数据
            string html = SimilateSendPostDate.SimilateAndGetDataWithRefer("", refer_str, requestUriString);
            //向服务器请求登录页面的信息，获取ViewStae控制状态记录，然后用正则表达式提取
            Regex regex = new Regex("name=\"__VIEWSTATE\" value=\".*\"\\s*/>");
            Match match = regex.Match(html);
            viewState = System.Web.HttpUtility.UrlEncode(match.Value.Replace("name=\"__VIEWSTATE\" value=\"", "").Replace("\"", "").Replace("/>", "").Trim());
            MatchCollection matches = new Regex("id=\"kcmcGrid__(?<text>.*?)_jc\"([\\s\\S]*?)<a [\\w\\W]*?>(?<text1>.*?)</a>").Matches(html);
            if (matches.Count > 0)
            {
                for (int i = 0; i < matches.Count; i++)
                {
                    string key = matches[i].Groups["text"].Value;
                    string value = matches[i].Groups["text1"].Value;
                    dic_Course.Add(key, value);
                }
            }
            cbo_Course.ItemsSource = dic_Course.Values;
            cbo_Course.SelectedIndex = 1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(Choose);
            thread.Start();
            progressbar.Visibility = Visibility.Visible;
        }
        public void Choose()
        {
            string html = "";
            Dispatcher.Invoke((Action)delegate
            {
                do
                {
                    string key = dic_Course.FirstOrDefault(x => x.Value.ToString() == cbo_Course.SelectedValue.ToString()).Key.ToString();
                    string postData = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=" + viewState + "&ddl_ywyl=&ddl_kcgs=&ddl_sksj=&ddl_xqbs=1&kcmcGrid%3A_" + key + "%3Axk=on&Button1=+%CC%E1+%BD%BB+";
                    string referRequest_str = "http://" + SimilateSendPostDate.host + "/(" + SimilateSendPostDate.TagCode + ")/xf_xsyxxxk.aspx?xh=" + Student.StuNum + "&xm=" + System.Web.HttpUtility.UrlEncode(Student.StuName, Encoding.GetEncoding("GB2312")) + "&gnmkdm=N121110";
                    html = SimilateSendPostDate.SimilateAndGetDataWithRefer(postData, referRequest_str, referRequest_str);
                    if (html.IndexOf("现在不是选课时间！") > 0)
                    {
                        status += "现在还不是选课时间\n";
                        Txb_Status.Text = status;
                        MessageBox.Show("现在不是选课时间,请耐心等候！");
                        progressbar.Visibility = Visibility.Collapsed;
                        return;
                    }
                }
                while (html.IndexOf("请重新登陆，如无法解决，请稍后再试") > 0  || html.IndexOf("该门课程已选") < 0);
                if (html.IndexOf("该门课程已选") > 0)
                    MessageBox.Show("选课成功！");
                else
                    MessageBox.Show("好像没选成功！");
                progressbar.Visibility = Visibility.Collapsed;
            });

        }
    }
}
