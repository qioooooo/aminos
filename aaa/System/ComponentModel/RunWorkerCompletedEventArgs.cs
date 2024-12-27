using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000138 RID: 312
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class RunWorkerCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06000A3D RID: 2621 RVA: 0x00023D6C File Offset: 0x00022D6C
		public RunWorkerCompletedEventArgs(object result, Exception error, bool cancelled)
			: base(error, cancelled, null)
		{
			this.result = result;
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000A3E RID: 2622 RVA: 0x00023D7E File Offset: 0x00022D7E
		public object Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.result;
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000A3F RID: 2623 RVA: 0x00023D8C File Offset: 0x00022D8C
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		public new object UserState
		{
			get
			{
				return base.UserState;
			}
		}

		// Token: 0x04000A68 RID: 2664
		private object result;
	}
}
