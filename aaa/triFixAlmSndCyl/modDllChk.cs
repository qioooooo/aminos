using System;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace triFixAlmSndCyl
{
	// Token: 0x02000014 RID: 20
	[StandardModule]
	internal sealed class modDllChk
	{
		// Token: 0x0600022F RID: 559 RVA: 0x000056E4 File Offset: 0x00003AE4
		public static void subDLLCheck()
		{
			string text = Application.StartupPath + "\\";
			modDllChk.subDLLCheck(text + "GeFanuc.iFixToolkit.Adapter", "GeFanuc.iFixToolkit.Adapter.Helper");
			modDllChk.subDLLCheck(text + "kvNetClass", "kvNetClass.clsCrypt");
			modDllChk.subDLLCheck(text + "Trendtek.iFix", "Trendtek.iFix.FixHelper");
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00005740 File Offset: 0x00003B40
		public static void subDLLCheck(string DllName, string ClassName)
		{
			try
			{
				Assembly assembly = Assembly.LoadFrom(DllName + ".dll");
				Type type = assembly.GetType(ClassName);
				if (Information.IsNothing(type))
				{
					MessageBox.Show(string.Concat(new string[] { "In the <", DllName, ".dll> doesn't have <", ClassName, "> CLASS" }), "Class Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					Environment.Exit(0);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Missing  <'" + DllName + ".dll> file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Environment.Exit(0);
			}
		}
	}
}
