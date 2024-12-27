using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.JScript
{
	// Token: 0x02000135 RID: 309
	[Guid("6DFE759A-CB8B-4ca0-A973-1D04E0BF0B53")]
	[ComVisible(true)]
	public interface IDebugVsaScriptCodeItem
	{
		// Token: 0x06000DFF RID: 3583
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[return: MarshalAs(UnmanagedType.Interface)]
		object Evaluate();

		// Token: 0x06000E00 RID: 3584
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		bool ParseNamedBreakPoint(string input, out string functionName, out int nargs, out string arguments, out string returnType, out ulong offset);
	}
}
