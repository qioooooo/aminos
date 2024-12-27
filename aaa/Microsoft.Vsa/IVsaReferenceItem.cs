using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.Vsa
{
	// Token: 0x0200000D RID: 13
	[Guid("E0C0FFE6-7eea-4ee5-b7e4-0080c7eb0b74")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
	public interface IVsaReferenceItem : IVsaItem
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000066 RID: 102
		// (set) Token: 0x06000067 RID: 103
		string AssemblyName
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set;
		}
	}
}
