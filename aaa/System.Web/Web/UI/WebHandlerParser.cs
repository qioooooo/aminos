using System;

namespace System.Web.UI
{
	// Token: 0x02000461 RID: 1121
	internal class WebHandlerParser : SimpleWebHandlerParser
	{
		// Token: 0x0600351C RID: 13596 RVA: 0x000E5E38 File Offset: 0x000E4E38
		internal WebHandlerParser(string virtualPath)
			: base(null, virtualPath, null)
		{
		}

		// Token: 0x17000BE0 RID: 3040
		// (get) Token: 0x0600351D RID: 13597 RVA: 0x000E5E43 File Offset: 0x000E4E43
		protected override string DefaultDirectiveName
		{
			get
			{
				return "webhandler";
			}
		}

		// Token: 0x0600351E RID: 13598 RVA: 0x000E5E4A File Offset: 0x000E4E4A
		internal override void ValidateBaseType(Type t)
		{
			Util.CheckAssignableType(typeof(IHttpHandler), t);
		}
	}
}
