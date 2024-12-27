using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x020000EB RID: 235
	[Guid("FE6777FB-A674-4177-8F32-6D707E113484")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IAsyncErrorNotify
	{
		// Token: 0x06000556 RID: 1366
		void OnError(int hresult);
	}
}
