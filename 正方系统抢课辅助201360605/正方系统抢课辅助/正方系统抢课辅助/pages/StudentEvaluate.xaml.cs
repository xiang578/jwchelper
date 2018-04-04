using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.ComponentModel;

namespace 正方系统抢课辅助.Pages
{
    /// <summary>
    /// StudentEvaluate.xaml 的交互逻辑
    /// </summary>
    public partial class StudentEvaluate : Page
    {

        private Dictionary<string,string> dic_ClassAndTeacher=new Dictionary<string,string>();
        private List<string> list_Url = new List<string>();
        public ObservableCollection<EvaluateDetail> list_EvaluateDetail = new ObservableCollection<EvaluateDetail>();
        private string viewState = "";
        public class EvaluateDetail
        {
            public string id
            {
                get;
                set;

            }
            public string content{ get; set; }
            public string highPoint { get; set; }
            public string point { get; set; }


        }
        public StudentEvaluate()
        {

            try
            {
                InitializeComponent();
                BindClassAndTeacher();

                cmb_ClassName.SelectedIndex = 1;
                cmb_TeacherName.SelectedIndex = 1;
                BindEvaluateData();
            }
            catch (Exception ex)
            {

                MessageBox.Show("你已经评价过！");
            }
        }
        /// <summary>
        /// 绑定课程名称和任课老师数据
        /// </summary>
        private void BindClassAndTeacher()
        {
            string referStr="http://"+SimilateSendPostDate.host+"/("+SimilateSendPostDate.TagCode+")/xs_main.aspx?xh="+Student.StuNum;
            string requestStr="http://"+SimilateSendPostDate.host+"/("+SimilateSendPostDate.TagCode+")/xsjxpj2.aspx?xh="+Student.StuNum+"&xm="+System.Web.HttpUtility.UrlDecode(Student.StuName,Encoding.GetEncoding("GB2312"))+"&gnmkdm=N121901";
            string htmlData = SimilateSendPostDate.SimilateAndGetDataWithRefer("", referStr, requestStr);
            
            Regex regex = new Regex("<td>(?<className>.*?)</td><td><a href='#' onclick=\"window.open\\('(?<url>[\\w\\W]*?)','xsjxpj','toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1'\\)\">&nbsp;&nbsp;(?<teacherName>.*?)&nbsp;&nbsp;</a></td>");
            MatchCollection matchCollection=regex.Matches(htmlData);
            dic_ClassAndTeacher.Clear();
            for (int i = 0; i < matchCollection.Count; i++)
			{
                dic_ClassAndTeacher.Add(matchCollection[i].Groups["className"].Value.ToString().Replace("【课堂教学】", ""), matchCollection[i].Groups["teacherName"].Value.ToString().Replace("</font>","").Replace("<font color='#FF0000'>",""));
                list_Url.Add(matchCollection[i].Groups["url"].Value.ToString());
            }
           cmb_ClassName.ItemsSource = dic_ClassAndTeacher.Keys;
           cmb_TeacherName.ItemsSource = dic_ClassAndTeacher.Values;
        }
        /// <summary>
        /// 绑定评价的详细数据
        /// </summary>
        /// <param name="url"></param>
        private void BindEvaluateData()
        {
            string requestStr = "http://" + SimilateSendPostDate.host + "/(" + SimilateSendPostDate.TagCode + ")/" + list_Url[cmb_ClassName.SelectedIndex];
            string htmlDetail=SimilateSendPostDate.SimilateGetData(requestStr);
            //向服务器请求登录页面的信息，获取ViewStae控制状态记录，然后用正则表达式提取
            Regex regex_ViewState = new Regex("name=\"__VIEWSTATE\" value=\".*\"\\s*/>");
            Match match = regex_ViewState.Match(htmlDetail);
            viewState = System.Web.HttpUtility.UrlEncode(match.Value.Replace("name=\"__VIEWSTATE\" value=\"", "").Replace("\"", "").Replace("/>", "").Trim());
            Regex regex = new Regex("<td>[\\w\\W]*?</td><td>(?<id>[\\d].*?)</td><td>(?<content>.*?)</td><td>&nbsp;</td><td>(?<highPoint>.*?)</td><td>");
            MatchCollection match_Detail = regex.Matches(htmlDetail);
            list_EvaluateDetail.Clear();
            for (int i = 0; i < match_Detail.Count; i++)
            {
                EvaluateDetail evaluateDetail = new EvaluateDetail() { 
                id=match_Detail[i].Groups["id"].Value.ToString(),
                content=match_Detail[i].Groups["content"].Value.ToString(),
                highPoint=match_Detail[i].Groups["highPoint"].Value.ToString(),
                point = match_Detail[i].Groups["highPoint"].Value.ToString()
                };
                list_EvaluateDetail.Add(evaluateDetail);
            }
            grid_DataDetail.ItemsSource=list_EvaluateDetail;
            txb_Tip.Text = "你现在评价的是："+cmb_ClassName.SelectedValue.ToString()+"的"+cmb_TeacherName.SelectedValue.ToString()+"老师：";
        }



