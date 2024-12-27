using System;
using System.Collections;

namespace Microsoft.JScript
{
	// Token: 0x02000115 RID: 277
	public sealed class SimpleHashtable
	{
		// Token: 0x06000B74 RID: 2932 RVA: 0x000575B9 File Offset: 0x000565B9
		public SimpleHashtable(uint threshold)
		{
			if (threshold < 8U)
			{
				threshold = 8U;
			}
			this.table = new HashtableEntry[threshold * 2U - 1U];
			this.count = 0;
			this.threshold = threshold;
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x000575E6 File Offset: 0x000565E6
		public IDictionaryEnumerator GetEnumerator()
		{
			return new SimpleHashtableEnumerator(this.table);
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x000575F4 File Offset: 0x000565F4
		private HashtableEntry GetHashtableEntry(object key, uint hashCode)
		{
			int num = (int)(hashCode % (uint)this.table.Length);
			HashtableEntry hashtableEntry = this.table[num];
			if (hashtableEntry == null)
			{
				return null;
			}
			if (hashtableEntry.key == key)
			{
				return hashtableEntry;
			}
			for (HashtableEntry hashtableEntry2 = hashtableEntry.next; hashtableEntry2 != null; hashtableEntry2 = hashtableEntry2.next)
			{
				if (hashtableEntry2.key == key)
				{
					return hashtableEntry2;
				}
			}
			if (hashtableEntry.hashCode == hashCode && hashtableEntry.key.Equals(key))
			{
				hashtableEntry.key = key;
				return hashtableEntry;
			}
			for (HashtableEntry hashtableEntry2 = hashtableEntry.next; hashtableEntry2 != null; hashtableEntry2 = hashtableEntry2.next)
			{
				if (hashtableEntry2.hashCode == hashCode && hashtableEntry2.key.Equals(key))
				{
					hashtableEntry2.key = key;
					return hashtableEntry2;
				}
			}
			return null;
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x00057698 File Offset: 0x00056698
		internal object IgnoreCaseGet(string name)
		{
			uint num = 0U;
			uint num2 = (uint)this.table.Length;
			while (num < num2)
			{
				for (HashtableEntry hashtableEntry = this.table[(int)((UIntPtr)num)]; hashtableEntry != null; hashtableEntry = hashtableEntry.next)
				{
					if (string.Compare((string)hashtableEntry.key, name, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return hashtableEntry.value;
					}
				}
				num += 1U;
			}
			return null;
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x000576EC File Offset: 0x000566EC
		private void Rehash()
		{
			HashtableEntry[] array = this.table;
			uint num = (this.threshold = (uint)(array.Length + 1));
			uint num2 = num * 2U - 1U;
			HashtableEntry[] array2 = (this.table = new HashtableEntry[num2]);
			uint num3 = num - 1U;
			while (num3-- > 0U)
			{
				HashtableEntry hashtableEntry = array[(int)num3];
				while (hashtableEntry != null)
				{
					HashtableEntry hashtableEntry2 = hashtableEntry;
					hashtableEntry = hashtableEntry.next;
					int num4 = (int)(hashtableEntry2.hashCode % num2);
					hashtableEntry2.next = array2[num4];
					array2[num4] = hashtableEntry2;
				}
			}
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x00057770 File Offset: 0x00056770
		public void Remove(object key)
		{
			uint hashCode = (uint)key.GetHashCode();
			int num = (int)(hashCode % (uint)this.table.Length);
			HashtableEntry hashtableEntry = this.table[num];
			this.count--;
			while (hashtableEntry != null && hashtableEntry.hashCode == hashCode && (hashtableEntry.key == key || hashtableEntry.key.Equals(key)))
			{
				hashtableEntry = hashtableEntry.next;
			}
			this.table[num] = hashtableEntry;
			while (hashtableEntry != null)
			{
				HashtableEntry hashtableEntry2 = hashtableEntry.next;
				while (hashtableEntry2 != null && hashtableEntry2.hashCode == hashCode && (hashtableEntry2.key == key || hashtableEntry2.key.Equals(key)))
				{
					hashtableEntry2 = hashtableEntry2.next;
				}
				hashtableEntry.next = hashtableEntry2;
				hashtableEntry = hashtableEntry2;
			}
		}

		// Token: 0x170001F6 RID: 502
		public object this[object key]
		{
			get
			{
				HashtableEntry hashtableEntry = this.GetHashtableEntry(key, (uint)key.GetHashCode());
				if (hashtableEntry == null)
				{
					return null;
				}
				return hashtableEntry.value;
			}
			set
			{
				uint hashCode = (uint)key.GetHashCode();
				HashtableEntry hashtableEntry = this.GetHashtableEntry(key, hashCode);
				if (hashtableEntry != null)
				{
					hashtableEntry.value = value;
					return;
				}
				if ((long)(++this.count) >= (long)((ulong)this.threshold))
				{
					this.Rehash();
				}
				int num = (int)(hashCode % (uint)this.table.Length);
				this.table[num] = new HashtableEntry(key, value, hashCode, this.table[num]);
			}
		}

		// Token: 0x040006E9 RID: 1769
		private HashtableEntry[] table;

		// Token: 0x040006EA RID: 1770
		internal int count;

		// Token: 0x040006EB RID: 1771
		private uint threshold;
	}
}
