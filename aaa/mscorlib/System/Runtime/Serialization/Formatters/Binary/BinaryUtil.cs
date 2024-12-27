using System;
using System.Diagnostics;

namespace System.Runtime.Serialization.Formatters.Binary
{
	// Token: 0x020007BF RID: 1983
	internal static class BinaryUtil
	{
		// Token: 0x060046D6 RID: 18134 RVA: 0x000F369D File Offset: 0x000F269D
		[Conditional("_LOGGING")]
		public static void NVTraceI(string name, string value)
		{
			BCLDebug.CheckEnabled("BINARY");
		}

		// Token: 0x060046D7 RID: 18135 RVA: 0x000F36AA File Offset: 0x000F26AA
		[Conditional("_LOGGING")]
		public static void NVTraceI(string name, object value)
		{
			BCLDebug.CheckEnabled("BINARY");
		}
	}
}
