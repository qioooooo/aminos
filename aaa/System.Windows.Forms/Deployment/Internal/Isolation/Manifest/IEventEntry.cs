using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000AC RID: 172
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("8AD3FC86-AFD3-477a-8FD5-146C291195BB")]
	[ComImport]
	internal interface IEventEntry
	{
		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060002D2 RID: 722
		EventEntry AllData { get; }

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060002D3 RID: 723
		uint EventID { get; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060002D4 RID: 724
		uint Level { get; }

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060002D5 RID: 725
		uint Version { get; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060002D6 RID: 726
		Guid Guid { get; }

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060002D7 RID: 727
		string SubTypeName
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060002D8 RID: 728
		uint SubTypeValue { get; }

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060002D9 RID: 729
		string DisplayName
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060002DA RID: 730
		uint EventNameMicrodomIndex { get; }
	}
}
