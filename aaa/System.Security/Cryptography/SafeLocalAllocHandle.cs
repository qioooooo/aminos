using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography
{
	// Token: 0x0200004F RID: 79
	internal sealed class SafeLocalAllocHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06000083 RID: 131 RVA: 0x00003622 File Offset: 0x00002622
		private SafeLocalAllocHandle()
			: base(true)
		{
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000362B File Offset: 0x0000262B
		internal SafeLocalAllocHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000085 RID: 133 RVA: 0x0000363B File Offset: 0x0000263B
		internal static SafeLocalAllocHandle InvalidHandle
		{
			get
			{
				return new SafeLocalAllocHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06000086 RID: 134
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr LocalFree(IntPtr handle);

		// Token: 0x06000087 RID: 135 RVA: 0x00003647 File Offset: 0x00002647
		protected override bool ReleaseHandle()
		{
			return SafeLocalAllocHandle.LocalFree(this.handle) == IntPtr.Zero;
		}
	}
}
