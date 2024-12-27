using System;
using System.Diagnostics;

namespace System.ComponentModel
{
	// Token: 0x02000010 RID: 16
	internal static class CompModSwitches
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002D9C File Offset: 0x00001D9C
		public static TraceSwitch HandleLeak
		{
			get
			{
				if (CompModSwitches.handleLeak == null)
				{
					CompModSwitches.handleLeak = new TraceSwitch("HANDLELEAK", "HandleCollector: Track Win32 Handle Leaks");
				}
				return CompModSwitches.handleLeak;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002DBE File Offset: 0x00001DBE
		public static BooleanSwitch TraceCollect
		{
			get
			{
				if (CompModSwitches.traceCollect == null)
				{
					CompModSwitches.traceCollect = new BooleanSwitch("TRACECOLLECT", "HandleCollector: Trace HandleCollector operations");
				}
				return CompModSwitches.traceCollect;
			}
		}

		// Token: 0x040000CC RID: 204
		private static TraceSwitch handleLeak;

		// Token: 0x040000CD RID: 205
		private static BooleanSwitch traceCollect;
	}
}
