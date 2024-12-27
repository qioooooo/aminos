using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000175 RID: 373
	[Guid("285a8861-c84a-11d7-850f-005cd062464f")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISectionEntry
	{
		// Token: 0x060013DD RID: 5085
		object GetField(uint fieldId);

		// Token: 0x060013DE RID: 5086
		string GetFieldName(uint fieldId);
	}
}
