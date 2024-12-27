using System;
using System.Runtime.InteropServices;

namespace System.Web.Configuration
{
	// Token: 0x020001C3 RID: 451
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
	internal class COAUTHINFO_X64 : IDisposable
	{
		// Token: 0x0600199E RID: 6558 RVA: 0x00079468 File Offset: 0x00078468
		internal COAUTHINFO_X64(RpcAuthent authent, RpcAuthor author, string serverprinc, RpcLevel level, RpcImpers impers, IntPtr ciptr)
		{
			this.authnsvc = authent;
			this.authzsvc = author;
			this.serverprincname = serverprinc;
			this.authnlevel = level;
			this.impersonationlevel = impers;
			this.authidentitydata = ciptr;
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x0007949D File Offset: 0x0007849D
		void IDisposable.Dispose()
		{
			this.authidentitydata = IntPtr.Zero;
			GC.SuppressFinalize(this);
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x000794B0 File Offset: 0x000784B0
		~COAUTHINFO_X64()
		{
		}

		// Token: 0x0400177C RID: 6012
		internal RpcAuthent authnsvc;

		// Token: 0x0400177D RID: 6013
		internal RpcAuthor authzsvc;

		// Token: 0x0400177E RID: 6014
		[MarshalAs(UnmanagedType.LPWStr)]
		internal string serverprincname;

		// Token: 0x0400177F RID: 6015
		internal RpcLevel authnlevel;

		// Token: 0x04001780 RID: 6016
		internal RpcImpers impersonationlevel;

		// Token: 0x04001781 RID: 6017
		internal IntPtr authidentitydata;

		// Token: 0x04001782 RID: 6018
		internal int capabilities;

		// Token: 0x04001783 RID: 6019
		internal int padding;
	}
}
