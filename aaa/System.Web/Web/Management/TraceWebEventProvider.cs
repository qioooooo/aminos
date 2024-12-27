using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.Management
{
	// Token: 0x020002FD RID: 765
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class TraceWebEventProvider : WebEventProvider, IInternalWebEventProvider
	{
		// Token: 0x0600260E RID: 9742 RVA: 0x000A2EA0 File Offset: 0x000A1EA0
		internal TraceWebEventProvider()
		{
		}

		// Token: 0x0600260F RID: 9743 RVA: 0x000A2EA8 File Offset: 0x000A1EA8
		public override void Initialize(string name, NameValueCollection config)
		{
			base.Initialize(name, config);
			ProviderUtil.CheckUnrecognizedAttributes(config, name);
		}

		// Token: 0x06002610 RID: 9744 RVA: 0x000A2EB9 File Offset: 0x000A1EB9
		public override void ProcessEvent(WebBaseEvent eventRaised)
		{
			if (eventRaised is WebBaseErrorEvent)
			{
				Trace.TraceError(eventRaised.ToString());
				return;
			}
			Trace.TraceInformation(eventRaised.ToString());
		}

		// Token: 0x06002611 RID: 9745 RVA: 0x000A2EDA File Offset: 0x000A1EDA
		public override void Flush()
		{
		}

		// Token: 0x06002612 RID: 9746 RVA: 0x000A2EDC File Offset: 0x000A1EDC
		public override void Shutdown()
		{
		}
	}
}
