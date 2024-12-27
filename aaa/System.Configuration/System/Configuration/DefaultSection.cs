using System;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x0200005A RID: 90
	public sealed class DefaultSection : ConfigurationSection
	{
		// Token: 0x0600038B RID: 907 RVA: 0x00012814 File Offset: 0x00011814
		private static ConfigurationPropertyCollection EnsureStaticPropertyBag()
		{
			if (DefaultSection.s_properties == null)
			{
				ConfigurationPropertyCollection configurationPropertyCollection = new ConfigurationPropertyCollection();
				DefaultSection.s_properties = configurationPropertyCollection;
			}
			return DefaultSection.s_properties;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00012839 File Offset: 0x00011839
		public DefaultSection()
		{
			DefaultSection.EnsureStaticPropertyBag();
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600038D RID: 909 RVA: 0x00012852 File Offset: 0x00011852
		protected internal override ConfigurationPropertyCollection Properties
		{
			get
			{
				return DefaultSection.EnsureStaticPropertyBag();
			}
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00012859 File Offset: 0x00011859
		protected internal override bool IsModified()
		{
			return this._isModified;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00012861 File Offset: 0x00011861
		protected internal override void ResetModified()
		{
			this._isModified = false;
		}

		// Token: 0x06000390 RID: 912 RVA: 0x0001286A File Offset: 0x0001186A
		protected internal override void Reset(ConfigurationElement parentSection)
		{
			this._rawXml = string.Empty;
			this._isModified = false;
		}

		// Token: 0x06000391 RID: 913 RVA: 0x0001287E File Offset: 0x0001187E
		protected internal override void DeserializeSection(XmlReader xmlReader)
		{
			if (!xmlReader.Read() || xmlReader.NodeType != XmlNodeType.Element)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_expected_to_find_element"), xmlReader);
			}
			this._rawXml = xmlReader.ReadOuterXml();
			this._isModified = true;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x000128B5 File Offset: 0x000118B5
		protected internal override string SerializeSection(ConfigurationElement parentSection, string name, ConfigurationSaveMode saveMode)
		{
			return this._rawXml;
		}

		// Token: 0x040002E3 RID: 739
		private static ConfigurationPropertyCollection s_properties;

		// Token: 0x040002E4 RID: 740
		private string _rawXml = string.Empty;

		// Token: 0x040002E5 RID: 741
		private bool _isModified;
	}
}
