using System;
using System.IO;
using System.Resources;

namespace System.Web.Compilation
{
	// Token: 0x0200018F RID: 399
	internal class ResourcesBuildProvider : BaseResourcesBuildProvider
	{
		// Token: 0x0600110B RID: 4363 RVA: 0x0004C999 File Offset: 0x0004B999
		protected override IResourceReader GetResourceReader(Stream inputStream)
		{
			return new ResourceReader(inputStream);
		}
	}
}
