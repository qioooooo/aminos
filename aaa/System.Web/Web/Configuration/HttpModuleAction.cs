using System;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Configuration.Common;
using System.Web.Security;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x020001FC RID: 508
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HttpModuleAction : ConfigurationElement
	{
		// Token: 0x06001BAB RID: 7083 RVA: 0x0007FB04 File Offset: 0x0007EB04
		static HttpModuleAction()
		{
			HttpModuleAction._properties.Add(HttpModuleAction._propName);
			HttpModuleAction._properties.Add(HttpModuleAction._propType);
		}

		// Token: 0x06001BAC RID: 7084 RVA: 0x0007FB9E File Offset: 0x0007EB9E
		internal HttpModuleAction()
		{
		}

		// Token: 0x06001BAD RID: 7085 RVA: 0x0007FBA6 File Offset: 0x0007EBA6
		public HttpModuleAction(string name, string type)
			: this()
		{
			this.Name = name;
			this.Type = type;
			this._modualEntry = null;
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06001BAE RID: 7086 RVA: 0x0007FBC3 File Offset: 0x0007EBC3
		internal string Key
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x1700055D RID: 1373
		// (get) Token: 0x06001BAF RID: 7087 RVA: 0x0007FBCB File Offset: 0x0007EBCB
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return HttpModuleAction._properties;
			}
		}

		// Token: 0x1700055E RID: 1374
		// (get) Token: 0x06001BB0 RID: 7088 RVA: 0x0007FBD2 File Offset: 0x0007EBD2
		// (set) Token: 0x06001BB1 RID: 7089 RVA: 0x0007FBE4 File Offset: 0x0007EBE4
		[ConfigurationProperty("name", IsRequired = true, IsKey = true, DefaultValue = "")]
		[StringValidator(MinLength = 1)]
		public string Name
		{
			get
			{
				return (string)base[HttpModuleAction._propName];
			}
			set
			{
				base[HttpModuleAction._propName] = value;
			}
		}

		// Token: 0x1700055F RID: 1375
		// (get) Token: 0x06001BB2 RID: 7090 RVA: 0x0007FBF2 File Offset: 0x0007EBF2
		// (set) Token: 0x06001BB3 RID: 7091 RVA: 0x0007FC04 File Offset: 0x0007EC04
		[ConfigurationProperty("type", IsRequired = true, DefaultValue = "")]
		public string Type
		{
			get
			{
				return (string)base[HttpModuleAction._propType];
			}
			set
			{
				base[HttpModuleAction._propType] = value;
			}
		}

		// Token: 0x17000560 RID: 1376
		// (get) Token: 0x06001BB4 RID: 7092 RVA: 0x0007FC12 File Offset: 0x0007EC12
		internal string FileName
		{
			get
			{
				return base.ElementInformation.Properties["name"].Source;
			}
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x06001BB5 RID: 7093 RVA: 0x0007FC2E File Offset: 0x0007EC2E
		internal int LineNumber
		{
			get
			{
				return base.ElementInformation.Properties["name"].LineNumber;
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x06001BB6 RID: 7094 RVA: 0x0007FC4C File Offset: 0x0007EC4C
		internal ModulesEntry Entry
		{
			get
			{
				ModulesEntry modualEntry;
				try
				{
					if (this._modualEntry == null)
					{
						this._modualEntry = new ModulesEntry(this.Name, this.Type, HttpModuleAction._propType.Name, this);
					}
					modualEntry = this._modualEntry;
				}
				catch (Exception ex)
				{
					throw new ConfigurationErrorsException(ex.Message, base.ElementInformation.Properties[HttpModuleAction._propType.Name].Source, base.ElementInformation.Properties[HttpModuleAction._propType.Name].LineNumber);
				}
				return modualEntry;
			}
		}

		// Token: 0x06001BB7 RID: 7095 RVA: 0x0007FCE8 File Offset: 0x0007ECE8
		internal static bool IsSpecialModule(string className)
		{
			return ModulesEntry.IsTypeMatch(typeof(DefaultAuthenticationModule), className);
		}

		// Token: 0x06001BB8 RID: 7096 RVA: 0x0007FCFA File Offset: 0x0007ECFA
		internal static bool IsSpecialModuleName(string name)
		{
			return StringUtil.EqualsIgnoreCase(name, "DefaultAuthentication");
		}

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06001BB9 RID: 7097 RVA: 0x0007FD07 File Offset: 0x0007ED07
		protected override ConfigurationElementProperty ElementProperty
		{
			get
			{
				return HttpModuleAction.s_elemProperty;
			}
		}

		// Token: 0x06001BBA RID: 7098 RVA: 0x0007FD10 File Offset: 0x0007ED10
		private static void Validate(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("httpModule");
			}
			HttpModuleAction httpModuleAction = (HttpModuleAction)value;
			if (HttpModuleAction.IsSpecialModule(httpModuleAction.Type))
			{
				throw new ConfigurationErrorsException(SR.GetString("Special_module_cannot_be_added_manually", new object[] { httpModuleAction.Type }), httpModuleAction.ElementInformation.Properties["type"].Source, httpModuleAction.ElementInformation.Properties["type"].LineNumber);
			}
			if (HttpModuleAction.IsSpecialModuleName(httpModuleAction.Name))
			{
				throw new ConfigurationErrorsException(SR.GetString("Special_module_cannot_be_added_manually", new object[] { httpModuleAction.Name }), httpModuleAction.ElementInformation.Properties["name"].Source, httpModuleAction.ElementInformation.Properties["name"].LineNumber);
			}
		}

		// Token: 0x0400187D RID: 6269
		private static readonly ConfigurationElementProperty s_elemProperty = new ConfigurationElementProperty(new CallbackValidator(typeof(HttpModuleAction), new ValidatorCallback(HttpModuleAction.Validate)));

		// Token: 0x0400187E RID: 6270
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x0400187F RID: 6271
		private static readonly ConfigurationProperty _propName = new ConfigurationProperty("name", typeof(string), null, null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x04001880 RID: 6272
		private static readonly ConfigurationProperty _propType = new ConfigurationProperty("type", typeof(string), string.Empty, ConfigurationPropertyOptions.IsRequired);

		// Token: 0x04001881 RID: 6273
		private ModulesEntry _modualEntry;
	}
}
