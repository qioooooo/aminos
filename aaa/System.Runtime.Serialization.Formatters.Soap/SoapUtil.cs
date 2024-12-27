using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace System.Runtime.Serialization.Formatters.Soap
{
	// Token: 0x0200000C RID: 12
	internal static class SoapUtil
	{
		// Token: 0x06000051 RID: 81 RVA: 0x000050A0 File Offset: 0x000040A0
		[Conditional("SER_LOGGING")]
		internal static void DumpHash(string tag, Hashtable hashTable)
		{
			IDictionaryEnumerator enumerator = hashTable.GetEnumerator();
			while (enumerator.MoveNext())
			{
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000050BC File Offset: 0x000040BC
		private static ResourceManager InitResourceManager()
		{
			if (SoapUtil.SystemResMgr == null)
			{
				SoapUtil.SystemResMgr = new ResourceManager("SoapFormatter", typeof(SoapParser).Module.Assembly);
			}
			return SoapUtil.SystemResMgr;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x000050F0 File Offset: 0x000040F0
		internal static string GetResourceString(string key)
		{
			if (SoapUtil.SystemResMgr == null)
			{
				SoapUtil.InitResourceManager();
			}
			return SoapUtil.SystemResMgr.GetString(key, null);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00005118 File Offset: 0x00004118
		internal static string GetResourceString(string key, params object[] values)
		{
			if (SoapUtil.SystemResMgr == null)
			{
				SoapUtil.InitResourceManager();
			}
			string @string = SoapUtil.SystemResMgr.GetString(key, null);
			return string.Format(CultureInfo.CurrentCulture, @string, values);
		}

		// Token: 0x04000044 RID: 68
		internal static Type typeofString = typeof(string);

		// Token: 0x04000045 RID: 69
		internal static Type typeofBoolean = typeof(bool);

		// Token: 0x04000046 RID: 70
		internal static Type typeofObject = typeof(object);

		// Token: 0x04000047 RID: 71
		internal static Type typeofSoapFault = typeof(SoapFault);

		// Token: 0x04000048 RID: 72
		internal static Assembly urtAssembly = Assembly.GetAssembly(SoapUtil.typeofString);

		// Token: 0x04000049 RID: 73
		internal static string urtAssemblyString = SoapUtil.urtAssembly.FullName;

		// Token: 0x0400004A RID: 74
		internal static ResourceManager SystemResMgr;
	}
}
