using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace System.Web.Configuration
{
	// Token: 0x02000202 RID: 514
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("9e3aaeb4-d1cd-11d2-bab9-00c04f8eceae")]
	[ComImport]
	internal interface IAssemblyCacheItem
	{
		// Token: 0x06001C03 RID: 7171
		void CreateStream([MarshalAs(UnmanagedType.LPWStr)] string pszName, uint dwFormat, uint dwFlags, uint dwMaxSize, out IStream ppStream);

		// Token: 0x06001C04 RID: 7172
		void IsNameEqual(IAssemblyName pName);

		// Token: 0x06001C05 RID: 7173
		void Commit(uint dwFlags);

		// Token: 0x06001C06 RID: 7174
		void MarkAssemblyVisible(uint dwFlags);
	}
}
