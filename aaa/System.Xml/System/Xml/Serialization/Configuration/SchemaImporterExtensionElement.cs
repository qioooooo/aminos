using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;

namespace System.Xml.Serialization.Configuration
{
	// Token: 0x0200034F RID: 847
	public sealed class SchemaImporterExtensionElement : ConfigurationElement
	{
		// Token: 0x06002918 RID: 10520 RVA: 0x000D32B4 File Offset: 0x000D22B4
		public SchemaImporterExtensionElement()
		{
			this.properties.Add(this.name);
			this.properties.Add(this.type);
		}

		// Token: 0x06002919 RID: 10521 RVA: 0x000D3332 File Offset: 0x000D2332
		public SchemaImporterExtensionElement(string name, string type)
			: this()
		{
			this.Name = name;
			base[this.type] = new SchemaImporterExtensionElement.TypeAndName(type);
		}

		// Token: 0x0600291A RID: 10522 RVA: 0x000D3353 File Offset: 0x000D2353
		public SchemaImporterExtensionElement(string name, Type type)
			: this()
		{
			this.Name = name;
			this.Type = type;
		}

		// Token: 0x170009BD RID: 2493
		// (get) Token: 0x0600291B RID: 10523 RVA: 0x000D3369 File Offset: 0x000D2369
		// (set) Token: 0x0600291C RID: 10524 RVA: 0x000D337C File Offset: 0x000D237C
		[ConfigurationProperty("name", IsRequired = true, IsKey = true)]
		public string Name
		{
			get
			{
				return (string)base[this.name];
			}
			set
			{
				base[this.name] = value;
			}
		}

		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x0600291D RID: 10525 RVA: 0x000D338B File Offset: 0x000D238B
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x0600291E RID: 10526 RVA: 0x000D3393 File Offset: 0x000D2393
		// (set) Token: 0x0600291F RID: 10527 RVA: 0x000D33AB File Offset: 0x000D23AB
		[ConfigurationProperty("type", IsRequired = true, IsKey = false)]
		[TypeConverter(typeof(SchemaImporterExtensionElement.TypeTypeConverter))]
		public Type Type
		{
			get
			{
				return ((SchemaImporterExtensionElement.TypeAndName)base[this.type]).type;
			}
			set
			{
				base[this.type] = new SchemaImporterExtensionElement.TypeAndName(value);
			}
		}

		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x06002920 RID: 10528 RVA: 0x000D33BF File Offset: 0x000D23BF
		internal string Key
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x040016DF RID: 5855
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x040016E0 RID: 5856
		private readonly ConfigurationProperty name = new ConfigurationProperty("name", typeof(string), null, ConfigurationPropertyOptions.IsKey);

		// Token: 0x040016E1 RID: 5857
		private readonly ConfigurationProperty type = new ConfigurationProperty("type", typeof(Type), null, new SchemaImporterExtensionElement.TypeTypeConverter(), null, ConfigurationPropertyOptions.IsRequired);

		// Token: 0x02000350 RID: 848
		private class TypeAndName
		{
			// Token: 0x06002921 RID: 10529 RVA: 0x000D33C7 File Offset: 0x000D23C7
			public TypeAndName(string name)
			{
				this.type = Type.GetType(name, true, true);
				this.name = name;
			}

			// Token: 0x06002922 RID: 10530 RVA: 0x000D33E4 File Offset: 0x000D23E4
			public TypeAndName(Type type)
			{
				this.type = type;
			}

			// Token: 0x06002923 RID: 10531 RVA: 0x000D33F3 File Offset: 0x000D23F3
			public override int GetHashCode()
			{
				return this.type.GetHashCode();
			}

			// Token: 0x06002924 RID: 10532 RVA: 0x000D3400 File Offset: 0x000D2400
			public override bool Equals(object comparand)
			{
				return this.type.Equals(((SchemaImporterExtensionElement.TypeAndName)comparand).type);
			}

			// Token: 0x040016E2 RID: 5858
			public readonly Type type;

			// Token: 0x040016E3 RID: 5859
			public readonly string name;
		}

		// Token: 0x02000351 RID: 849
		private class TypeTypeConverter : TypeConverter
		{
			// Token: 0x06002925 RID: 10533 RVA: 0x000D3418 File Offset: 0x000D2418
			public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
			{
				return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
			}

			// Token: 0x06002926 RID: 10534 RVA: 0x000D3431 File Offset: 0x000D2431
			public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
			{
				if (value is string)
				{
					return new SchemaImporterExtensionElement.TypeAndName((string)value);
				}
				return base.ConvertFrom(context, culture, value);
			}

			// Token: 0x06002927 RID: 10535 RVA: 0x000D3450 File Offset: 0x000D2450
			public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
			{
				if (destinationType != typeof(string))
				{
					return base.ConvertTo(context, culture, value, destinationType);
				}
				SchemaImporterExtensionElement.TypeAndName typeAndName = (SchemaImporterExtensionElement.TypeAndName)value;
				if (typeAndName.name != null)
				{
					return typeAndName.name;
				}
				return typeAndName.type.AssemblyQualifiedName;
			}
		}
	}
}
