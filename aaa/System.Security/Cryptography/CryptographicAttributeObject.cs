using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Pkcs;
using System.Security.Permissions;

namespace System.Security.Cryptography
{
	// Token: 0x02000055 RID: 85
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class CryptographicAttributeObject
	{
		// Token: 0x060000A1 RID: 161 RVA: 0x0000375B File Offset: 0x0000275B
		private CryptographicAttributeObject()
		{
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00003763 File Offset: 0x00002763
		internal CryptographicAttributeObject(IntPtr pAttribute)
			: this((CAPIBase.CRYPT_ATTRIBUTE)Marshal.PtrToStructure(pAttribute, typeof(CAPIBase.CRYPT_ATTRIBUTE)))
		{
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00003780 File Offset: 0x00002780
		internal CryptographicAttributeObject(CAPIBase.CRYPT_ATTRIBUTE cryptAttribute)
			: this(new Oid(cryptAttribute.pszObjId), PkcsUtils.GetAsnEncodedDataCollection(cryptAttribute))
		{
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000379A File Offset: 0x0000279A
		internal CryptographicAttributeObject(CAPIBase.CRYPT_ATTRIBUTE_TYPE_VALUE cryptAttribute)
			: this(new Oid(cryptAttribute.pszObjId), PkcsUtils.GetAsnEncodedDataCollection(cryptAttribute))
		{
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000037B4 File Offset: 0x000027B4
		internal CryptographicAttributeObject(AsnEncodedData asnEncodedData)
			: this(asnEncodedData.Oid, new AsnEncodedDataCollection(asnEncodedData))
		{
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000037C8 File Offset: 0x000027C8
		public CryptographicAttributeObject(Oid oid)
			: this(oid, new AsnEncodedDataCollection())
		{
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000037D8 File Offset: 0x000027D8
		public CryptographicAttributeObject(Oid oid, AsnEncodedDataCollection values)
		{
			this.m_oid = new Oid(oid);
			if (values == null)
			{
				this.m_values = new AsnEncodedDataCollection();
				return;
			}
			foreach (AsnEncodedData asnEncodedData in values)
			{
				if (string.Compare(asnEncodedData.Oid.Value, oid.Value, StringComparison.Ordinal) != 0)
				{
					throw new InvalidOperationException(SecurityResources.GetResourceString("InvalidOperation_DuplicateItemNotAllowed"));
				}
			}
			this.m_values = values;
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x0000384E File Offset: 0x0000284E
		public Oid Oid
		{
			get
			{
				return new Oid(this.m_oid);
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x0000385B File Offset: 0x0000285B
		public AsnEncodedDataCollection Values
		{
			get
			{
				return this.m_values;
			}
		}

		// Token: 0x0400042D RID: 1069
		private Oid m_oid;

		// Token: 0x0400042E RID: 1070
		private AsnEncodedDataCollection m_values;
	}
}
