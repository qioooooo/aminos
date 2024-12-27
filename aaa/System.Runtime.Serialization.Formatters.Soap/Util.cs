using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Soap
{
	// Token: 0x02000010 RID: 16
	internal static class Util
	{
		// Token: 0x06000061 RID: 97 RVA: 0x000056CA File Offset: 0x000046CA
		internal static string PString(string value)
		{
			if (value == null)
			{
				return "";
			}
			return value;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000056D6 File Offset: 0x000046D6
		[Conditional("SER_LOGGING")]
		internal static void NVTrace(string name, string value)
		{
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000056D8 File Offset: 0x000046D8
		[Conditional("SER_LOGGING")]
		internal static void NVTrace(string name, object value)
		{
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000056DA File Offset: 0x000046DA
		[Conditional("_LOGGING")]
		internal static void NVTraceI(string name, string value)
		{
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000056DC File Offset: 0x000046DC
		[Conditional("_LOGGING")]
		internal static void NVTraceI(string name, object value)
		{
		}
	}
}
