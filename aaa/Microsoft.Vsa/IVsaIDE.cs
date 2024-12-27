using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.Vsa
{
	// Token: 0x02000011 RID: 17
	[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("78470A10-8153-407d-AB1B-05067C54C36B")]
	public interface IVsaIDE
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000072 RID: 114
		// (set) Token: 0x06000073 RID: 115
		IVsaIDESite Site
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set;
		}

		// Token: 0x06000074 RID: 116
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void ShowIDE(bool showOrHide);

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000075 RID: 117
		// (set) Token: 0x06000076 RID: 118
		string DefaultSearchPath
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set;
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000077 RID: 119
		object ExtensibilityObject
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x06000078 RID: 120
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void EnableMainWindow(bool isEnable);

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000079 RID: 121
		VsaIDEMode IDEMode
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
		}
	}
}
