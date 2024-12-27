using System;
using System.Web.Services.Diagnostics;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200008A RID: 138
	internal class SyncSessionlessHandler : WebServiceHandler, IHttpHandler
	{
		// Token: 0x060003AD RID: 941 RVA: 0x00012A12 File Offset: 0x00011A12
		internal SyncSessionlessHandler(ServerProtocol protocol)
			: base(protocol)
		{
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060003AE RID: 942 RVA: 0x00012A1B File Offset: 0x00011A1B
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00012A20 File Offset: 0x00011A20
		public void ProcessRequest(HttpContext context)
		{
			TraceMethod traceMethod = (Tracing.On ? new TraceMethod(this, "ProcessRequest", new object[0]) : null);
			if (Tracing.On)
			{
				Tracing.Enter("IHttpHandler.ProcessRequest", traceMethod, Tracing.Details(context.Request));
			}
			base.CoreProcessRequest();
			if (Tracing.On)
			{
				Tracing.Exit("IHttpHandler.ProcessRequest", traceMethod);
			}
		}
	}
}
