using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000073 RID: 115
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("A2A55FAD-349B-469b-BF12-ADC33D14A937")]
	[ComImport]
	internal interface IFileEntry
	{
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x0600023F RID: 575
		FileEntry AllData { get; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000240 RID: 576
		string Name
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000241 RID: 577
		uint HashAlgorithm { get; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000242 RID: 578
		string LoadFrom
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000243 RID: 579
		string SourcePath
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000244 RID: 580
		string ImportPath
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x06000245 RID: 581
		string SourceName
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000246 RID: 582
		string Location
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000247 RID: 583
		object HashValue
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000248 RID: 584
		ulong Size { get; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000249 RID: 585
		string Group
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600024A RID: 586
		uint Flags { get; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600024B RID: 587
		IMuiResourceMapEntry MuiMapping { get; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600024C RID: 588
		uint WritableType { get; }

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600024D RID: 589
		ISection HashElements { get; }
	}
}
