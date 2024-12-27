using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x020002AE RID: 686
	[SuppressUnmanagedCodeSecurity]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	internal sealed class SafeEventLogReadHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600169D RID: 5789 RVA: 0x00048289 File Offset: 0x00047289
		internal SafeEventLogReadHandle()
			: base(true)
		{
		}

		// Token: 0x0600169E RID: 5790
		[DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern SafeEventLogReadHandle OpenEventLog(string UNCServerName, string sourceName);

		// Token: 0x0600169F RID: 5791
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool CloseEventLog(IntPtr hEventLog);

		// Token: 0x060016A0 RID: 5792 RVA: 0x00048292 File Offset: 0x00047292
		protected override bool ReleaseHandle()
		{
			return SafeEventLogReadHandle.CloseEventLog(this.handle);
		}
	}
}
