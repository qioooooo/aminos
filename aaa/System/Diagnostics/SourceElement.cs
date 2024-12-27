using System;
using System.Collections;
using System.Configuration;
using System.Xml;

namespace System.Diagnostics
{
	// Token: 0x020001CC RID: 460
	internal class SourceElement : ConfigurationElement
	{
		// Token: 0x06000E55 RID: 3669 RVA: 0x0002D9FC File Offset: 0x0002C9FC
		static SourceElement()
		{
			SourceElement._properties.Add(SourceElement._propName);
			SourceElement._properties.Add(SourceElement._propSwitchName);
			SourceElement._properties.Add(SourceElement._propSwitchValue);
			SourceElement._properties.Add(SourceElement._propSwitchType);
			SourceElement._properties.Add(SourceElement._propListeners);
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000E56 RID: 3670 RVA: 0x0002DAED File Offset: 0x0002CAED
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

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000E57 RID: 3671 RVA: 0x0002DB0D File Offset: 0x0002CB0D
		[ConfigurationProperty("listeners")]
		public ListenerElementsCollection Listeners
		{
			get
			{
				return (ListenerElementsCollection)base[SourceElement._propListeners];
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000E58 RID: 3672 RVA: 0x0002DB1F File Offset: 0x0002CB1F
		[ConfigurationProperty("name", IsRequired = true, DefaultValue = "")]
		public string Name
		{
			get
			{
				return (string)base[SourceElement._propName];
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000E59 RID: 3673 RVA: 0x0002DB31 File Offset: 0x0002CB31
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return SourceElement._properties;
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000E5A RID: 3674 RVA: 0x0002DB38 File Offset: 0x0002CB38
		[ConfigurationProperty("switchName")]
		public string SwitchName
		{
			get
			{
				return (string)base[SourceElement._propSwitchName];
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000E5B RID: 3675 RVA: 0x0002DB4A File Offset: 0x0002CB4A
		[ConfigurationProperty("switchValue")]
		public string SwitchValue
		{
			get
			{
				return (string)base[SourceElement._propSwitchValue];
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000E5C RID: 3676 RVA: 0x0002DB5C File Offset: 0x0002CB5C
		[ConfigurationProperty("switchType")]
		public string SwitchType
		{
			get
			{
				return (string)base[SourceElement._propSwitchType];
			}
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x0002DB70 File Offset: 0x0002CB70
		protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
		{
			base.DeserializeElement(reader, serializeCollectionKey);
			if (!string.IsNullOrEmpty(this.SwitchName) && !string.IsNullOrEmpty(this.SwitchValue))
			{
				throw new ConfigurationErrorsException(SR.GetString("Only_specify_one", new object[] { this.Name }));
			}
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x0002DBC0 File Offset: 0x0002CBC0
		protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
		{
			ConfigurationProperty configurationProperty = new ConfigurationProperty(name, typeof(string), value);
			SourceElement._properties.Add(configurationProperty);
			base[configurationProperty] = value;
			this.Attributes.Add(name, value);
			return true;
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x0002DC00 File Offset: 0x0002CC00
		internal void ResetProperties()
		{
			if (this._attributes != null)
			{
				this._attributes.Clear();
				SourceElement._properties.Clear();
				SourceElement._properties.Add(SourceElement._propName);
				SourceElement._properties.Add(SourceElement._propSwitchName);
				SourceElement._properties.Add(SourceElement._propSwitchValue);
				SourceElement._properties.Add(SourceElement._propSwitchType);
				SourceElement._properties.Add(SourceElement._propListeners);
			}
		}

		// Token: 0x04000EF3 RID: 3827
		private static readonly ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();

		// Token: 0x04000EF4 RID: 3828
		private static readonly ConfigurationProperty _propName = new ConfigurationProperty("name", typeof(string), "", ConfigurationPropertyOptions.IsRequired);

		// Token: 0x04000EF5 RID: 3829
		private static readonly ConfigurationProperty _propSwitchName = new ConfigurationProperty("switchName", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x04000EF6 RID: 3830
		private static readonly ConfigurationProperty _propSwitchValue = new ConfigurationProperty("switchValue", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x04000EF7 RID: 3831
		private static readonly ConfigurationProperty _propSwitchType = new ConfigurationProperty("switchType", typeof(string), null, ConfigurationPropertyOptions.None);

		// Token: 0x04000EF8 RID: 3832
		private static readonly ConfigurationProperty _propListeners = new ConfigurationProperty("listeners", typeof(ListenerElementsCollection), new ListenerElementsCollection(), ConfigurationPropertyOptions.None);

		// Token: 0x04000EF9 RID: 3833
		private Hashtable _attributes;
	}
}
