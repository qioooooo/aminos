using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x02000466 RID: 1126
	internal sealed class SafeRegistryHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002D5B RID: 11611 RVA: 0x00098B29 File Offset: 0x00097B29
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal SafeRegistryHandle()
			: base(true)
		{
		}

		// Token: 0x06002D5C RID: 11612 RVA: 0x00098B32 File Offset: 0x00097B32
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal SafeRegistryHandle(IntPtr preexistingHandle, bool ownsHandle)
			: base(ownsHandle)
		{
			base.SetHandle(preexistingHandle);
		}

		// Token: 0x06002D5D RID: 11613
		[SuppressUnmanagedCodeSecurity]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("advapi32.dll")]
		private static extern int RegCloseKey(IntPtr hKey);

		// Token: 0x06002D5E RID: 11614 RVA: 0x00098B44 File Offset: 0x00097B44
		protected override bool ReleaseHandle()
		{
			int num = SafeRegistryHandle.RegCloseKey(this.handle);
			return num == 0;
		}
	}
}
