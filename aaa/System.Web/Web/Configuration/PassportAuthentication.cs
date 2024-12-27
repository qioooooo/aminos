using System;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x0200021E RID: 542
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class PassportAuthentication : ConfigurationElement
	{
		// Token: 0x06001D2A RID: 7466 RVA: 0x00084B3C File Offset: 0x00083B3C
		static PassportAuthentication()
		{
			PassportAuthentication._properties.Add(PassportAuthentication._propRedirectUrl);
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06001D2C RID: 7468 RVA: 0x00084BAE File Offset: 0x00083BAE
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return PassportAuthentication._properties;
			}
		}

		// Token: 0x170005D5 RID: 1493
		// (get) Token: 0x06001D2D RID: 7469 RVA: 0x00084BB5 File Offset: 0x00083BB5
		// (set) Token: 0x06001D2E RID: 7470 RVA: 0x00084BC7 File Offset: 0x00083BC7
		[StringValidator]
		[ConfigurationProperty("redirectUrl", DefaultValue = "internal")]
		public string RedirectUrl
		{
			get
			{
				return (string)base[PassportAuthentication._propRedirectUrl];
			}
			set
			{
				base[PassportAuthentication._propRedirectUrl] = value;
			}
		}

		// Token: 0x170005D6 RID: 1494
		// (get) Token: 0x06001D2F RID: 7471 RVA: 0x00084BD5 File Offset: 0x00083BD5
		protected override ConfigurationElementProperty ElementProperty
		{
			get
			{
				return PassportAuthentication.s_elemProperty;
			}
		}

		// Token: 0x06001D30 RID: 7472 RVA: 0x00084BDC File Offset: 0x00083BDC
		private static void Validate(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("passport");
			}
			PassportAuthentication passportAuthentication = (PassportAuthentication)value;
			if (StringUtil.StringStartsWith(passportAuthentication.RedirectUrl, "\\\\") || (passportAuthentication.RedirectUrl.Length > 1 && passportAuthentication.RedirectUrl[1] == ':'))
			{
				throw new ConfigurationErrorsException(SR.GetString("Auth_bad_url"));
			}
		}

		// Token: 0x04001942 RID: 6466
		private static readonly ConfigurationElementProperty s_elemProperty = new ConfigurationElementProperty(new CallbackValidator(typeof(PassportAuthentication), new ValidatorCallback(PassportAuthentication.Validate)));

		// Token: 0x04001943 RID: 6467
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001944 RID: 6468
		private static readonly ConfigurationProperty _propRedirectUrl = new ConfigurationProperty("redirectUrl", typeof(string), "internal", ConfigurationPropertyOptions.None);
	}
}
