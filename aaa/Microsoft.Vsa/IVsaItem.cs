using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.Vsa
{
	// Token: 0x0200000B RID: 11
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("E0C0FFE5-7eea-4ee5-b7e4-0080c7eb0b74")]
	[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
	public interface IVsaItem
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600005A RID: 90
		// (set) Token: 0x0600005B RID: 91
		string Name
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set;
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600005C RID: 92
		VsaItemType ItemType
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600005D RID: 93
		bool IsDirty
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
		}

		// Token: 0x0600005E RID: 94
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		object GetOption(string name);

		// Token: 0x0600005F RID: 95
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void SetOption(string name, object value);
	}
}
