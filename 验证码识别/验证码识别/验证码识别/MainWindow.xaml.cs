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
using System.Net;
using System.IO;
using System.Drawing;

namespace 验证码识别
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_Recognize_Click(object sender, RoutedEventArgs e)
        {
            if (txb_Adress.Text == "")
            {
                MessageBox.Show("请输入验证码地址！");
                return;
            }
            Bitmap bitmapObj=new Bitmap(GetImage(txb_Adress.Text).StreamSource);
            IdentifyCheckCode identifyCheckCode = new IdentifyCheckCode(bitmapObj);
            txb_CheckCode.Text = identifyCheckCode.Recognite();

            //让前端看清楚要进行的步骤
            IdentifyCheckCode idenfifyCheckCode1 = new IdentifyCheckCode(bitmapObj);
            Img_img1.Source =BitmapToBitmapImage(bitmapObj);
            Img_img2.Source = BitmapToBitmapImage(idenfifyCheckCode1.GrayPixels());
            Img_img3.Source = BitmapToBitmapImage(idenfifyCheckCode1.ClearNoise(128, 3));
            

        }
        /// 根据图片地址获取图片
        /// </summary>
        /// <param name="requestURL">图片的请求地址</param>
        /// <returns>BitmapImage类型图片</returns>
        private BitmapImage GetImage(string requestURL)
        {
            WebClient webClient = new WebClient();
            Byte[] imgByte = webClient.DownloadData(requestURL);
            MemoryStream memoryStream = new MemoryStream(imgByte);
            BitmapImage bitmapImages = new BitmapImage();
            bitmapImages.BeginInit();
            bitmapImages.StreamSource = memoryStream;
            bitmapImages.EndInit();
            return bitmapImages;
        }
        /// <summary>
        /// 把Bitmap转化为BitmapImgage
        /// </summary>
        /// <param name="bitmap">bitmap图像</param>
        /// <returns></returns>
        public BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            Bitmap bitmapSource = new Bitmap(bitmap.Width, bitmap.Height);
            int i, j;
            for (i = 0; i < bitmap.Width; i++)
                for (j = 0; j < bitmap.Height; j++)
                {
                    System.Drawing.Color pixelColor = bitmap.GetPixel(i, j);
                    System.Drawing.Color newColor = System.Drawing.Color.FromArgb(pixelColor.R, pixelColor.G, pixelColor.B);
                    bitmapSource.SetPixel(i, j, newColor);
                }
            MemoryStream ms = new MemoryStream();
            bitmapSource.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(ms.ToArray());
            bitmapImage.EndInit();

            return bitmapImage;
        }
    }
}
