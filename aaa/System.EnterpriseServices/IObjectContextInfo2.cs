using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x0200000E RID: 14
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("594BE71A-4BC4-438b-9197-CFD176248B09")]
	[ComImport]
	internal interface IObjectContextInfo2
	{
		// Token: 0x06000030 RID: 48
		[return: MarshalAs(UnmanagedType.Bool)]
		bool IsInTransaction();

		// Token: 0x06000031 RID: 49
		[return: MarshalAs(UnmanagedType.Interface)]
		object GetTransaction();

		// Token: 0x06000032 RID: 50
		Guid GetTransactionId();

		// Token: 0x06000033 RID: 51
		Guid GetActivityId();

		// Token: 0x06000034 RID: 52
		Guid GetContextId();

		// Token: 0x06000035 RID: 53
		Guid GetPartitionId();

		// Token: 0x06000036 RID: 54
		Guid GetApplicationId();

		// Token: 0x06000037 RID: 55
		Guid GetApplicationInstanceId();
	}
}
