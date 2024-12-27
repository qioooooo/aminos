using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000B7 RID: 183
	[Guid("c7b67079-8255-42c6-9ec0-6994a3548780")]
	[ComImport]
	internal interface IAppDomainHelper
	{
		// Token: 0x06000458 RID: 1112
		void Initialize(IntPtr pUnkAD, IntPtr pfnShutdownCB, IntPtr data);

		// Token: 0x06000459 RID: 1113
		void DoCallback(IntPtr pUnkAD, IntPtr pfnCallbackCB, IntPtr data);
	}
}
