using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace jwchelper
{
	static class Program
	{
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]

		static void Main()
		{
			Login login = Login.get();
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			while (true)
			{
				if (!login.isLogin())
				{
					Application.Run(new FormLogin());
				}
				if (login.isLogin())
				{
					Application.Run(new FormMain());
				}
				if (login.isLogout() == false)
                    break;
			}
		}
	}
}
