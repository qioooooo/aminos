using System;
using System.Diagnostics;

namespace System.ComponentModel
{
	// Token: 0x0200003A RID: 58
	internal static class CoreSwitches
	{
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x0600019C RID: 412 RVA: 0x00006614 File Offset: 0x00005614
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

		// Token: 0x04000B5F RID: 2911
		private static BooleanSwitch perfTrack;
	}
}
