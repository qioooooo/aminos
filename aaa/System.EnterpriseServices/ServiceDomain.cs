using System;
using System.EnterpriseServices.Thunk;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x020000E9 RID: 233
	[ComVisible(false)]
	public sealed class ServiceDomain
	{
		// Token: 0x06000552 RID: 1362 RVA: 0x00012301 File Offset: 0x00011301
		private ServiceDomain()
		{
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00012309 File Offset: 0x00011309
		public static void Enter(ServiceConfig cfg)
		{
			Platform.Assert(Platform.Supports(PlatformFeature.SWC), "ServiceDomain");
			ServiceDomainThunk.EnterServiceDomain(cfg.SCT);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00012328 File Offset: 0x00011328
		public static TransactionStatus Leave()
		{
			Platform.Assert(Platform.Supports(PlatformFeature.SWC), "ServiceDomain");
			int num = ServiceDomainThunk.LeaveServiceDomain();
			int num2 = num;
			if (num2 <= -2147168231)
			{
				if (num2 == -2147168242)
				{
					return TransactionStatus.NoTransaction;
				}
				if (num2 == -2147168231)
				{
					return TransactionStatus.Aborted;
				}
			}
			else
			{
				if (num2 == -2147168215)
				{
					return TransactionStatus.Aborting;
				}
				if (num2 == 0)
				{
					return TransactionStatus.Commited;
				}
				if (num2 == 315402)
				{
					return TransactionStatus.LocallyOk;
				}
			}
			Marshal.ThrowExceptionForHR(num);
			return TransactionStatus.Commited;
		}

		// Token: 0x0400023E RID: 574
		private const int S_OK = 0;

		// Token: 0x0400023F RID: 575
		private const int XACT_S_LOCALLY_OK = 315402;

		// Token: 0x04000240 RID: 576
		private const int XACT_E_NOTRANSACTION = -2147168242;

		// Token: 0x04000241 RID: 577
		private const int XACT_E_ABORTING = -2147168215;

		// Token: 0x04000242 RID: 578
		private const int XACT_E_ABORTED = -2147168231;
	}
}
