﻿using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000ED RID: 237
	[Guid("A31B6577-71D2-4344-AEDF-ADC1B0DC5347")]
	public interface ISoapServerVRoot
	{
		// Token: 0x0600055C RID: 1372
		[DispId(1)]
		void CreateVirtualRootEx([MarshalAs(UnmanagedType.BStr)] string rootWebServer, [MarshalAs(UnmanagedType.BStr)] string inBaseUrl, [MarshalAs(UnmanagedType.BStr)] string inVirtualRoot, [MarshalAs(UnmanagedType.BStr)] string homePage, [MarshalAs(UnmanagedType.BStr)] string discoFile, [MarshalAs(UnmanagedType.BStr)] string secureSockets, [MarshalAs(UnmanagedType.BStr)] string authentication, [MarshalAs(UnmanagedType.BStr)] string operation, [MarshalAs(UnmanagedType.BStr)] out string baseUrl, [MarshalAs(UnmanagedType.BStr)] out string virtualRoot, [MarshalAs(UnmanagedType.BStr)] out string physicalPath);

		// Token: 0x0600055D RID: 1373
		[DispId(2)]
		void DeleteVirtualRootEx([MarshalAs(UnmanagedType.BStr)] string rootWebServer, [MarshalAs(UnmanagedType.BStr)] string baseUrl, [MarshalAs(UnmanagedType.BStr)] string virtualRoot);

		// Token: 0x0600055E RID: 1374
		[DispId(3)]
		void GetVirtualRootStatus([MarshalAs(UnmanagedType.BStr)] string rootWebServer, [MarshalAs(UnmanagedType.BStr)] string inBaseUrl, [MarshalAs(UnmanagedType.BStr)] string inVirtualRoot, [MarshalAs(UnmanagedType.BStr)] out string exists, [MarshalAs(UnmanagedType.BStr)] out string secureSockets, [MarshalAs(UnmanagedType.BStr)] out string windowsAuth, [MarshalAs(UnmanagedType.BStr)] out string anonymous, [MarshalAs(UnmanagedType.BStr)] out string homePage, [MarshalAs(UnmanagedType.BStr)] out string discoFile, [MarshalAs(UnmanagedType.BStr)] out string physicalPath, [MarshalAs(UnmanagedType.BStr)] out string baseUrl, [MarshalAs(UnmanagedType.BStr)] out string virtualRoot);
	}
}