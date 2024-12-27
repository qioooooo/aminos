using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Cryptography
{
	// Token: 0x02000056 RID: 86
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public sealed class CryptographicAttributeObjectCollection : ICollection, IEnumerable
	{
		// Token: 0x060000AA RID: 170 RVA: 0x00003863 File Offset: 0x00002863
		public CryptographicAttributeObjectCollection()
		{
			this.m_list = new ArrayList();
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00003876 File Offset: 0x00002876
		private CryptographicAttributeObjectCollection(IntPtr pCryptAttributes)
			: this((CAPIBase.CRYPT_ATTRIBUTES)Marshal.PtrToStructure(pCryptAttributes, typeof(CAPIBase.CRYPT_ATTRIBUTES)))
		{
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00003893 File Offset: 0x00002893
		internal CryptographicAttributeObjectCollection(SafeLocalAllocHandle pCryptAttributes)
			: this(pCryptAttributes.DangerousGetHandle())
		{
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000038A4 File Offset: 0x000028A4
		internal CryptographicAttributeObjectCollection(CAPIBase.CRYPT_ATTRIBUTES cryptAttributes)
		{
			this.m_list = new ArrayList();
			for (uint num = 0U; num < cryptAttributes.cAttr; num += 1U)
			{
				IntPtr intPtr = new IntPtr((long)cryptAttributes.rgAttr + (long)((ulong)num * (ulong)((long)Marshal.SizeOf(typeof(CAPIBase.CRYPT_ATTRIBUTE)))));
				this.m_list.Add(new CryptographicAttributeObject(intPtr));
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000390D File Offset: 0x0000290D
		public CryptographicAttributeObjectCollection(CryptographicAttributeObject attribute)
		{
			this.m_list = new ArrayList();
			this.m_list.Add(attribute);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000392D File Offset: 0x0000292D
		public int Add(AsnEncodedData asnEncodedData)
		{
			if (asnEncodedData == null)
			{
				throw new ArgumentNullException("asnEncodedData");
			}
			return this.Add(new CryptographicAttributeObject(asnEncodedData));
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x0000394C File Offset: 0x0000294C
		public int Add(CryptographicAttributeObject attribute)
		{
			if (attribute == null)
			{
				throw new ArgumentNullException("attribute");
			}
			string text = null;
			if (attribute.Oid != null)
			{
				text = attribute.Oid.Value;
			}
			int i = 0;
			while (i < this.m_list.Count)
			{
				CryptographicAttributeObject cryptographicAttributeObject = (CryptographicAttributeObject)this.m_list[i];
				if (cryptographicAttributeObject.Values == attribute.Values)
				{
					throw new InvalidOperationException(SecurityResources.GetResourceString("InvalidOperation_DuplicateItemNotAllowed"));
				}
				string text2 = null;
				if (cryptographicAttributeObject.Oid != null)
				{
					text2 = cryptographicAttributeObject.Oid.Value;
				}
				if (text == null && text2 == null)
				{
					foreach (AsnEncodedData asnEncodedData in attribute.Values)
					{
						cryptographicAttributeObject.Values.Add(asnEncodedData);
					}
					return i;
				}
				if (text != null && text2 != null && string.Compare(text, text2, StringComparison.OrdinalIgnoreCase) == 0)
				{
					if (string.Compare(text, "1.2.840.113549.1.9.5", StringComparison.OrdinalIgnoreCase) == 0)
					{
						throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Pkcs9_MultipleSigningTimeNotAllowed"));
					}
					foreach (AsnEncodedData asnEncodedData2 in attribute.Values)
					{
						cryptographicAttributeObject.Values.Add(asnEncodedData2);
					}
					return i;
				}
				else
				{
					i++;
				}
			}
			return this.m_list.Add(attribute);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00003A81 File Offset: 0x00002A81
		public void Remove(CryptographicAttributeObject attribute)
		{
			if (attribute == null)
			{
				throw new ArgumentNullException("attribute");
			}
			this.m_list.Remove(attribute);
		}

		// Token: 0x1700000A RID: 10
		public CryptographicAttributeObject this[int index]
		{
			get
			{
				return (CryptographicAttributeObject)this.m_list[index];
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00003AB0 File Offset: 0x00002AB0
		public int Count
		{
			get
			{
				return this.m_list.Count;
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003ABD File Offset: 0x00002ABD
		public CryptographicAttributeObjectEnumerator GetEnumerator()
		{
			return new CryptographicAttributeObjectEnumerator(this);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00003AC5 File Offset: 0x00002AC5
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new CryptographicAttributeObjectEnumerator(this);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00003AD0 File Offset: 0x00002AD0
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(SecurityResources.GetResourceString("Arg_RankMultiDimNotSupported"));
			}
			if (index < 0 || index >= array.Length)
			{
				throw new ArgumentOutOfRangeException("index", SecurityResources.GetResourceString("ArgumentOutOfRange_Index"));
			}
			if (index + this.Count > array.Length)
			{
				throw new ArgumentException(SecurityResources.GetResourceString("Argument_InvalidOffLen"));
			}
			for (int i = 0; i < this.Count; i++)
			{
				array.SetValue(this[i], index);
				index++;
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00003B6A File Offset: 0x00002B6A
		public void CopyTo(CryptographicAttributeObject[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00003B74 File Offset: 0x00002B74
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00003B77 File Offset: 0x00002B77
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x0400042F RID: 1071
		private ArrayList m_list;
	}
}
