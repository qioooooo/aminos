using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Web
{
	// Token: 0x0200006C RID: 108
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public sealed class HttpUnhandledException : HttpException
	{
		// Token: 0x060004AC RID: 1196 RVA: 0x00013E3C File Offset: 0x00012E3C
		public HttpUnhandledException()
		{
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x00013E44 File Offset: 0x00012E44
		public HttpUnhandledException(string message)
			: base(message)
		{
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00013E4D File Offset: 0x00012E4D
		public HttpUnhandledException(string message, Exception innerException)
			: base(message, innerException)
		{
			base.SetFormatter(new UnhandledErrorFormatter(innerException, message, null));
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00013E65 File Offset: 0x00012E65
		internal HttpUnhandledException(string message, string postMessage, Exception innerException)
			: base(message, innerException)
		{
			base.SetFormatter(new UnhandledErrorFormatter(innerException, message, postMessage));
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00013E7D File Offset: 0x00012E7D
		private HttpUnhandledException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
