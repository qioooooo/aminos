using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.Vsa
{
	// Token: 0x02000010 RID: 16
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
	[Guid("7BD84086-1FB5-4b5d-8E05-EAA2F17218E0")]
	public interface IVsaIDESite
	{
		// Token: 0x06000071 RID: 113
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void Notify(string notify, object optional);
	}
}
