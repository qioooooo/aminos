using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography
{
	// Token: 0x02000877 RID: 2167
	[ComVisible(true)]
	public class MACTripleDES : KeyedHashAlgorithm
	{
		// Token: 0x06004F59 RID: 20313 RVA: 0x00114EB4 File Offset: 0x00113EB4
		public MACTripleDES()
		{
			this.KeyValue = new byte[24];
			Utils.StaticRandomNumberGenerator.GetBytes(this.KeyValue);
			this.des = TripleDES.Create();
			this.HashSizeValue = this.des.BlockSize;
			this.m_bytesPerBlock = this.des.BlockSize / 8;
			this.des.IV = new byte[this.m_bytesPerBlock];
			this.des.Padding = PaddingMode.Zeros;
			this.m_encryptor = null;
		}

		// Token: 0x06004F5A RID: 20314 RVA: 0x00114F3C File Offset: 0x00113F3C
		public MACTripleDES(byte[] rgbKey)
			: this("System.Security.Cryptography.TripleDES", rgbKey)
		{
		}

		// Token: 0x06004F5B RID: 20315 RVA: 0x00114F4C File Offset: 0x00113F4C
		public MACTripleDES(string strTripleDES, byte[] rgbKey)
		{
			if (rgbKey == null)
			{
				throw new ArgumentNullException("rgbKey");
			}
			if (strTripleDES == null)
			{
				this.des = TripleDES.Create();
			}
			else
			{
				this.des = TripleDES.Create(strTripleDES);
			}
			this.HashSizeValue = this.des.BlockSize;
			this.KeyValue = (byte[])rgbKey.Clone();
			this.m_bytesPerBlock = this.des.BlockSize / 8;
			this.des.IV = new byte[this.m_bytesPerBlock];
			this.des.Padding = PaddingMode.Zeros;
			this.m_encryptor = null;
		}

		// Token: 0x06004F5C RID: 20316 RVA: 0x00114FE7 File Offset: 0x00113FE7
		public override void Initialize()
		{
			this.m_encryptor = null;
		}

		// Token: 0x17000DEA RID: 3562
		// (get) Token: 0x06004F5D RID: 20317 RVA: 0x00114FF0 File Offset: 0x00113FF0
		// (set) Token: 0x06004F5E RID: 20318 RVA: 0x00114FFD File Offset: 0x00113FFD
		[ComVisible(false)]
		public PaddingMode Padding
		{
			get
			{
				return this.des.Padding;
			}
			set
			{
				if (value < PaddingMode.None || PaddingMode.ISO10126 < value)
				{
					throw new CryptographicException(Environment.GetResourceString("Cryptography_InvalidPaddingMode"));
				}
				this.des.Padding = value;
			}
		}

		// Token: 0x06004F5F RID: 20319 RVA: 0x00115024 File Offset: 0x00114024
		protected override void HashCore(byte[] rgbData, int ibStart, int cbSize)
		{
			if (this.m_encryptor == null)
			{
				this.des.Key = this.Key;
				this.m_encryptor = this.des.CreateEncryptor();
				this._ts = new TailStream(this.des.BlockSize / 8);
				this._cs = new CryptoStream(this._ts, this.m_encryptor, CryptoStreamMode.Write);
			}
			this._cs.Write(rgbData, ibStart, cbSize);
		}

		// Token: 0x06004F60 RID: 20320 RVA: 0x0011509C File Offset: 0x0011409C
		protected override byte[] HashFinal()
		{
			if (this.m_encryptor == null)
			{
				this.des.Key = this.Key;
				this.m_encryptor = this.des.CreateEncryptor();
				this._ts = new TailStream(this.des.BlockSize / 8);
				this._cs = new CryptoStream(this._ts, this.m_encryptor, CryptoStreamMode.Write);
			}
			this._cs.FlushFinalBlock();
			return this._ts.Buffer;
		}

		// Token: 0x06004F61 RID: 20321 RVA: 0x0011511C File Offset: 0x0011411C
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.des != null)
				{
					this.des.Clear();
				}
				if (this.m_encryptor != null)
				{
					this.m_encryptor.Dispose();
				}
				if (this._cs != null)
				{
					this._cs.Clear();
				}
				if (this._ts != null)
				{
					this._ts.Clear();
				}
			}
			base.Dispose(disposing);
		}

		// Token: 0x040028BD RID: 10429
		private const int m_bitsPerByte = 8;

		// Token: 0x040028BE RID: 10430
		private ICryptoTransform m_encryptor;

		// Token: 0x040028BF RID: 10431
		private CryptoStream _cs;

		// Token: 0x040028C0 RID: 10432
		private TailStream _ts;

		// Token: 0x040028C1 RID: 10433
		private int m_bytesPerBlock;

		// Token: 0x040028C2 RID: 10434
		private TripleDES des;
	}
}
