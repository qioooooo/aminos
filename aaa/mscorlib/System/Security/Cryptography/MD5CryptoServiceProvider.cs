using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200087A RID: 2170
	[ComVisible(true)]
	public sealed class MD5CryptoServiceProvider : MD5
	{
		// Token: 0x06004F74 RID: 20340 RVA: 0x001153C4 File Offset: 0x001143C4
		public MD5CryptoServiceProvider()
		{
			if (Utils.FipsAlgorithmPolicy == 1)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Cryptography_NonCompliantFIPSAlgorithm"));
			}
			SafeHashHandle invalidHandle = SafeHashHandle.InvalidHandle;
			Utils._CreateHash(Utils.StaticProvHandle, 32771, ref invalidHandle);
			this._safeHashHandle = invalidHandle;
		}

		// Token: 0x06004F75 RID: 20341 RVA: 0x0011540D File Offset: 0x0011440D
		protected override void Dispose(bool disposing)
		{
			if (this._safeHashHandle != null && !this._safeHashHandle.IsClosed)
			{
				this._safeHashHandle.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06004F76 RID: 20342 RVA: 0x00115438 File Offset: 0x00114438
		public override void Initialize()
		{
			if (this._safeHashHandle != null && !this._safeHashHandle.IsClosed)
			{
				this._safeHashHandle.Dispose();
			}
			SafeHashHandle invalidHandle = SafeHashHandle.InvalidHandle;
			Utils._CreateHash(Utils.StaticProvHandle, 32771, ref invalidHandle);
			this._safeHashHandle = invalidHandle;
		}

		// Token: 0x06004F77 RID: 20343 RVA: 0x00115483 File Offset: 0x00114483
		protected override void HashCore(byte[] rgb, int ibStart, int cbSize)
		{
			Utils._HashData(this._safeHashHandle, rgb, ibStart, cbSize);
		}

		// Token: 0x06004F78 RID: 20344 RVA: 0x00115493 File Offset: 0x00114493
		protected override byte[] HashFinal()
		{
			return Utils._EndHash(this._safeHashHandle);
		}

		// Token: 0x040028C7 RID: 10439
		private SafeHashHandle _safeHashHandle;
	}
}
