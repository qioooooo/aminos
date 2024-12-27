using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000067 RID: 103
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("72380d55-8d2b-43a3-8513-2b6ef31434e9")]
	[ComImport]
	public interface IProcessInitControl
	{
		// Token: 0x06000226 RID: 550
		void ResetInitializerTimeout(int dwSecondsRemaining);
	}
}
