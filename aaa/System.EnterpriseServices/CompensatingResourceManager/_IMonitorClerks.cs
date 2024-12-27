using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.CompensatingResourceManager
{
	// Token: 0x020000B0 RID: 176
	[Guid("70C8E442-C7ED-11D1-82FB-00A0C91EEDE9")]
	internal interface _IMonitorClerks
	{
		// Token: 0x06000435 RID: 1077
		object Item(object index);

		// Token: 0x06000436 RID: 1078
		[return: MarshalAs(UnmanagedType.Interface)]
		object _NewEnum();

		// Token: 0x06000437 RID: 1079
		int Count();

		// Token: 0x06000438 RID: 1080
		object ProgIdCompensator(object index);

		// Token: 0x06000439 RID: 1081
		object Description(object index);

		// Token: 0x0600043A RID: 1082
		object TransactionUOW(object index);

		// Token: 0x0600043B RID: 1083
		object ActivityId(object index);
	}
}
