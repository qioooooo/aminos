using System;

namespace System.Data.Design
{
	// Token: 0x020000BA RID: 186
	internal class SourceNameService : SimpleNameService
	{
		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000846 RID: 2118 RVA: 0x0001526C File Offset: 0x0001426C
		internal new static SourceNameService DefaultInstance
		{
			get
			{
				if (SourceNameService.defaultInstance == null)
				{
					SourceNameService.defaultInstance = new SourceNameService();
				}
				return SourceNameService.defaultInstance;
			}
		}

		// Token: 0x04000C13 RID: 3091
		private static SourceNameService defaultInstance;
	}
}
