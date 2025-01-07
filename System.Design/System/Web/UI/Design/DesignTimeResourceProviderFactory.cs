using System;
using System.Web.Compilation;

namespace System.Web.UI.Design
{
	public abstract class DesignTimeResourceProviderFactory
	{
		public abstract IResourceProvider CreateDesignTimeGlobalResourceProvider(IServiceProvider serviceProvider, string classKey);

		public abstract IResourceProvider CreateDesignTimeLocalResourceProvider(IServiceProvider serviceProvider);

		public abstract IDesignTimeResourceWriter CreateDesignTimeLocalResourceWriter(IServiceProvider serviceProvider);
	}
}
