using System;
using System.Runtime.InteropServices;

namespace System.Web.Configuration
{
	// Token: 0x020001E2 RID: 482
	internal interface IGac
	{
		// Token: 0x06001ACA RID: 6858
		[DispId(13)]
		void GacInstall([MarshalAs(UnmanagedType.BStr)] string assemblyPath);
	}
}
