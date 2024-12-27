using System;
using System.Runtime.InteropServices;

namespace System.Web.Configuration
{
	// Token: 0x020001C2 RID: 450
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
	internal class COAUTHINFO : IDisposable
	{
		// Token: 0x0600199B RID: 6555 RVA: 0x000793F7 File Offset: 0x000783F7
		internal COAUTHINFO(RpcAuthent authent, RpcAuthor author, string serverprinc, RpcLevel level, RpcImpers impers, IntPtr ciptr)
		{
			this.authnsvc = authent;
			this.authzsvc = author;
			this.serverprincname = serverprinc;
			this.authnlevel = level;
			this.impersonationlevel = impers;
			this.authidentitydata = ciptr;
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x0007942C File Offset: 0x0007842C
		void IDisposable.Dispose()
		{
			this.authidentitydata = IntPtr.Zero;
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x00079440 File Offset: 0x00078440
		~COAUTHINFO()
		{
		}

		// Token: 0x04001775 RID: 6005
		internal RpcAuthent authnsvc;

		// Token: 0x04001776 RID: 6006
		internal RpcAuthor authzsvc;

		// Token: 0x04001777 RID: 6007
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string serverprincname;

		// Token: 0x04001778 RID: 6008
		internal RpcLevel authnlevel;

		// Token: 0x04001779 RID: 6009
		internal RpcImpers impersonationlevel;

		// Token: 0x0400177A RID: 6010
		internal IntPtr authidentitydata;

		// Token: 0x0400177B RID: 6011
		internal int capabilities;
	}
}
