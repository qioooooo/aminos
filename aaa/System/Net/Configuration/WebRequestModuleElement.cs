using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;

namespace System.Net.Configuration
{
	// Token: 0x0200066B RID: 1643
	public sealed class WebRequestModuleElement : ConfigurationElement
	{
		// Token: 0x060032C8 RID: 13000 RVA: 0x000D750C File Offset: 0x000D650C
		public WebRequestModuleElement()
		{
			this.properties.Add(this.prefix);
			this.properties.Add(this.type);
		}

		// Token: 0x060032C9 RID: 13001 RVA: 0x000D758A File Offset: 0x000D658A
		public WebRequestModuleElement(string prefix, string type)
			: this()
		{
			this.Prefix = prefix;
			base[this.type] = new WebRequestModuleElement.TypeAndName(type);
		}

		// Token: 0x060032CA RID: 13002 RVA: 0x000D75AB File Offset: 0x000D65AB
		public WebRequestModuleElement(string prefix, Type type)
			: this()
		{
			this.Prefix = prefix;
			this.Type = type;
		}

		// Token: 0x17000BEC RID: 3052
		// (get) Token: 0x060032CB RID: 13003 RVA: 0x000D75C1 File Offset: 0x000D65C1
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x17000BED RID: 3053
		// (get) Token: 0x060032CC RID: 13004 RVA: 0x000D75C9 File Offset: 0x000D65C9
		// (set) Token: 0x060032CD RID: 13005 RVA: 0x000D75DC File Offset: 0x000D65DC
		[ConfigurationProperty("prefix", IsRequired = true, IsKey = true)]
		public string Prefix
		{
			get
			{
				return (string)base[this.prefix];
			}
			set
			{
				base[this.prefix] = value;
			}
		}

		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x060032CE RID: 13006 RVA: 0x000D75EC File Offset: 0x000D65EC
		// (set) Token: 0x060032CF RID: 13007 RVA: 0x000D7616 File Offset: 0x000D6616
		[TypeConverter(typeof(WebRequestModuleElement.TypeTypeConverter))]
		[ConfigurationProperty("type")]
		public Type Type
		{
			get
			{
				WebRequestModuleElement.TypeAndName typeAndName = (WebRequestModuleElement.TypeAndName)base[this.type];
				if (typeAndName != null)
				{
					return typeAndName.type;
				}
				return null;
			}
			set
			{
				base[this.type] = new WebRequestModuleElement.TypeAndName(value);
			}
		}

		// Token: 0x17000BEF RID: 3055
		// (get) Token: 0x060032D0 RID: 13008 RVA: 0x000D762A File Offset: 0x000D662A
		internal string Key
		{
			get
			{
				return this.Prefix;
			}
		}

		// Token: 0x04002F6A RID: 12138
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04002F6B RID: 12139
		private readonly ConfigurationProperty prefix = new ConfigurationProperty("prefix", typeof(string), null, ConfigurationPropertyOptions.IsKey);

		// Token: 0x04002F6C RID: 12140
		private readonly ConfigurationProperty type = new ConfigurationProperty("type", typeof(WebRequestModuleElement.TypeAndName), null, new WebRequestModuleElement.TypeTypeConverter(), null, ConfigurationPropertyOptions.None);

		// Token: 0x0200066C RID: 1644
		private class TypeAndName
		{
			// Token: 0x060032D1 RID: 13009 RVA: 0x000D7632 File Offset: 0x000D6632
			public TypeAndName(string name)
			{
				this.type = Type.GetType(name, true, true);
				this.name = name;
			}

			// Token: 0x060032D2 RID: 13010 RVA: 0x000D764F File Offset: 0x000D664F
			public TypeAndName(Type type)
			{
				this.type = type;
			}

			// Token: 0x060032D3 RID: 13011 RVA: 0x000D765E File Offset: 0x000D665E
			public override int GetHashCode()
			{
				return this.type.GetHashCode();
			}

			// Token: 0x060032D4 RID: 13012 RVA: 0x000D766B File Offset: 0x000D666B
			public override bool Equals(object comparand)
			{
				return this.type.Equals(((WebRequestModuleElement.TypeAndName)comparand).type);
			}

			// Token: 0x04002F6D RID: 12141
			public readonly Type type;

			// Token: 0x04002F6E RID: 12142
			public readonly string name;
		}

		// Token: 0x0200066D RID: 1645
		private class TypeTypeConverter : TypeConverter
		{
			// Token: 0x060032D5 RID: 13013 RVA: 0x000D7683 File Offset: 0x000D6683
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
			}

			// Token: 0x060032D6 RID: 13014 RVA: 0x000D769C File Offset: 0x000D669C
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				if (value is string)
				{
					return new WebRequestModuleElement.TypeAndName((string)value);
				}
				return base.ConvertFrom(context, culture, value);
			}

			// Token: 0x060032D7 RID: 13015 RVA: 0x000D76BC File Offset: 0x000D66BC
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType != typeof(string))
				{
					return base.ConvertTo(context, culture, value, destinationType);
				}
				WebRequestModuleElement.TypeAndName typeAndName = (WebRequestModuleElement.TypeAndName)value;
				if (typeAndName.name != null)
				{
					return typeAndName.name;
				}
				return typeAndName.type.AssemblyQualifiedName;
			}
		}
	}
}
