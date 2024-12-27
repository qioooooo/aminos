using System;
using System.IO;
using System.Net;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200004E RID: 78
	public class NopReturnReader : MimeReturnReader
	{
		// Token: 0x060001B7 RID: 439 RVA: 0x000075B2 File Offset: 0x000065B2
		public override object GetInitializer(LogicalMethodInfo methodInfo)
		{
			return this;
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x000075B5 File Offset: 0x000065B5
		public override void Initialize(object initializer)
		{
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x000075B7 File Offset: 0x000065B7
		public override object Read(WebResponse response, Stream responseStream)
		{
			response.Close();
			return null;
		}
	}
}
