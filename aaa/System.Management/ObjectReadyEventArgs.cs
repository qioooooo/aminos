using System;

namespace System.Management
{
	// Token: 0x02000010 RID: 16
	public class ObjectReadyEventArgs : ManagementEventArgs
	{
		// Token: 0x060000A4 RID: 164 RVA: 0x00006132 File Offset: 0x00005132
		internal ObjectReadyEventArgs(object context, ManagementBaseObject wmiObject)
			: base(context)
		{
			this.wmiObject = wmiObject;
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00006142 File Offset: 0x00005142
		public ManagementBaseObject NewObject
		{
			get
			{
				return this.wmiObject;
			}
		}

		// Token: 0x04000080 RID: 128
		private ManagementBaseObject wmiObject;
	}
}
