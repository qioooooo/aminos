using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001A7 RID: 423
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("3903B11B-FBE8-477c-825F-DB828B5FD174")]
	[ComImport]
	internal interface ICOMServerEntry
	{
		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06001448 RID: 5192
		COMServerEntry AllData { get; }

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06001449 RID: 5193
		Guid Clsid { get; }

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x0600144A RID: 5194
		uint Flags { get; }

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x0600144B RID: 5195
		Guid ConfiguredGuid { get; }

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x0600144C RID: 5196
		Guid ImplementedClsid { get; }

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x0600144D RID: 5197
		Guid TypeLibrary { get; }

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x0600144E RID: 5198
		uint ThreadingModel { get; }

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x0600144F RID: 5199
		string RuntimeVersion
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06001450 RID: 5200
		string HostFile
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
