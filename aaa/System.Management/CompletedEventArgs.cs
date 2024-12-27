using System;

namespace System.Management
{
	// Token: 0x02000011 RID: 17
	public class CompletedEventArgs : ManagementEventArgs
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x0000614A File Offset: 0x0000514A
		internal CompletedEventArgs(object context, int status, ManagementBaseObject wmiStatusObject)
			: base(context)
		{
			this.wmiObject = wmiStatusObject;
			this.status = status;
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00006161 File Offset: 0x00005161
		public ManagementBaseObject StatusObject
		{
			get
			{
				return this.wmiObject;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00006169 File Offset: 0x00005169
		public ManagementStatus Status
		{
			get
			{
				return (ManagementStatus)this.status;
			}
		}

		// Token: 0x04000081 RID: 129
		private readonly int status;

		// Token: 0x04000082 RID: 130
		private readonly ManagementBaseObject wmiObject;
	}
}
