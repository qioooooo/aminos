using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32
{
	// Token: 0x0200042C RID: 1068
	[Guid("7c23ff90-33af-11d3-95da-00a024a85b51")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IApplicationContext
	{
		// Token: 0x06002C12 RID: 11282
		void SetContextNameObject(IAssemblyName pName);

		// Token: 0x06002C13 RID: 11283
		void GetContextNameObject(out IAssemblyName ppName);

		// Token: 0x06002C14 RID: 11284
		void Set([MarshalAs(UnmanagedType.LPWStr)] string szName, int pvValue, uint cbValue, uint dwFlags);

		// Token: 0x06002C15 RID: 11285
		void Get([MarshalAs(UnmanagedType.LPWStr)] string szName, out int pvValue, ref uint pcbValue, uint dwFlags);

		// Token: 0x06002C16 RID: 11286
		void GetDynamicDirectory(out int wzDynamicDir, ref uint pdwSize);
	}
}
