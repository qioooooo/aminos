using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000C9 RID: 201
	[Guid("7c23ff90-33af-11d3-95da-00a024a85b51")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IApplicationContext
	{
		// Token: 0x0600049C RID: 1180
		void SetContextNameObject(IAssemblyName pName);

		// Token: 0x0600049D RID: 1181
		void GetContextNameObject(out IAssemblyName ppName);

		// Token: 0x0600049E RID: 1182
		void Set([MarshalAs(UnmanagedType.LPWStr)] string szName, int pvValue, uint cbValue, uint dwFlags);

		// Token: 0x0600049F RID: 1183
		void Get([MarshalAs(UnmanagedType.LPWStr)] string szName, out int pvValue, ref uint pcbValue, uint dwFlags);

		// Token: 0x060004A0 RID: 1184
		void GetDynamicDirectory(out int wzDynamicDir, ref uint pdwSize);
	}
}
