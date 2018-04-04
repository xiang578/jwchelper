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
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Threading;

namespace 正方系统抢课辅助.pages
{
    /// <summary>
    /// PublicCourseChoose.xaml 的交互逻辑
    /// </summary>
    public partial class PublicCourseChoose : Page
    {
        string[] CourseBelong = new string[] { };
        string[] TakeCourseTime = new string[] { };
        string responseHtml = "";
        string requestStr = "";
        string viewState = "";
        bool isPostBack = true;
        public string IsHave
        {
            get
            {
                try
                {
                    return cbx_had.SelectedValue.ToString();
                }
                catch (Exception)
                {

                    return "";
                }
            }

        }//是否有课
        public string courseBelongs
        {
            get
            {
                try
                {

                    return cbx_CourseBelong.SelectedValue.ToString();
                }
                catch (Exception)
                {

                    return "";
                }

            }
        }//课程归属
        public string courserTime//上课时间
        {
            get
            {
                try
                {

                    return cbx_TakeCourseTime.SelectedValue.ToString();
                }
                catch (Exception)
                {

                    return "";
                }
            }
        }
        public string courseName
        {
            get
            {
                try
                {

                    return Txb_CourseName.Text.ToString();
                }
                catch (Exception)
                {

                    return "";
                }
            }
        }//课程名称
        ObservableCollection<CourseMsg> list_courseMsg = new ObservableCollection<CourseMsg>() { };
        public PublicCourseChoose()
        {
            InitializeComponent();
            BindTtile();
        }
        private void BindTtile()
        {
            string referStr = "http://" + SimilateSendPostDate.host + "/(" + SimilateSendPostDate.TagCode + ")/xs_main.aspx?xh=" + Student.StuNum;
            requestStr = "http://" + SimilateSendPostDate.host + "/(" + SimilateSendPostDate.TagCode + ")/xf_xsqxxxk.aspx?xh=" + Student.StuNum + "&xm=" + System.Web.HttpUtility.UrlEncode(Student.StuName, Encoding.GetEncoding("GB2312")) + "&gnmkdm=N121109";
            do{
            responseHtml = SimilateSendPostDate.SimilateAndGetDataWithRefer("", referStr, requestStr);}
            while (responseHtml.IndexOf("出错啦！") > 0 || responseHtml.IndexOf("三秒防刷") > 0);
            //课程属性提取
            Regex CBregex = new Regex("<select name=\"ddl_kcgs\"[\\w\\W]*?id=\"ddl_kcgs\">(?<text>[\\w\\W]*?)</select> ");
            Match CBmatch = CBregex.Match(responseHtml);
            if (CBmatch.Success)
            {
                string str = CBmatch.Groups["text"].Value.Trim();
                CourseBelong = str.Split(new string[] { "</option>" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < CourseBelong.Length; i++)
                {
                    CourseBelong[i] = CourseBelong[i].Remove(0, CourseBelong[i].IndexOf(">") + 1);
                }
                cbx_CourseBelong.ItemsSource = CourseBelong;
            }
            //上课时间提取
            Regex TCTregex = new Regex("<select name=\"ddl_sksj\"[\\w\\W]*?id=\"ddl_sksj\">(?<text>[\\w\\W]*?)</select> ");
            Match TCYmatch = TCTregex.Match(responseHtml);
            if (CBmatch.Success)
            {
                string str = TCYmatch.Groups["text"].Value.Trim();
                TakeCourseTime = str.Split(new string[] { "</option>" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < TakeCourseTime.Length; i++)
                {
                    TakeCourseTime[i] = TakeCourseTime[i].Remove(0, TakeCourseTime[i].IndexOf(">") + 1);
                }
                cbx_TakeCourseTime.ItemsSource = TakeCourseTime;
            }
            BindCourse();

            //向服务器请求登录页面的信息，获取ViewStae控制状态记录，然后用正则表达式提取
            Regex regex = new Regex("name=\"__VIEWSTATE\" value=\".*\"\\s*/>");
            Match match = regex.Match(responseHtml);
            viewState = System.Web.HttpUtility.UrlEncode(match.Value.Replace("name=\"__VIEWSTATE\" value=\"", "").Replace("\"", "").Replace("/>", "").Trim());


            cbx_CourseBelong.SelectedValue = "";
            cbx_TakeCourseTime.SelectedValue = "";

            isPostBack = false;
        }

        private void BindCourse()
        {
            //绑定选的课程
            Regex CRegex = new Regex("<tr( class=\"alt\"){0,}>[\\w\\W]*?<input id=\"kcmcGrid__ct(?<text>.*?)_xk\"[\\w\\W]*?</td><td><a[\\w\\W]*?>(?<courseName>.*?)</a></td><td>(?<courseNum>.*?)</td><td><a[\\w\\W]*?>(?<courseTeacherName>.*?)</a></td><td title=\"(?<courseTime>.*?)\">[\\w\\W]*?</td><td>(?<coursePlace>.*?)</td><td>(?<courseScore>.*?)</td><td>(?<courseWeekTime>.*?)</td><td>(?<courseStarEndWeek>.*?)</td><td>(?<courseAll>.*?)</td><td>(?<courseRemaind>.*?)</td><td>(?<courseBelongs>.*?)</td><td>(?<courseProperty>.*?)</td><td>校本部</td><td>(?<courseOpenCollege>.*?)</td>[\\w\\W]*?</tr>");
            MatchCollection CMatchCoolection = CRegex.Matches(responseHtml);


            for (int i = 0; i < CMatchCoolection.Count; i++)
            {
                CourseMsg courseMsg = new CourseMsg();
                courseMsg.CourseAll = CMatchCoolection[i].Groups["courseAll"].Value.ToString();
                courseMsg.CourseBelongs = CMatchCoolection[i].Groups["courseBelongs"].Value.ToString();
                courseMsg.CourseName = CMatchCoolection[i].Groups["courseName"].Value.ToString();
                courseMsg.CourseNum = CMatchCoolection[i].Groups["courseNum"].Value.ToString();
                courseMsg.CourseOpenCollege = CMatchCoolection[i].Groups["courseOpenCollege"].Value.ToString();
                courseMsg.CoursePlace = CMatchCoolection[i].Groups["coursePlace"].Value.ToString();
                courseMsg.Courseproperty = CMatchCoolection[i].Groups["courseproperty"].Value.ToString();
                courseMsg.CourseRemaind = CMatchCoolection[i].Groups["courseRemaind"].Value.ToString();
                courseMsg.CourseScore = CMatchCoolection[i].Groups["courseScore"].Value.ToString();
                courseMsg.CourseStarEndWeek = CMatchCoolection[i].Groups["courseStarEndWeek"].Value.ToString();
                courseMsg.CourseTeacherName = CMatchCoolection[i].Groups["courseTeacherName"].Value.ToString();
                courseMsg.CourseTime = CMatchCoolection[i].Groups["courseTime"].Value.ToString();
                courseMsg.CourseWeekTime = CMatchCoolection[i].Groups["courseWeekTime"].Value.ToString();
                courseMsg.IsChecked = "False";
                courseMsg.CourseId = CMatchCoolection[i].Groups["text"].Value.ToString();
                list_courseMsg.Add(courseMsg);
            }
            lv_Course.ItemsSource = list_courseMsg;
        }

        public class CourseMsg
        {
            public string CourseName { get; set; }
            public string CourseNum { get; set; }
            public string CourseTime { get; set; }
            public string CoursePlace { get; set; }
            public string CourseScore { get; set; }
            public string CourseWeekTime { get; set; }
            public string CourseStarEndWeek { get; set; }
            public string CourseAll { get; set; }
            public string CourseRemaind { get; set; }
            public string CourseBelongs { get; set; }
            public string Courseproperty { get; set; }
            public string CourseOpenCollege { get; set; }
            public string CourseTeacherName { get; set; }
            public string IsChecked { get; set; }
            public string CourseId { get; set; }

        }

        private void btn_SearchAll_Click(object sender, RoutedEventArgs e)
        {

            do
            {
                string referStr = "http://" + SimilateSendPostDate.host + "/(" + SimilateSendPostDate.TagCode + ")/xf_xsqxxxk.aspx?xh=" + Student.StuNum + "&xm=" + System.Web.HttpUtility.UrlEncode(Student.StuName, Encoding.GetEncoding("GB2312")) + "&gnmkdm=N121109";
                string postData = "__EVENTTARGET=dpkcmcGrid%3AtxtPageSize&__EVENTARGUMENT=&__VIEWSTATE=" + viewState + "&ddl_kcxz=&ddl_ywyl=%D3%D0&ddl_kcgs=&ddl_xqbs=1&ddl_sksj=&TextBox1=&kcmcGrid%3A_ctl2%3Ajcnr=%7C%7C%7C&kcmcGrid%3A_ctl3%3Ajcnr=%7C%7C%7C&kcmcGrid%3A_ctl4%3Ajcnr=%7C%7C%7C&kcmcGrid%3A_ctl5%3Ajcnr=%7C%7C%7C&kcmcGrid%3A_ctl6%3Ajcnr=%7C%7C%7C&kcmcGrid%3A_ctl7%3Ajcnr=%7C%7C%7C&dpkcmcGrid%3AtxtChoosePage=1&dpkcmcGrid%3AtxtPageSize=1000";
                responseHtml = SimilateSendPostDate.SimilateAndGetDataWithRefer(postData, referStr, requestStr);
            } while (responseHtml.IndexOf("出错啦！") > 0 || responseHtml.IndexOf("三秒防刷") > 0);
            BindCourse();
        }
        private void SearchData(object sender, SelectionChangedEventArgs e)
        {
            if (!isPostBack)
            {
                do
                {
                    if (requestStr == "")
                        return;
                    string referStr = "http://" + SimilateSendPostDate.host + "/(" + SimilateSendPostDate.TagCode + ")/xf_xsqxxxk.aspx?xh=" + Student.StuNum + "&xm=" + System.Web.HttpUtility.UrlEncode(Student.StuName, Encoding.GetEncoding("GB2312")) + "&gnmkdm=N121109";
                    string postData = "__EVENTTARGET=dpkcmcGrid%3AtxtPageSize&__EVENTARGUMENT=&__VIEWSTATE=" + viewState + "&ddl_kcxz=&ddl_ywyl=" + System.Web.HttpUtility.UrlEncode(IsHave, Encoding.GetEncoding("GB2312")) + "&ddl_kcgs=" + System.Web.HttpUtility.UrlEncode(courseBelongs, Encoding.GetEncoding("GB2312")) + "&ddl_xqbs=1&ddl_sksj=" + System.Web.HttpUtility.UrlEncode(courserTime, Encoding.GetEncoding("GB2312")) + "&TextBox1=" + System.Web.HttpUtility.UrlEncode(courseName, Encoding.GetEncoding("GB2312")) + "&Button2=%C8%B7%B6%A8&kcmcGrid%3A_ctl2%3Ajcnr=%7C%7C%7C&kcmcGrid%3A_ctl3%3Ajcnr=%7C%7C%7C&kcmcGrid%3A_ctl4%3Ajcnr=%7C%7C%7C&kcmcGrid%3A_ctl5%3Ajcnr=%7C%7C%7C&kcmcGrid%3A_ctl6%3Ajcnr=%7C%7C%7C&kcmcGrid%3A_ctl7%3Ajcnr=%7C%7C%7C&dpkcmcGrid%3AtxtChoosePage=1&dpkcmcGrid%3AtxtPageSize=1000";
                    responseHtml = SimilateSendPostDate.SimilateAndGetDataWithRefer(postData, referStr, requestStr);
                } while (responseHtml.IndexOf("出错啦！") > 0);
                BindCourse();
            }
        }

        

        private void SearchData(object sender, RoutedEventArgs e)
        {
            do
            {
                if (requestStr == "")
                    return;
                string referStr = "http://" + SimilateSendPostDate.host + "/(" + SimilateSendPostDate.TagCode + ")/xf_xsqxxxk.aspx?xh=" + Student.StuNum + "&xm=" + System.Web.HttpUtility.UrlEncode(Student.StuName, Encoding.GetEncoding("GB2312")) + "&gnmkdm=N121109";
                string postData = "__EVENTTARGET=dpkcmcGrid%3AtxtPageSize&__EVENTARGUMENT=&__VIEWSTATE=" + viewState + "&ddl_kcxz=&ddl_ywyl=" + System.Web.HttpUtility.UrlEncode(IsHave, Encoding.GetEncoding("GB2312")) + "&ddl_kcgs=" + System.Web.HttpUtility.UrlEncode(courseBelongs, Encoding.GetEncoding("GB2312")) + "&ddl_xqbs=1&ddl_sksj=" + System.Web.HttpUtility.UrlEncode(courserTime, Encoding.GetEncoding("GB2312")) + "&TextBox1=" + System.Web.HttpUtility.UrlEncode(courseName, Encoding.GetEncoding("GB2312")) + "&Button2=%C8%B7%B6%A8&kcmcGrid%3A_ctl2%3Ajcnr=%7C%7C%7C&kcmcGrid%3A_ctl3%3Ajcnr=%7C%7C%7C&kcmcGrid%3A_ctl4%3Ajcnr=%7C%7C%7C&kcmcGrid%3A_ctl5%3Ajcnr=%7C%7C%7C&kcmcGrid%3A_ctl6%3Ajcnr=%7C%7C%7C&kcmcGrid%3A_ctl7%3Ajcnr=%7C%7C%7C&dpkcmcGrid%3AtxtChoosePage=1&dpkcmcGrid%3AtxtPageSize=1000";
                responseHtml = SimilateSendPostDate.SimilateAndGetDataWithRefer(postData, referStr, requestStr);
            } while (responseHtml.IndexOf("出错啦！") > 0);
            BindCourse();
        }

        private void btn_Choose_Click(object sender, RoutedEventArgs e)
        {
            tip.Visibility = Visibility.Visible;
            Thread thread = new Thread(ChooseLesson);
            thread.Start();
        }


        private void ChooseLesson()
        {

            string postData = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=" + viewState + "&ddl_kcxz=&ddl_ywyl=%D3%D0&ddl_kcgs=&ddl_xqbs=1&ddl_sksj=&TextBox1=";
            foreach (CourseMsg item in list_courseMsg)
            {
                postData += "&kcmcGrid%3A_ct" + item.CourseId + "%3Ajcnr=%7C%7C%7C";
                if (item.IsChecked != "False")
                {
                    postData += "&kcmcGrid%3A_ct" + item.CourseId + "%3Axk=on";
                }
            }
            postData += "&dpkcmcGrid%3AtxtChoosePage=1&dpkcmcGrid%3AtxtPageSize=6&Button1=++%CC%E1%BD%BB++";
            string referStr = "http://" + SimilateSendPostDate.host + "/(" + SimilateSendPostDate.TagCode + ")/xf_xsqxxxk.aspx?xh=" + Student.StuNum + "&xm=" + System.Web.HttpUtility.UrlEncode(Student.StuName, Encoding.GetEncoding("GB2312")) + "&gnmkdm=N121109";
            string rHtmlp = "";
            do
            {
                rHtmlp = SimilateSendPostDate.SimilateAndGetDataWithRefer(postData, referStr, referStr);
                if (rHtmlp.IndexOf("现在不是选课时间") > 0)
                {
                    MessageBox.Show("现在不是选课时间！");
                    tip.Visibility = Visibility.Collapsed;
                    return;
                }
                else if (rHtmlp.IndexOf("该门课程已选") > 0)
                {
                    MessageBox.Show("该门课程已选，请谢谢我qq50595531");
                    tip.Visibility = Visibility.Collapsed;
                    return;
                }
            } while (rHtmlp.IndexOf("请重新登陆，如无法解决，请稍后再试") > 0 || rHtmlp.IndexOf("该门课程已选") < 0 );

            //while (rHtmlp.IndexOf("请重新登陆，如无法解决，请稍后再试") > 0 || rHtmlp.IndexOf("该门课程已选") < 0 || rHtmlp.IndexOf("现在还不是选课时间") > 0);
            
        }
 




    }
}
