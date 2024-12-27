using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000013 RID: 19
	[Guid("0FB15084-AF41-11CE-BD2B-204C4F4F5020")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface ITransaction
	{
		// Token: 0x0600003D RID: 61
		void Commit(int fRetaining, int grfTC, int grfRM);

		// Token: 0x0600003E RID: 62
		void Abort(ref BOID pboidReason, int fRetaining, int fAsync);

		// Token: 0x0600003F RID: 63
		void GetTransactionInfo(out XACTTRANSINFO pinfo);
	}
}
