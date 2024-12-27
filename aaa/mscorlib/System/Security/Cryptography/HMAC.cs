using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x0200086E RID: 2158
	[ComVisible(true)]
	public abstract class HMAC : KeyedHashAlgorithm
	{
		// Token: 0x17000DD6 RID: 3542
		// (get) Token: 0x06004F27 RID: 20263 RVA: 0x001143F7 File Offset: 0x001133F7
		// (set) Token: 0x06004F28 RID: 20264 RVA: 0x001143FF File Offset: 0x001133FF
		protected int BlockSizeValue
		{
			get
			{
				return this.blockSizeValue;
			}
			set
			{
				this.blockSizeValue = value;
			}
		}

		// Token: 0x06004F29 RID: 20265 RVA: 0x00114408 File Offset: 0x00113408
		private void UpdateIOPadBuffers()
		{
			if (this.m_inner == null)
			{
				this.m_inner = new byte[this.BlockSizeValue];
			}
			if (this.m_outer == null)
			{
				this.m_outer = new byte[this.BlockSizeValue];
			}
			for (int i = 0; i < this.BlockSizeValue; i++)
			{
				this.m_inner[i] = 54;
				this.m_outer[i] = 92;
			}
			for (int i = 0; i < this.KeyValue.Length; i++)
			{
				byte[] inner = this.m_inner;
				int num = i;
				inner[num] ^= this.KeyValue[i];
				byte[] outer = this.m_outer;
				int num2 = i;
				outer[num2] ^= this.KeyValue[i];
			}
		}

		// Token: 0x06004F2A RID: 20266 RVA: 0x001144C4 File Offset: 0x001134C4
		internal void InitializeKey(byte[] key)
		{
			this.m_inner = null;
			this.m_outer = null;
			if (key.Length > this.BlockSizeValue)
			{
				this.KeyValue = this.m_hash1.ComputeHash(key);
			}
			else
			{
				this.KeyValue = (byte[])key.Clone();
			}
			this.UpdateIOPadBuffers();
		}

		// Token: 0x17000DD7 RID: 3543
		// (get) Token: 0x06004F2B RID: 20267 RVA: 0x00114515 File Offset: 0x00113515
		// (set) Token: 0x06004F2C RID: 20268 RVA: 0x00114527 File Offset: 0x00113527
		public override byte[] Key
		{
			get
			{
				return (byte[])this.KeyValue.Clone();
			}
			set
			{
				if (this.m_hashing)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_HashKeySet"));
				}
				this.InitializeKey(value);
			}
		}

		// Token: 0x17000DD8 RID: 3544
		// (get) Token: 0x06004F2D RID: 20269 RVA: 0x00114548 File Offset: 0x00113548
		// (set) Token: 0x06004F2E RID: 20270 RVA: 0x00114550 File Offset: 0x00113550
		public string HashName
		{
			get
			{
				return this.m_hashName;
			}
			set
			{
				if (this.m_hashing)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_HashNameSet"));
				}
				this.m_hashName = value;
				this.m_hash1 = HashAlgorithm.Create(this.m_hashName);
				this.m_hash2 = HashAlgorithm.Create(this.m_hashName);
			}
		}

		// Token: 0x06004F2F RID: 20271 RVA: 0x0011459E File Offset: 0x0011359E
		public new static HMAC Create()
		{
			return HMAC.Create("System.Security.Cryptography.HMAC");
		}

		// Token: 0x06004F30 RID: 20272 RVA: 0x001145AA File Offset: 0x001135AA
		public new static HMAC Create(string algorithmName)
		{
			return (HMAC)CryptoConfig.CreateFromName(algorithmName);
		}

		// Token: 0x06004F31 RID: 20273 RVA: 0x001145B7 File Offset: 0x001135B7
		public override void Initialize()
		{
			this.m_hash1.Initialize();
			this.m_hash2.Initialize();
			this.m_hashing = false;
		}

		// Token: 0x06004F32 RID: 20274 RVA: 0x001145D8 File Offset: 0x001135D8
		protected override void HashCore(byte[] rgb, int ib, int cb)
		{
			if (!this.m_hashing)
			{
				this.m_hash1.TransformBlock(this.m_inner, 0, this.m_inner.Length, this.m_inner, 0);
				this.m_hashing = true;
			}
			this.m_hash1.TransformBlock(rgb, ib, cb, rgb, ib);
		}

		// Token: 0x06004F33 RID: 20275 RVA: 0x00114628 File Offset: 0x00113628
		protected override byte[] HashFinal()
		{
			if (!this.m_hashing)
			{
				this.m_hash1.TransformBlock(this.m_inner, 0, this.m_inner.Length, this.m_inner, 0);
				this.m_hashing = true;
			}
			this.m_hash1.TransformFinalBlock(new byte[0], 0, 0);
			byte[] hashValue = this.m_hash1.HashValue;
			this.m_hash2.TransformBlock(this.m_outer, 0, this.m_outer.Length, this.m_outer, 0);
			this.m_hash2.TransformBlock(hashValue, 0, hashValue.Length, hashValue, 0);
			this.m_hashing = false;
			this.m_hash2.TransformFinalBlock(new byte[0], 0, 0);
			return this.m_hash2.HashValue;
		}

		// Token: 0x06004F34 RID: 20276 RVA: 0x001146E4 File Offset: 0x001136E4
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.m_hash1 != null)
				{
					this.m_hash1.Clear();
				}
				if (this.m_hash2 != null)
				{
					this.m_hash2.Clear();
				}
				if (this.m_inner != null)
				{
					Array.Clear(this.m_inner, 0, this.m_inner.Length);
				}
				if (this.m_outer != null)
				{
					Array.Clear(this.m_outer, 0, this.m_outer.Length);
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x040028AF RID: 10415
		private int blockSizeValue = 64;

		// Token: 0x040028B0 RID: 10416
		internal string m_hashName;

		// Token: 0x040028B1 RID: 10417
		internal HashAlgorithm m_hash1;

		// Token: 0x040028B2 RID: 10418
		internal HashAlgorithm m_hash2;

		// Token: 0x040028B3 RID: 10419
		private byte[] m_inner;

		// Token: 0x040028B4 RID: 10420
		private byte[] m_outer;

		// Token: 0x040028B5 RID: 10421
		private bool m_hashing;
	}
}
