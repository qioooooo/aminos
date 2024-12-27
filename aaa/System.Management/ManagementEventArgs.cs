using System;

namespace System.Management
{
	// Token: 0x0200000F RID: 15
	public abstract class ManagementEventArgs : EventArgs
	{
		// Token: 0x060000A2 RID: 162 RVA: 0x0000611B File Offset: 0x0000511B
		internal ManagementEventArgs(object context)
		{
			this.context = context;
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x0000612A File Offset: 0x0000512A
		public object Context
		{
			get
			{
				return this.context;
			}
		}

		// Token: 0x0400007F RID: 127
		private object context;
	}
}
