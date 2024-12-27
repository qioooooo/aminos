using System;

namespace System.Security.Cryptography
{
	// Token: 0x020002C5 RID: 709
	public class AsnEncodedData
	{
		// Token: 0x06001834 RID: 6196 RVA: 0x00053698 File Offset: 0x00052698
		internal AsnEncodedData(Oid oid)
		{
			this.m_oid = oid;
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x000536A7 File Offset: 0x000526A7
		internal AsnEncodedData(string oid, CAPIBase.CRYPTOAPI_BLOB encodedBlob)
			: this(oid, CAPI.BlobToByteArray(encodedBlob))
		{
		}

		// Token: 0x06001836 RID: 6198 RVA: 0x000536B6 File Offset: 0x000526B6
		internal AsnEncodedData(Oid oid, CAPIBase.CRYPTOAPI_BLOB encodedBlob)
			: this(oid, CAPI.BlobToByteArray(encodedBlob))
		{
		}

		// Token: 0x06001837 RID: 6199 RVA: 0x000536C5 File Offset: 0x000526C5
		protected AsnEncodedData()
		{
		}

		// Token: 0x06001838 RID: 6200 RVA: 0x000536CD File Offset: 0x000526CD
		public AsnEncodedData(byte[] rawData)
		{
			this.Reset(null, rawData);
		}

		// Token: 0x06001839 RID: 6201 RVA: 0x000536DD File Offset: 0x000526DD
		public AsnEncodedData(string oid, byte[] rawData)
		{
			this.Reset(new Oid(oid), rawData);
		}

		// Token: 0x0600183A RID: 6202 RVA: 0x000536F2 File Offset: 0x000526F2
		public AsnEncodedData(Oid oid, byte[] rawData)
		{
			this.Reset(oid, rawData);
		}

		// Token: 0x0600183B RID: 6203 RVA: 0x00053702 File Offset: 0x00052702
		public AsnEncodedData(AsnEncodedData asnEncodedData)
		{
			if (asnEncodedData == null)
			{
				throw new ArgumentNullException("asnEncodedData");
			}
			this.Reset(asnEncodedData.m_oid, asnEncodedData.m_rawData);
		}

		// Token: 0x170004B3 RID: 1203
		// (get) Token: 0x0600183C RID: 6204 RVA: 0x0005372A File Offset: 0x0005272A
		// (set) Token: 0x0600183D RID: 6205 RVA: 0x00053732 File Offset: 0x00052732
		public Oid Oid
		{
			get
			{
				return this.m_oid;
			}
			set
			{
				if (value == null)
				{
					this.m_oid = null;
					return;
				}
				this.m_oid = new Oid(value);
			}
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x0600183E RID: 6206 RVA: 0x0005374B File Offset: 0x0005274B
		// (set) Token: 0x0600183F RID: 6207 RVA: 0x00053753 File Offset: 0x00052753
		public byte[] RawData
		{
			get
			{
				return this.m_rawData;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_rawData = (byte[])value.Clone();
			}
		}

		// Token: 0x06001840 RID: 6208 RVA: 0x00053774 File Offset: 0x00052774
		public virtual void CopyFrom(AsnEncodedData asnEncodedData)
		{
			if (asnEncodedData == null)
			{
				throw new ArgumentNullException("asnEncodedData");
			}
			this.Reset(asnEncodedData.m_oid, asnEncodedData.m_rawData);
		}

		// Token: 0x06001841 RID: 6209 RVA: 0x00053798 File Offset: 0x00052798
		public virtual string Format(bool multiLine)
		{
			if (this.m_rawData == null || this.m_rawData.Length == 0)
			{
				return string.Empty;
			}
			string text = string.Empty;
			if (this.m_oid != null && this.m_oid.Value != null)
			{
				text = this.m_oid.Value;
			}
			return CAPI.CryptFormatObject(1U, multiLine ? 1U : 0U, text, this.m_rawData);
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x000537F8 File Offset: 0x000527F8
		private void Reset(Oid oid, byte[] rawData)
		{
			this.Oid = oid;
			this.RawData = rawData;
		}

		// Token: 0x0400162D RID: 5677
		internal Oid m_oid;

		// Token: 0x0400162E RID: 5678
		internal byte[] m_rawData;
	}
}
