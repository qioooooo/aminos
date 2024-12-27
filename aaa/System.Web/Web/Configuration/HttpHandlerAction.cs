using System;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x020001F9 RID: 505
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HttpHandlerAction : ConfigurationElement
	{
		// Token: 0x06001B84 RID: 7044 RVA: 0x0007F604 File Offset: 0x0007E604
		static HttpHandlerAction()
		{
			HttpHandlerAction._properties.Add(HttpHandlerAction._propPath);
			HttpHandlerAction._properties.Add(HttpHandlerAction._propVerb);
			HttpHandlerAction._properties.Add(HttpHandlerAction._propType);
			HttpHandlerAction._properties.Add(HttpHandlerAction._propValidate);
		}

		// Token: 0x06001B85 RID: 7045 RVA: 0x0007F6DA File Offset: 0x0007E6DA
		public HttpHandlerAction(string path, string type, string verb)
			: this(path, type, verb, true)
		{
		}

		// Token: 0x06001B86 RID: 7046 RVA: 0x0007F6E6 File Offset: 0x0007E6E6
		public HttpHandlerAction(string path, string type, string verb, bool validate)
		{
			this.Path = path;
			this.Type = type;
			this.Verb = verb;
			this.Validate = validate;
		}

		// Token: 0x06001B87 RID: 7047 RVA: 0x0007F70B File Offset: 0x0007E70B
		internal HttpHandlerAction()
		{
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06001B88 RID: 7048 RVA: 0x0007F713 File Offset: 0x0007E713
		internal string Key
		{
			get
			{
				return "verb=" + this.Verb + " | path=" + this.Path;
			}
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06001B89 RID: 7049 RVA: 0x0007F730 File Offset: 0x0007E730
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return HttpHandlerAction._properties;
			}
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06001B8A RID: 7050 RVA: 0x0007F737 File Offset: 0x0007E737
		// (set) Token: 0x06001B8B RID: 7051 RVA: 0x0007F749 File Offset: 0x0007E749
		[ConfigurationProperty("path", IsRequired = true, IsKey = true)]
		public string Path
		{
			get
			{
				return (string)base[HttpHandlerAction._propPath];
			}
			set
			{
				base[HttpHandlerAction._propPath] = value;
			}
		}

		// Token: 0x17000552 RID: 1362
		// (get) Token: 0x06001B8C RID: 7052 RVA: 0x0007F757 File Offset: 0x0007E757
		// (set) Token: 0x06001B8D RID: 7053 RVA: 0x0007F769 File Offset: 0x0007E769
		[ConfigurationProperty("verb", IsRequired = true, IsKey = true)]
		public string Verb
		{
			get
			{
				return (string)base[HttpHandlerAction._propVerb];
			}
			set
			{
				base[HttpHandlerAction._propVerb] = value;
			}
		}

		// Token: 0x17000553 RID: 1363
		// (get) Token: 0x06001B8E RID: 7054 RVA: 0x0007F777 File Offset: 0x0007E777
		// (set) Token: 0x06001B8F RID: 7055 RVA: 0x0007F79D File Offset: 0x0007E79D
		[ConfigurationProperty("type", IsRequired = true)]
		public string Type
		{
			get
			{
				if (this.typeCache == null)
				{
					this.typeCache = (string)base[HttpHandlerAction._propType];
				}
				return this.typeCache;
			}
			set
			{
				base[HttpHandlerAction._propType] = value;
				this.typeCache = value;
			}
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06001B90 RID: 7056 RVA: 0x0007F7B2 File Offset: 0x0007E7B2
		internal Type TypeInternal
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06001B91 RID: 7057 RVA: 0x0007F7BA File Offset: 0x0007E7BA
		// (set) Token: 0x06001B92 RID: 7058 RVA: 0x0007F7CC File Offset: 0x0007E7CC
		[ConfigurationProperty("validate", DefaultValue = true)]
		public bool Validate
		{
			get
			{
				return (bool)base[HttpHandlerAction._propValidate];
			}
			set
			{
				base[HttpHandlerAction._propValidate] = value;
			}
		}

		// Token: 0x06001B93 RID: 7059 RVA: 0x0007F7E0 File Offset: 0x0007E7E0
		internal void InitValidateInternal()
		{
			string text = this.Verb;
			text = text.Replace(" ", string.Empty);
			this._requestType = new Wildcard(text, false);
			this._path = new WildcardUrl(this.Path, true);
			if (!this.Validate)
			{
				this._type = null;
				return;
			}
			this._type = ConfigUtil.GetType(this.Type, "type", this);
			if (!ConfigUtil.IsTypeHandlerOrFactory(this._type))
			{
				throw new ConfigurationErrorsException(SR.GetString("Type_not_factory_or_handler", new object[] { this.Type }), base.ElementInformation.Source, base.ElementInformation.LineNumber);
			}
		}

		// Token: 0x06001B94 RID: 7060 RVA: 0x0007F88F File Offset: 0x0007E88F
		internal bool IsMatch(string verb, VirtualPath path)
		{
			return this._path.IsSuffix(path.VirtualPathString) && this._requestType.IsMatch(verb);
		}

		// Token: 0x06001B95 RID: 7061 RVA: 0x0007F8B4 File Offset: 0x0007E8B4
		internal object Create()
		{
			if (this._type == null)
			{
				Type type = ConfigUtil.GetType(this.Type, "type", this);
				if (!ConfigUtil.IsTypeHandlerOrFactory(type))
				{
					throw new ConfigurationErrorsException(SR.GetString("Type_not_factory_or_handler", new object[] { this.Type }), base.ElementInformation.Source, base.ElementInformation.LineNumber);
				}
				this._type = type;
			}
			return HttpRuntime.CreateNonPublicInstance(this._type);
		}

		// Token: 0x04001870 RID: 6256
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04001871 RID: 6257
		private static readonly ConfigurationProperty _propPath = new ConfigurationProperty("path", typeof(string), null, null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x04001872 RID: 6258
		private static readonly ConfigurationProperty _propVerb = new ConfigurationProperty("verb", typeof(string), null, null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x04001873 RID: 6259
		private static readonly ConfigurationProperty _propType = new ConfigurationProperty("type", typeof(string), null, null, StdValidatorsAndConverters.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired);

		// Token: 0x04001874 RID: 6260
		private static readonly ConfigurationProperty _propValidate = new ConfigurationProperty("validate", typeof(bool), true, ConfigurationPropertyOptions.None);

		// Token: 0x04001875 RID: 6261
		private Wildcard _requestType;

		// Token: 0x04001876 RID: 6262
		private WildcardUrl _path;

		// Token: 0x04001877 RID: 6263
		private Type _type;

		// Token: 0x04001878 RID: 6264
		private string typeCache;
	}
}
