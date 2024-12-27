using System;
using System.Web.Hosting;

namespace System.Web.Configuration
{
	// Token: 0x020001F2 RID: 498
	internal class HostingPreferredMapPath : IConfigMapPath
	{
		// Token: 0x06001B42 RID: 6978 RVA: 0x0007E2C8 File Offset: 0x0007D2C8
		internal static IConfigMapPath GetInstance()
		{
			IConfigMapPath instance = IISMapPath.GetInstance();
			IConfigMapPath configMapPath = HostingEnvironment.ConfigMapPath;
			if (configMapPath == null || instance.GetType() == configMapPath.GetType())
			{
				return instance;
			}
			return new HostingPreferredMapPath(instance, configMapPath);
		}

		// Token: 0x06001B43 RID: 6979 RVA: 0x0007E2FB File Offset: 0x0007D2FB
		private HostingPreferredMapPath(IConfigMapPath iisConfigMapPath, IConfigMapPath hostingConfigMapPath)
		{
			this._iisConfigMapPath = iisConfigMapPath;
			this._hostingConfigMapPath = hostingConfigMapPath;
		}

		// Token: 0x06001B44 RID: 6980 RVA: 0x0007E314 File Offset: 0x0007D314
		public string GetMachineConfigFilename()
		{
			string text = this._hostingConfigMapPath.GetMachineConfigFilename();
			if (string.IsNullOrEmpty(text))
			{
				text = this._iisConfigMapPath.GetMachineConfigFilename();
			}
			return text;
		}

		// Token: 0x06001B45 RID: 6981 RVA: 0x0007E344 File Offset: 0x0007D344
		public string GetRootWebConfigFilename()
		{
			string text = this._hostingConfigMapPath.GetRootWebConfigFilename();
			if (string.IsNullOrEmpty(text))
			{
				text = this._iisConfigMapPath.GetRootWebConfigFilename();
			}
			return text;
		}

		// Token: 0x06001B46 RID: 6982 RVA: 0x0007E372 File Offset: 0x0007D372
		public void GetPathConfigFilename(string siteID, string path, out string directory, out string baseName)
		{
			this._hostingConfigMapPath.GetPathConfigFilename(siteID, path, out directory, out baseName);
			if (string.IsNullOrEmpty(directory))
			{
				this._iisConfigMapPath.GetPathConfigFilename(siteID, path, out directory, out baseName);
			}
		}

		// Token: 0x06001B47 RID: 6983 RVA: 0x0007E39D File Offset: 0x0007D39D
		public void GetDefaultSiteNameAndID(out string siteName, out string siteID)
		{
			this._hostingConfigMapPath.GetDefaultSiteNameAndID(out siteName, out siteID);
			if (string.IsNullOrEmpty(siteID))
			{
				this._iisConfigMapPath.GetDefaultSiteNameAndID(out siteName, out siteID);
			}
		}

		// Token: 0x06001B48 RID: 6984 RVA: 0x0007E3C2 File Offset: 0x0007D3C2
		public void ResolveSiteArgument(string siteArgument, out string siteName, out string siteID)
		{
			this._hostingConfigMapPath.ResolveSiteArgument(siteArgument, out siteName, out siteID);
			if (string.IsNullOrEmpty(siteID))
			{
				this._iisConfigMapPath.ResolveSiteArgument(siteArgument, out siteName, out siteID);
			}
		}

		// Token: 0x06001B49 RID: 6985 RVA: 0x0007E3EC File Offset: 0x0007D3EC
		public string MapPath(string siteID, string path)
		{
			string text = this._hostingConfigMapPath.MapPath(siteID, path);
			if (string.IsNullOrEmpty(text))
			{
				text = this._iisConfigMapPath.MapPath(siteID, path);
			}
			return text;
		}

		// Token: 0x06001B4A RID: 6986 RVA: 0x0007E420 File Offset: 0x0007D420
		public string GetAppPathForPath(string siteID, string path)
		{
			string text = this._hostingConfigMapPath.GetAppPathForPath(siteID, path);
			if (text == null)
			{
				text = this._iisConfigMapPath.GetAppPathForPath(siteID, path);
			}
			return text;
		}

		// Token: 0x04001841 RID: 6209
		private IConfigMapPath _iisConfigMapPath;

		// Token: 0x04001842 RID: 6210
		private IConfigMapPath _hostingConfigMapPath;
	}
}
