using System;
using System.Runtime.InteropServices;

namespace System.Threading
{
	// Token: 0x02000143 RID: 323
	internal class IUnknownSafeHandle : SafeHandle
	{
		// Token: 0x06001228 RID: 4648 RVA: 0x00032F9C File Offset: 0x00031F9C
		public IUnknownSafeHandle()
			: base(IntPtr.Zero, true)
		{
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06001229 RID: 4649 RVA: 0x00032FAA File Offset: 0x00031FAA
		public override bool IsInvalid
		{
			get
			{
				return this.handle == IntPtr.Zero;
			}
		}

		// Token: 0x0600122A RID: 4650 RVA: 0x00032FBC File Offset: 0x00031FBC
		protected override bool ReleaseHandle()
		{
			HostExecutionContextManager.ReleaseHostSecurityContext(this.handle);
			return true;
		}

		// Token: 0x0600122B RID: 4651 RVA: 0x00032FCC File Offset: 0x00031FCC
		internal object Clone()
		{
			IUnknownSafeHandle unknownSafeHandle = new IUnknownSafeHandle();
			if (!this.IsInvalid)
			{
				HostExecutionContextManager.CloneHostSecurityContext(this, unknownSafeHandle);
			}
			return unknownSafeHandle;
		}
	}
}
