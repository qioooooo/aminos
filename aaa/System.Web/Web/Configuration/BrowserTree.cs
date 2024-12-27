using System;
using System.Collections.Specialized;

namespace System.Web.Configuration
{
	// Token: 0x020001AC RID: 428
	internal class BrowserTree : OrderedDictionary
	{
		// Token: 0x060018DC RID: 6364 RVA: 0x000777EC File Offset: 0x000767EC
		internal BrowserTree()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}
	}
}
