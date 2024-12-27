using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters
{
	// Token: 0x020007A4 RID: 1956
	internal static class SerTrace
	{
		// Token: 0x0600462B RID: 17963 RVA: 0x000F043D File Offset: 0x000EF43D
		[Conditional("_LOGGING")]
		internal static void InfoLog(params object[] messages)
		{
		}

		// Token: 0x0600462C RID: 17964 RVA: 0x000F043F File Offset: 0x000EF43F
		[Conditional("SER_LOGGING")]
		internal static void Log(params object[] messages)
		{
			if (!(messages[0] is string))
			{
				messages[0] = messages[0].GetType().Name + " ";
				return;
			}
			messages[0] = messages[0] + " ";
		}
	}
}
