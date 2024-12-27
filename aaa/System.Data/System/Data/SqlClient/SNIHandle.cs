using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Data.SqlClient
{
	// Token: 0x02000330 RID: 816
	internal sealed class SNIHandle : SafeHandle
	{
		// Token: 0x06002A94 RID: 10900 RVA: 0x0029D2AC File Offset: 0x0029C6AC
		internal SNIHandle(SNINativeMethodWrapper.ConsumerInfo myInfo, string serverName, byte[] spnBuffer, bool ignoreSniOpenTimeout, int timeout, out byte[] instanceName, bool flushCache, bool fSync, bool fParallel)
			: base(IntPtr.Zero, true)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				this._fSync = fSync;
				instanceName = new byte[256];
				if (ignoreSniOpenTimeout)
				{
					timeout = -1;
				}
				this._status = SNINativeMethodWrapper.SNIOpenSyncEx(myInfo, serverName, ref this.handle, spnBuffer, instanceName, flushCache, fSync, timeout, fParallel);
			}
		}

		// Token: 0x06002A95 RID: 10901 RVA: 0x0029D32C File Offset: 0x0029C72C
		internal SNIHandle(SNINativeMethodWrapper.ConsumerInfo myInfo, string serverName, SNIHandle parent)
			: base(IntPtr.Zero, true)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				this._status = SNINativeMethodWrapper.SNIOpen(myInfo, serverName, parent, ref this.handle, parent._fSync);
			}
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06002A96 RID: 10902 RVA: 0x0029D38C File Offset: 0x0029C78C
		public override bool IsInvalid
		{
			get
			{
				return IntPtr.Zero == this.handle;
			}
		}

		// Token: 0x06002A97 RID: 10903 RVA: 0x0029D3AC File Offset: 0x0029C7AC
		protected override bool ReleaseHandle()
		{
			IntPtr handle = this.handle;
			this.handle = IntPtr.Zero;
			return !(IntPtr.Zero != handle) || SNINativeMethodWrapper.SNIClose(handle) == 0U;
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06002A98 RID: 10904 RVA: 0x0029D3E4 File Offset: 0x0029C7E4
		internal uint Status
		{
			get
			{
				return this._status;
			}
		}

		// Token: 0x04001BF6 RID: 7158
		private readonly uint _status = uint.MaxValue;

		// Token: 0x04001BF7 RID: 7159
		private readonly bool _fSync;
	}
}
