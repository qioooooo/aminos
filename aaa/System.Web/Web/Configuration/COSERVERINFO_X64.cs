using System;
using System.Runtime.InteropServices;

namespace System.Web.Configuration
{
	// Token: 0x020001CC RID: 460
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
	internal class COSERVERINFO_X64 : IDisposable
	{
		// Token: 0x06001A0E RID: 6670 RVA: 0x0007AD30 File Offset: 0x00079D30
		internal COSERVERINFO_X64(string srvname, IntPtr authinf)
		{
			this.servername = srvname;
			this.authinfo = authinf;
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x0007AD46 File Offset: 0x00079D46
		void IDisposable.Dispose()
		{
			this.authinfo = IntPtr.Zero;
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x0007AD5C File Offset: 0x00079D5C
		~COSERVERINFO_X64()
		{
		}

		// Token: 0x040017B2 RID: 6066
		internal int reserved1;

		// Token: 0x040017B3 RID: 6067
		internal int padding1;

		// Token: 0x040017B4 RID: 6068
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string servername;

		// Token: 0x040017B5 RID: 6069
		internal IntPtr authinfo;

		// Token: 0x040017B6 RID: 6070
		internal int reserved2;

		// Token: 0x040017B7 RID: 6071
		internal int padding2;
	}
}
