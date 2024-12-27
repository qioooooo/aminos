using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x020002B1 RID: 689
	[SuppressUnmanagedCodeSecurity]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	internal sealed class SafeFileMapViewHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060016A8 RID: 5800 RVA: 0x000482CB File Offset: 0x000472CB
		internal SafeFileMapViewHandle()
			: base(true)
		{
		}

		// Token: 0x060016A9 RID: 5801
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		internal static extern SafeFileMapViewHandle MapViewOfFile(SafeFileMappingHandle hFileMappingObject, int dwDesiredAccess, int dwFileOffsetHigh, int dwFileOffsetLow, UIntPtr dwNumberOfBytesToMap);

		// Token: 0x060016AA RID: 5802
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		private static extern bool UnmapViewOfFile(IntPtr handle);

		// Token: 0x060016AB RID: 5803 RVA: 0x000482D4 File Offset: 0x000472D4
		protected override bool ReleaseHandle()
		{
			return SafeFileMapViewHandle.UnmapViewOfFile(this.handle);
		}
	}
}
