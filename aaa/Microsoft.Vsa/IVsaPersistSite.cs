using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.Vsa
{
	// Token: 0x02000009 RID: 9
	[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
	[Guid("E0C0FFE3-7eea-4ee2-b7e4-0080c7eb0b74")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IVsaPersistSite
	{
		// Token: 0x0600004F RID: 79
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void SaveElement(string name, string source);

		// Token: 0x06000050 RID: 80
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		string LoadElement(string name);
	}
}
