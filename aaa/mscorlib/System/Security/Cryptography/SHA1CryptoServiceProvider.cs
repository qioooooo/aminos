using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000895 RID: 2197
	[ComVisible(true)]
	public sealed class SHA1CryptoServiceProvider : SHA1
	{
		// Token: 0x0600504C RID: 20556 RVA: 0x0011FAD0 File Offset: 0x0011EAD0
		public SHA1CryptoServiceProvider()
		{
			SafeHashHandle invalidHandle = SafeHashHandle.InvalidHandle;
			Utils._CreateHash(Utils.StaticProvHandle, 32772, ref invalidHandle);
			this._safeHashHandle = invalidHandle;
		}

		// Token: 0x0600504D RID: 20557 RVA: 0x0011FB01 File Offset: 0x0011EB01
		protected override void Dispose(bool disposing)
		{
			if (this._safeHashHandle != null && !this._safeHashHandle.IsClosed)
			{
				this._safeHashHandle.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600504E RID: 20558 RVA: 0x0011FB2C File Offset: 0x0011EB2C
		public override void Initialize()
		{
			if (this._safeHashHandle != null && !this._safeHashHandle.IsClosed)
			{
				this._safeHashHandle.Dispose();
			}
			SafeHashHandle invalidHandle = SafeHashHandle.InvalidHandle;
			Utils._CreateHash(Utils.StaticProvHandle, 32772, ref invalidHandle);
			this._safeHashHandle = invalidHandle;
		}

		// Token: 0x0600504F RID: 20559 RVA: 0x0011FB77 File Offset: 0x0011EB77
		protected override void HashCore(byte[] rgb, int ibStart, int cbSize)
		{
			Utils._HashData(this._safeHashHandle, rgb, ibStart, cbSize);
		}

		// Token: 0x06005050 RID: 20560 RVA: 0x0011FB87 File Offset: 0x0011EB87
		protected override byte[] HashFinal()
		{
			return Utils._EndHash(this._safeHashHandle);
		}

		// Token: 0x04002925 RID: 10533
		private SafeHashHandle _safeHashHandle;
	}
}
