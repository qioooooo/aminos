using System;

namespace System.Diagnostics
{
	// Token: 0x020001CA RID: 458
	public class EventTypeFilter : TraceFilter
	{
		// Token: 0x06000E4B RID: 3659 RVA: 0x0002D97F File Offset: 0x0002C97F
		public EventTypeFilter(SourceLevels level)
		{
			this.level = level;
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x0002D98E File Offset: 0x0002C98E
		public override bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data)
		{
			return (eventType & (TraceEventType)this.level) != (TraceEventType)0;
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000E4D RID: 3661 RVA: 0x0002D99E File Offset: 0x0002C99E
		// (set) Token: 0x06000E4E RID: 3662 RVA: 0x0002D9A6 File Offset: 0x0002C9A6
		public SourceLevels EventType
		{
			get
			{
				return this.level;
			}
			set
			{
				this.level = value;
			}
		}

		// Token: 0x04000EF2 RID: 3826
		private SourceLevels level;
	}
}
