using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000185 RID: 389
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("A2A55FAD-349B-469b-BF12-ADC33D14A937")]
	[ComImport]
	internal interface IFileEntry
	{
		// Token: 0x17000197 RID: 407
		// (get) Token: 0x0600078A RID: 1930
		FileEntry AllData { get; }

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x0600078B RID: 1931
		string Name
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x0600078C RID: 1932
		uint HashAlgorithm { get; }

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x0600078D RID: 1933
		string LoadFrom
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x0600078E RID: 1934
		string SourcePath
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x0600078F RID: 1935
		string ImportPath
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000790 RID: 1936
		string SourceName
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000791 RID: 1937
		string Location
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000792 RID: 1938
		object HashValue
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000793 RID: 1939
		ulong Size { get; }

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000794 RID: 1940
		string Group
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000795 RID: 1941
		uint Flags { get; }

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000796 RID: 1942
		IMuiResourceMapEntry MuiMapping { get; }

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000797 RID: 1943
		uint WritableType { get; }

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000798 RID: 1944
		ISection HashElements { get; }
	}
}
