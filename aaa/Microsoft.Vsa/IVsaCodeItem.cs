using System;
using System.CodeDom;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.Vsa
{
	// Token: 0x0200000E RID: 14
	[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("E0C0FFE7-7eea-4ee5-b7e4-0080c7eb0b74")]
	public interface IVsaCodeItem : IVsaItem
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000068 RID: 104
		// (set) Token: 0x06000069 RID: 105
		string SourceText
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set;
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600006A RID: 106
		CodeObject CodeDOM
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
		}

		// Token: 0x0600006B RID: 107
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void AppendSourceText(string text);

		// Token: 0x0600006C RID: 108
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void AddEventSource(string eventSourceName, string eventSourceType);

		// Token: 0x0600006D RID: 109
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void RemoveEventSource(string eventSourceName);
	}
}