        /// <summary>
        /// 课程选择框改变时的动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmb_ClassName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmb_TeacherName.SelectedIndex = cmb_ClassName.SelectedIndex;
            BindEvaluateData();
        }
        /// <summary>
        /// 老师名选择框改变时的动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmb_TeacherName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmb_ClassName.SelectedIndex = cmb_TeacherName.SelectedIndex;
            BindEvaluateData();
        }

        private void btn_tj_Click(object sender, RoutedEventArgs e)
        {
            string postData = "__VIEWSTATE=" + viewState + "&DataGrid1%3A_ctl2%3Atxt_pf=" + list_EvaluateDetail[0].point + "&DataGrid1%3A_ctl3%3Atxt_pf=" + list_EvaluateDetail[1].point + "&DataGrid1%3A_ctl4%3Atxt_pf=" + list_EvaluateDetail[2].point + "&DataGrid1%3A_ctl5%3Atxt_pf=" + list_EvaluateDetail[3].point + "&DataGrid1%3A_ctl6%3Atxt_pf=" + list_EvaluateDetail[4].point + "&DataGrid1%3A_ctl7%3Atxt_pf=" + list_EvaluateDetail[5].point + "&DataGrid1%3A_ctl8%3Atxt_pf=" + list_EvaluateDetail[6].point + "&DataGrid1%3A_ctl9%3Atxt_pf=" + list_EvaluateDetail[7].point + "&DataGrid1%3A_ctl10%3Atxt_pf=" + list_EvaluateDetail[8].point + "&DataGrid1%3A_ctl11%3Atxt_pf=" + list_EvaluateDetail[9].point + "&DataGrid1%3A_ctl12%3Atxt_pf=" + list_EvaluateDetail[10].point + "&txt_pjxx=" + Rtxb_whole.Text + "&Button1=%B1%A3++%B4%E6&TextBox1=";
            string requestStr = "http://" + SimilateSendPostDate.host + "/(" + SimilateSendPostDate.TagCode + ")/" + list_Url[cmb_ClassName.SelectedIndex];
           string Responsehtml=SimilateSendPostDate.SimilateGetDataByPost(postData, requestStr);
           MessageBox.Show("评价成功！别忘了进网页提交确定评价！");
           BindClassAndTeacher();
           BindEvaluateData();
        }

        private void Btn_SelectAll_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < dic_ClassAndTeacher.Count; i++)
            {
                    string postData = "__VIEWSTATE=" + viewState + "&DataGrid1%3A_ctl2%3Atxt_pf=" + list_EvaluateDetail[0].point + "&DataGrid1%3A_ctl3%3Atxt_pf=" + list_EvaluateDetail[1].point + "&DataGrid1%3A_ctl4%3Atxt_pf=" + list_EvaluateDetail[2].point + "&DataGrid1%3A_ctl5%3Atxt_pf=" + list_EvaluateDetail[3].point + "&DataGrid1%3A_ctl6%3Atxt_pf=" + list_EvaluateDetail[4].point + "&DataGrid1%3A_ctl7%3Atxt_pf=" + list_EvaluateDetail[5].point + "&DataGrid1%3A_ctl8%3Atxt_pf=" + list_EvaluateDetail[6].point + "&DataGrid1%3A_ctl9%3Atxt_pf=" + list_EvaluateDetail[7].point + "&DataGrid1%3A_ctl10%3Atxt_pf=" + list_EvaluateDetail[8].point + "&DataGrid1%3A_ctl11%3Atxt_pf=" + list_EvaluateDetail[9].point + "&DataGrid1%3A_ctl12%3Atxt_pf=" + list_EvaluateDetail[10].point + "&txt_pjxx=" + Rtxb_whole.Text + "&Button1=%B1%A3++%B4%E6&TextBox1=";
                    string requestStr = "http://" + SimilateSendPostDate.host + "/(" + SimilateSendPostDate.TagCode + ")/" + list_Url[i];
                    string Responsehtml=SimilateSendPostDate.SimilateGetDataByPost(postData, requestStr);
            }
            MessageBox.Show("全部评价成功！别忘了进网页提交确定评价！");
        }  
    }
}
