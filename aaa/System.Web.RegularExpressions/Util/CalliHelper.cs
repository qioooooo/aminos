using System;

namespace System.Web.Util
{
	// Token: 0x02000050 RID: 80
	internal sealed class CalliHelper
	{
		// Token: 0x060000B7 RID: 183 RVA: 0x00013391 File Offset: 0x00012391
		internal static void EventArgFunctionCaller(IntPtr fp, object o, object t, EventArgs e)
		{
			calli(System.Void(System.Object,System.EventArgs), o, t, e, fp);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0001339C File Offset: 0x0001239C
		internal static void ArglessFunctionCaller(IntPtr fp, object o)
		{
			calli(System.Void(), o, fp);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000133A5 File Offset: 0x000123A5
		internal CalliHelper()
		{
		}
	}
}
