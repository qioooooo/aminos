using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000042 RID: 66
	[Guid("a5f325af-572f-46da-b8ab-827c3d95d99e")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IManagedActivationEvents
	{
		// Token: 0x06000139 RID: 313
		void CreateManagedStub(IManagedObjectInfo pInfo, [MarshalAs(UnmanagedType.Bool)] bool fDist);

		// Token: 0x0600013A RID: 314
		void DestroyManagedStub(IManagedObjectInfo pInfo);
	}
}
