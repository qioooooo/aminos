using System;
using System.ComponentModel;
using System.Configuration;

namespace System.Web.Services.Configuration
{
	// Token: 0x0200013A RID: 314
	public sealed class TypeElement : ConfigurationElement
	{
		// Token: 0x060009AF RID: 2479 RVA: 0x00045EFC File Offset: 0x00044EFC
		public TypeElement()
		{
			this.properties.Add(this.type);
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x00045F4D File Offset: 0x00044F4D
		public TypeElement(string type)
			: this()
		{
			base[this.type] = new TypeAndName(type);
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x00045F67 File Offset: 0x00044F67
		public TypeElement(Type type)
			: this(type.AssemblyQualifiedName)
		{
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x060009B2 RID: 2482 RVA: 0x00045F75 File Offset: 0x00044F75
		// (set) Token: 0x060009B3 RID: 2483 RVA: 0x00045F8D File Offset: 0x00044F8D
		[ConfigurationProperty("type", IsKey = true)]
		[TypeConverter(typeof(TypeAndNameConverter))]
		public Type Type
		{
			get
			{
				return ((TypeAndName)base[this.type]).type;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				base[this.type] = new TypeAndName(value);
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x060009B4 RID: 2484 RVA: 0x00045FAF File Offset: 0x00044FAF
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x04000614 RID: 1556
		private ConfigurationPropertyCollection properties = new ConfigurationPropertyCollection();

		// Token: 0x04000615 RID: 1557
		private readonly ConfigurationProperty type = new ConfigurationProperty("type", typeof(TypeAndName), null, new TypeAndNameConverter(), null, ConfigurationPropertyOptions.IsKey);
	}
}
