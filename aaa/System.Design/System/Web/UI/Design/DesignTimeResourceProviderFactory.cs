using System;
using System.Web.Compilation;

namespace System.Web.UI.Design
{
	// Token: 0x02000364 RID: 868
	public abstract class DesignTimeResourceProviderFactory
	{
		// Token: 0x060020AA RID: 8362
		public abstract IResourceProvider CreateDesignTimeGlobalResourceProvider(IServiceProvider serviceProvider, string classKey);

		// Token: 0x060020AB RID: 8363
		public abstract IResourceProvider CreateDesignTimeLocalResourceProvider(IServiceProvider serviceProvider);

		// Token: 0x060020AC RID: 8364
		public abstract IDesignTimeResourceWriter CreateDesignTimeLocalResourceWriter(IServiceProvider serviceProvider);
	}
}
