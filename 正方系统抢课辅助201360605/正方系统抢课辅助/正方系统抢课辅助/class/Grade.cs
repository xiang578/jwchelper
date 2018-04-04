using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Configuration;

namespace 正方系统抢课辅助
{
    class Grade : Student
    {
        private static string viewState;//控件提交的状态码
        /// <summary>
        /// 构造函数,当传入学年，学期，课程性质时，初始化各门科目的分数
        /// </summary>
        /// <param name="schoolYear">学年</param>
        /// <param name="term">学期</param>
        /// <param name="courseProperties">课程性质</param>
        public Grade(string schoolYear, string term, string courseProperties)
        {
            //refer之前的字符串
            string refer_str = "http://"+SimilateSendPostDate.host+"/("+SimilateSendPostDate.TagCode+")/xscjcx.aspx?xh=" + Student.StuNum + "&xm=" + System.Web.HttpUtility.UrlEncode(Student.StuName, Encoding.GetEncoding("GB2312")) + "&gnmkdm=N121605";
            //post请求的数据
            string requestData = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE="+viewState+"&hidLanguage=&ddlXN=" + schoolYear + "&ddlXQ=" + term + "&ddl_kcxz=" + courseProperties + (term == "" ? "&btn_xn=%D1%A7%C4%EA%B3%C9%BC%A8" : "&btn_xq=%D1%A7%C6%DA%B3%C9%BC%A8");
            
            //错误检测，从配置文件读取最大错误次数数据，然后进行检测，没出错时跳出循环
            int maxErrorTimes = 1000;//最大错误次数
            int errorTimeCount = 0;//当前错误次数
            string htmlResponseGrade = "";//接收课程表的数据
            while (errorTimeCount++ < maxErrorTimes)
            {
                //response接收回滚的正文
                htmlResponseGrade = SimilateSendPostDate.SimilateAndGetDataWithRefer(requestData, refer_str, "http://"+SimilateSendPostDate.host+"/("+SimilateSendPostDate.TagCode+")/xscjcx.aspx?xh=" + Student.StuNum + "&xm=" + Student.StuName + "&gnmkdm=N121605");
                if (htmlResponseGrade.IndexOf("出错啦") < 0)
                    break;
            }

            //用正则表达式数组来接收返回来的数组
            MatchCollection grade_match = new Regex("<td>(?<text1>\\d{8})</td><td>(?<text2>[\\w]*?)</td><td>(?<text3>[\\w]*?)</td><td>(?<text4>.*?)</td><td>(?<text5>[\\d\\.]*?)</td><td>(?<text6>[\\d\\.]*?)</td><td>(?<text7>[\\d\\.]*?)</td><td>(?<text8>.*?)</td><td>(?<text9>.*?)</td><td>(?<text10>.*?)</td><td>(?<text11>[\\w]*?)</td><td></td><td></td>").Matches(htmlResponseGrade);
            if (grade_match.Count > 0)
            {
                for (int i = 0; i < grade_match.Count; i++)
                {
                    GradeMessage gradeMsg = new GradeMessage();
                    gradeMsg.kcdm = grade_match[i].Groups["text1"].Value.ToString();
                    gradeMsg.kcmc = grade_match[i].Groups["text2"].Value.ToString();
                    gradeMsg.kcxz = grade_match[i].Groups["text3"].Value.ToString();
                    gradeMsg.kcgs = grade_match[i].Groups["text4"].Value.ToString() == "&nbsp;" ? "" : grade_match[i].Groups["text4"].Value.ToString();
                    gradeMsg.xf = grade_match[i].Groups["text5"].Value.ToString();
                    gradeMsg.jd = grade_match[i].Groups["text6"].Value.ToString();
                    gradeMsg.cj = grade_match[i].Groups["text7"].Value.ToString();
                    gradeMsg.fxbj = grade_match[i].Groups["text8"].Value.ToString() == "&nbsp;" ? "" : grade_match[i].Groups["text8"].Value.ToString();
                    gradeMsg.bkcj = grade_match[i].Groups["text9"].Value.ToString() == "&nbsp;" ? "没补考" : grade_match[i].Groups["text9"].Value.ToString();
                    gradeMsg.cxcj = grade_match[i].Groups["text10"].Value.ToString() == "&nbsp;" ? "没重修" : grade_match[i].Groups["text9"].Value.ToString();
                    gradeMsg.kkxy = grade_match[i].Groups["text11"].Value.ToString();
                    GradeMessage_list.Add(gradeMsg);
                }
            }

        }
        /// <summary>
        /// 无参数的构造函数,初始化学年，学期，课程性质里的选择项
        /// </summary>
        public Grade()
        {
            //请示的字符串
            string htmlResponse = SimilateSendPostDate.SimilateAndGetDataWithRefer("", "http://"+SimilateSendPostDate.host+"/("+SimilateSendPostDate.TagCode+")/xs_main.aspx?xh=" + Student.StuNum, "http://"+SimilateSendPostDate.host+"/("+SimilateSendPostDate.TagCode+")/xscjcx.aspx?xh=" + Student.StuNum + "&xm=" + System.Web.HttpUtility.UrlEncode(Student.StuName, Encoding.GetEncoding("GB2312")) + "&gnmkdm=N121605");
            //匹配学年字符串，并提取出来
            MatchCollection gradeSchoolYear_Match = new Regex("\\d{4}\\-\\d{4}").Matches(htmlResponse);
            if (gradeSchoolYear_Match.Count > 0)
            {
                for (int i = 0; i < gradeSchoolYear_Match.Count; i += 2)
                {
                    SchoolYear_list.Add(gradeSchoolYear_Match[i].Value.ToString());
                }
            }
            //匹配学期字符串，并提取出来
            MatchCollection gradeTerm_Match = new Regex("<option value=\"\\d\">(?<text>\\d)</option>").Matches(htmlResponse);
            if (gradeTerm_Match.Count > 0)
            {
                Term_list.Add("");
                for (int term = 0; term < gradeTerm_Match.Count; term++)
                {
                    Term_list.Add(gradeTerm_Match[term].Groups["text"].Value.ToString());
                }
            }
            //匹配课程性质字符串，并提取出来保存
            MatchCollection gradeCourseProperties_Match = new Regex("<option value=\"\\d+\">(?<text>[.\\D]+?)</option>").Matches(htmlResponse);
            if (gradeCourseProperties_Match.Count > 0)
            {
                CourseProperties_list.Add("");
                for (int courseProperties = 0; courseProperties < gradeCourseProperties_Match.Count; courseProperties++)
                {
                    CourseProperties_list.Add(gradeCourseProperties_Match[courseProperties].Groups["text"].Value.ToString());
                }
            }
            //向服务器请求登录页面的信息，获取ViewStae控制状态记录，然后用正则表达式提取
            Regex regex = new Regex("name=\"__VIEWSTATE\" value=\".*\"\\s*/>");
            Match match = regex.Match(htmlResponse);
            viewState = System.Web.HttpUtility.UrlEncode(match.Value.Replace("name=\"__VIEWSTATE\" value=\"", "").Replace("\"", "").Replace("/>", "").Trim());
        }
        /// <summary>
        /// 学年List
        /// </summary>
        public List<string> SchoolYear_list = new List<string>() { };
        /// <summary>
        /// 学期List
        /// </summary>
        public List<string> Term_list = new List<string>() { };
        /// <summary>
        /// 课程性质List
        /// </summary>
        public List<string> CourseProperties_list = new List<string>() { };
        /// <summary>
        /// 课程信息List
        /// </summary>
        public List<GradeMessage> GradeMessage_list = new List<GradeMessage>() { };
        /// <summary>
        /// 课程基本信息类
        /// </summary>
        public class GradeMessage
        {

            public string kcdm { get; set; }//课程代码
            public string kcmc { get; set; }//课程名称
            public string kcxz { get; set; }//课程性质
            public string kcgs { get; set; }//课程归属
            public string xf { get; set; }//学分
            public string jd { get; set; }//绩点
            public string cj { get; set; }//成绩
            public string fxbj { get; set; }//辅修标记
            public string bkcj { get; set; }//补考成绩
            public string cxcj { get; set; }//重修成绩
            public string kkxy { get; set; }//开课学院
            public string xqpjjd { get; set; }//学期平均绩点
            public string xnpjjd { get; set; }//学年平均绩点
        }
    }
}
