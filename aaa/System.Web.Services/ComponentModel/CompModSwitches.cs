using System;
using System.Diagnostics;

namespace System.ComponentModel
{
	// Token: 0x02000007 RID: 7
	internal sealed class CompModSwitches
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000022AC File Offset: 0x000012AC
		public static BooleanSwitch DisableRemoteDebugging
		{
			get
			{
				if (CompModSwitches.disableRemoteDebugging == null)
				{
					CompModSwitches.disableRemoteDebugging = new BooleanSwitch("Remote.Disable", "Disable remote debugging for web methods.");
				}
				return CompModSwitches.disableRemoteDebugging;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000022CE File Offset: 0x000012CE
		public static TraceSwitch DynamicDiscoverySearcher
		{
			get
			{
				if (CompModSwitches.dynamicDiscoSearcher == null)
				{
					CompModSwitches.dynamicDiscoSearcher = new TraceSwitch("DynamicDiscoverySearcher", "Enable tracing for the DynamicDiscoverySearcher class.");
				}
				return CompModSwitches.dynamicDiscoSearcher;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000022F0 File Offset: 0x000012F0
		public static BooleanSwitch DynamicDiscoveryVirtualSearch
		{
			get
			{
				if (CompModSwitches.dynamicDiscoVirtualSearch == null)
				{
					CompModSwitches.dynamicDiscoVirtualSearch = new BooleanSwitch("DynamicDiscoveryVirtualSearch", "Force virtual search for DiscoveryRequestHandler class.");
				}
				return CompModSwitches.dynamicDiscoVirtualSearch;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002312 File Offset: 0x00001312
		public static TraceSwitch Remote
		{
			get
			{
				if (CompModSwitches.remote == null)
				{
					CompModSwitches.remote = new TraceSwitch("Microsoft.WFC.Remote", "Enable tracing for remote method calls.");
				}
				return CompModSwitches.remote;
			}
		}

		// Token: 0x040001CB RID: 459
		private static BooleanSwitch dynamicDiscoVirtualSearch;

		// Token: 0x040001CC RID: 460
		private static TraceSwitch dynamicDiscoSearcher;

		// Token: 0x040001CD RID: 461
		private static BooleanSwitch disableRemoteDebugging;

		// Token: 0x040001CE RID: 462
		private static TraceSwitch remote;
	}
}
