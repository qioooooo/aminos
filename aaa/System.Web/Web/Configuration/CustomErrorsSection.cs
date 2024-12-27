using System;
using System.Configuration;
using System.Globalization;
using System.Security.Permissions;
using System.Web.Util;
using System.Xml;

namespace System.Web.Configuration
{
	// Token: 0x020001D1 RID: 465
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class CustomErrorsSection : ConfigurationSection
	{
		// Token: 0x06001A2E RID: 6702 RVA: 0x0007B014 File Offset: 0x0007A014
		static CustomErrorsSection()
		{
			CustomErrorsSection._properties.Add(CustomErrorsSection._propDefaultRedirect);
			CustomErrorsSection._properties.Add(CustomErrorsSection._propRedirectMode);
			CustomErrorsSection._properties.Add(CustomErrorsSection._propMode);
			CustomErrorsSection._properties.Add(CustomErrorsSection._propErrors);
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06001A30 RID: 6704 RVA: 0x0007B0E7 File Offset: 0x0007A0E7
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return CustomErrorsSection._properties;
			}
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06001A31 RID: 6705 RVA: 0x0007B0EE File Offset: 0x0007A0EE
		// (set) Token: 0x06001A32 RID: 6706 RVA: 0x0007B100 File Offset: 0x0007A100
		[ConfigurationProperty("defaultRedirect")]
		public string DefaultRedirect
		{
			get
			{
				return (string)base[CustomErrorsSection._propDefaultRedirect];
			}
			set
			{
				base[CustomErrorsSection._propDefaultRedirect] = value;
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06001A33 RID: 6707 RVA: 0x0007B10E File Offset: 0x0007A10E
		// (set) Token: 0x06001A34 RID: 6708 RVA: 0x0007B120 File Offset: 0x0007A120
		[ConfigurationProperty("redirectMode", DefaultValue = CustomErrorsRedirectMode.ResponseRedirect)]
		public CustomErrorsRedirectMode RedirectMode
		{
			get
			{
				return (CustomErrorsRedirectMode)base[CustomErrorsSection._propRedirectMode];
			}
			set
			{
				base[CustomErrorsSection._propRedirectMode] = value;
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06001A35 RID: 6709 RVA: 0x0007B133 File Offset: 0x0007A133
		// (set) Token: 0x06001A36 RID: 6710 RVA: 0x0007B145 File Offset: 0x0007A145
		[ConfigurationProperty("mode", DefaultValue = CustomErrorsMode.RemoteOnly)]
		public CustomErrorsMode Mode
		{
			get
			{
				return (CustomErrorsMode)base[CustomErrorsSection._propMode];
			}
			set
			{
				base[CustomErrorsSection._propMode] = value;
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06001A37 RID: 6711 RVA: 0x0007B158 File Offset: 0x0007A158
		[ConfigurationProperty("", IsDefaultCollection = true)]
		public CustomErrorCollection Errors
		{
			get
			{
				return (CustomErrorCollection)base[CustomErrorsSection._propErrors];
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06001A38 RID: 6712 RVA: 0x0007B16A File Offset: 0x0007A16A
		internal string DefaultAbsolutePath
		{
			get
			{
				if (this._DefaultAbsolutePath == null)
				{
					this._DefaultAbsolutePath = CustomErrorsSection.GetAbsoluteRedirect(this.DefaultRedirect, this.basepath);
				}
				return this._DefaultAbsolutePath;
			}
		}

		// Token: 0x06001A39 RID: 6713 RVA: 0x0007B194 File Offset: 0x0007A194
		internal string GetRedirectString(int code)
		{
			string text = null;
			if (this.Errors != null)
			{
				CustomError customError = this.Errors[code.ToString(CultureInfo.InvariantCulture)];
				if (customError != null)
				{
					text = CustomErrorsSection.GetAbsoluteRedirect(customError.Redirect, this.basepath);
				}
			}
			if (text == null)
			{
				text = this.DefaultAbsolutePath;
			}
			return text;
		}

		// Token: 0x06001A3A RID: 6714 RVA: 0x0007B1E4 File Offset: 0x0007A1E4
		protected override void Reset(ConfigurationElement parentElement)
		{
			base.Reset(parentElement);
			CustomErrorsSection customErrorsSection = parentElement as CustomErrorsSection;
			if (customErrorsSection != null)
			{
				this.basepath = customErrorsSection.basepath;
			}
		}

		// Token: 0x06001A3B RID: 6715 RVA: 0x0007B210 File Offset: 0x0007A210
		protected override void DeserializeSection(XmlReader reader)
		{
			base.DeserializeSection(reader);
			WebContext webContext = base.EvaluationContext.HostingContext as WebContext;
			if (webContext != null)
			{
				this.basepath = UrlPath.AppendSlashToPathIfNeeded(webContext.Path);
			}
		}

		// Token: 0x06001A3C RID: 6716 RVA: 0x0007B249 File Offset: 0x0007A249
		internal static string GetAbsoluteRedirect(string path, string basePath)
		{
			if (path != null && UrlPath.IsRelativeUrl(path))
			{
				if (string.IsNullOrEmpty(basePath))
				{
					basePath = "/";
				}
				path = UrlPath.Combine(basePath, path);
			}
			return path;
		}

		// Token: 0x06001A3D RID: 6717 RVA: 0x0007B26F File Offset: 0x0007A26F
		internal static CustomErrorsSection GetSettings(HttpContext context)
		{
			return CustomErrorsSection.GetSettings(context, false);
		}

		// Token: 0x06001A3E RID: 6718 RVA: 0x0007B278 File Offset: 0x0007A278
		internal static CustomErrorsSection GetSettings(HttpContext context, bool canThrow)
		{
			CustomErrorsSection customErrorsSection = null;
			if (canThrow)
			{
				RuntimeConfig runtimeConfig = RuntimeConfig.GetConfig(context);
				if (runtimeConfig != null)
				{
					customErrorsSection = runtimeConfig.CustomErrors;
				}
			}
			else
			{
				RuntimeConfig runtimeConfig = RuntimeConfig.GetLKGConfig(context);
				if (runtimeConfig != null)
				{
					customErrorsSection = runtimeConfig.CustomErrors;
				}
				if (customErrorsSection == null)
				{
					if (CustomErrorsSection._default == null)
					{
						CustomErrorsSection._default = new CustomErrorsSection();
					}
					customErrorsSection = CustomErrorsSection._default;
				}
			}
			return customErrorsSection;
		}

		// Token: 0x06001A3F RID: 6719 RVA: 0x0007B2CC File Offset: 0x0007A2CC
		internal bool CustomErrorsEnabled(HttpRequest request)
		{
			try
			{
				if (DeploymentSection.RetailInternal)
				{
					return true;
				}
			}
			catch
			{
			}
			switch (this.Mode)
			{
			case CustomErrorsMode.RemoteOnly:
				return !request.IsLocal;
			case CustomErrorsMode.On:
				return true;
			case CustomErrorsMode.Off:
				return false;
			default:
				return false;
			}
			bool flag;
			return flag;
		}

		// Token: 0x040017C3 RID: 6083
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040017C4 RID: 6084
		private static readonly ConfigurationProperty _propDefaultRedirect = new ConfigurationProperty("defaultRedirect", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x040017C5 RID: 6085
		private static readonly ConfigurationProperty _propRedirectMode = new ConfigurationProperty("redirectMode", typeof(CustomErrorsRedirectMode), CustomErrorsRedirectMode.ResponseRedirect, ConfigurationPropertyOptions.None);

		// Token: 0x040017C6 RID: 6086
		private static readonly ConfigurationProperty _propMode = new ConfigurationProperty("mode", typeof(CustomErrorsMode), CustomErrorsMode.RemoteOnly, ConfigurationPropertyOptions.None);

		// Token: 0x040017C7 RID: 6087
		private static readonly ConfigurationProperty _propErrors = new ConfigurationProperty(null, typeof(CustomErrorCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);

		// Token: 0x040017C8 RID: 6088
		private string basepath;

		// Token: 0x040017C9 RID: 6089
		private string _DefaultAbsolutePath;

		// Token: 0x040017CA RID: 6090
		private static CustomErrorsSection _default = null;
	}
}
