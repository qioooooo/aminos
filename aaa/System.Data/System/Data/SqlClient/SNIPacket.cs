using System;
using System.Runtime.InteropServices;

namespace System.Data.SqlClient
{
	// Token: 0x02000331 RID: 817
	internal sealed class SNIPacket : SafeHandle
	{
		// Token: 0x06002A99 RID: 10905 RVA: 0x0029D3F8 File Offset: 0x0029C7F8
		internal SNIPacket(SafeHandle sniHandle)
			: base(IntPtr.Zero, true)
		{
			SNINativeMethodWrapper.SNIPacketAllocate(sniHandle, SNINativeMethodWrapper.IOType.WRITE, ref this.handle);
			if (IntPtr.Zero == this.handle)
			{
				throw SQL.SNIPacketAllocationFailure();
			}
		}

		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06002A9A RID: 10906 RVA: 0x0029D438 File Offset: 0x0029C838
		public override bool IsInvalid
		{
			get
			{
				return IntPtr.Zero == this.handle;
			}
		}

		// Token: 0x06002A9B RID: 10907 RVA: 0x0029D458 File Offset: 0x0029C858
		protected override bool ReleaseHandle()
		{
			IntPtr handle = this.handle;
			this.handle = IntPtr.Zero;
			if (IntPtr.Zero != handle)
			{
				SNINativeMethodWrapper.SNIPacketRelease(handle);
			}
			return true;
		}
	}
}
