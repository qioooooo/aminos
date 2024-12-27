using System;
using System.ComponentModel;

namespace System.Diagnostics
{
	// Token: 0x0200074C RID: 1868
	public class EventInstance
	{
		// Token: 0x060038E3 RID: 14563 RVA: 0x000F02CD File Offset: 0x000EF2CD
		public EventInstance(long instanceId, int categoryId)
		{
			this.CategoryId = categoryId;
			this.InstanceId = instanceId;
		}

		// Token: 0x060038E4 RID: 14564 RVA: 0x000F02EA File Offset: 0x000EF2EA
		public EventInstance(long instanceId, int categoryId, EventLogEntryType entryType)
			: this(instanceId, categoryId)
		{
			this.EntryType = entryType;
		}

		// Token: 0x17000D2C RID: 3372
		// (get) Token: 0x060038E5 RID: 14565 RVA: 0x000F02FB File Offset: 0x000EF2FB
		// (set) Token: 0x060038E6 RID: 14566 RVA: 0x000F0303 File Offset: 0x000EF303
		public int CategoryId
		{
			get
			{
				return this._categoryNumber;
			}
			set
			{
				if (value > 65535 || value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._categoryNumber = value;
			}
		}

		// Token: 0x17000D2D RID: 3373
		// (get) Token: 0x060038E7 RID: 14567 RVA: 0x000F0323 File Offset: 0x000EF323
		// (set) Token: 0x060038E8 RID: 14568 RVA: 0x000F032B File Offset: 0x000EF32B
		public EventLogEntryType EntryType
		{
			get
			{
				return this._entryType;
			}
			set
			{
				if (!Enum.IsDefined(typeof(EventLogEntryType), value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(EventLogEntryType));
				}
				this._entryType = value;
			}
		}

		// Token: 0x17000D2E RID: 3374
		// (get) Token: 0x060038E9 RID: 14569 RVA: 0x000F0361 File Offset: 0x000EF361
		// (set) Token: 0x060038EA RID: 14570 RVA: 0x000F0369 File Offset: 0x000EF369
		public long InstanceId
		{
			get
			{
				return this._instanceId;
			}
			set
			{
				if (value > (long)((ulong)(-1)) || value < 0L)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._instanceId = value;
			}
		}

		// Token: 0x04003269 RID: 12905
		private int _categoryNumber;

		// Token: 0x0400326A RID: 12906
		private EventLogEntryType _entryType = EventLogEntryType.Information;

		// Token: 0x0400326B RID: 12907
		private long _instanceId;
	}
}
