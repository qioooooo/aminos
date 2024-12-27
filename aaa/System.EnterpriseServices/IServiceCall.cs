using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x020000EA RID: 234
	[Guid("BD3E2E12-42DD-40f4-A09A-95A50C58304B")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IServiceCall
	{
		// Token: 0x06000555 RID: 1365
		void OnCall();
	}
}
