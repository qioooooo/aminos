using System;

namespace System.Web
{
	// Token: 0x0200002A RID: 42
	internal enum EtwWorkerRequestType
	{
		// Token: 0x04000D96 RID: 3478
		Undefined = -1,
		// Token: 0x04000D97 RID: 3479
		InProc,
		// Token: 0x04000D98 RID: 3480
		OutOfProc,
		// Token: 0x04000D99 RID: 3481
		IIS7Integrated = 3,
		// Token: 0x04000D9A RID: 3482
		Unknown = 999
	}
}
