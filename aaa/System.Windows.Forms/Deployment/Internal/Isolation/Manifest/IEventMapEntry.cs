using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000AF RID: 175
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("8AD3FC86-AFD3-477a-8FD5-146C291195BC")]
	[ComImport]
	internal interface IEventMapEntry
	{
		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060002DC RID: 732
		EventMapEntry AllData { get; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060002DD RID: 733
		string MapName
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060002DE RID: 734
		string Name
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060002DF RID: 735
		uint Value { get; }

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060002E0 RID: 736
		bool IsValueMap { get; }
	}
}
