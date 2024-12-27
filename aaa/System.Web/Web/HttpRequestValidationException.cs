using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Web
{
	// Token: 0x0200006F RID: 111
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public sealed class HttpRequestValidationException : HttpException
	{
		// Token: 0x060004CB RID: 1227 RVA: 0x0001421B File Offset: 0x0001321B
		public HttpRequestValidationException()
		{
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x00014223 File Offset: 0x00013223
		public HttpRequestValidationException(string message)
			: base(message)
		{
			base.SetFormatter(new UnhandledErrorFormatter(this, SR.GetString("Dangerous_input_detected_descr"), null));
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00014243 File Offset: 0x00013243
		public HttpRequestValidationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0001424D File Offset: 0x0001324D
		private HttpRequestValidationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
