using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x020002B0 RID: 688
	[SuppressUnmanagedCodeSecurity]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	internal sealed class SafeFileMappingHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060016A5 RID: 5797 RVA: 0x000482B5 File Offset: 0x000472B5
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal SafeFileMappingHandle()
			: base(true)
		{
		}

		// Token: 0x060016A6 RID: 5798
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		private static extern bool CloseHandle(IntPtr handle);

		// Token: 0x060016A7 RID: 5799 RVA: 0x000482BE File Offset: 0x000472BE
		protected override bool ReleaseHandle()
		{
			return SafeFileMappingHandle.CloseHandle(this.handle);
		}
	}
}
