using System;

namespace System.Configuration.Internal
{
	// Token: 0x020000B6 RID: 182
	public interface IConfigSystem
	{
		// Token: 0x060006EA RID: 1770
		void Init(Type typeConfigHost, params object[] hostInitParams);

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x060006EB RID: 1771
		IInternalConfigHost Host { get; }

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x060006EC RID: 1772
		IInternalConfigRoot Root { get; }
	}
}
