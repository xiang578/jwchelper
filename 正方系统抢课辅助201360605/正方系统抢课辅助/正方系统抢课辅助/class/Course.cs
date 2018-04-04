using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Configuration;
using System.Windows;

namespace 正方系统抢课辅助
{
    class Course
    {
        private static string viewState;//控件提交的状态码
        //课程的学年
        public  List<string> CourseShoolYear_list = new List<string>() { };
        //课程的学期
        public  List<string> CourseTerm_list = new List<string>() { };

        //用一个二维字符串数组保存课程表的全部信息，第一维为星期几，第二维为具体的节次
        //一天最多就11节课，每次都2节或者3节一起的，现在把所有当作2节课进行处理
        public string[][] weekCourse = new string[7][]
            {
                new string[] { "", "", "", "", "", "" },
                new string[] { "", "", "", "", "", "" },
                new string[] { "", "", "", "", "", "" },
                new string[] { "", "", "", "", "", "" },
                new string[] { "", "", "", "", "", "" },
                new string[] { "", "", "", "", "", "" },
                new string[] { "", "", "", "", "", "" },
            };

        //在构造函数中初始化
        public Course()
        {
            init("", "http://"+SimilateSendPostDate.host+"/("+SimilateSendPostDate.TagCode+")/xs_main.aspx?xh=" + Student.StuNum, "http://"+SimilateSendPostDate.host+"/("+SimilateSendPostDate.TagCode+")/xskbcx.aspx?xh=" + Student.StuNum.ToString() + "&xm=" + Student.StuName + "gnmkdm=N121603");   
        }
        public Course(string schoolYear,string term)
        {
            init("__EVENTTARGET=xqd&__EVENTARGUMENT=&__VIEWSTATE="+viewState+"&xnd=" + schoolYear + "&xqd=" + term + "", "http://" + SimilateSendPostDate.host + "/(" + SimilateSendPostDate.TagCode + ")/xskbcx.aspx?xh=" + Student.StuNum + "&xm=" + System.Web.HttpUtility.UrlEncode(Student.StuName, Encoding.GetEncoding("GB2312")) + "&gnmkdm=N121603", "http://" + SimilateSendPostDate.host + "/(" + SimilateSendPostDate.TagCode + ")/xskbcx.aspx?xh=" + Student.StuNum + "&xm=" + Student.StuName + "&gnmkdm=N121603");
        }
        /// <summary>
        /// 课程表初始化函数
        /// </summary>
        private void init(string postData,string refer,string urlStr)
        {
            //错误检测，从配置文件读取最大错误次数数据，然后进行检测，没出错时跳出循环
            int maxErrorTimes =1000;//最大错误次数
            int errorTimeCount = 0;//当前错误次数
            string responData = "";//接收课程表的数据
            while (++errorTimeCount<maxErrorTimes)
            {
                responData = SimilateSendPostDate.SimilateAndGetDataWithRefer(postData, refer, urlStr);
                if (responData.IndexOf("出错啦") < 0)
                    break;
                else if (errorTimeCount == maxErrorTimes)
                {
                    MessageBox.Show("系统出错啦！");
                    return;
                }
            }
            //向服务器请求登录页面的信息，获取ViewStae控制状态记录，然后用正则表达式提取
            Regex regex = new Regex("name=\"__VIEWSTATE\" value=\".*\"\\s*/>");
            Match match = regex.Match(responData);
            viewState = System.Web.HttpUtility.UrlEncode(match.Value.Replace("name=\"__VIEWSTATE\" value=\"", "").Replace("\"", "").Replace("/>", "").Trim());
            //初始化当前所在学期的课程
            for (int lesson = 0; lesson < 6; lesson++)
            {
                //正则表达式匹配数据
                //并保存课程表
                Regex courseRegex = new Regex("<td.*?>第" + (lesson * 2 + 1) + "节</td><td.*?>(?<text1>.*?)</td><td.*?>(?<text2>.*?)</td><td.*?>(?<text3>.*?)</td><td.*?>(?<text4>.*?)</td><td.*?>(?<text5>.*?)</td><td.*?>(?<text6>.*?)</td><td.*?>(?<text7>.*?)</td>");
                Match courseMatch = courseRegex.Match(responData);
                if (courseMatch.Success)
                {
                    weekCourse[0][lesson] = courseMatch.Groups["text1"].Value.ToString() == "&nbsp;" ? "" : courseMatch.Groups["text1"].Value.ToString();
                    weekCourse[1][lesson] = courseMatch.Groups["text2"].Value.ToString() == "&nbsp;" ? "" : courseMatch.Groups["text2"].Value.ToString();
                    weekCourse[2][lesson] = courseMatch.Groups["text3"].Value.ToString() == "&nbsp;" ? "" : courseMatch.Groups["text3"].Value.ToString();
                    weekCourse[3][lesson] = courseMatch.Groups["text4"].Value.ToString() == "&nbsp;" ? "" : courseMatch.Groups["text4"].Value.ToString();
                    weekCourse[4][lesson] = courseMatch.Groups["text5"].Value.ToString() == "&nbsp;" ? "" : courseMatch.Groups["text5"].Value.ToString();
                    weekCourse[5][lesson] = courseMatch.Groups["text6"].Value.ToString() == "&nbsp;" ? "" : courseMatch.Groups["text6"].Value.ToString();
                    weekCourse[6][lesson] = courseMatch.Groups["text7"].Value.ToString() == "&nbsp;" ? "" : courseMatch.Groups["text7"].Value.ToString();
                } 
            }
            //课程表学年
            Regex schoolYearRegex = new Regex("(?<text>\\d{4}\\-\\d{4})</option>");
            MatchCollection schoolMatch = schoolYearRegex.Matches(responData);
            if (schoolMatch.Count > 0)
            {
                for (int i = 0; i < schoolMatch.Count; i += 2)
                {
                    CourseShoolYear_list.Add(schoolMatch[i].Groups["text"].Value.ToString());
                }

            }
            //课程表学期
            Regex termRegex = new Regex("value=\"(?<text>\\d)\"");
            MatchCollection termMatchs = termRegex.Matches(responData);
            if (termMatchs.Count > 0)
            {
                for (int j = 0; j < termMatchs.Count; j++)
                {
                    CourseTerm_list.Add(termMatchs[j].Groups["text"].ToString());
                }
            }
        }

        
    }
}
