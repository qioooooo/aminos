using System;

namespace System.Web
{
	// Token: 0x02000087 RID: 135
	internal class ResponseDependencyInfo
	{
		// Token: 0x06000672 RID: 1650 RVA: 0x0001BC0C File Offset: 0x0001AC0C
		internal ResponseDependencyInfo(string[] items, DateTime utcDate)
		{
			this.items = items;
			this.utcDate = utcDate;
		}

		// Token: 0x040010EC RID: 4332
		internal readonly string[] items;

		// Token: 0x040010ED RID: 4333
		internal readonly DateTime utcDate;
	}
}
