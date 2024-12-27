using System;
using System.Collections;

namespace System.Security.Cryptography
{
	// Token: 0x020002C6 RID: 710
	public sealed class AsnEncodedDataCollection : ICollection, IEnumerable
	{
		// Token: 0x06001843 RID: 6211 RVA: 0x00053808 File Offset: 0x00052808
		public AsnEncodedDataCollection()
		{
			this.m_list = new ArrayList();
			this.m_oid = null;
		}

		// Token: 0x06001844 RID: 6212 RVA: 0x00053822 File Offset: 0x00052822
		public AsnEncodedDataCollection(AsnEncodedData asnEncodedData)
			: this()
		{
			this.m_list.Add(asnEncodedData);
		}

		// Token: 0x06001845 RID: 6213 RVA: 0x00053838 File Offset: 0x00052838
		public int Add(AsnEncodedData asnEncodedData)
		{
			if (asnEncodedData == null)
			{
				throw new ArgumentNullException("asnEncodedData");
			}
			if (this.m_oid != null)
			{
				string value = this.m_oid.Value;
				string value2 = asnEncodedData.Oid.Value;
				if (value != null && value2 != null)
				{
					if (string.Compare(value, value2, StringComparison.OrdinalIgnoreCase) != 0)
					{
						throw new CryptographicException(SR.GetString("Cryptography_Asn_MismatchedOidInCollection"));
					}
				}
				else if (value != null || value2 != null)
				{
					throw new CryptographicException(SR.GetString("Cryptography_Asn_MismatchedOidInCollection"));
				}
			}
			return this.m_list.Add(asnEncodedData);
		}

		// Token: 0x06001846 RID: 6214 RVA: 0x000538B5 File Offset: 0x000528B5
		public void Remove(AsnEncodedData asnEncodedData)
		{
			if (asnEncodedData == null)
			{
				throw new ArgumentNullException("asnEncodedData");
			}
			this.m_list.Remove(asnEncodedData);
		}

		// Token: 0x170004B5 RID: 1205
		public AsnEncodedData this[int index]
		{
			get
			{
				return (AsnEncodedData)this.m_list[index];
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06001848 RID: 6216 RVA: 0x000538E4 File Offset: 0x000528E4
		public int Count
		{
			get
			{
				return this.m_list.Count;
			}
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x000538F1 File Offset: 0x000528F1
		public AsnEncodedDataEnumerator GetEnumerator()
		{
			return new AsnEncodedDataEnumerator(this);
		}

		// Token: 0x0600184A RID: 6218 RVA: 0x000538F9 File Offset: 0x000528F9
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new AsnEncodedDataEnumerator(this);
		}

		// Token: 0x0600184B RID: 6219 RVA: 0x00053904 File Offset: 0x00052904
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(SR.GetString("Arg_RankMultiDimNotSupported"));
			}
			if (index < 0 || index >= array.Length)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("ArgumentOutOfRange_Index"));
			}
			if (index + this.Count > array.Length)
			{
				throw new ArgumentException(SR.GetString("Argument_InvalidOffLen"));
			}
			for (int i = 0; i < this.Count; i++)
			{
				array.SetValue(this[i], index);
				index++;
			}
		}

		// Token: 0x0600184C RID: 6220 RVA: 0x0005399E File Offset: 0x0005299E
		public void CopyTo(AsnEncodedData[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x0600184D RID: 6221 RVA: 0x000539A8 File Offset: 0x000529A8
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x0600184E RID: 6222 RVA: 0x000539AB File Offset: 0x000529AB
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x0400162F RID: 5679
		private ArrayList m_list;

		// Token: 0x04001630 RID: 5680
		private Oid m_oid;
	}
}
