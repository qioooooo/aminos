using System;
using System.Runtime.InteropServices;

namespace System.Threading
{
	// Token: 0x02000133 RID: 307
	internal class SafeCompressedStackHandle : SafeHandle
	{
		// Token: 0x060011A4 RID: 4516 RVA: 0x0003203A File Offset: 0x0003103A
		public SafeCompressedStackHandle()
			: base(IntPtr.Zero, true)
		{
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x060011A5 RID: 4517 RVA: 0x00032048 File Offset: 0x00031048
		public override bool IsInvalid
		{
			get
			{
				return this.handle == IntPtr.Zero;
			}
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x0003205A File Offset: 0x0003105A
		protected override bool ReleaseHandle()
		{
			CompressedStack.DestroyDelayedCompressedStack(this.handle);
			this.handle = IntPtr.Zero;
			return true;
		}
	}
}
