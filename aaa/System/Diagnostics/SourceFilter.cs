using System;

namespace System.Diagnostics
{
	// Token: 0x020001CD RID: 461
	public class SourceFilter : TraceFilter
	{
		// Token: 0x06000E61 RID: 3681 RVA: 0x0002DC7D File Offset: 0x0002CC7D
		public SourceFilter(string source)
		{
			this.Source = source;
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x0002DC8C File Offset: 0x0002CC8C
		public override bool ShouldTrace(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			return string.Equals(this.src, source);
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000E63 RID: 3683 RVA: 0x0002DCA8 File Offset: 0x0002CCA8
		// (set) Token: 0x06000E64 RID: 3684 RVA: 0x0002DCB0 File Offset: 0x0002CCB0
		public string Source
		{
			get
			{
				return this.src;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("source");
				}
				this.src = value;
			}
		}

		// Token: 0x04000EFA RID: 3834
		private string src;
	}
}
