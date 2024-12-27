using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography
{
	// Token: 0x0200031C RID: 796
	internal sealed class SafeCryptMsgHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600190D RID: 6413 RVA: 0x0005578F File Offset: 0x0005478F
		private SafeCryptMsgHandle()
			: base(true)
		{
		}

		// Token: 0x0600190E RID: 6414 RVA: 0x00055798 File Offset: 0x00054798
		internal SafeCryptMsgHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x0600190F RID: 6415 RVA: 0x000557A8 File Offset: 0x000547A8
		internal static SafeCryptMsgHandle InvalidHandle
		{
			get
			{
				return new SafeCryptMsgHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06001910 RID: 6416
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("crypt32.dll", SetLastError = true)]
		private static extern bool CryptMsgClose(IntPtr handle);

		// Token: 0x06001911 RID: 6417 RVA: 0x000557B4 File Offset: 0x000547B4
		protected override bool ReleaseHandle()
		{
			return SafeCryptMsgHandle.CryptMsgClose(this.handle);
		}
	}
}
