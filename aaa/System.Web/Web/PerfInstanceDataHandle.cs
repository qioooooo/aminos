using System;
using System.Runtime.InteropServices;

namespace System.Web
{
	// Token: 0x020000BA RID: 186
	internal sealed class PerfInstanceDataHandle : SafeHandle
	{
		// Token: 0x060008B2 RID: 2226 RVA: 0x00027B30 File Offset: 0x00026B30
		internal PerfInstanceDataHandle()
			: base(IntPtr.Zero, true)
		{
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x060008B3 RID: 2227 RVA: 0x00027B3E File Offset: 0x00026B3E
		internal IntPtr UnsafeHandle
		{
			get
			{
				return this.handle;
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x060008B4 RID: 2228 RVA: 0x00027B46 File Offset: 0x00026B46
		public override bool IsInvalid
		{
			get
			{
				return this.handle == IntPtr.Zero;
			}
		}

		// Token: 0x060008B5 RID: 2229 RVA: 0x00027B58 File Offset: 0x00026B58
		protected override bool ReleaseHandle()
		{
			UnsafeNativeMethods.PerfCloseAppCounters(this.handle);
			this.handle = IntPtr.Zero;
			return true;
		}
	}
}
