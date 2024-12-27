using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography
{
	// Token: 0x02000050 RID: 80
	internal sealed class SafeCryptProvHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06000088 RID: 136 RVA: 0x0000365E File Offset: 0x0000265E
		private SafeCryptProvHandle()
			: base(true)
		{
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00003667 File Offset: 0x00002667
		internal SafeCryptProvHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00003677 File Offset: 0x00002677
		internal static SafeCryptProvHandle InvalidHandle
		{
			get
			{
				return new SafeCryptProvHandle(IntPtr.Zero);
			}
		}

		// Token: 0x0600008B RID: 139
		[SuppressUnmanagedCodeSecurity]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool CryptReleaseContext(IntPtr hCryptProv, uint dwFlags);

		// Token: 0x0600008C RID: 140 RVA: 0x00003683 File Offset: 0x00002683
		protected override bool ReleaseHandle()
		{
			return SafeCryptProvHandle.CryptReleaseContext(this.handle, 0U);
		}
	}
}
