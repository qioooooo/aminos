using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x0200000C RID: 12
	[Guid("E74A7215-014D-11D1-A63C-00A0C911B4E0")]
	[ComImport]
	internal interface SecurityProperty
	{
		// Token: 0x06000027 RID: 39
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetDirectCallerName();

		// Token: 0x06000028 RID: 40
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetDirectCreatorName();

		// Token: 0x06000029 RID: 41
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetOriginalCallerName();

		// Token: 0x0600002A RID: 42
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetOriginalCreatorName();
	}
}
