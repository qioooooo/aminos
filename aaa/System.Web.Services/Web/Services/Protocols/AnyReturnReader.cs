using System;
using System.IO;
using System.Net;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000023 RID: 35
	public class AnyReturnReader : MimeReturnReader
	{
		// Token: 0x0600007E RID: 126 RVA: 0x00002F25 File Offset: 0x00001F25
		public override void Initialize(object o)
		{
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00002F27 File Offset: 0x00001F27
		public override object GetInitializer(LogicalMethodInfo methodInfo)
		{
			if (methodInfo.IsVoid)
			{
				return null;
			}
			return this;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00002F34 File Offset: 0x00001F34
		public override object Read(WebResponse response, Stream responseStream)
		{
			return responseStream;
		}
	}
}
