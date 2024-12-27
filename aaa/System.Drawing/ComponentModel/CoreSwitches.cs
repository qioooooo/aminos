using System;
using System.Diagnostics;

namespace System.ComponentModel
{
	// Token: 0x02000011 RID: 17
	internal static class CoreSwitches
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000039 RID: 57 RVA: 0x00002DE0 File Offset: 0x00001DE0
		public static BooleanSwitch PerfTrack
		{
			get
			{
				if (CoreSwitches.perfTrack == null)
				{
					CoreSwitches.perfTrack = new BooleanSwitch("PERFTRACK", "Debug performance critical sections.");
				}
				return CoreSwitches.perfTrack;
			}
		}

		// Token: 0x040000CE RID: 206
		private static BooleanSwitch perfTrack;
	}
}
