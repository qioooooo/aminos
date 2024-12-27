using System;
using System.Runtime.InteropServices;

namespace System.Web.Configuration
{
	// Token: 0x020001CB RID: 459
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
	internal class COSERVERINFO : IDisposable
	{
		// Token: 0x06001A0B RID: 6667 RVA: 0x0007ACDD File Offset: 0x00079CDD
		internal COSERVERINFO(string srvname, IntPtr authinf)
		{
			this.servername = srvname;
			this.authinfo = authinf;
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x0007ACF3 File Offset: 0x00079CF3
		void IDisposable.Dispose()
		{
			this.authinfo = IntPtr.Zero;
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001A0D RID: 6669 RVA: 0x0007AD08 File Offset: 0x00079D08
		~COSERVERINFO()
		{
		}

		// Token: 0x040017AE RID: 6062
		internal int reserved1;

		// Token: 0x040017AF RID: 6063
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string servername;

		// Token: 0x040017B0 RID: 6064
		internal IntPtr authinfo;

		// Token: 0x040017B1 RID: 6065
		internal int reserved2;
	}
}
