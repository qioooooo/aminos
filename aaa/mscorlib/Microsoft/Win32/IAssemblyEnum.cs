using System;
using System.Runtime.InteropServices;

namespace Microsoft.Win32
{
	// Token: 0x0200042B RID: 1067
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("21b8916c-f28e-11d2-a473-00c04f8ef448")]
	[ComImport]
	internal interface IAssemblyEnum
	{
		// Token: 0x06002C0F RID: 11279
		[PreserveSig]
		int GetNextAssembly(out IApplicationContext ppAppCtx, out IAssemblyName ppName, uint dwFlags);

		// Token: 0x06002C10 RID: 11280
		[PreserveSig]
		int Reset();

		// Token: 0x06002C11 RID: 11281
		[PreserveSig]
		int Clone(out IAssemblyEnum ppEnum);
	}
}
