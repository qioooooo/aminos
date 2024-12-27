using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000049 RID: 73
	[Guid("285a8861-c84a-11d7-850f-005cd062464f")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISectionEntry
	{
		// Token: 0x060001F8 RID: 504
		object GetField(uint fieldId);

		// Token: 0x060001F9 RID: 505
		string GetFieldName(uint fieldId);
	}
}
