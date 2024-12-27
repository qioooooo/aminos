using System;
using System.Web.UI;

namespace System.Web.Compilation
{
	// Token: 0x02000193 RID: 403
	internal class WebHandlerBuildProvider : SimpleHandlerBuildProvider
	{
		// Token: 0x0600111D RID: 4381 RVA: 0x0004CB7D File Offset: 0x0004BB7D
		protected override SimpleWebHandlerParser CreateParser()
		{
			return new WebHandlerParser(base.VirtualPath);
		}
	}
}
