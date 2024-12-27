using System;
using System.Collections;

namespace Microsoft.JScript
{
	// Token: 0x02000116 RID: 278
	internal sealed class SimpleHashtableEnumerator : IDictionaryEnumerator, IEnumerator
	{
		// Token: 0x06000B7C RID: 2940 RVA: 0x000578B0 File Offset: 0x000568B0
		internal SimpleHashtableEnumerator(HashtableEntry[] table)
		{
			this.table = table;
			this.count = table.Length;
			this.index = -1;
			this.currentEntry = null;
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000B7D RID: 2941 RVA: 0x000578D6 File Offset: 0x000568D6
		public object Current
		{
			get
			{
				return this.Key;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000B7E RID: 2942 RVA: 0x000578DE File Offset: 0x000568DE
		public DictionaryEntry Entry
		{
			get
			{
				return new DictionaryEntry(this.Key, this.Value);
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000B7F RID: 2943 RVA: 0x000578F1 File Offset: 0x000568F1
		public object Key
		{
			get
			{
				return this.currentEntry.key;
			}
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x00057900 File Offset: 0x00056900
		public bool MoveNext()
		{
			HashtableEntry[] array = this.table;
			if (this.currentEntry != null)
			{
				this.currentEntry = this.currentEntry.next;
				if (this.currentEntry != null)
				{
					return true;
				}
			}
			int i = ++this.index;
			int num = this.count;
			while (i < num)
			{
				if (array[i] != null)
				{
					this.index = i;
					this.currentEntry = array[i];
					return true;
				}
				i++;
			}
			return false;
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x00057972 File Offset: 0x00056972
		public void Reset()
		{
			this.index = -1;
			this.currentEntry = null;
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000B82 RID: 2946 RVA: 0x00057982 File Offset: 0x00056982
		public object Value
		{
			get
			{
				return this.currentEntry.value;
			}
		}

		// Token: 0x040006EC RID: 1772
		private HashtableEntry[] table;

		// Token: 0x040006ED RID: 1773
		private int count;

		// Token: 0x040006EE RID: 1774
		private int index;

		// Token: 0x040006EF RID: 1775
		private HashtableEntry currentEntry;
	}
}
