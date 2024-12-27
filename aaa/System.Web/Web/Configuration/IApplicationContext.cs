using System;
using System.Runtime.InteropServices;

namespace System.Web.Configuration
{
	// Token: 0x02000200 RID: 512
	[Guid("7c23ff90-33af-11d3-95da-00a024a85b51")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IApplicationContext
	{
		// Token: 0x06001BF9 RID: 7161
		void SetContextNameObject(IAssemblyName pName);

		// Token: 0x06001BFA RID: 7162
		void GetContextNameObject(out IAssemblyName ppName);

		// Token: 0x06001BFB RID: 7163
		void Set([MarshalAs(UnmanagedType.LPWStr)] string szName, int pvValue, uint cbValue, uint dwFlags);

		// Token: 0x06001BFC RID: 7164
		void Get([MarshalAs(UnmanagedType.LPWStr)] string szName, out int pvValue, ref uint pcbValue, uint dwFlags);

		// Token: 0x06001BFD RID: 7165
		void GetDynamicDirectory(out int wzDynamicDir, ref uint pdwSize);
	}
}
