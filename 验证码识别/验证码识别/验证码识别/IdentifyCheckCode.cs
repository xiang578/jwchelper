using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 验证码识别
{
    /// <summary>
    /// 使用方法，先实例化一个对象，然后再调用方法Recognize
    /// </summary>
    class IdentifyCheckCode
    {
        Bitmap _bitmapObj = null;
        //数字和样本的键值对
        Dictionary<string, List<string>> m_Classifications = new Dictionary<string, List<string>>();
        //从数字1到9的验证码样本
        private string str_Template =
      "0:011111000111111011100111110000111100001111000011110000111100001111000011110000110111111001111100" + "\r\n" +
      "1:000011000001110000111100011111000110110000001100000011000000110000001100000011000000110000001100" + "\r\n" +
      "2:001111000111111011100011110000110000001100000110000011100001110000110000011100001111111111111111" + "\r\n" +
      "3:001111100111111101100011000001110001111100011111000001110000001111000011111001110111110000111000" + "\r\n" +
      "4:000001100000111000011110000111100011011000110110011001101100011011111111111111110000011000000110" + "\r\n" +
      "5:011111100111111001100000111000001111111011111110111111110000001111100011111001110111111000111100" + "\r\n" +
      "6:001111100111111111100011110000001101110011111110111000101100001111000011111001110111111000111100" + "\r\n" +
      "7:111111111111111100000110000001100000110000011100000110000001100000110000001100000011000000110000" + "\r\n" +
      "8:011111000111111011000011110000111110001100111110011111101100001111000011110000110111111000111100" + "\r\n" +
      "9:001111000111111011100111110000111110001111100111011111110011101100000011110001101111111000111000";
        public IdentifyCheckCode(Bitmap bitmapOjb)
        {
            LoadFFromString(str_Template);
            _bitmapObj = new Bitmap(bitmapOjb);
        }
        /// <summary>
        /// 初始化储存模板数据的键值对
        /// </summary>
        /// <param name="str_template">数字模板</param>
        public void LoadFFromString(string str_template)
        {
            string[] strs = str_template.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);//去掉\r\n和空格回车
            foreach (string s in strs)
            {
                string[] strKv = s.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);//去掉：和空格回车
                //不存在这个键就new一个list<string>然后给这赋值
                if (!m_Classifications.ContainsKey(strKv[0]))
                {
                    m_Classifications[strKv[0]] = new List<string>();
                }
                var list = m_Classifications[strKv[0]];
                list.Add(strKv[1]);
            }
        }
        /// <summary>
        /// 识别验证码
        /// </summary>
        /// <param name="bitmap_Code">验证码图片</param>
        /// <returns>验证码</returns>
        public string Recognite()
        {
            GrayPixels();
            ClearNoise(128,3);
            string strRet = "";
            //分割图片
            //当是四个纯数字时，string[4]是nOffest+=9,6,8,12现在的是五个纯数字
            string[] strs = new string[5];
            for (int i = 0, nOffset = -4; i < strs.Length; ++i)
                strs[i] = getF(_bitmapObj, new System.Drawing.Rectangle(nOffset += 9, 6, 8, 12));
            //把四个结果组成一个字符串
            foreach (string str in strs)
                strRet += Classifiy(str);
            return strRet;
        }
        /// <summary>
        /// 得到分割后的矩形
        /// </summary>
        /// <param name="bitmap_Code">验证码图片</param>
        /// <param name="rect">分割矩形的大小</param>
        /// <returns>图片01字符</returns>
        string getF(Bitmap bitmap_Code, System.Drawing.Rectangle rect)
        {
            StringBuilder sb = new StringBuilder();
            for (int y = rect.Y; y < rect.Bottom; ++y)
            {
                for (int x = rect.X; x < rect.Right; ++x)
                {
                    var cr = bitmap_Code.GetPixel(x, y);
                    if (cr.ToArgb() == -1)
                    {
                        sb.Append("0");
                    }
                    else
                    {
                        sb.Append("1");
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 和模板值进行比对
        /// </summary>
        /// <param name="strF">验证码的单个图片01字符</param>
        /// <returns></returns>
        string Classifiy(string strF)
        {
            string strRet = "";

            double Max = 0;
            foreach (var kv in m_Classifications)
            {
                foreach (var F in kv.Value)
                {
                    double nCount = 0;
                    for (int i = 0; i < strF.Length; ++i)
                    {
                        if (strF[i] == F[i])
                        {
                            ++nCount;
                        }
                    }

                    double a = nCount / 96;
                    if (Max < a)
                    {
                        Max = a;
                        strRet = kv.Key;
                    }
                }
            }

            return strRet;
        }
        /// <summary>
        /// 二值化，进行灰度转换，通过每个点
        /// </summary>
        public Bitmap GrayPixels()
        {
            for(int h=0;h<_bitmapObj.Height;h++)
                for (int w = 0; w < _bitmapObj.Width; w++)
                {
                    int temp=GetGrayNumColor(_bitmapObj.GetPixel(w,h));
                    _bitmapObj.SetPixel(w,h,Color.FromArgb(temp,temp,temp));
                }
            return _bitmapObj;
        }
        /// <summary>
        /// 根据RGB,计算灰度值
        /// </summary>
        /// <param name="color">某像素Color值</param>
        /// <returns>灰度值</returns>
        private int GetGrayNumColor(Color color)
        {
            return(color.R*19595+color.G*38469+color.B*7472)>>16;
        }
        /// <summary>
        ///  去噪，去掉杂点（适合杂点/杂线粗为1）
        /// </summary>
        /// <param name="dgGrayValue">背前景灰色界限一般的是128</param>
        /// <returns></returns>
        public Bitmap ClearNoise(int dgGrayValue, int MaxNearPoints)
        {
            Color piexl;
            int nearDots = 0;
            //逐点判断
            for (int i = 0; i < _bitmapObj.Width; i++)
                for (int j = 0; j < _bitmapObj.Height; j++)
                {
                    piexl = _bitmapObj.GetPixel(i, j);
                    if (piexl.R < dgGrayValue)
                    {
                        nearDots = 0;
                        //判断周围8个点是否全为空
                        if (i == 0 || i == _bitmapObj.Width - 1 || j == 0 || j == _bitmapObj.Height - 1)  //边框全去掉
                        {
                            _bitmapObj.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                        }
                        else
                        {
                            if (_bitmapObj.GetPixel(i - 1, j - 1).R < dgGrayValue) nearDots++;
                            if (_bitmapObj.GetPixel(i, j - 1).R < dgGrayValue) nearDots++;
                            if (_bitmapObj.GetPixel(i + 1, j - 1).R < dgGrayValue) nearDots++;
                            if (_bitmapObj.GetPixel(i - 1, j).R < dgGrayValue) nearDots++;
                            if (_bitmapObj.GetPixel(i + 1, j).R < dgGrayValue) nearDots++;
                            if (_bitmapObj.GetPixel(i - 1, j + 1).R < dgGrayValue) nearDots++;
                            if (_bitmapObj.GetPixel(i, j + 1).R < dgGrayValue) nearDots++;
                            if (_bitmapObj.GetPixel(i + 1, j + 1).R < dgGrayValue) nearDots++;
                        }

                        if (nearDots < MaxNearPoints)
                            _bitmapObj.SetPixel(i, j, Color.FromArgb(255, 255, 255));   //去掉单点 && 粗细小3邻边点
                    }
                    else  //背景
                        _bitmapObj.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                }
            return _bitmapObj;
        }
    }
}
