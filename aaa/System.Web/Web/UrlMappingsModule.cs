using System;
using System.Web.Configuration;

namespace System.Web
{
	// Token: 0x020000E6 RID: 230
	internal sealed class UrlMappingsModule : IHttpModule
	{
		// Token: 0x06000AB3 RID: 2739 RVA: 0x0002B67A File Offset: 0x0002A67A
		internal UrlMappingsModule()
		{
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x0002B684 File Offset: 0x0002A684
		public void Init(HttpApplication application)
		{
			UrlMappingsSection urlMappings = RuntimeConfig.GetConfig().UrlMappings;
			bool flag = urlMappings.IsEnabled && urlMappings.UrlMappings.Count > 0;
			if (flag)
			{
				application.BeginRequest += this.OnEnter;
			}
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x0002B6CD File Offset: 0x0002A6CD
		public void Dispose()
		{
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x0002B6D0 File Offset: 0x0002A6D0
		internal void OnEnter(object source, EventArgs eventArgs)
		{
			HttpApplication httpApplication = (HttpApplication)source;
			UrlMappingsSection urlMappings = RuntimeConfig.GetAppConfig().UrlMappings;
			string text = urlMappings.HttpResolveMapping(httpApplication.Request.RawUrl);
			if (text == null)
			{
				text = urlMappings.HttpResolveMapping(httpApplication.Request.Path);
			}
			if (!string.IsNullOrEmpty(text))
			{
				httpApplication.Context.RewritePath(text, false);
			}
		}
	}
}
