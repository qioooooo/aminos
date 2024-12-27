using System;
using System.Collections;
using System.Configuration;

namespace System.Diagnostics
{
	// Token: 0x020001D2 RID: 466
	internal class SwitchElement : ConfigurationElement
	{
		// Token: 0x06000E7A RID: 3706 RVA: 0x0002DECC File Offset: 0x0002CECC
		static SwitchElement()
		{
			SwitchElement._properties.Add(SwitchElement._propName);
			SwitchElement._properties.Add(SwitchElement._propValue);
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000E7B RID: 3707 RVA: 0x0002DF3B File Offset: 0x0002CF3B
		public Hashtable Attributes
		{
			get
			{
				if (this._attributes == null)
				{
					this._attributes = new Hashtable(StringComparer.OrdinalIgnoreCase);
				}
				return this._attributes;
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000E7C RID: 3708 RVA: 0x0002DF5B File Offset: 0x0002CF5B
		[ConfigurationProperty("name", DefaultValue = "", IsRequired = true, IsKey = true)]
		public string Name
		{
			get
			{
				return (string)base[SwitchElement._propName];
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000E7D RID: 3709 RVA: 0x0002DF6D File Offset: 0x0002CF6D
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return SwitchElement._properties;
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000E7E RID: 3710 RVA: 0x0002DF74 File Offset: 0x0002CF74
		[ConfigurationProperty("value", IsRequired = true)]
		public string Value
		{
			get
			{
				return (string)base[SwitchElement._propValue];
			}
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x0002DF88 File Offset: 0x0002CF88
		protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
		{
			ConfigurationProperty configurationProperty = new ConfigurationProperty(name, typeof(string), value);
			SwitchElement._properties.Add(configurationProperty);
			base[configurationProperty] = value;
			this.Attributes.Add(name, value);
			return true;
		}

		// Token: 0x06000E80 RID: 3712 RVA: 0x0002DFC8 File Offset: 0x0002CFC8
		internal void ResetProperties()
		{
			if (this._attributes != null)
			{
				this._attributes.Clear();
				SwitchElement._properties.Clear();
				SwitchElement._properties.Add(SwitchElement._propName);
				SwitchElement._properties.Add(SwitchElement._propValue);
			}
		}

		// Token: 0x04000F07 RID: 3847
		private static readonly ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04000F08 RID: 3848
		private static readonly ConfigurationProperty _propName = new ConfigurationProperty("name", typeof(string), "", ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x04000F09 RID: 3849
		private static readonly ConfigurationProperty _propValue = new ConfigurationProperty("value", typeof(string), null, ConfigurationPropertyOptions.IsRequired);

		// Token: 0x04000F0A RID: 3850
		private Hashtable _attributes;
	}
}
