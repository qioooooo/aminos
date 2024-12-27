using System;
using System.Text;

namespace System.Collections.Generic
{
	// Token: 0x0200028D RID: 653
	[Serializable]
	public struct KeyValuePair<TKey, TValue>
	{
		// Token: 0x060019ED RID: 6637 RVA: 0x00044154 File Offset: 0x00043154
		public KeyValuePair(TKey key, TValue value)
		{
			this.key = key;
			this.value = value;
		}

		// Token: 0x170003FB RID: 1019
		// (get) Token: 0x060019EE RID: 6638 RVA: 0x00044164 File Offset: 0x00043164
		public TKey Key
		{
			get
			{
				return this.key;
			}
		}

		// Token: 0x170003FC RID: 1020
		// (get) Token: 0x060019EF RID: 6639 RVA: 0x0004416C File Offset: 0x0004316C
		public TValue Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x00044174 File Offset: 0x00043174
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('[');
			if (this.Key != null)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				TKey tkey = this.Key;
				stringBuilder2.Append(tkey.ToString());
			}
			stringBuilder.Append(", ");
			if (this.Value != null)
			{
				StringBuilder stringBuilder3 = stringBuilder;
				TValue tvalue = this.Value;
				stringBuilder3.Append(tvalue.ToString());
			}
			stringBuilder.Append(']');
			return stringBuilder.ToString();
		}

		// Token: 0x040009E1 RID: 2529
		private TKey key;

		// Token: 0x040009E2 RID: 2530
		private TValue value;
	}
}
