using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x0200004A RID: 74
	[Guid("CAFC823C-B441-11D1-B82B-0000F8757E2A")]
	[ComImport]
	internal interface ISecurityIdentityColl
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600017F RID: 383
		int Count
		{
			[DispId(1610743808)]
			get;
		}

		// Token: 0x06000180 RID: 384
		[DispId(0)]
		object GetItem([MarshalAs(UnmanagedType.BStr)] [In] string lIndex);

		// Token: 0x06000181 RID: 385
		[DispId(-4)]
		void GetEnumerator(out IEnumerator pEnum);
	}
}
