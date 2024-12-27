using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.Vsa
{
	// Token: 0x0200000F RID: 15
	[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("4E76D92E-E29D-46f3-AE22-0333158109F1")]
	public interface IVsaGlobalItem : IVsaItem
	{
		// Token: 0x1700002F RID: 47
		// (set) Token: 0x0600006E RID: 110
		string TypeString
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set;
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600006F RID: 111
		// (set) Token: 0x06000070 RID: 112
		bool ExposeMembers
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set;
		}
	}
}
