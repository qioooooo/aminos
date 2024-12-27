using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000070 RID: 112
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("9D46FB70-7B54-4f4f-9331-BA9E87833FF5")]
	[ComImport]
	internal interface IHashElementEntry
	{
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000234 RID: 564
		HashElementEntry AllData { get; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000235 RID: 565
		uint index { get; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000236 RID: 566
		byte Transform { get; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000237 RID: 567
		object TransformMetadata
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000238 RID: 568
		byte DigestMethod { get; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000239 RID: 569
		object DigestValue
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600023A RID: 570
		string Xml
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
