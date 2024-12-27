using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001DA RID: 474
	[Guid("285a8861-c84a-11d7-850f-005cd062464f")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISectionEntry
	{
		// Token: 0x06000867 RID: 2151
		object GetField(uint fieldId);

		// Token: 0x06000868 RID: 2152
		string GetFieldName(uint fieldId);
	}
}
