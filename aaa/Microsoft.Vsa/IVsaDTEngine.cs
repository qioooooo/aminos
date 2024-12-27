using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace Microsoft.Vsa
{
	// Token: 0x02000012 RID: 18
	[Guid("E0C0FFEE-7eea-4ee5-b7e4-0080c7eb0b74")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
	public interface IVsaDTEngine
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x0600007A RID: 122
		// (set) Token: 0x0600007B RID: 123
		string TargetURL
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get;
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set;
		}

		// Token: 0x0600007C RID: 124
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void InitCompleted();

		// Token: 0x0600007D RID: 125
		IVsaIDE GetIDE();

		// Token: 0x0600007E RID: 126
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void AttachDebugger(bool isAttach);
	}
}
