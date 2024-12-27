using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography
{
	// Token: 0x02000893 RID: 2195
	internal sealed class SafeHashHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06005045 RID: 20549 RVA: 0x0011FA7A File Offset: 0x0011EA7A
		private SafeHashHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000E12 RID: 3602
		// (get) Token: 0x06005046 RID: 20550 RVA: 0x0011FA8A File Offset: 0x0011EA8A
		internal static SafeHashHandle InvalidHandle
		{
			get
			{
				return new SafeHashHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06005047 RID: 20551
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _FreeHash(IntPtr pHashCtx);

		// Token: 0x06005048 RID: 20552 RVA: 0x0011FA96 File Offset: 0x0011EA96
		protected override bool ReleaseHandle()
		{
			SafeHashHandle._FreeHash(this.handle);
			return true;
		}
	}
}
