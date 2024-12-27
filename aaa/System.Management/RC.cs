using System;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace System.Management
{
	// Token: 0x0200007B RID: 123
	internal sealed class RC
	{
		// Token: 0x06000367 RID: 871 RVA: 0x0000DFBD File Offset: 0x0000CFBD
		private RC()
		{
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0000DFC5 File Offset: 0x0000CFC5
		public static string GetString(string strToGet)
		{
			return RC.resMgr.GetString(strToGet, CultureInfo.CurrentCulture);
		}

		// Token: 0x040001CD RID: 461
		private static readonly ResourceManager resMgr = new ResourceManager(Assembly.GetExecutingAssembly().GetName().Name, Assembly.GetExecutingAssembly(), null);
	}
}
