using System;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Compilation;

namespace System.Web.Configuration
{
	// Token: 0x020001D9 RID: 473
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ExpressionBuilder : ConfigurationElement
	{
		// Token: 0x06001A74 RID: 6772 RVA: 0x0007B720 File Offset: 0x0007A720
		static ExpressionBuilder()
		{
			ExpressionBuilder._properties.Add(ExpressionBuilder._propExpressionPrefix);
			ExpressionBuilder._properties.Add(ExpressionBuilder._propType);
		}

		// Token: 0x06001A75 RID: 6773 RVA: 0x0007B797 File Offset: 0x0007A797
		internal ExpressionBuilder()
		{
		}

		// Token: 0x06001A76 RID: 6774 RVA: 0x0007B79F File Offset: 0x0007A79F
		public ExpressionBuilder(string expressionPrefix, string theType)
		{
			this.ExpressionPrefix = expressionPrefix;
			this.Type = theType;
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06001A77 RID: 6775 RVA: 0x0007B7B5 File Offset: 0x0007A7B5
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return ExpressionBuilder._properties;
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06001A78 RID: 6776 RVA: 0x0007B7BC File Offset: 0x0007A7BC
		// (set) Token: 0x06001A79 RID: 6777 RVA: 0x0007B7CE File Offset: 0x0007A7CE
		[StringValidator(MinLength = 1)]
		[ConfigurationProperty("expressionPrefix", IsRequired = true, IsKey = true, DefaultValue = "")]
		public string ExpressionPrefix
		{
			get
			{
				return (string)base[ExpressionBuilder._propExpressionPrefix];
			}
			set
			{
				base[ExpressionBuilder._propExpressionPrefix] = value;
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06001A7A RID: 6778 RVA: 0x0007B7DC File Offset: 0x0007A7DC
		// (set) Token: 0x06001A7B RID: 6779 RVA: 0x0007B7EE File Offset: 0x0007A7EE
		[StringValidator(MinLength = 1)]
		[ConfigurationProperty("type", IsRequired = true, DefaultValue = "")]
		public string Type
		{
			get
			{
				return (string)base[ExpressionBuilder._propType];
			}
			set
			{
				base[ExpressionBuilder._propType] = value;
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06001A7C RID: 6780 RVA: 0x0007B7FC File Offset: 0x0007A7FC
		internal Type TypeInternal
		{
			get
			{
				return CompilationUtil.LoadTypeWithChecks(this.Type, typeof(ExpressionBuilder), null, this, "type");
			}
		}

		// Token: 0x040017DA RID: 6106
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x040017DB RID: 6107
		private static readonly ConfigurationProperty _propExpressionPrefix = new ConfigurationProperty("expressionPrefix", typeof(string), null, null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x040017DC RID: 6108
		private static readonly ConfigurationProperty _propType = new ConfigurationProperty("type", typeof(string), null, null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired);
	}
}
