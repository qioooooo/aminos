using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.JScript
{
	// Token: 0x02000134 RID: 308
	[ComVisible(true)]
	[Guid("59447635-3E26-4873-BF26-05F173B80F5E")]
	public interface IDebugScriptScope
	{
		// Token: 0x06000DFE RID: 3582
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void SetThisValue([MarshalAs(UnmanagedType.Interface)] object thisValue);
	}
}
