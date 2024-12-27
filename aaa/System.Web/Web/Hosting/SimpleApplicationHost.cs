using System;
using System.Web.Configuration;
using System.Web.Util;

namespace System.Web.Hosting
{
	// Token: 0x020002BF RID: 703
	internal class SimpleApplicationHost : MarshalByRefObject, IApplicationHost
	{
		// Token: 0x06002438 RID: 9272 RVA: 0x0009B280 File Offset: 0x0009A280
		internal SimpleApplicationHost(VirtualPath virtualPath, string physicalPath)
		{
			if (string.IsNullOrEmpty(physicalPath))
			{
				throw ExceptionUtil.ParameterNullOrEmpty("physicalPath");
			}
			if (FileUtil.IsSuspiciousPhysicalPath(physicalPath))
			{
				throw ExceptionUtil.ParameterInvalid(physicalPath);
			}
			this._appVirtualPath = virtualPath;
			this._appPhysicalPath = (StringUtil.StringEndsWith(physicalPath, "\\") ? physicalPath : (physicalPath + "\\"));
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x0009B2DD File Offset: 0x0009A2DD
		public override object InitializeLifetimeService()
		{
			return null;
		}

		// Token: 0x0600243A RID: 9274 RVA: 0x0009B2E0 File Offset: 0x0009A2E0
		public string GetVirtualPath()
		{
			return this._appVirtualPath.VirtualPathString;
		}

		// Token: 0x0600243B RID: 9275 RVA: 0x0009B2ED File Offset: 0x0009A2ED
		string IApplicationHost.GetPhysicalPath()
		{
			return this._appPhysicalPath;
		}

		// Token: 0x0600243C RID: 9276 RVA: 0x0009B2F5 File Offset: 0x0009A2F5
		IConfigMapPathFactory IApplicationHost.GetConfigMapPathFactory()
		{
			return new SimpleConfigMapPathFactory();
		}

		// Token: 0x0600243D RID: 9277 RVA: 0x0009B2FC File Offset: 0x0009A2FC
		IntPtr IApplicationHost.GetConfigToken()
		{
			return IntPtr.Zero;
		}

		// Token: 0x0600243E RID: 9278 RVA: 0x0009B303 File Offset: 0x0009A303
		string IApplicationHost.GetSiteName()
		{
			return WebConfigurationHost.DefaultSiteName;
		}

		// Token: 0x0600243F RID: 9279 RVA: 0x0009B30A File Offset: 0x0009A30A
		string IApplicationHost.GetSiteID()
		{
			return "1";
		}

		// Token: 0x06002440 RID: 9280 RVA: 0x0009B311 File Offset: 0x0009A311
		public void MessageReceived()
		{
		}

		// Token: 0x04001C3A RID: 7226
		private VirtualPath _appVirtualPath;

		// Token: 0x04001C3B RID: 7227
		private string _appPhysicalPath;
	}
}
