using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000198 RID: 408
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("A2A55FAD-349B-469b-BF12-ADC33D14A937")]
	[ComImport]
	internal interface IFileEntry
	{
		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06001424 RID: 5156
		FileEntry AllData { get; }

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06001425 RID: 5157
		string Name
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06001426 RID: 5158
		uint HashAlgorithm { get; }

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06001427 RID: 5159
		string LoadFrom
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06001428 RID: 5160
		string SourcePath
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06001429 RID: 5161
		string ImportPath
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x0600142A RID: 5162
		string SourceName
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x0600142B RID: 5163
		string Location
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x0600142C RID: 5164
		object HashValue
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x0600142D RID: 5165
		ulong Size { get; }

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x0600142E RID: 5166
		string Group
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x0600142F RID: 5167
		uint Flags { get; }

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06001430 RID: 5168
		IMuiResourceMapEntry MuiMapping { get; }

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06001431 RID: 5169
		uint WritableType { get; }

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06001432 RID: 5170
		ISection HashElements { get; }
	}
}
