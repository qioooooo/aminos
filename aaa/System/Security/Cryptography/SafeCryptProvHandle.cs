using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography
{
	// Token: 0x02000319 RID: 793
	internal sealed class SafeCryptProvHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060018FE RID: 6398 RVA: 0x000556F7 File Offset: 0x000546F7
		private SafeCryptProvHandle()
			: base(true)
		{
		}

		// Token: 0x060018FF RID: 6399 RVA: 0x00055700 File Offset: 0x00054700
		internal SafeCryptProvHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06001900 RID: 6400 RVA: 0x00055710 File Offset: 0x00054710
		internal static SafeCryptProvHandle InvalidHandle
		{
			get
			{
				return new SafeCryptProvHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06001901 RID: 6401
		[SuppressUnmanagedCodeSecurity]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("advapi32.dll", SetLastError = true)]
		private static extern bool CryptReleaseContext(IntPtr hCryptProv, uint dwFlags);

		// Token: 0x06001902 RID: 6402 RVA: 0x0005571C File Offset: 0x0005471C
		protected override bool ReleaseHandle()
		{
			return SafeCryptProvHandle.CryptReleaseContext(this.handle, 0U);
		}
	}
}
