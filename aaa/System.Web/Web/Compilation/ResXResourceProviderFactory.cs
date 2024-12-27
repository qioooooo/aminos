using System;

namespace System.Web.Compilation
{
	// Token: 0x0200018E RID: 398
	internal class ResXResourceProviderFactory : ResourceProviderFactory
	{
		// Token: 0x06001108 RID: 4360 RVA: 0x0004C97C File Offset: 0x0004B97C
		public override IResourceProvider CreateGlobalResourceProvider(string classKey)
		{
			return new GlobalResXResourceProvider(classKey);
		}

		// Token: 0x06001109 RID: 4361 RVA: 0x0004C984 File Offset: 0x0004B984
		public override IResourceProvider CreateLocalResourceProvider(string virtualPath)
		{
			return new LocalResXResourceProvider(VirtualPath.Create(virtualPath));
		}
	}
}
