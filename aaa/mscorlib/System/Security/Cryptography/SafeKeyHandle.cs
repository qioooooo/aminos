using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Cryptography
{
	// Token: 0x02000892 RID: 2194
	internal sealed class SafeKeyHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06005041 RID: 20545 RVA: 0x0011FA50 File Offset: 0x0011EA50
		private SafeKeyHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x17000E11 RID: 3601
		// (get) Token: 0x06005042 RID: 20546 RVA: 0x0011FA60 File Offset: 0x0011EA60
		internal static SafeKeyHandle InvalidHandle
		{
			get
			{
				return new SafeKeyHandle(IntPtr.Zero);
			}
		}

		// Token: 0x06005043 RID: 20547
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _FreeHKey(IntPtr pKeyCtx);

		// Token: 0x06005044 RID: 20548 RVA: 0x0011FA6C File Offset: 0x0011EA6C
		protected override bool ReleaseHandle()
		{
			SafeKeyHandle._FreeHKey(this.handle);
			return true;
		}
	}
}
