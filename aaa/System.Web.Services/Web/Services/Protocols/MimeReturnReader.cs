using System;
using System.IO;
using System.Net;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000022 RID: 34
	public abstract class MimeReturnReader : MimeFormatter
	{
		// Token: 0x0600007C RID: 124
		public abstract object Read(WebResponse response, Stream responseStream);
	}
}
