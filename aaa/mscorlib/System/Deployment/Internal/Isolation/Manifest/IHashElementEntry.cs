using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000195 RID: 405
	[Guid("9D46FB70-7B54-4f4f-9331-BA9E87833FF5")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IHashElementEntry
	{
		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06001419 RID: 5145
		HashElementEntry AllData { get; }

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x0600141A RID: 5146
		uint index { get; }

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x0600141B RID: 5147
		byte Transform { get; }

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x0600141C RID: 5148
		object TransformMetadata
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x0600141D RID: 5149
		byte DigestMethod { get; }

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x0600141E RID: 5150
		object DigestValue
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x0600141F RID: 5151
		string Xml
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
