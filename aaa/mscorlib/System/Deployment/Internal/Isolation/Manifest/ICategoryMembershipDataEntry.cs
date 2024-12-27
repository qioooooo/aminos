using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200019E RID: 414
	[Guid("DA0C3B27-6B6B-4b80-A8F8-6CE14F4BC0A4")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ICategoryMembershipDataEntry
	{
		// Token: 0x17000277 RID: 631
		// (get) Token: 0x0600143B RID: 5179
		CategoryMembershipDataEntry AllData { get; }

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x0600143C RID: 5180
		uint index { get; }

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x0600143D RID: 5181
		string Xml
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x0600143E RID: 5182
		string Description
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
