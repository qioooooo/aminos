using System;
using System.Collections.Specialized;
using System.IO;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x0200000E RID: 14
	public sealed class AppSettingsSection : ConfigurationSection
	{
		// Token: 0x06000066 RID: 102 RVA: 0x00005BD8 File Offset: 0x00004BD8
		private static ConfigurationPropertyCollection EnsureStaticPropertyBag()
		{
			if (AppSettingsSection.s_properties == null)
			{
				AppSettingsSection.s_propAppSettings = new ConfigurationProperty(null, typeof(KeyValueConfigurationCollection), null, ConfigurationPropertyOptions.IsDefaultCollection);
				AppSettingsSection.s_propFile = new ConfigurationProperty("file", typeof(string), string.Empty, ConfigurationPropertyOptions.None);
				AppSettingsSection.s_properties = new ConfigurationPropertyCollection
				{
					AppSettingsSection.s_propAppSettings,
					AppSettingsSection.s_propFile
				};
			}
			return AppSettingsSection.s_properties;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00005C49 File Offset: 0x00004C49
		public AppSettingsSection()
		{
			AppSettingsSection.EnsureStaticPropertyBag();
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00005C57 File Offset: 0x00004C57
		protected internal override ConfigurationPropertyCollection Properties
		{
			get
			{
				return AppSettingsSection.EnsureStaticPropertyBag();
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00005C5E File Offset: 0x00004C5E
		protected internal override object GetRuntimeObject()
		{
			this.SetReadOnly();
			return this.InternalSettings;
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00005C6C File Offset: 0x00004C6C
		internal NameValueCollection InternalSettings
		{
			get
			{
				if (this._KeyValueCollection == null)
				{
					this._KeyValueCollection = new KeyValueInternalCollection(this);
				}
				return this._KeyValueCollection;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600006B RID: 107 RVA: 0x00005C88 File Offset: 0x00004C88
		[ConfigurationProperty("", IsDefaultCollection = true)]
		public KeyValueConfigurationCollection Settings
		{
			get
			{
				return (KeyValueConfigurationCollection)base[AppSettingsSection.s_propAppSettings];
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00005C9C File Offset: 0x00004C9C
		// (set) Token: 0x0600006D RID: 109 RVA: 0x00005CC4 File Offset: 0x00004CC4
		[ConfigurationProperty("file", DefaultValue = "")]
		public string File
		{
			get
			{
				string text = (string)base[AppSettingsSection.s_propFile];
				if (text == null)
				{
					return string.Empty;
				}
				return text;
			}
			set
			{
				base[AppSettingsSection.s_propFile] = value;
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00005CD2 File Offset: 0x00004CD2
		protected internal override void Reset(ConfigurationElement parentSection)
		{
			this._KeyValueCollection = null;
			base.Reset(parentSection);
			if (!string.IsNullOrEmpty((string)base[AppSettingsSection.s_propFile]))
			{
				base.SetPropertyValue(AppSettingsSection.s_propFile, null, true);
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00005D06 File Offset: 0x00004D06
		protected internal override bool IsModified()
		{
			return base.IsModified();
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00005D0E File Offset: 0x00004D0E
		protected internal override string SerializeSection(ConfigurationElement parentElement, string name, ConfigurationSaveMode saveMode)
		{
			return base.SerializeSection(parentElement, name, saveMode);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00005D1C File Offset: 0x00004D1C
		protected internal override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
		{
			string name = reader.Name;
			base.DeserializeElement(reader, serializeCollectionKey);
			if (this.File != null && this.File.Length > 0)
			{
				string source = base.ElementInformation.Source;
				string text;
				if (string.IsNullOrEmpty(source))
				{
					text = this.File;
				}
				else
				{
					string directoryName = Path.GetDirectoryName(source);
					text = Path.Combine(directoryName, this.File);
				}
				if (global::System.IO.File.Exists(text))
				{
					int num = 0;
					string text2 = null;
					using (Stream stream = new FileStream(text, FileMode.Open, FileAccess.Read, FileShare.Read))
					{
						using (XmlUtil xmlUtil = new XmlUtil(stream, text, true))
						{
							if (xmlUtil.Reader.Name != name)
							{
								throw new ConfigurationErrorsException(SR.GetString("Config_name_value_file_section_file_invalid_root", new object[] { name }), xmlUtil);
							}
							num = xmlUtil.Reader.LineNumber;
							text2 = xmlUtil.CopySection();
							while (!xmlUtil.Reader.EOF)
							{
								XmlNodeType nodeType = xmlUtil.Reader.NodeType;
								if (nodeType != XmlNodeType.Comment)
								{
									throw new ConfigurationErrorsException(SR.GetString("Config_source_file_format"), xmlUtil);
								}
								xmlUtil.Reader.Read();
							}
						}
					}
					ConfigXmlReader configXmlReader = new ConfigXmlReader(text2, text, num);
					configXmlReader.Read();
					if (configXmlReader.MoveToNextAttribute())
					{
						throw new ConfigurationErrorsException(SR.GetString("Config_base_unrecognized_attribute", new object[] { configXmlReader.Name }), configXmlReader);
					}
					configXmlReader.MoveToElement();
					base.DeserializeElement(configXmlReader, serializeCollectionKey);
				}
			}
		}

		// Token: 0x0400015E RID: 350
		private static ConfigurationPropertyCollection s_properties;

		// Token: 0x0400015F RID: 351
		private static ConfigurationProperty s_propAppSettings;

		// Token: 0x04000160 RID: 352
		private static ConfigurationProperty s_propFile;

		// Token: 0x04000161 RID: 353
		private KeyValueInternalCollection _KeyValueCollection;
	}
}
