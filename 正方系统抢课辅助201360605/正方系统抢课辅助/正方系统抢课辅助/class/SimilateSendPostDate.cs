using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Configuration;
using System.Text.RegularExpressions;

namespace 正方系统抢课辅助
{
    class SimilateSendPostDate
    {
        public  static string host = "202.192.143.230";
        //static string loginRefer = "http://202.192.128.41/jwc";登录之前的页面，好像删除后没影响
        public static string TagCode="";//登录标致码
        static string loginUri = "";//响应标题头的location,登录页面跳转
        public static string viewState = "";//浏览器提交数据时的控件的状态记录

        /// <summary>
        /// 模拟向RequestUriString请求数据,并返回学校登录页面分配的网址
        /// </summary>
        /// <param name="requestUriString">请求地址</param>
        /// <returns>返回登录页面网址</returns>
        public static string SimilateGetLoginAddress(string requestUriString)
        {
            HttpWebResponse httpWebResponse;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
            httpWebRequest.Method = "GET";
            httpWebRequest.KeepAlive = true;
            httpWebRequest.Accept = "text/html, application/xhtml+xml, */*";
            //httpWebRequest.Referer =loginRefer;
            httpWebRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
            httpWebRequest.Headers.Add("Accept-Language", "zh-Hans-CN,zh-Hans;q=0.5");
            //httpWebRequest.Host = host;
            httpWebRequest.KeepAlive = true;
            try
            {
                using (httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    loginUri = httpWebResponse.ResponseUri.ToString();//获取响应标题头的信息
                    TagCode = loginUri.Remove(0, loginUri.IndexOf("(")+1);//获取识别码
                    TagCode = TagCode.Remove(TagCode.IndexOf(")"), TagCode.Length - TagCode.IndexOf(")"));
                    using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.Default))
                    {
                       string htmlData = streamReader.ReadToEnd();
                       //向服务器请求登录页面的信息，获取ViewStae控制状态记录，然后用正则表达式提取
                       Regex regex = new Regex("name=\"__VIEWSTATE\" value=\".*\"\\s*/>");
                       Match match = regex.Match(htmlData);
                       viewState = System.Web.HttpUtility.UrlEncode(match.Value.Replace("name=\"__VIEWSTATE\" value=\"", "").Replace("\"", "").Replace("/>", "").Trim());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("出错啦！",ex+ "正方系统崩溃中……");
            }
            return loginUri;//登录页面地址
        }
        /// <summary>
        /// 模拟向RequestUriString请求数据,没有带有Post请求的数据
        /// </summary>
        /// <param name="requestUriString">请求地址</param>
        /// <returns>返回Response正文</returns>
        public static string SimilateGetData(string requestUriString)
        {
            string htmlData = "";
            HttpWebResponse httpWebResponse;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
            httpWebRequest.Method = "GET";
            httpWebRequest.KeepAlive = true;
            httpWebRequest.Accept = "text/html, application/xhtml+xml, */*";
            //httpWebRequest.Referer = loginRefer;//登录之前的页面
            httpWebRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
            httpWebRequest.Headers.Add("Accept-Language", "zh-Hans-CN,zh-Hans;q=0.5");
            //httpWebRequest.Host = host;
            httpWebRequest.KeepAlive = true;
            try
            {
                using (httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.Default))
                    {
                        htmlData = streamReader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生异常！" + ex.ToString());
            }
            return htmlData;
        }
        /// <summary>
        /// 模拟请求图片
        /// </summary>
        /// <param name="requestUriString">图片的地址</param>
        /// <returns>BitMapImage类型图片</returns>
        public static BitmapImage SimilateRequestPicture(string requestUriString)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(requestUriString);
            httpWebRequest.AllowWriteStreamBuffering = true;
            httpWebRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;
            httpWebRequest.MaximumResponseHeadersLength = -1;
            httpWebRequest.Accept = "image/png, image/svg+xml, image/*;q=0.8, */*;q=0.5";
            httpWebRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.Method = "GET";
            httpWebRequest.Headers.Add("Accept-Language", "zh-Hans-CN,zh-Hans;q=0.5");
            httpWebRequest.Headers.Add("Accept-Encoding", "gzip,deflate");
            httpWebRequest.KeepAlive = true;
            try
            {
                BitmapImage bitMapImage = new BitmapImage();
                using (HttpWebResponse WebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (Stream stream = WebResponse.GetResponseStream())
                    {
                        List<Byte> list = new List<byte>();
                        while (true)
                        {
                            int data = stream.ReadByte();
                            if (data == -1)
                                break;
                            list.Add((byte)data);
                        }
                        MemoryStream memoryStream = new MemoryStream(list.ToArray());
                        bitMapImage.BeginInit();
                        bitMapImage.StreamSource = memoryStream;
                        bitMapImage.EndInit();
                        return bitMapImage;
                    }
                }

            }
            catch (Exception ex)
            {
                return null;
            }

        }
        /// <summary>
        /// 带有Post请求的数据
        /// </summary>
        /// <param name="data">Post请求的数据</param>
        /// <param name="requestUriString">请求址</param>
        /// <returns>返回Response正文</returns>
        public static string SimilateGetDataByPost(string data, string requestUriString)
        {
            byte[] postData = Encoding.Default.GetBytes(data);
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(requestUriString);
            HttpWebResponse httpWebResponse;
            string responseData = "";
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
            httpWebRequest.ContentLength = postData.Length;
            httpWebRequest.Headers.Add("Accept-Language", "zh-Hans-CN,zh-Hans;q=0.5");
            httpWebRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            httpWebRequest.Accept = "text/html, application/xhtml+xml, */*";
            //httpWebRequest.Referer = loginUri;
            //httpWebRequest.Host = host;

            httpWebRequest.KeepAlive = true;
            using (Stream stream = httpWebRequest.GetRequestStream())
            {
                stream.Write(postData, 0, postData.Length);//向stream后面加Post数据
            }

            try
            {
                using (httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), System.Text.Encoding.GetEncoding("GB2312")))
                    {
                        responseData = streamReader.ReadToEnd();

                    }
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show("出现异常！" + ex.Message);
            }

            return responseData;
        }
        /// <summary>
        /// 模拟请求数据，带有之前页面请求，在Fram下
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="Refer">之前的页面</param>
        /// <param name="requestUriString">链接的字符串</param>
        /// <returns></returns>
        public static string SimilateAndGetDataWithRefer(string data, string Refer, string requestUriString)
        {
            byte[] postData = Encoding.Default.GetBytes(data);
            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(requestUriString);
            HttpWebResponse httpWebResponse;
            string responseData = "";
            //请求的数据头
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
            httpWebRequest.ContentLength = postData.Length;
            httpWebRequest.Headers.Add("Accept-Language", "zh-Hans-CN,zh-Hans;q=0.5");
            httpWebRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
            httpWebRequest.Accept = "text/html, application/xhtml+xml, */*";
            httpWebRequest.Referer =Refer;
            //httpWebRequest.Host = host;//主机地址

            httpWebRequest.KeepAlive = true;
            using (Stream stream = httpWebRequest.GetRequestStream())
            {
                stream.Write(postData, 0, postData.Length);
            }

            try
            {
                using (httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), System.Text.Encoding.GetEncoding("GB2312")))
                    {
                        responseData = streamReader.ReadToEnd();

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("出现异常！" + ex.Message);
            }
            return responseData;
        }
    }
}
