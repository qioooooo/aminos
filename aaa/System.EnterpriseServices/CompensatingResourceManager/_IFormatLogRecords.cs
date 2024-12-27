using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.CompensatingResourceManager
{
	// Token: 0x020000A9 RID: 169
	[Guid("9C51D821-C98B-11D1-82FB-00A0C91EEDE9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface _IFormatLogRecords
	{
		// Token: 0x060003FD RID: 1021
		int GetColumnCount();

		// Token: 0x060003FE RID: 1022
		object GetColumnHeaders();

		// Token: 0x060003FF RID: 1023
		object GetColumn([In] _LogRecord crmLogRec);

		// Token: 0x06000400 RID: 1024
		object GetColumnVariants([In] object logRecord);
	}
}
