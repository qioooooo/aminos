using System;

namespace System.Management
{
	// Token: 0x02000014 RID: 20
	public class EventArrivedEventArgs : ManagementEventArgs
	{
		// Token: 0x060000AF RID: 175 RVA: 0x000061CE File Offset: 0x000051CE
		internal EventArrivedEventArgs(object context, ManagementBaseObject eventObject)
			: base(context)
		{
			this.eventObject = eventObject;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x000061DE File Offset: 0x000051DE
		public ManagementBaseObject NewEvent
		{
			get
			{
				return this.eventObject;
			}
		}

		// Token: 0x04000087 RID: 135
		private ManagementBaseObject eventObject;
	}
}
