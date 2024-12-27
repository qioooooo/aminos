using System;
using System.Web.Util;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000503 RID: 1283
	internal class ContentBuilderInternalFactory : IWebObjectFactory
	{
		// Token: 0x06003EA7 RID: 16039 RVA: 0x001050C9 File Offset: 0x001040C9
		object IWebObjectFactory.CreateInstance()
		{
			return new ContentBuilderInternal();
		}
	}
}
