using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000CB RID: 203
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("9e3aaeb4-d1cd-11d2-bab9-00c04f8eceae")]
	[ComImport]
	internal interface IAssemblyCacheItem
	{
		// Token: 0x060004AA RID: 1194
		void CreateStream([MarshalAs(UnmanagedType.LPWStr)] string pszName, uint dwFormat, uint dwFlags, uint dwMaxSize, out IStream ppStream);

		// Token: 0x060004AB RID: 1195
		void IsNameEqual(IAssemblyName pName);

		// Token: 0x060004AC RID: 1196
		void Commit(uint dwFlags);

		// Token: 0x060004AD RID: 1197
		void MarkAssemblyVisible(uint dwFlags);
	}
}
