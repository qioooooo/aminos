using System;
using System.Collections.Specialized;
using System.Security.Permissions;
using System.Web.Hosting;
using System.Web.Util;

namespace System.Web.Management
{
	// Token: 0x020002C6 RID: 710
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class IisTraceWebEventProvider : WebEventProvider
	{
		// Token: 0x0600247F RID: 9343 RVA: 0x0009BF0C File Offset: 0x0009AF0C
		public IisTraceWebEventProvider()
		{
			HttpContext httpContext = HttpContext.Current;
			if (httpContext != null && !HttpRuntime.UseIntegratedPipeline && !(httpContext.WorkerRequest is ISAPIWorkerRequestInProcForIIS7))
			{
				throw new PlatformNotSupportedException(SR.GetString("Requires_Iis_7"));
			}
		}

		// Token: 0x06002480 RID: 9344 RVA: 0x0009BF4C File Offset: 0x0009AF4C
		public override void Initialize(string name, NameValueCollection config)
		{
			base.Initialize(name, config);
			ProviderUtil.CheckUnrecognizedAttributes(config, name);
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x0009BF60 File Offset: 0x0009AF60
		public override void ProcessEvent(WebBaseEvent eventRaised)
		{
			HttpContext httpContext = HttpContext.Current;
			if (httpContext != null)
			{
				httpContext.WorkerRequest.RaiseTraceEvent(eventRaised);
			}
		}

		// Token: 0x06002482 RID: 9346 RVA: 0x0009BF82 File Offset: 0x0009AF82
		public override void Flush()
		{
		}

		// Token: 0x06002483 RID: 9347 RVA: 0x0009BF84 File Offset: 0x0009AF84
		public override void Shutdown()
		{
		}
	}
}
