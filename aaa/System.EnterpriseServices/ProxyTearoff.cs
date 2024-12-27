using System;

namespace System.EnterpriseServices
{
	// Token: 0x02000026 RID: 38
	internal abstract class ProxyTearoff
	{
		// Token: 0x06000082 RID: 130 RVA: 0x00002631 File Offset: 0x00001631
		internal ProxyTearoff()
		{
		}

		// Token: 0x06000083 RID: 131
		internal abstract void Init(ServicedComponentProxy scp);

		// Token: 0x06000084 RID: 132
		internal abstract void SetCanBePooled(bool fCanBePooled);
	}
}
