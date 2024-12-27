using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Transactions.Oletx
{
	// Token: 0x02000076 RID: 118
	internal sealed class CoTaskMemHandle : SafeHandle
	{
		// Token: 0x06000349 RID: 841 RVA: 0x00033D70 File Offset: 0x00033170
		public CoTaskMemHandle()
			: base(IntPtr.Zero, true)
		{
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600034A RID: 842 RVA: 0x00033D8C File Offset: 0x0003318C
		public override bool IsInvalid
		{
			get
			{
				return base.IsClosed || this.handle == IntPtr.Zero;
			}
		}

		// Token: 0x0600034B RID: 843
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("ole32.dll")]
		private static extern void CoTaskMemFree(IntPtr ptr);

		// Token: 0x0600034C RID: 844 RVA: 0x00033DB4 File Offset: 0x000331B4
		protected override bool ReleaseHandle()
		{
			CoTaskMemHandle.CoTaskMemFree(this.handle);
			return true;
		}
	}
}
