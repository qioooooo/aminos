using System;
using System.Web.UI;

namespace System.Web.Compilation
{
	// Token: 0x02000127 RID: 295
	internal class ApplicationFileCodeDomTreeGenerator : BaseCodeDomTreeGenerator
	{
		// Token: 0x06000D71 RID: 3441 RVA: 0x00037A69 File Offset: 0x00036A69
		internal ApplicationFileCodeDomTreeGenerator(ApplicationFileParser appParser)
			: base(appParser)
		{
			this._appParser = appParser;
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06000D72 RID: 3442 RVA: 0x00037A79 File Offset: 0x00036A79
		protected override bool IsGlobalAsaxGenerator
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04001502 RID: 5378
		protected ApplicationFileParser _appParser;
	}
}
