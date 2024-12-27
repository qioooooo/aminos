using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x0200051D RID: 1309
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct SSPIHandle
	{
		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06002849 RID: 10313 RVA: 0x000A5DC1 File Offset: 0x000A4DC1
		public bool IsZero
		{
			get
			{
				return this.HandleHi == IntPtr.Zero && this.HandleLo == IntPtr.Zero;
			}
		}

		// Token: 0x0600284A RID: 10314 RVA: 0x000A5DE7 File Offset: 0x000A4DE7
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void SetToInvalid()
		{
			this.HandleHi = IntPtr.Zero;
			this.HandleLo = IntPtr.Zero;
		}

		// Token: 0x0600284B RID: 10315 RVA: 0x000A5DFF File Offset: 0x000A4DFF
		public override string ToString()
		{
			return this.HandleHi.ToString("x") + ":" + this.HandleLo.ToString("x");
		}

		// Token: 0x04002776 RID: 10102
		private IntPtr HandleHi;

		// Token: 0x04002777 RID: 10103
		private IntPtr HandleLo;
	}
}
