using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000079 RID: 121
	[Guid("DA0C3B27-6B6B-4b80-A8F8-6CE14F4BC0A4")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ICategoryMembershipDataEntry
	{
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000256 RID: 598
		CategoryMembershipDataEntry AllData { get; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000257 RID: 599
		uint index { get; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000258 RID: 600
		string Xml
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000259 RID: 601
		string Description
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
