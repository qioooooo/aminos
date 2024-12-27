using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x020002B3 RID: 691
	[SuppressUnmanagedCodeSecurity]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	internal sealed class SafeLocalMemHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060016B0 RID: 5808 RVA: 0x000482F7 File Offset: 0x000472F7
		internal SafeLocalMemHandle()
			: base(true)
		{
		}

		// Token: 0x060016B1 RID: 5809 RVA: 0x00048300 File Offset: 0x00047300
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal SafeLocalMemHandle(IntPtr existingHandle, bool ownsHandle)
			: base(ownsHandle)
		{
			base.SetHandle(existingHandle);
		}

		// Token: 0x060016B2 RID: 5810
		[DllImport("advapi32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern bool ConvertStringSecurityDescriptorToSecurityDescriptor(string StringSecurityDescriptor, int StringSDRevision, out SafeLocalMemHandle pSecurityDescriptor, IntPtr SecurityDescriptorSize);

		// Token: 0x060016B3 RID: 5811
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll")]
		private static extern IntPtr LocalFree(IntPtr hMem);

		// Token: 0x060016B4 RID: 5812 RVA: 0x00048310 File Offset: 0x00047310
		protected override bool ReleaseHandle()
		{
			return SafeLocalMemHandle.LocalFree(this.handle) == IntPtr.Zero;
		}
	}
}
