using System;
using System.Runtime.InteropServices;

namespace SafeNet.Sentinel
{
	// Token: 0x02000007 RID: 7
	internal struct sntl_admin_context_t
	{
		// Token: 0x0400000C RID: 12
		private ushort lm_version_major;

		// Token: 0x0400000D RID: 13
		private ushort lm_version_minor;

		// Token: 0x0400000E RID: 14
		private ushort lm_http_status;

		// Token: 0x0400000F RID: 15
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string lm_server_str;

		// Token: 0x04000010 RID: 16
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
		private string address;

		// Token: 0x04000011 RID: 17
		private ushort port;

		// Token: 0x04000012 RID: 18
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
		private string password;

		// Token: 0x04000013 RID: 19
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
		private string password_hash;
	}
}
