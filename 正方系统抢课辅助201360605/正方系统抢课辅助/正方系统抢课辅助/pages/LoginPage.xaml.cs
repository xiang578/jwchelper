using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
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
using System.Windows.Threading;
using 正方系统抢课辅助.Class;

namespace 正方系统抢课辅助
{
    /// <summary>
    /// LoginPage.xaml 的交互逻辑
    /// </summary>
    public partial class LoginPage : Page
    {
        public delegate void CallBackDelegate();
        SaveUserNameAndPassword saveUserNameAndPassword = new SaveUserNameAndPassword();
        private enum LoginStatus
        {
            Succeed,
            WrongPassword,
            Exception,
            Default,
        }
        LoginStatus loginStatus = LoginStatus.Default;//登录状态枚举，四种状态，登录成功，密码错误，系统出错，初始没提交
        public LoginPage()
        {
            InitializeComponent();
           
        }
        /// <summary>
        /// 初始化viewStatue的值 
        /// </summary>
        private void init()
        {

            //向服务器请求登录页面的地址并获取viewState和TagCode
            string loginAddress = SimilateSendPostDate.SimilateGetLoginAddress("http://" + SimilateSendPostDate.host);
            Txb_StuNum.ItemsSource = saveUserNameAndPassword.GetAllUserId();
            Txb_StuNum.SelectedIndex = 0;
            loginStatus = LoginStatus.Default;
        }
        //回调方法
        private void CallBack()
        {
            //主线程报告信息,可以根据这个信息做判断操作,执行不同逻辑.
            Dispatcher.Invoke((Action)delegate
            {
                if (loginStatus == LoginStatus.Succeed)
                {
                    if (checkBox_isSave.IsChecked == true)
                    {
                        saveUserNameAndPassword.SaveUserIdAndPwd(Txb_StuNum.Text.ToString(), Txb_Password.Password.ToString());
                    }
                    //登录成功跳转导航
                    this.NavigationService.Navigate(new Uri("MainPages.xaml", UriKind.Relative));
                }
                else if (loginStatus == LoginStatus.WrongPassword)
                    if (MessageBox.Show("好像密码错误了！") == MessageBoxResult.OK)
                        Canvas_progressBar.Visibility = Visibility.Collapsed;
                    else if (loginStatus == LoginStatus.Exception)
                        if (MessageBox.Show("系统出错啦！") == MessageBoxResult.OK)
                            Canvas_progressBar.Visibility = Visibility.Collapsed;
            });
        }
        //线程执行的方法 参数是个委托, 线程中参数需要用object类型.
        public void Fun(object o)
        {
            //这里是你的操作代码,循环,根据条件退出while
            while (loginStatus == LoginStatus.Default)
            {
                loginJudge();
            }
            //把传来的参数转换为委托
            CallBackDelegate cbd = o as CallBackDelegate;
            //执行回调.
            cbd();
        }

        
        /// <summary>
        ///登陆判断
        /// </summary>
        void loginJudge()
        {

            Dispatcher.Invoke((Action)delegate
            {

                //模拟请求验证码，并识别
                string reconizeNum = "0";
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage = SimilateSendPostDate.SimilateRequestPicture("http://" + SimilateSendPostDate.host + "/(" + SimilateSendPostDate.TagCode + ")/CheckCode.aspx");
                if (bitmapImage != null)
                {
                    Bitmap bitmap = new Bitmap(bitmapImage.StreamSource);
                    Class.IdentifyCheckCode identifyCheckCode = new Class.IdentifyCheckCode(bitmap);
                    reconizeNum = identifyCheckCode.Recognite();
                }


                //模拟提交表单数据，并接收Response 的正文
                string requestStr = "__VIEWSTATE=" + SimilateSendPostDate.viewState + "&TextBox1=" + Txb_StuNum.Text.ToString() + "&TextBox2=" + Txb_Password.Password.ToString() + "&TextBox3=" + reconizeNum + "&RadioButtonList1=%D1%A7%C9%FA&Button1=&lbLanguage=";
                string ResponseHtml = SimilateSendPostDate.SimilateGetDataByPost(requestStr, "http://" + SimilateSendPostDate.host + "/(" + SimilateSendPostDate.TagCode + ")/default2.aspx");

                //根据系统返回的信息判断是否成功登录,并记录状态
                if (ResponseHtml.IndexOf("密码错误") > 0)
                {
                    loginStatus = LoginStatus.WrongPassword;
                    return;
                }
                    //当出现出错啦的时候继续刷进去
                else if (ResponseHtml.IndexOf("出错啦！") > 0)
                {
                  
                }
                else if (ResponseHtml.IndexOf("xhxm") > 0)
                {
                    saveUserNameAndPassword.SaveUserIdAndPwd(Txb_StuNum.Text, Txb_Password.Password);
                    //保存学号，姓名信息
                    Match match = new Regex("<span id=\"xhxm\">(.*?)</span></em>").Match(ResponseHtml);
                    Student.StuName = match.Value.Replace("<span id=\"xhxm\">" + Txb_StuNum.Text.ToString(), "").Replace("同学</span></em>", "").Trim();
                    Student.StuNum = Txb_StuNum.Text.ToString();
                    loginStatus = LoginStatus.Succeed;
                    return;
                }
            });
        }
       

        

        /// <summary>
        /// 点击登录按钮的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Txb_StuNum.Text == "")
            {
                MessageBox.Show("请输入学号！");
                return;
            }
            
            //把回调的方法给委托变量
            CallBackDelegate cbd = CallBack;
            //启动线程
            Thread th = new Thread(Fun);
            th.Start(cbd);//开始线程，代入参数
            Canvas_progressBar.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 当输入完帐号后判断是不是数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Txb_StuNum_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(saveUserNameAndPassword.GetPwd(Txb_StuNum.Text.Trim())))
            {
                Txb_Password.Password = saveUserNameAndPassword.GetPwd(Txb_StuNum.Text.Trim());
                checkBox_isSave.IsChecked = true;
            }
        }

        private void loginPage_Loaded(object sender, RoutedEventArgs e)
        {
            Txb_StuNum.Focus();//用户名输入框获得焦点
            init();
        }

        private void btn_Click(object sender, RoutedEventArgs e)
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
                    return;
                }
                else if (rHtmlp.IndexOf("该门课程已选") > 0)
                {
                    MessageBox.Show("该门课程已选，请谢谢我qq50595531");
                    return;
                }
            } while (rHtmlp.IndexOf("请重新登陆，如无法解决，请稍后再试") > 0 || rHtmlp.IndexOf("该门课程已选") < 0);
        }  
    }
}
