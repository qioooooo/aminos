using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x0200004B RID: 75
	[Guid("CAFC823E-B441-11D1-B82B-0000F8757E2A")]
	[ComImport]
	internal interface ISecurityCallContext
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000182 RID: 386
		int Count
		{
			[DispId(1610743813)]
			get;
		}

		// Token: 0x06000183 RID: 387
		[DispId(0)]
		object GetItem([MarshalAs(UnmanagedType.BStr)] [In] string name);

		// Token: 0x06000184 RID: 388
		[DispId(-4)]
		void GetEnumerator(out IEnumerator pEnum);

		// Token: 0x06000185 RID: 389
		[DispId(1610743814)]
		bool IsCallerInRole([MarshalAs(UnmanagedType.BStr)] [In] string role);

		// Token: 0x06000186 RID: 390
		[DispId(1610743815)]
		bool IsSecurityEnabled();

		// Token: 0x06000187 RID: 391
		[DispId(1610743816)]
		bool IsUserInRole([MarshalAs(UnmanagedType.Struct)] [In] ref object pUser, [MarshalAs(UnmanagedType.BStr)] [In] string role);
	}
}
