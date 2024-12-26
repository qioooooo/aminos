using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000132 RID: 306
	[ComVisible(true)]
	[Guid("BFF6C97F-0705-4394-88B8-A03A4B8B4CD7")]
	public interface IEngine2
	{
		// Token: 0x06000DF0 RID: 3568
		Assembly GetAssembly();

		// Token: 0x06000DF1 RID: 3569
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void Run(AppDomain domain);

		// Token: 0x06000DF2 RID: 3570
		bool CompileEmpty();

		// Token: 0x06000DF3 RID: 3571
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void RunEmpty();

		// Token: 0x06000DF4 RID: 3572
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void DisconnectEvents();

		// Token: 0x06000DF5 RID: 3573
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void ConnectEvents();

		// Token: 0x06000DF6 RID: 3574
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void RegisterEventSource(string name);

		// Token: 0x06000DF7 RID: 3575
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void Interrupt();

		// Token: 0x06000DF8 RID: 3576
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void InitVsaEngine(string rootMoniker, IVsaSite site);

		// Token: 0x06000DF9 RID: 3577
		IVsaScriptScope GetGlobalScope();

		// Token: 0x06000DFA RID: 3578
		Module GetModule();

		// Token: 0x06000DFB RID: 3579
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		IVsaEngine Clone(AppDomain domain);

		// Token: 0x06000DFC RID: 3580
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void Restart();
	}
}
