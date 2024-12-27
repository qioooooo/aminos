using System;

namespace System.Diagnostics
{
	// Token: 0x020001C9 RID: 457
	public abstract class TraceFilter
	{
		// Token: 0x06000E46 RID: 3654
		public abstract bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data);

		// Token: 0x06000E47 RID: 3655 RVA: 0x0002D918 File Offset: 0x0002C918
		internal bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage)
		{
			return this.ShouldTrace(cache, source, eventType, id, formatOrMessage, null, null, null);
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x0002D938 File Offset: 0x0002C938
		internal bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args)
		{
			return this.ShouldTrace(cache, source, eventType, id, formatOrMessage, args, null, null);
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x0002D958 File Offset: 0x0002C958
		internal bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1)
		{
			return this.ShouldTrace(cache, source, eventType, id, formatOrMessage, args, data1, null);
		}

		// Token: 0x04000EF1 RID: 3825
		internal string initializeData;
	}
}
