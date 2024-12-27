using System;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x0200006C RID: 108
	public sealed class IgnoreSection : ConfigurationSection
	{
		// Token: 0x06000417 RID: 1047 RVA: 0x00013DBC File Offset: 0x00012DBC
		private static ConfigurationPropertyCollection EnsureStaticPropertyBag()
		{
			if (IgnoreSection.s_properties == null)
			{
				ConfigurationPropertyCollection configurationPropertyCollection = new ConfigurationPropertyCollection();
				IgnoreSection.s_properties = configurationPropertyCollection;
			}
			return IgnoreSection.s_properties;
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00013DE1 File Offset: 0x00012DE1
		public IgnoreSection()
		{
			IgnoreSection.EnsureStaticPropertyBag();
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x00013DFA File Offset: 0x00012DFA
		protected internal override ConfigurationPropertyCollection Properties
		{
			get
			{
				return IgnoreSection.EnsureStaticPropertyBag();
			}
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00013E01 File Offset: 0x00012E01
		protected internal override bool IsModified()
		{
			return this._isModified;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00013E09 File Offset: 0x00012E09
		protected internal override void ResetModified()
		{
			this._isModified = false;
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00013E12 File Offset: 0x00012E12
		protected internal override void Reset(ConfigurationElement parentSection)
		{
			this._rawXml = string.Empty;
			this._isModified = false;
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00013E26 File Offset: 0x00012E26
		protected internal override void DeserializeSection(XmlReader xmlReader)
		{
			if (!xmlReader.Read() || xmlReader.NodeType != XmlNodeType.Element)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_expected_to_find_element"), xmlReader);
			}
			this._rawXml = xmlReader.ReadOuterXml();
			this._isModified = true;
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x00013E5D File Offset: 0x00012E5D
		protected internal override string SerializeSection(ConfigurationElement parentSection, string name, ConfigurationSaveMode saveMode)
		{
			return this._rawXml;
		}

		// Token: 0x04000317 RID: 791
		private static ConfigurationPropertyCollection s_properties;

		// Token: 0x04000318 RID: 792
		private string _rawXml = string.Empty;

		// Token: 0x04000319 RID: 793
		private bool _isModified;
	}
}
