using System;

namespace System.Diagnostics
{
	// Token: 0x0200074A RID: 1866
	public class EntryWrittenEventArgs : EventArgs
	{
		// Token: 0x060038DC RID: 14556 RVA: 0x000F02AE File Offset: 0x000EF2AE
		public EntryWrittenEventArgs()
		{
		}

		// Token: 0x060038DD RID: 14557 RVA: 0x000F02B6 File Offset: 0x000EF2B6
		public EntryWrittenEventArgs(EventLogEntry entry)
		{
			this.entry = entry;
		}

		// Token: 0x17000D2B RID: 3371
		// (get) Token: 0x060038DE RID: 14558 RVA: 0x000F02C5 File Offset: 0x000EF2C5
		public EventLogEntry Entry
		{
			get
			{
				return this.entry;
			}
		}

		// Token: 0x04003268 RID: 12904
		private EventLogEntry entry;
	}
}
