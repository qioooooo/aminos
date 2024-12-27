using System;
using System.Globalization;
using System.Resources;

namespace System.EnterpriseServices
{
	// Token: 0x0200009F RID: 159
	internal static class Resource
	{
		// Token: 0x060003C7 RID: 967 RVA: 0x0000C413 File Offset: 0x0000B413
		private static void InitResourceManager()
		{
			if (Resource._resmgr == null)
			{
				Resource._resmgr = new ResourceManager("System.EnterpriseServices", typeof(Resource).Module.Assembly);
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000C440 File Offset: 0x0000B440
		internal static string GetString(string key)
		{
			Resource.InitResourceManager();
			return Resource._resmgr.GetString(key, null);
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000C460 File Offset: 0x0000B460
		internal static string FormatString(string key)
		{
			return Resource.GetString(key);
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000C468 File Offset: 0x0000B468
		internal static string FormatString(string key, object a1)
		{
			return string.Format(CultureInfo.CurrentCulture, Resource.GetString(key), new object[] { a1 });
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000C494 File Offset: 0x0000B494
		internal static string FormatString(string key, object a1, object a2)
		{
			return string.Format(CultureInfo.CurrentCulture, Resource.GetString(key), new object[] { a1, a2 });
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000C4C4 File Offset: 0x0000B4C4
		internal static string FormatString(string key, object a1, object a2, object a3)
		{
			return string.Format(CultureInfo.CurrentCulture, Resource.GetString(key), new object[] { a1, a2, a3 });
		}

		// Token: 0x040001B5 RID: 437
		private static ResourceManager _resmgr;
	}
}
