using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x02000399 RID: 921
	[ComVisible(true)]
	[Serializable]
	public class DaylightTime
	{
		// Token: 0x06002558 RID: 9560 RVA: 0x00068C1F File Offset: 0x00067C1F
		private DaylightTime()
		{
		}

		// Token: 0x06002559 RID: 9561 RVA: 0x00068C27 File Offset: 0x00067C27
		public DaylightTime(DateTime start, DateTime end, TimeSpan delta)
		{
			this.m_start = start;
			this.m_end = end;
			this.m_delta = delta;
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x0600255A RID: 9562 RVA: 0x00068C44 File Offset: 0x00067C44
		public DateTime Start
		{
			get
			{
				return this.m_start;
			}
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x0600255B RID: 9563 RVA: 0x00068C4C File Offset: 0x00067C4C
		public DateTime End
		{
			get
			{
				return this.m_end;
			}
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x0600255C RID: 9564 RVA: 0x00068C54 File Offset: 0x00067C54
		public TimeSpan Delta
		{
			get
			{
				return this.m_delta;
			}
		}

		// Token: 0x040010BA RID: 4282
		internal DateTime m_start;

		// Token: 0x040010BB RID: 4283
		internal DateTime m_end;

		// Token: 0x040010BC RID: 4284
		internal TimeSpan m_delta;
	}
}
