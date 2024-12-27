using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020003FE RID: 1022
	internal struct SSL_EXTRA_CERT_CHAIN_POLICY_PARA
	{
		// Token: 0x060020B0 RID: 8368 RVA: 0x00080D66 File Offset: 0x0007FD66
		internal SSL_EXTRA_CERT_CHAIN_POLICY_PARA(bool amIServer)
		{
			this.u.cbStruct = SSL_EXTRA_CERT_CHAIN_POLICY_PARA.StructSize;
			this.u.cbSize = SSL_EXTRA_CERT_CHAIN_POLICY_PARA.StructSize;
			this.dwAuthType = (amIServer ? 1 : 2);
			this.fdwChecks = 0U;
			this.pwszServerName = null;
		}

		// Token: 0x04002056 RID: 8278
		internal SSL_EXTRA_CERT_CHAIN_POLICY_PARA.U u;

		// Token: 0x04002057 RID: 8279
		internal int dwAuthType;

		// Token: 0x04002058 RID: 8280
		internal uint fdwChecks;

		// Token: 0x04002059 RID: 8281
		internal unsafe char* pwszServerName;

		// Token: 0x0400205A RID: 8282
		private static readonly uint StructSize = (uint)Marshal.SizeOf(typeof(SSL_EXTRA_CERT_CHAIN_POLICY_PARA));

		// Token: 0x020003FF RID: 1023
		[StructLayout(LayoutKind.Explicit)]
		internal struct U
		{
			// Token: 0x0400205B RID: 8283
			[FieldOffset(0)]
			internal uint cbStruct;

			// Token: 0x0400205C RID: 8284
			[FieldOffset(0)]
			internal uint cbSize;
		}
	}
}
