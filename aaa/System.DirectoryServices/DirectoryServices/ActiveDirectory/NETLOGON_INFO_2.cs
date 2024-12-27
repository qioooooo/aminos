using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000123 RID: 291
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class NETLOGON_INFO_2
	{
		// Token: 0x040006CC RID: 1740
		public int netlog2_flags;

		// Token: 0x040006CD RID: 1741
		public int netlog2_pdc_connection_status;

		// Token: 0x040006CE RID: 1742
		public IntPtr netlog2_trusted_dc_name;

		// Token: 0x040006CF RID: 1743
		public int netlog2_tc_connection_status;
	}
}
