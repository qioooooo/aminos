using System;
using System.Web.UI;

namespace System.Web.Compilation
{
	// Token: 0x02000192 RID: 402
	internal class WebServiceBuildProvider : SimpleHandlerBuildProvider
	{
		// Token: 0x0600111B RID: 4379 RVA: 0x0004CB68 File Offset: 0x0004BB68
		protected override SimpleWebHandlerParser CreateParser()
		{
			return new WebServiceParser(base.VirtualPath);
		}
	}
}
