using System;
using System.Diagnostics;

namespace System.ComponentModel
{
	// Token: 0x0200000A RID: 10
	internal static class CompModSwitches
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000D RID: 13 RVA: 0x0000229D File Offset: 0x0000129D
		public static TraceSwitch InstallerDesign
		{
			get
			{
				if (CompModSwitches.installerDesign == null)
				{
					CompModSwitches.installerDesign = new TraceSwitch("InstallerDesign", "Enable tracing for design-time code for installers");
				}
				return CompModSwitches.installerDesign;
			}
		}

		// Token: 0x040000DC RID: 220
		private static TraceSwitch installerDesign;
	}
}
