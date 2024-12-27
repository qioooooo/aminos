using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x0200001A RID: 26
	[Guid("2A005C01-A5DE-11CF-9E66-00AA00A3F464")]
	[ComImport]
	internal interface ISharedProperty
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000051 RID: 81
		// (set) Token: 0x06000052 RID: 82
		object Value { get; set; }
	}
}
