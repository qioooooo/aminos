using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

namespace System.Security.Cryptography
{
	// Token: 0x02000320 RID: 800
	public sealed class OidCollection : ICollection, IEnumerable
	{
		// Token: 0x06001920 RID: 6432 RVA: 0x00055929 File Offset: 0x00054929
		public OidCollection()
		{
			this.m_list = new ArrayList();
		}

		// Token: 0x06001921 RID: 6433 RVA: 0x0005593C File Offset: 0x0005493C
		public int Add(Oid oid)
		{
			return this.m_list.Add(oid);
		}

		// Token: 0x170004C4 RID: 1220
		public Oid this[int index]
		{
			get
			{
				return this.m_list[index] as Oid;
			}
		}

		// Token: 0x170004C5 RID: 1221
		public Oid this[string oid]
		{
			get
			{
				string text = X509Utils.FindOidInfo(2U, oid, OidGroup.AllGroups);
				if (text == null)
				{
					text = oid;
				}
				foreach (object obj in this.m_list)
				{
					Oid oid2 = (Oid)obj;
					if (oid2.Value == text)
					{
						return oid2;
					}
				}
				return null;
			}
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06001924 RID: 6436 RVA: 0x000559D8 File Offset: 0x000549D8
		public int Count
		{
			get
			{
				return this.m_list.Count;
			}
		}

		// Token: 0x06001925 RID: 6437 RVA: 0x000559E5 File Offset: 0x000549E5
		public OidEnumerator GetEnumerator()
		{
			return new OidEnumerator(this);
		}

		// Token: 0x06001926 RID: 6438 RVA: 0x000559ED File Offset: 0x000549ED
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new OidEnumerator(this);
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x000559F8 File Offset: 0x000549F8
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

		// Token: 0x06001928 RID: 6440 RVA: 0x00055A92 File Offset: 0x00054A92
		public void CopyTo(Oid[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06001929 RID: 6441 RVA: 0x00055A9C File Offset: 0x00054A9C
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x0600192A RID: 6442 RVA: 0x00055A9F File Offset: 0x00054A9F
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x04001A7C RID: 6780
		private ArrayList m_list;
	}
}
