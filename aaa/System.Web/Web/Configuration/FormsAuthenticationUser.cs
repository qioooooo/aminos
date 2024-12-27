using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x020001DE RID: 478
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class FormsAuthenticationUser : ConfigurationElement
	{
		// Token: 0x06001AAE RID: 6830 RVA: 0x0007BFE8 File Offset: 0x0007AFE8
		static FormsAuthenticationUser()
		{
			FormsAuthenticationUser._properties.Add(FormsAuthenticationUser._propName);
			FormsAuthenticationUser._properties.Add(FormsAuthenticationUser._propPassword);
		}

		// Token: 0x06001AAF RID: 6831 RVA: 0x0007C061 File Offset: 0x0007B061
		internal FormsAuthenticationUser()
		{
		}

		// Token: 0x06001AB0 RID: 6832 RVA: 0x0007C069 File Offset: 0x0007B069
		public FormsAuthenticationUser(string name, string password)
			: this()
		{
			this.Name = name.ToLower(CultureInfo.InvariantCulture);
			this.Password = password;
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06001AB1 RID: 6833 RVA: 0x0007C089 File Offset: 0x0007B089
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return FormsAuthenticationUser._properties;
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06001AB2 RID: 6834 RVA: 0x0007C090 File Offset: 0x0007B090
		// (set) Token: 0x06001AB3 RID: 6835 RVA: 0x0007C0A2 File Offset: 0x0007B0A2
		[ConfigurationProperty("name", IsRequired = true, IsKey = true, DefaultValue = "")]
		[TypeConverter(typeof(LowerCaseStringConverter))]
		[StringValidator]
		public string Name
		{
			get
			{
				return (string)base[FormsAuthenticationUser._propName];
			}
			set
			{
				base[FormsAuthenticationUser._propName] = value;
			}
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06001AB4 RID: 6836 RVA: 0x0007C0B0 File Offset: 0x0007B0B0
		// (set) Token: 0x06001AB5 RID: 6837 RVA: 0x0007C0C2 File Offset: 0x0007B0C2
		[StringValidator]
		[ConfigurationProperty("password", IsRequired = true, DefaultValue = "")]
		public string Password
		{
			get
			{
				return (string)base[FormsAuthenticationUser._propPassword];
			}
			set
			{
				base[FormsAuthenticationUser._propPassword] = value;
			}
		}

		// Token: 0x040017F3 RID: 6131
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040017F4 RID: 6132
		private static readonly ConfigurationProperty _propName = new ConfigurationProperty("name", typeof(string), "", new LowerCaseStringConverter(), null, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x040017F5 RID: 6133
		private static readonly ConfigurationProperty _propPassword = new ConfigurationProperty("password", typeof(string), "", ConfigurationPropertyOptions.IsRequired);
	}
}
