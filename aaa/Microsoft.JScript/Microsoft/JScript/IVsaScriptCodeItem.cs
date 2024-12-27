using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000138 RID: 312
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("E0C0FFE8-7eea-4ee5-b7e4-0080c7eb0b74")]
	[ComVisible(true)]
	public interface IVsaScriptCodeItem : IVsaCodeItem, IVsaItem
	{
		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06000E0B RID: 3595
		// (set) Token: 0x06000E0C RID: 3596
		int StartLine { get; set; }

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x06000E0D RID: 3597
		// (set) Token: 0x06000E0E RID: 3598
		int StartColumn { get; set; }

		// Token: 0x06000E0F RID: 3599
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		object Execute();
	}
}
