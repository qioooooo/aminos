using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000049 RID: 73
	[Guid("CAFC823D-B441-11D1-B82B-0000F8757E2A")]
	[ComImport]
	internal interface ISecurityCallersColl
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600017C RID: 380
		int Count
		{
			[DispId(1610743808)]
			get;
		}

		// Token: 0x0600017D RID: 381
		[DispId(0)]
		ISecurityIdentityColl GetItem(int lIndex);

		// Token: 0x0600017E RID: 382
		[DispId(-4)]
		void GetEnumerator(out IEnumerator pEnum);
	}
}
