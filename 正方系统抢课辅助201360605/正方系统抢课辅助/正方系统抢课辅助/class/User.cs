using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Xml;

namespace 正方系统抢课辅助
{
    class User
    {
        //使用者的帐号，既是学生的学号
        private string userName=Student.StuNum;
        public string UserName 
        {
            get
            {
                return userName;
            }
        }
        //使用都的密码
        private string userPassword;
        public string  UserPassword 
        {
            set
            {
                userPassword = value;
            }
        }

        private void SaveUser()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load("user.xml");
        }
    }
}
