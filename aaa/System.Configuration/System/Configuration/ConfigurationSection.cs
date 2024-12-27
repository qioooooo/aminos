using System;
using System.Globalization;
using System.IO;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x0200000D RID: 13
	public abstract class ConfigurationSection : ConfigurationElement
	{
		// Token: 0x0600005F RID: 95 RVA: 0x00005AE9 File Offset: 0x00004AE9
		protected ConfigurationSection()
		{
			this._section = new SectionInformation(this);
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000060 RID: 96 RVA: 0x00005AFD File Offset: 0x00004AFD
		public SectionInformation SectionInformation
		{
			get
			{
				return this._section;
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00005B05 File Offset: 0x00004B05
		protected internal virtual object GetRuntimeObject()
		{
			return this;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00005B08 File Offset: 0x00004B08
		protected internal override bool IsModified()
		{
			return this.SectionInformation.IsModifiedFlags() || base.IsModified();
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00005B1F File Offset: 0x00004B1F
		protected internal override void ResetModified()
		{
			this.SectionInformation.ResetModifiedFlags();
			base.ResetModified();
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00005B32 File Offset: 0x00004B32
		protected internal virtual void DeserializeSection(XmlReader reader)
		{
			if (!reader.Read() || reader.NodeType != XmlNodeType.Element)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_expected_to_find_element"), reader);
			}
			this.DeserializeElement(reader, false);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00005B60 File Offset: 0x00004B60
		protected internal virtual string SerializeSection(ConfigurationElement parentElement, string name, ConfigurationSaveMode saveMode)
		{
			ConfigurationElement.ValidateElement(this, null, true);
			ConfigurationElement configurationElement = base.CreateElement(base.GetType());
			configurationElement.Unmerge(this, parentElement, saveMode);
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlTextWriter.Indentation = 4;
			xmlTextWriter.IndentChar = ' ';
			configurationElement.DataToWriteInternal = saveMode != ConfigurationSaveMode.Minimal;
			configurationElement.SerializeToXmlElement(xmlTextWriter, name);
			xmlTextWriter.Flush();
			return stringWriter.ToString();
		}

		// Token: 0x0400015D RID: 349
		private SectionInformation _section;
	}
}
