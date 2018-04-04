using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

namespace 正方系统抢课辅助.Class
{
    public class SaveUserNameAndPassword
    {
        readonly string _filePath = "d:/userMessage.xml";
        XmlDocument xmlDoc = new XmlDocument();
        public SaveUserNameAndPassword()
        {
            //判断文件是不是存在，如果不存在则创建
            if (!File.Exists(_filePath))
            {
                XmlTextWriter xmlTextWriter = new XmlTextWriter(_filePath, Encoding.Default);
                xmlTextWriter.Formatting = Formatting.Indented;
                xmlTextWriter.WriteStartDocument();  
                xmlTextWriter.WriteStartElement("root");
                xmlTextWriter.Close();
            }
        }


        /// <summary>
        /// 保存用户的帐号和密码
        /// </summary>
        public  void SaveUserIdAndPwd(string id, string pwd)
        {
            xmlDoc.Load(_filePath);
            XmlNodeList userMessageNodes = xmlDoc.SelectSingleNode("root").ChildNodes;//选择根结点
            foreach (XmlNode userMessageNode in userMessageNodes)//遍历根结点下的子结点
            {
                XmlNodeList userIdandPwd = userMessageNode.ChildNodes;
                foreach (XmlNode userId in userIdandPwd)
                {
                    if (userId.InnerText == id)
                    {
                        pwd = userId.NextSibling.InnerText.ToString();
                        xmlDoc.Save(_filePath);
                        return;
                    }
                }
            }
            //当结点不存在用户信息的时候添加进去
            XmlNode rootNode = xmlDoc.SelectSingleNode("root");
            XmlElement Ele_userMessage = xmlDoc.CreateElement("userMsg");
            XmlElement Ele_userId = xmlDoc.CreateElement("userId");
            XmlElement Ele_userPwd = xmlDoc.CreateElement("userPwd");
            Ele_userId.InnerText = id;
            Ele_userPwd.InnerText = DESEncrypt.DesEncrypt(pwd);
            Ele_userMessage.AppendChild(Ele_userId);
            Ele_userMessage.AppendChild(Ele_userPwd);
            rootNode.AppendChild(Ele_userMessage);
            xmlDoc.Save(_filePath);
        }

        /// <summary>
        /// 提取密码
        /// </summary>
        /// <param name="userId">账号</param>
        /// <returns></returns>
        public string GetPwd(string id)
        {
            xmlDoc.Load(_filePath);
            XmlNodeList userMsgNodes = xmlDoc.SelectSingleNode("root").ChildNodes;
            foreach (XmlNode userMsgNode in userMsgNodes)
            {
                XmlNodeList userIdandPwd = userMsgNode.ChildNodes;
                foreach (XmlNode userId in userIdandPwd)
                {
                    if (userId.InnerText == id)
                    {
                        string pwd = userId.NextSibling.InnerText;
                        if (pwd != "")
                        {
                            return DESEncrypt.DesDecrypt(pwd);
                        }
                    }
                }
            }

            return "";
        }

        public List<string> GetAllUserId()
        {
            List<string> userId_list = new List<string>() { };
            xmlDoc.Load(_filePath);
            XmlNodeList userMsgNodes = xmlDoc.SelectSingleNode("root").ChildNodes;
            foreach (XmlNode userMsgNode in userMsgNodes)
            {
                userId_list.Add(userMsgNode.FirstChild.InnerText.ToString());
            }
            return userId_list;
        }
    }
}
