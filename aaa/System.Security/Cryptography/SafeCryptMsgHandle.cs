using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography
{
	// Token: 0x02000053 RID: 83
	internal sealed class SafeCryptMsgHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06000097 RID: 151 RVA: 0x000036F6 File Offset: 0x000026F6
		private SafeCryptMsgHandle()
			: base(true)
		{
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000036FF File Offset: 0x000026FF
		internal SafeCryptMsgHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000099 RID: 153 RVA: 0x0000370F File Offset: 0x0000270F
		internal static SafeCryptMsgHandle InvalidHandle
		{
			get
			{
				return new SafeCryptMsgHandle(IntPtr.Zero);
			}
		}

		// Token: 0x0600009A RID: 154
		[SuppressUnmanagedCodeSecurity]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("crypt32.dll", SetLastError = true)]
		private static extern bool CryptMsgClose(IntPtr handle);

		// Token: 0x0600009B RID: 155 RVA: 0x0000371B File Offset: 0x0000271B
		protected override bool ReleaseHandle()
		{
			return SafeCryptMsgHandle.CryptMsgClose(this.handle);
		}
	}
}
