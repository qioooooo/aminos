using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.Vsa
{
	// Token: 0x02000013 RID: 19
	[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
	[Guid("E0C0FFED-7eea-4ee5-b7e4-0080c7eb0b74")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IVsaDTCodeItem
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600007F RID: 127
		// (set) Token: 0x06000080 RID: 128
		bool CanDelete
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set;
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000081 RID: 129
		// (set) Token: 0x06000082 RID: 130
		bool CanMove
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set;
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000083 RID: 131
		// (set) Token: 0x06000084 RID: 132
		bool CanRename
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set;
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000085 RID: 133
		// (set) Token: 0x06000086 RID: 134
		bool Hidden
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set;
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000087 RID: 135
		// (set) Token: 0x06000088 RID: 136
		bool ReadOnly
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set;
		}
	}
}
