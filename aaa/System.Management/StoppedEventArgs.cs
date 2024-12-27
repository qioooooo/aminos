using System;

namespace System.Management
{
	// Token: 0x02000015 RID: 21
	public class StoppedEventArgs : ManagementEventArgs
	{
		// Token: 0x060000B1 RID: 177 RVA: 0x000061E6 File Offset: 0x000051E6
		internal StoppedEventArgs(object context, int status)
			: base(context)
		{
			this.status = status;
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x000061F6 File Offset: 0x000051F6
		public ManagementStatus Status
		{
			get
			{
				return (ManagementStatus)this.status;
			}
		}

		// Token: 0x04000088 RID: 136
		private int status;
	}
}
