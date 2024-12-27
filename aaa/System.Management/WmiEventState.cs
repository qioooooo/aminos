using System;
using System.Threading;

namespace System.Management
{
	// Token: 0x02000028 RID: 40
	internal class WmiEventState
	{
		// Token: 0x06000147 RID: 327 RVA: 0x00008298 File Offset: 0x00007298
		internal WmiEventState(Delegate d, ManagementEventArgs args, AutoResetEvent h)
		{
			this.d = d;
			this.args = args;
			this.h = h;
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000148 RID: 328 RVA: 0x000082B5 File Offset: 0x000072B5
		public Delegate Delegate
		{
			get
			{
				return this.d;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000149 RID: 329 RVA: 0x000082BD File Offset: 0x000072BD
		public ManagementEventArgs Args
		{
			get
			{
				return this.args;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600014A RID: 330 RVA: 0x000082C5 File Offset: 0x000072C5
		public AutoResetEvent AutoResetEvent
		{
			get
			{
				return this.h;
			}
		}

		// Token: 0x04000120 RID: 288
		private Delegate d;

		// Token: 0x04000121 RID: 289
		private ManagementEventArgs args;

		// Token: 0x04000122 RID: 290
		private AutoResetEvent h;
	}
}
