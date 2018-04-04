using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace 正方系统抢课辅助
{
    class Student
    {
        /// <summary>
        /// 学生信息的构造函数，在其中初始化学生的基本信息
        /// </summary>
        public Student()
        {

           string str=SimilateSendPostDate.SimilateAndGetDataWithRefer("", "http://"+SimilateSendPostDate.host+"/("+SimilateSendPostDate.TagCode+")/xs_main.aspx?xh=" + StuNum, "http://"+SimilateSendPostDate.host+"/("+SimilateSendPostDate.TagCode+")/xsgrxx.aspx?xh=" + StuNum + "&xm=" + StuName + "gnmkdm=N121501");       
            String regex_str ="([\\s\\S]*)id=\"lbl_zyfx\">(?<text1>.*?)</span></TD>"+
								"([\\s\\S]*)id=\"lbl_xb\">(?<text2>.*?)</span></TD>"+
								"([\\s\\S]*)id=\"lbl_csrq\">(?<text3>.*?)</span></TD>"+
								"([\\s\\S]*)id=\"lbl_byzx\">(?<text4>.*?)</span></TD>"+
								"([\\s\\S]*)id=\"lbl_mz\">(?<text5>.*?)</span></TD>"+
								"([\\s\\S]*)id=\"lbl_jg\">(?<text6>.*?)</span></TD>"+
								"([\\s\\S]*)id=\"lbl_zzmm\">(?<text7>.*?)</span></TD>"+
								"([\\s\\S]*)id=\"lbl_sfzh\">(?<text8>.*?)</span></TD>"+
								"([\\s\\S]*)id=\"lbl_CC\">(?<text9>.*?)</span></TD>"+
								"([\\s\\S]*)id=\"lbl_xy\">(?<text10>.*?)</span></TD>"+
								"([\\s\\S]*)id=\"lbl_zymc\">(?<text11>.*?)</span></TD>"+
								"([\\s\\S]*)id=\"lbl_xzb\">(?<text12>.*?)</span></TD>"+
								"([\\s\\S]*)id=\"lbl_xz\">(?<text13>.*?)</span></TD>"+
								"([\\s\\S]*)id=\"lbl_dqszj\">(?<text14>.*?)</span></TD>";
          Match majorField_match = new Regex(regex_str).Match(str);
          if (majorField_match.Success)
          {
              Gender=majorField_match.Groups["text2"].Value.ToString();
              MajorField = majorField_match.Groups["text1"].Value.ToString();
              Brithday = majorField_match.Groups["text3"].Value.ToString();
              GraditionMiddleSchool = majorField_match.Groups["text4"].Value.ToString();
              Nation = majorField_match.Groups["text5"].Value.ToString();
              NativePlace = majorField_match.Groups["text6"].Value.ToString();
              PoliticsStatus = majorField_match.Groups["text7"].Value.ToString();
              IdentityCard = majorField_match.Groups["text8"].Value.ToString();
              QualificationLevels = majorField_match.Groups["text9"].Value.ToString();
              Academy = majorField_match.Groups["text10"].Value.ToString();
              MajorName = majorField_match.Groups["text11"].Value.ToString();
              LengthOfSchool = majorField_match.Groups["text13"].Value.ToString();
              ExamineeNum = majorField_match.Groups["text14"].Value.ToString();
              ExecutiveCourses=majorField_match.Groups["text12"].Value.ToString();
          }
           
        }
        public static string  StuName { get; set; }//名字
        public static string  StuNum { get; set; }//学号
        public string  Gender { get; set; }//性别
        public string  MajorField { get; set; }//专业方向
        public string  Brithday { get; set; }//出生日期
        public string  GraditionMiddleSchool{ get; set; }//毕业中学
        public string  Nation { get; set; }//民族
        public string  NativePlace { get; set; }//籍贯
        public string  PoliticsStatus { get; set; }//政治面貌
        public string  IdentityCard { get; set; }//身份证号
        public string  QualificationLevels { get; set; }//学历层次
        public string  Academy { get; set; }//学院
        public string  MajorName { get; set; }//专业名称
        public string  LengthOfSchool { get; set; }//学制
        public string  ExamineeNum { get; set; }//考生号
        public string  ExecutiveCourses { get; set; }//行政班
    }
}
