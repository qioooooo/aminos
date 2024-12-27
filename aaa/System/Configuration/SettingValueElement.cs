using System;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x02000724 RID: 1828
	public sealed class SettingValueElement : ConfigurationElement
	{
		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x060037CA RID: 14282 RVA: 0x000EC087 File Offset: 0x000EB087
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				if (SettingValueElement._properties == null)
				{
					SettingValueElement._properties = new ConfigurationPropertyCollection();
				}
				return SettingValueElement._properties;
			}
		}

		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x060037CB RID: 14283 RVA: 0x000EC09F File Offset: 0x000EB09F
		// (set) Token: 0x060037CC RID: 14284 RVA: 0x000EC0A7 File Offset: 0x000EB0A7
		public XmlNode ValueXml
		{
			get
			{
				return this._valueXml;
			}
			set
			{
				this._valueXml = value;
				this.isModified = true;
			}
		}

		// Token: 0x060037CD RID: 14285 RVA: 0x000EC0B7 File Offset: 0x000EB0B7
		protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
		{
			this.ValueXml = SettingValueElement.doc.ReadNode(reader);
		}

		// Token: 0x060037CE RID: 14286 RVA: 0x000EC0CC File Offset: 0x000EB0CC
		public override bool Equals(object settingValue)
		{
			SettingValueElement settingValueElement = settingValue as SettingValueElement;
			return settingValueElement != null && object.Equals(settingValueElement.ValueXml, this.ValueXml);
		}

		// Token: 0x060037CF RID: 14287 RVA: 0x000EC0F6 File Offset: 0x000EB0F6
		public override int GetHashCode()
		{
			return this.ValueXml.GetHashCode();
		}

		// Token: 0x060037D0 RID: 14288 RVA: 0x000EC103 File Offset: 0x000EB103
		protected override bool IsModified()
		{
			return this.isModified;
		}

		// Token: 0x060037D1 RID: 14289 RVA: 0x000EC10B File Offset: 0x000EB10B
		protected override void ResetModified()
		{
			this.isModified = false;
		}

		// Token: 0x060037D2 RID: 14290 RVA: 0x000EC114 File Offset: 0x000EB114
		protected override bool SerializeToXmlElement(XmlWriter writer, string elementName)
		{
			if (this.ValueXml != null)
			{
				if (writer != null)
				{
					this.ValueXml.WriteTo(writer);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060037D3 RID: 14291 RVA: 0x000EC130 File Offset: 0x000EB130
		protected override void Reset(ConfigurationElement parentElement)
		{
			base.Reset(parentElement);
			this.ValueXml = ((SettingValueElement)parentElement).ValueXml;
		}

		// Token: 0x060037D4 RID: 14292 RVA: 0x000EC14A File Offset: 0x000EB14A
		protected override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
		{
			base.Unmerge(sourceElement, parentElement, saveMode);
			this.ValueXml = ((SettingValueElement)sourceElement).ValueXml;
		}

		// Token: 0x040031E0 RID: 12768
		private static ConfigurationPropertyCollection _properties;

		// Token: 0x040031E1 RID: 12769
		private static XmlDocument doc = new XmlDocument();

		// Token: 0x040031E2 RID: 12770
		private XmlNode _valueXml;

		// Token: 0x040031E3 RID: 12771
		private bool isModified;
	}
}
