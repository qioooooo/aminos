using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000082 RID: 130
	[Guid("3903B11B-FBE8-477c-825F-DB828B5FD174")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ICOMServerEntry
	{
		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000263 RID: 611
		COMServerEntry AllData { get; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000264 RID: 612
		Guid Clsid { get; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000265 RID: 613
		uint Flags { get; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000266 RID: 614
		Guid ConfiguredGuid { get; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000267 RID: 615
		Guid ImplementedClsid { get; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000268 RID: 616
		Guid TypeLibrary { get; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000269 RID: 617
		uint ThreadingModel { get; }

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600026A RID: 618
		string RuntimeVersion
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600026B RID: 619
		string HostFile
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
