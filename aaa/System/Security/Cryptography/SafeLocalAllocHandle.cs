using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography
{
	// Token: 0x02000318 RID: 792
	internal sealed class SafeLocalAllocHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060018F9 RID: 6393 RVA: 0x000556BB File Offset: 0x000546BB
		private SafeLocalAllocHandle()
			: base(true)
		{
		}

		// Token: 0x060018FA RID: 6394 RVA: 0x000556C4 File Offset: 0x000546C4
		internal SafeLocalAllocHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x060018FB RID: 6395 RVA: 0x000556D4 File Offset: 0x000546D4
		internal static SafeLocalAllocHandle InvalidHandle
		{
			get
			{
				return new SafeLocalAllocHandle(IntPtr.Zero);
			}
		}

		// Token: 0x060018FC RID: 6396
		[SuppressUnmanagedCodeSecurity]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr LocalFree(IntPtr handle);

		// Token: 0x060018FD RID: 6397 RVA: 0x000556E0 File Offset: 0x000546E0
		protected override bool ReleaseHandle()
		{
			return SafeLocalAllocHandle.LocalFree(this.handle) == IntPtr.Zero;
		}
	}
}
