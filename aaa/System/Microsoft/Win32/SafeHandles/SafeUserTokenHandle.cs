using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x020002B7 RID: 695
	[SuppressUnmanagedCodeSecurity]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	internal sealed class SafeUserTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060016C1 RID: 5825 RVA: 0x0004839C File Offset: 0x0004739C
		internal SafeUserTokenHandle()
			: base(true)
		{
		}

		// Token: 0x060016C2 RID: 5826 RVA: 0x000483A5 File Offset: 0x000473A5
		internal SafeUserTokenHandle(IntPtr existingHandle, bool ownsHandle)
			: base(ownsHandle)
		{
			base.SetHandle(existingHandle);
		}

		// Token: 0x060016C3 RID: 5827
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool DuplicateTokenEx(SafeHandle hToken, int access, NativeMethods.SECURITY_ATTRIBUTES tokenAttributes, int impersonationLevel, int tokenType, out SafeUserTokenHandle hNewToken);

		// Token: 0x060016C4 RID: 5828
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		private static extern bool CloseHandle(IntPtr handle);

		// Token: 0x060016C5 RID: 5829 RVA: 0x000483B5 File Offset: 0x000473B5
		protected override bool ReleaseHandle()
		{
			return SafeUserTokenHandle.CloseHandle(this.handle);
		}
	}
}
