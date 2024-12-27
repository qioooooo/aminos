using System;

namespace System.Management
{
	// Token: 0x0200000E RID: 14
	internal class InternalObjectPutEventArgs : EventArgs
	{
		// Token: 0x060000A0 RID: 160 RVA: 0x000060FF File Offset: 0x000050FF
		internal InternalObjectPutEventArgs(ManagementPath path)
		{
			this.path = path.Clone();
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00006113 File Offset: 0x00005113
		internal ManagementPath Path
		{
			get
			{
				return this.path;
			}
		}

		// Token: 0x0400007E RID: 126
		private ManagementPath path;
	}
}
