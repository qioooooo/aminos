using System;
using System.IO;
using System.Net;
using System.Text;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200003A RID: 58
	public class HtmlFormParameterWriter : UrlEncodedParameterWriter
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00005628 File Offset: 0x00004628
		public override bool UsesWriteRequest
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000562B File Offset: 0x0000462B
		public override void InitializeRequest(WebRequest request, object[] values)
		{
			request.ContentType = ContentType.Compose("application/x-www-form-urlencoded", this.RequestEncoding);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00005644 File Offset: 0x00004644
		public override void WriteRequest(Stream requestStream, object[] values)
		{
			if (values.Length == 0)
			{
				return;
			}
			TextWriter textWriter = new StreamWriter(requestStream, new ASCIIEncoding());
			base.Encode(textWriter, values);
			textWriter.Flush();
		}
	}
}
