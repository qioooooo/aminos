using System;
using System.EnterpriseServices.Thunk;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x020000EC RID: 236
	[ComVisible(false)]
	public sealed class Activity
	{
		// Token: 0x06000557 RID: 1367 RVA: 0x0001238E File Offset: 0x0001138E
		public Activity(ServiceConfig cfg)
		{
			Platform.Assert(Platform.Supports(PlatformFeature.SWC), "Activity");
			this.m_sat = new ServiceActivityThunk(cfg.SCT);
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x000123B7 File Offset: 0x000113B7
		public void SynchronousCall(IServiceCall serviceCall)
		{
			this.m_sat.SynchronousCall(serviceCall);
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x000123C5 File Offset: 0x000113C5
		public void AsynchronousCall(IServiceCall serviceCall)
		{
			this.m_sat.AsynchronousCall(serviceCall);
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x000123D3 File Offset: 0x000113D3
		public void BindToCurrentThread()
		{
			this.m_sat.BindToCurrentThread();
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x000123E0 File Offset: 0x000113E0
		public void UnbindFromThread()
		{
			this.m_sat.UnbindFromThread();
		}

		// Token: 0x04000243 RID: 579
		private ServiceActivityThunk m_sat;
	}
}
