using System;
using System.Collections;

namespace System.Diagnostics
{
	// Token: 0x02000752 RID: 1874
	public class EventLogEntryCollection : ICollection, IEnumerable
	{
		// Token: 0x06003970 RID: 14704 RVA: 0x000F4193 File Offset: 0x000F3193
		internal EventLogEntryCollection(EventLog log)
		{
			this.log = log;
		}

		// Token: 0x17000D50 RID: 3408
		// (get) Token: 0x06003971 RID: 14705 RVA: 0x000F41A2 File Offset: 0x000F31A2
		public int Count
		{
			get
			{
				return this.log.EntryCount;
			}
		}

		// Token: 0x17000D51 RID: 3409
		public virtual EventLogEntry this[int index]
		{
			get
			{
				return this.log.GetEntryAt(index);
			}
		}

		// Token: 0x06003973 RID: 14707 RVA: 0x000F41BD File Offset: 0x000F31BD
		public void CopyTo(EventLogEntry[] entries, int index)
		{
			((ICollection)this).CopyTo(entries, index);
		}

		// Token: 0x06003974 RID: 14708 RVA: 0x000F41C7 File Offset: 0x000F31C7
		public IEnumerator GetEnumerator()
		{
			return new EventLogEntryCollection.EntriesEnumerator(this);
		}

		// Token: 0x06003975 RID: 14709 RVA: 0x000F41CF File Offset: 0x000F31CF
		internal EventLogEntry GetEntryAtNoThrow(int index)
		{
			return this.log.GetEntryAtNoThrow(index);
		}

		// Token: 0x17000D52 RID: 3410
		// (get) Token: 0x06003976 RID: 14710 RVA: 0x000F41DD File Offset: 0x000F31DD
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D53 RID: 3411
		// (get) Token: 0x06003977 RID: 14711 RVA: 0x000F41E0 File Offset: 0x000F31E0
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06003978 RID: 14712 RVA: 0x000F41E4 File Offset: 0x000F31E4
		void ICollection.CopyTo(Array array, int index)
		{
			EventLogEntry[] allEntries = this.log.GetAllEntries();
			Array.Copy(allEntries, 0, array, index, allEntries.Length);
		}

		// Token: 0x040032AD RID: 12973
		private EventLog log;

		// Token: 0x02000753 RID: 1875
		private class EntriesEnumerator : IEnumerator
		{
			// Token: 0x06003979 RID: 14713 RVA: 0x000F4209 File Offset: 0x000F3209
			internal EntriesEnumerator(EventLogEntryCollection entries)
			{
				this.entries = entries;
			}

			// Token: 0x17000D54 RID: 3412
			// (get) Token: 0x0600397A RID: 14714 RVA: 0x000F421F File Offset: 0x000F321F
			public object Current
			{
				get
				{
					if (this.cachedEntry == null)
					{
						throw new InvalidOperationException(SR.GetString("NoCurrentEntry"));
					}
					return this.cachedEntry;
				}
			}

			// Token: 0x0600397B RID: 14715 RVA: 0x000F423F File Offset: 0x000F323F
			public bool MoveNext()
			{
				this.num++;
				this.cachedEntry = this.entries.GetEntryAtNoThrow(this.num);
				return this.cachedEntry != null;
			}

			// Token: 0x0600397C RID: 14716 RVA: 0x000F4272 File Offset: 0x000F3272
			public void Reset()
			{
				this.num = -1;
			}

			// Token: 0x040032AE RID: 12974
			private EventLogEntryCollection entries;

			// Token: 0x040032AF RID: 12975
			private int num = -1;

			// Token: 0x040032B0 RID: 12976
			private EventLogEntry cachedEntry;
		}
	}
}
