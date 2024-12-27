using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography
{
	// Token: 0x02000891 RID: 2193
	internal sealed class SafeProvHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600503D RID: 20541 RVA: 0x0011FA26 File Offset: 0x0011EA26
		private SafeProvHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000E10 RID: 3600
		// (get) Token: 0x0600503E RID: 20542 RVA: 0x0011FA36 File Offset: 0x0011EA36
		internal static SafeProvHandle InvalidHandle
		{
			get
			{
				return new SafeProvHandle(IntPtr.Zero);
			}
		}

		// Token: 0x0600503F RID: 20543
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _FreeCSP(IntPtr pProvCtx);

		// Token: 0x06005040 RID: 20544 RVA: 0x0011FA42 File Offset: 0x0011EA42
		protected override bool ReleaseHandle()
		{
			SafeProvHandle._FreeCSP(this.handle);
			return true;
		}
	}
}
