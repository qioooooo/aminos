using System;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x020001CD RID: 461
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class CustomError : ConfigurationElement
	{
		// Token: 0x06001A11 RID: 6673 RVA: 0x0007AD84 File Offset: 0x00079D84
		static CustomError()
		{
			CustomError._properties.Add(CustomError._propStatusCode);
			CustomError._properties.Add(CustomError._propRedirect);
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x0007AE02 File Offset: 0x00079E02
		internal CustomError()
		{
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x0007AE0A File Offset: 0x00079E0A
		public CustomError(int statusCode, string redirect)
			: this()
		{
			this.StatusCode = statusCode;
			this.Redirect = redirect;
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x0007AE20 File Offset: 0x00079E20
		public override bool Equals(object customError)
		{
			CustomError customError2 = customError as CustomError;
			return customError2 != null && customError2.StatusCode == this.StatusCode && customError2.Redirect == this.Redirect;
		}

		// Token: 0x06001A15 RID: 6677 RVA: 0x0007AE58 File Offset: 0x00079E58
		public override int GetHashCode()
		{
			return HashCodeCombiner.CombineHashCodes(this.StatusCode, this.Redirect.GetHashCode());
		}

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06001A16 RID: 6678 RVA: 0x0007AE70 File Offset: 0x00079E70
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return CustomError._properties;
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06001A17 RID: 6679 RVA: 0x0007AE77 File Offset: 0x00079E77
		// (set) Token: 0x06001A18 RID: 6680 RVA: 0x0007AE89 File Offset: 0x00079E89
		[ConfigurationProperty("statusCode", IsRequired = true, IsKey = true)]
		[IntegerValidator(MinValue = 100, MaxValue = 999)]
		public int StatusCode
		{
			get
			{
				return (int)base[CustomError._propStatusCode];
			}
			set
			{
				base[CustomError._propStatusCode] = value;
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06001A19 RID: 6681 RVA: 0x0007AE9C File Offset: 0x00079E9C
		// (set) Token: 0x06001A1A RID: 6682 RVA: 0x0007AEAE File Offset: 0x00079EAE
		[ConfigurationProperty("redirect", IsRequired = true)]
		[StringValidator(MinLength = 1)]
		public string Redirect
		{
			get
			{
				return (string)base[CustomError._propRedirect];
			}
			set
			{
				base[CustomError._propRedirect] = value;
			}
		}

		// Token: 0x040017B8 RID: 6072
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040017B9 RID: 6073
		private static readonly ConfigurationProperty _propStatusCode = new ConfigurationProperty("statusCode", typeof(int), null, null, new IntegerValidator(100, 999), ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x040017BA RID: 6074
		private static readonly ConfigurationProperty _propRedirect = new ConfigurationProperty("redirect", typeof(string), null, null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired);
	}
}
