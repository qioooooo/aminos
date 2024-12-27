using System;

namespace System.Management
{
	// Token: 0x02000012 RID: 18
	public class ObjectPutEventArgs : ManagementEventArgs
	{
		// Token: 0x060000A9 RID: 169 RVA: 0x00006171 File Offset: 0x00005171
		internal ObjectPutEventArgs(object context, ManagementPath path)
			: base(context)
		{
			this.wmiPath = path;
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00006181 File Offset: 0x00005181
		public ManagementPath Path
		{
			get
			{
				return this.wmiPath;
			}
		}

		// Token: 0x04000083 RID: 131
		private ManagementPath wmiPath;
	}
}
