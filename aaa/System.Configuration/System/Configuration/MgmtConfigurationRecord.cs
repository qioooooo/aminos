using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Internal;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Xml;

namespace System.Configuration
{
	// Token: 0x0200007B RID: 123
	internal sealed class MgmtConfigurationRecord : BaseConfigurationRecord
	{
		// Token: 0x06000468 RID: 1128 RVA: 0x00014620 File Offset: 0x00013620
		internal static MgmtConfigurationRecord Create(IInternalConfigRoot configRoot, IInternalConfigRecord parent, string configPath, string locationSubPath)
		{
			MgmtConfigurationRecord mgmtConfigurationRecord = new MgmtConfigurationRecord();
			mgmtConfigurationRecord.Init(configRoot, parent, configPath, locationSubPath);
			return mgmtConfigurationRecord;
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0001463E File Offset: 0x0001363E
		private MgmtConfigurationRecord()
		{
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00014648 File Offset: 0x00013648
		private void Init(IInternalConfigRoot configRoot, IInternalConfigRecord parent, string configPath, string locationSubPath)
		{
			base.Init(configRoot, (BaseConfigurationRecord)parent, configPath, locationSubPath);
			if (base.IsLocationConfig && (this.MgmtParent._locationTags == null || !this.MgmtParent._locationTags.Contains(this._locationSubPath)))
			{
				this._flags[16777216] = true;
			}
			this.InitStreamInfoUpdates();
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x000146AC File Offset: 0x000136AC
		private void InitStreamInfoUpdates()
		{
			this._streamInfoUpdates = new HybridDictionary(true);
			if (base.ConfigStreamInfo.HasStreamInfos)
			{
				foreach (object obj in base.ConfigStreamInfo.StreamInfos.Values)
				{
					StreamInfo streamInfo = (StreamInfo)obj;
					this._streamInfoUpdates.Add(streamInfo.StreamName, streamInfo.Clone());
				}
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600046C RID: 1132 RVA: 0x00014738 File Offset: 0x00013738
		private MgmtConfigurationRecord MgmtParent
		{
			get
			{
				return (MgmtConfigurationRecord)this._parent;
			}
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x0600046D RID: 1133 RVA: 0x00014745 File Offset: 0x00013745
		private UpdateConfigHost UpdateConfigHost
		{
			get
			{
				return (UpdateConfigHost)base.Host;
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600046E RID: 1134 RVA: 0x00014752 File Offset: 0x00013752
		protected override SimpleBitVector32 ClassFlags
		{
			get
			{
				return MgmtConfigurationRecord.MgmtClassFlags;
			}
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0001475C File Offset: 0x0001375C
		protected override object CreateSectionFactory(FactoryRecord factoryRecord)
		{
			Type type = TypeUtil.GetTypeWithReflectionPermission(base.Host, factoryRecord.FactoryTypeName, true);
			if (!typeof(ConfigurationSection).IsAssignableFrom(type))
			{
				TypeUtil.VerifyAssignableType(typeof(IConfigurationSectionHandler), type, true);
				type = typeof(DefaultSection);
			}
			return TypeUtil.GetConstructorWithReflectionPermission(type, typeof(ConfigurationSection), true);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x000147C0 File Offset: 0x000137C0
		protected override object CreateSection(bool inputIsTrusted, FactoryRecord factoryRecord, SectionRecord sectionRecord, object parentConfig, ConfigXmlReader reader)
		{
			ConstructorInfo constructorInfo = (ConstructorInfo)factoryRecord.Factory;
			ConfigurationSection configurationSection = (ConfigurationSection)TypeUtil.InvokeCtorWithReflectionPermission(constructorInfo);
			configurationSection.SectionInformation.AttachToConfigurationRecord(this, factoryRecord, sectionRecord);
			configurationSection.CallInit();
			ConfigurationSection configurationSection2 = (ConfigurationSection)parentConfig;
			configurationSection.Reset(configurationSection2);
			if (reader != null)
			{
				configurationSection.DeserializeSection(reader);
			}
			configurationSection.ResetModified();
			return configurationSection;
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0001481C File Offset: 0x0001381C
		private ConstructorInfo CreateSectionGroupFactory(FactoryRecord factoryRecord)
		{
			Type type;
			if (string.IsNullOrEmpty(factoryRecord.FactoryTypeName))
			{
				type = typeof(ConfigurationSectionGroup);
			}
			else
			{
				type = TypeUtil.GetTypeWithReflectionPermission(base.Host, factoryRecord.FactoryTypeName, true);
			}
			return TypeUtil.GetConstructorWithReflectionPermission(type, typeof(ConfigurationSectionGroup), true);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0001486C File Offset: 0x0001386C
		private ConstructorInfo EnsureSectionGroupFactory(FactoryRecord factoryRecord)
		{
			ConstructorInfo constructorInfo = (ConstructorInfo)factoryRecord.Factory;
			if (constructorInfo == null)
			{
				constructorInfo = this.CreateSectionGroupFactory(factoryRecord);
				factoryRecord.Factory = constructorInfo;
			}
			return constructorInfo;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00014898 File Offset: 0x00013898
		protected override object UseParentResult(string configKey, object parentResult, SectionRecord sectionRecord)
		{
			FactoryRecord factoryRecord = base.FindFactoryRecord(configKey, false);
			if (factoryRecord == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_unrecognized_configuration_section", new object[] { configKey }));
			}
			return base.CallCreateSection(false, factoryRecord, sectionRecord, parentResult, null, null, -1);
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x000148DC File Offset: 0x000138DC
		protected override object GetRuntimeObject(object result)
		{
			return result;
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x000148E0 File Offset: 0x000138E0
		private ConfigurationSection GetConfigSection(string configKey)
		{
			SectionRecord sectionRecord = base.GetSectionRecord(configKey, false);
			if (sectionRecord != null && sectionRecord.HasResult)
			{
				return (ConfigurationSection)sectionRecord.Result;
			}
			return null;
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000476 RID: 1142 RVA: 0x0001490E File Offset: 0x0001390E
		private Hashtable SectionGroups
		{
			get
			{
				if (this._sectionGroups == null)
				{
					this._sectionGroups = new Hashtable();
				}
				return this._sectionGroups;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x00014929 File Offset: 0x00013929
		private Hashtable RemovedSections
		{
			get
			{
				if (this._removedSections == null)
				{
					this._removedSections = new Hashtable();
				}
				return this._removedSections;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x00014944 File Offset: 0x00013944
		private Hashtable RemovedSectionGroups
		{
			get
			{
				if (this._removedSectionGroups == null)
				{
					this._removedSectionGroups = new Hashtable();
				}
				return this._removedSectionGroups;
			}
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00014960 File Offset: 0x00013960
		internal ConfigurationSectionGroup LookupSectionGroup(string configKey)
		{
			ConfigurationSectionGroup configurationSectionGroup = null;
			if (this._sectionGroups != null)
			{
				configurationSectionGroup = (ConfigurationSectionGroup)this._sectionGroups[configKey];
			}
			return configurationSectionGroup;
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0001498C File Offset: 0x0001398C
		internal ConfigurationSectionGroup GetSectionGroup(string configKey)
		{
			ConfigurationSectionGroup configurationSectionGroup = this.LookupSectionGroup(configKey);
			if (configurationSectionGroup == null)
			{
				BaseConfigurationRecord baseConfigurationRecord;
				FactoryRecord factoryRecord = base.FindFactoryRecord(configKey, false, out baseConfigurationRecord);
				if (factoryRecord == null)
				{
					return null;
				}
				if (!factoryRecord.IsGroup)
				{
					throw ExceptionUtil.ParameterInvalid("sectionGroupName");
				}
				if (factoryRecord.FactoryTypeName == null)
				{
					configurationSectionGroup = new ConfigurationSectionGroup();
				}
				else
				{
					ConstructorInfo constructorInfo = this.EnsureSectionGroupFactory(factoryRecord);
					try
					{
						configurationSectionGroup = (ConfigurationSectionGroup)TypeUtil.InvokeCtorWithReflectionPermission(constructorInfo);
					}
					catch (Exception ex)
					{
						throw new ConfigurationErrorsException(SR.GetString("Config_exception_creating_section_handler", new object[] { factoryRecord.ConfigKey }), ex, factoryRecord);
					}
					catch
					{
						throw new ConfigurationErrorsException(SR.GetString("Config_exception_creating_section_handler", new object[] { factoryRecord.ConfigKey }), factoryRecord);
					}
				}
				configurationSectionGroup.AttachToConfigurationRecord(this, factoryRecord);
				this.SectionGroups[configKey] = configurationSectionGroup;
			}
			return configurationSectionGroup;
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00014A74 File Offset: 0x00013A74
		internal ConfigurationLocationCollection GetLocationCollection(Configuration config)
		{
			ArrayList arrayList = new ArrayList();
			if (this._locationTags != null)
			{
				foreach (object obj in this._locationTags.Values)
				{
					string text = (string)obj;
					arrayList.Add(new ConfigurationLocation(config, text));
				}
			}
			return new ConfigurationLocationCollection(arrayList);
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x00014AF0 File Offset: 0x00013AF0
		protected override void AddLocation(string locationSubPath)
		{
			if (this._locationTags == null)
			{
				this._locationTags = new Hashtable(StringComparer.OrdinalIgnoreCase);
			}
			this._locationTags[locationSubPath] = locationSubPath;
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x0600047D RID: 1149 RVA: 0x00014B17 File Offset: 0x00013B17
		internal Hashtable SectionFactories
		{
			get
			{
				if (this._sectionFactories == null)
				{
					this._sectionFactories = this.GetAllFactories(false);
				}
				return this._sectionFactories;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x0600047E RID: 1150 RVA: 0x00014B34 File Offset: 0x00013B34
		internal Hashtable SectionGroupFactories
		{
			get
			{
				if (this._sectionGroupFactories == null)
				{
					this._sectionGroupFactories = this.GetAllFactories(true);
				}
				return this._sectionGroupFactories;
			}
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00014B54 File Offset: 0x00013B54
		private Hashtable GetAllFactories(bool isGroup)
		{
			Hashtable hashtable = new Hashtable();
			MgmtConfigurationRecord mgmtConfigurationRecord = this;
			do
			{
				if (mgmtConfigurationRecord._factoryRecords != null)
				{
					foreach (object obj in mgmtConfigurationRecord._factoryRecords.Values)
					{
						FactoryRecord factoryRecord = (FactoryRecord)obj;
						if (factoryRecord.IsGroup == isGroup)
						{
							string configKey = factoryRecord.ConfigKey;
							hashtable[configKey] = new FactoryId(factoryRecord.ConfigKey, factoryRecord.Group, factoryRecord.Name);
						}
					}
				}
				mgmtConfigurationRecord = mgmtConfigurationRecord.MgmtParent;
			}
			while (!mgmtConfigurationRecord.IsRootConfig);
			return hashtable;
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00014C04 File Offset: 0x00013C04
		internal ConfigurationSection FindImmediateParentSection(ConfigurationSection section)
		{
			ConfigurationSection configurationSection = null;
			string sectionName = section.SectionInformation.SectionName;
			SectionRecord sectionRecord = base.GetSectionRecord(sectionName, false);
			if (sectionRecord.HasLocationInputs)
			{
				SectionInput lastLocationInput = sectionRecord.LastLocationInput;
				configurationSection = (ConfigurationSection)lastLocationInput.Result;
			}
			else if (sectionRecord.HasIndirectLocationInputs)
			{
				SectionInput lastIndirectLocationInput = sectionRecord.LastIndirectLocationInput;
				configurationSection = (ConfigurationSection)lastIndirectLocationInput.Result;
			}
			else if (base.IsRootDeclaration(sectionName, true))
			{
				FactoryRecord factoryRecord = base.GetFactoryRecord(sectionName, false);
				object obj;
				object obj2;
				base.CreateSectionDefault(sectionName, false, factoryRecord, null, out obj, out obj2);
				configurationSection = (ConfigurationSection)obj;
			}
			else
			{
				MgmtConfigurationRecord mgmtConfigurationRecord = this.MgmtParent;
				while (!mgmtConfigurationRecord.IsRootConfig)
				{
					sectionRecord = mgmtConfigurationRecord.GetSectionRecord(sectionName, false);
					if (sectionRecord != null && sectionRecord.HasResult)
					{
						configurationSection = (ConfigurationSection)sectionRecord.Result;
						break;
					}
					mgmtConfigurationRecord = mgmtConfigurationRecord.MgmtParent;
				}
			}
			if (!configurationSection.IsReadOnly())
			{
				configurationSection.SetReadOnly();
			}
			return configurationSection;
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00014CE4 File Offset: 0x00013CE4
		internal ConfigurationSection FindAndCloneImmediateParentSection(ConfigurationSection configSection)
		{
			string configKey = configSection.SectionInformation.ConfigKey;
			ConfigurationSection configurationSection = this.FindImmediateParentSection(configSection);
			SectionRecord sectionRecord = base.GetSectionRecord(configKey, false);
			return (ConfigurationSection)this.UseParentResult(configKey, configurationSection, sectionRecord);
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00014D20 File Offset: 0x00013D20
		internal void RevertToParent(ConfigurationSection configSection)
		{
			configSection.SectionInformation.RawXml = null;
			try
			{
				ConfigurationSection configurationSection = this.FindImmediateParentSection(configSection);
				configSection.Reset(configurationSection);
				configSection.ResetModified();
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_exception_in_config_section_handler", new object[] { configSection.SectionInformation.SectionName }), ex, base.ConfigStreamInfo.StreamName, 0);
			}
			catch
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_exception_in_config_section_handler", new object[] { configSection.SectionInformation.SectionName }), null, base.ConfigStreamInfo.StreamName, 0);
			}
			configSection.SectionInformation.Removed = true;
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00014DE0 File Offset: 0x00013DE0
		internal string GetRawXml(string configKey)
		{
			SectionRecord sectionRecord = base.GetSectionRecord(configKey, false);
			if (sectionRecord == null || !sectionRecord.HasFileInput)
			{
				return null;
			}
			string[] array = configKey.Split(BaseConfigurationRecord.ConfigPathSeparatorParams);
			ConfigXmlReader sectionXmlReader = base.GetSectionXmlReader(array, sectionRecord.FileInput);
			return sectionXmlReader.RawXml;
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00014E24 File Offset: 0x00013E24
		internal void SetRawXml(ConfigurationSection configSection, string xmlElement)
		{
			if (string.IsNullOrEmpty(xmlElement))
			{
				this.RevertToParent(configSection);
				return;
			}
			this.ValidateSectionXml(xmlElement, configSection.SectionInformation.Name);
			ConfigurationSection configurationSection = this.FindImmediateParentSection(configSection);
			ConfigXmlReader configXmlReader = new ConfigXmlReader(xmlElement, null, 0);
			configSection.SectionInformation.RawXml = xmlElement;
			try
			{
				try
				{
					bool elementPresent = configSection.ElementPresent;
					PropertySourceInfo propertySourceInfo = configSection.ElementInformation.PropertyInfoInternal();
					configSection.Reset(configurationSection);
					configSection.DeserializeSection(configXmlReader);
					configSection.ResetModified();
					configSection.ElementPresent = elementPresent;
					configSection.ElementInformation.ChangeSourceAndLineNumber(propertySourceInfo);
				}
				catch
				{
					configSection.SectionInformation.RawXml = null;
					throw;
				}
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_exception_in_config_section_handler", new object[] { configSection.SectionInformation.SectionName }), ex, null, 0);
			}
			catch
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_exception_in_config_section_handler", new object[] { configSection.SectionInformation.SectionName }), null, null, 0);
			}
			configSection.SectionInformation.Removed = false;
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x00014F50 File Offset: 0x00013F50
		private bool IsStreamUsed(string oldStreamName)
		{
			MgmtConfigurationRecord mgmtConfigurationRecord = this;
			if (base.IsLocationConfig)
			{
				mgmtConfigurationRecord = this.MgmtParent;
				if (mgmtConfigurationRecord._sectionRecords != null)
				{
					foreach (object obj in mgmtConfigurationRecord._sectionRecords.Values)
					{
						SectionRecord sectionRecord = (SectionRecord)obj;
						if (sectionRecord.HasFileInput && StringUtil.EqualsIgnoreCase(sectionRecord.FileInput.SectionXmlInfo.ConfigSourceStreamName, oldStreamName))
						{
							return true;
						}
					}
				}
			}
			if (mgmtConfigurationRecord._locationSections != null)
			{
				foreach (object obj2 in mgmtConfigurationRecord._locationSections)
				{
					LocationSectionRecord locationSectionRecord = (LocationSectionRecord)obj2;
					if (StringUtil.EqualsIgnoreCase(locationSectionRecord.SectionXmlInfo.ConfigSourceStreamName, oldStreamName))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00015058 File Offset: 0x00014058
		internal void ChangeConfigSource(SectionInformation sectionInformation, string oldConfigSource, string oldConfigSourceStreamName, string newConfigSource)
		{
			if (string.IsNullOrEmpty(oldConfigSource))
			{
				oldConfigSource = null;
			}
			if (string.IsNullOrEmpty(newConfigSource))
			{
				newConfigSource = null;
			}
			if (StringUtil.EqualsIgnoreCase(oldConfigSource, newConfigSource))
			{
				return;
			}
			if (string.IsNullOrEmpty(base.ConfigStreamInfo.StreamName))
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_source_requires_file"));
			}
			string text = null;
			if (newConfigSource != null)
			{
				text = base.Host.GetStreamNameForConfigSource(base.ConfigStreamInfo.StreamName, newConfigSource);
			}
			if (text != null)
			{
				base.ValidateUniqueChildConfigSource(sectionInformation.ConfigKey, text, newConfigSource, null);
				StreamInfo streamInfo = (StreamInfo)this._streamInfoUpdates[text];
				if (streamInfo != null)
				{
					if (streamInfo.SectionName != sectionInformation.ConfigKey)
					{
						throw new ConfigurationErrorsException(SR.GetString("Config_source_cannot_be_shared", new object[] { newConfigSource }));
					}
				}
				else
				{
					streamInfo = new StreamInfo(sectionInformation.ConfigKey, newConfigSource, text);
					this._streamInfoUpdates.Add(text, streamInfo);
				}
			}
			if (oldConfigSourceStreamName != null && !this.IsStreamUsed(oldConfigSourceStreamName))
			{
				this._streamInfoUpdates.Remove(oldConfigSourceStreamName);
			}
			sectionInformation.ConfigSourceStreamName = text;
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0001515C File Offset: 0x0001415C
		private void ValidateSectionXml(string xmlElement, string configKey)
		{
			if (string.IsNullOrEmpty(xmlElement))
			{
				return;
			}
			XmlTextReader xmlTextReader = null;
			try
			{
				XmlParserContext xmlParserContext = new XmlParserContext(null, null, null, XmlSpace.Default, Encoding.Unicode);
				xmlTextReader = new XmlTextReader(xmlElement, XmlNodeType.Element, xmlParserContext);
				xmlTextReader.Read();
				if (xmlTextReader.NodeType != XmlNodeType.Element)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_unexpected_node_type", new object[] { xmlTextReader.NodeType }));
				}
				string text;
				string text2;
				BaseConfigurationRecord.SplitConfigKey(configKey, out text, out text2);
				if (xmlTextReader.Name != text2)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_unexpected_element_name", new object[] { xmlTextReader.Name }));
				}
				while (xmlTextReader.Read())
				{
					XmlNodeType nodeType = xmlTextReader.NodeType;
					if (nodeType == XmlNodeType.DocumentType || nodeType == XmlNodeType.XmlDeclaration)
					{
						throw new ConfigurationErrorsException(SR.GetString("Config_invalid_node_type"), xmlTextReader);
					}
					if (xmlTextReader.Depth <= 0 && xmlTextReader.NodeType != XmlNodeType.EndElement)
					{
						throw new ConfigurationErrorsException(SR.GetString("Config_more_data_than_expected"), xmlTextReader);
					}
				}
				if (xmlTextReader.Depth != 0)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_unexpected_element_end"), xmlTextReader);
				}
			}
			finally
			{
				if (xmlTextReader != null)
				{
					xmlTextReader.Close();
				}
			}
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00015288 File Offset: 0x00014288
		internal void AddConfigurationSection(string group, string name, ConfigurationSection configSection)
		{
			if (base.IsLocationConfig)
			{
				throw new InvalidOperationException(SR.GetString("Config_add_configurationsection_in_location_config"));
			}
			BaseConfigurationRecord.VerifySectionName(name, null, false);
			if (configSection == null)
			{
				throw new ArgumentNullException("configSection");
			}
			if (configSection.SectionInformation.Attached)
			{
				throw new InvalidOperationException(SR.GetString("Config_add_configurationsection_already_added"));
			}
			string text = BaseConfigurationRecord.CombineConfigKey(group, name);
			FactoryRecord factoryRecord = base.FindFactoryRecord(text, true);
			if (factoryRecord != null)
			{
				throw new ArgumentException(SR.GetString("Config_add_configurationsection_already_exists"));
			}
			if (!string.IsNullOrEmpty(configSection.SectionInformation.ConfigSource))
			{
				this.ChangeConfigSource(configSection.SectionInformation, null, null, configSection.SectionInformation.ConfigSource);
			}
			if (this._sectionFactories != null)
			{
				this._sectionFactories.Add(text, new FactoryId(text, group, name));
			}
			string text2 = configSection.SectionInformation.Type;
			if (text2 == null)
			{
				text2 = base.Host.GetConfigTypeName(configSection.GetType());
			}
			factoryRecord = new FactoryRecord(text, group, name, text2, configSection.SectionInformation.AllowLocation, configSection.SectionInformation.AllowDefinition, configSection.SectionInformation.AllowExeDefinition, configSection.SectionInformation.OverrideModeDefaultSetting, configSection.SectionInformation.RestartOnExternalChanges, configSection.SectionInformation.RequirePermission, this._flags[8192], false, base.ConfigStreamInfo.StreamName, -1);
			factoryRecord.Factory = TypeUtil.GetConstructorWithReflectionPermission(configSection.GetType(), typeof(ConfigurationSection), true);
			factoryRecord.IsFactoryTrustedWithoutAptca = TypeUtil.IsTypeFromTrustedAssemblyWithoutAptca(configSection.GetType());
			base.EnsureFactories()[text] = factoryRecord;
			SectionRecord sectionRecord = base.EnsureSectionRecordUnsafe(text, false);
			sectionRecord.Result = configSection;
			sectionRecord.ResultRuntimeObject = configSection;
			if (this._removedSections != null)
			{
				this._removedSections.Remove(text);
			}
			configSection.SectionInformation.AttachToConfigurationRecord(this, factoryRecord, sectionRecord);
			string rawXml = configSection.SectionInformation.RawXml;
			if (!string.IsNullOrEmpty(rawXml))
			{
				configSection.SectionInformation.RawXml = null;
				configSection.SectionInformation.SetRawXml(rawXml);
			}
		}

		// Token: 0x06000489 RID: 1161 RVA: 0x00015478 File Offset: 0x00014478
		internal void RemoveConfigurationSection(string group, string name)
		{
			bool flag = false;
			BaseConfigurationRecord.VerifySectionName(name, null, true);
			string text = BaseConfigurationRecord.CombineConfigKey(group, name);
			if (this.RemovedSections.Contains(text))
			{
				return;
			}
			if (base.FindFactoryRecord(text, true) == null)
			{
				return;
			}
			ConfigurationSection configSection = this.GetConfigSection(text);
			if (configSection != null)
			{
				configSection.SectionInformation.DetachFromConfigurationRecord();
			}
			bool flag2 = base.IsRootDeclaration(text, false);
			if (this._sectionFactories != null && flag2)
			{
				this._sectionFactories.Remove(text);
			}
			if (!base.IsLocationConfig && this._factoryRecords != null && this._factoryRecords.Contains(text))
			{
				flag = true;
				this._factoryRecords.Remove(text);
			}
			if (this._sectionRecords != null && this._sectionRecords.Contains(text))
			{
				flag = true;
				this._sectionRecords.Remove(text);
			}
			if (this._locationSections != null)
			{
				int i = 0;
				while (i < this._locationSections.Count)
				{
					LocationSectionRecord locationSectionRecord = (LocationSectionRecord)this._locationSections[i];
					if (locationSectionRecord.ConfigKey != text)
					{
						i++;
					}
					else
					{
						flag = true;
						this._locationSections.RemoveAt(i);
					}
				}
			}
			if (flag)
			{
				this.RemovedSections.Add(text, text);
			}
			List<string> list = new List<string>();
			foreach (object obj in this._streamInfoUpdates.Values)
			{
				StreamInfo streamInfo = (StreamInfo)obj;
				if (streamInfo.SectionName == text)
				{
					list.Add(streamInfo.StreamName);
				}
			}
			foreach (string text2 in list)
			{
				this._streamInfoUpdates.Remove(text2);
			}
		}

		// Token: 0x0600048A RID: 1162 RVA: 0x0001565C File Offset: 0x0001465C
		internal void AddConfigurationSectionGroup(string group, string name, ConfigurationSectionGroup configSectionGroup)
		{
			if (base.IsLocationConfig)
			{
				throw new InvalidOperationException(SR.GetString("Config_add_configurationsectiongroup_in_location_config"));
			}
			BaseConfigurationRecord.VerifySectionName(name, null, false);
			if (configSectionGroup == null)
			{
				throw ExceptionUtil.ParameterInvalid("name");
			}
			if (configSectionGroup.Attached)
			{
				throw new InvalidOperationException(SR.GetString("Config_add_configurationsectiongroup_already_added"));
			}
			string text = BaseConfigurationRecord.CombineConfigKey(group, name);
			FactoryRecord factoryRecord = base.FindFactoryRecord(text, true);
			if (factoryRecord != null)
			{
				throw new ArgumentException(SR.GetString("Config_add_configurationsectiongroup_already_exists"));
			}
			if (this._sectionGroupFactories != null)
			{
				this._sectionGroupFactories.Add(text, new FactoryId(text, group, name));
			}
			string text2 = configSectionGroup.Type;
			if (text2 == null)
			{
				text2 = base.Host.GetConfigTypeName(configSectionGroup.GetType());
			}
			factoryRecord = new FactoryRecord(text, group, name, text2, base.ConfigStreamInfo.StreamName, -1);
			base.EnsureFactories()[text] = factoryRecord;
			this.SectionGroups[text] = configSectionGroup;
			if (this._removedSectionGroups != null)
			{
				this._removedSectionGroups.Remove(text);
			}
			configSectionGroup.AttachToConfigurationRecord(this, factoryRecord);
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00015758 File Offset: 0x00014758
		private ArrayList GetDescendentSectionFactories(string configKey)
		{
			ArrayList arrayList = new ArrayList();
			string text;
			if (configKey.Length == 0)
			{
				text = string.Empty;
			}
			else
			{
				text = configKey + "/";
			}
			foreach (object obj in this.SectionFactories.Values)
			{
				FactoryId factoryId = (FactoryId)obj;
				if (factoryId.Group == configKey || StringUtil.StartsWith(factoryId.Group, text))
				{
					arrayList.Add(factoryId);
				}
			}
			return arrayList;
		}

		// Token: 0x0600048C RID: 1164 RVA: 0x000157FC File Offset: 0x000147FC
		private ArrayList GetDescendentSectionGroupFactories(string configKey)
		{
			ArrayList arrayList = new ArrayList();
			string text;
			if (configKey.Length == 0)
			{
				text = string.Empty;
			}
			else
			{
				text = configKey + "/";
			}
			foreach (object obj in this.SectionGroupFactories.Values)
			{
				FactoryId factoryId = (FactoryId)obj;
				if (factoryId.ConfigKey == configKey || StringUtil.StartsWith(factoryId.ConfigKey, text))
				{
					arrayList.Add(factoryId);
				}
			}
			return arrayList;
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x000158A0 File Offset: 0x000148A0
		internal void RemoveConfigurationSectionGroup(string group, string name)
		{
			BaseConfigurationRecord.VerifySectionName(name, null, false);
			string text = BaseConfigurationRecord.CombineConfigKey(group, name);
			if (base.FindFactoryRecord(text, true) == null)
			{
				return;
			}
			ArrayList descendentSectionFactories = this.GetDescendentSectionFactories(text);
			foreach (object obj in descendentSectionFactories)
			{
				FactoryId factoryId = (FactoryId)obj;
				this.RemoveConfigurationSection(factoryId.Group, factoryId.Name);
			}
			ArrayList descendentSectionGroupFactories = this.GetDescendentSectionGroupFactories(text);
			foreach (object obj2 in descendentSectionGroupFactories)
			{
				FactoryId factoryId2 = (FactoryId)obj2;
				if (!this.RemovedSectionGroups.Contains(factoryId2.ConfigKey))
				{
					ConfigurationSectionGroup configurationSectionGroup = this.LookupSectionGroup(factoryId2.ConfigKey);
					if (configurationSectionGroup != null)
					{
						configurationSectionGroup.DetachFromConfigurationRecord();
					}
					bool flag = base.IsRootDeclaration(factoryId2.ConfigKey, false);
					if (this._sectionGroupFactories != null && flag)
					{
						this._sectionGroupFactories.Remove(factoryId2.ConfigKey);
					}
					if (!base.IsLocationConfig && this._factoryRecords != null)
					{
						this._factoryRecords.Remove(factoryId2.ConfigKey);
					}
					if (this._sectionGroups != null)
					{
						this._sectionGroups.Remove(factoryId2.ConfigKey);
					}
					this.RemovedSectionGroups.Add(factoryId2.ConfigKey, factoryId2.ConfigKey);
				}
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x00015A30 File Offset: 0x00014A30
		internal string ConfigurationFilePath
		{
			get
			{
				string text = this.UpdateConfigHost.GetNewStreamname(base.ConfigStreamInfo.StreamName);
				if (text == null)
				{
					text = string.Empty;
				}
				if (!string.IsNullOrEmpty(text))
				{
					new FileIOPermission(FileIOPermissionAccess.PathDiscovery, text).Demand();
				}
				return text;
			}
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00015A74 File Offset: 0x00014A74
		internal void SaveAs(string filename, ConfigurationSaveMode saveMode, bool forceUpdateAll)
		{
			SectionUpdates configDeclarationUpdates = this.GetConfigDeclarationUpdates(saveMode, forceUpdateAll);
			bool flag = false;
			bool flag2 = filename != null;
			ConfigDefinitionUpdates configDefinitionUpdates;
			ArrayList arrayList;
			this.GetConfigDefinitionUpdates(flag2, saveMode, forceUpdateAll, out configDefinitionUpdates, out arrayList);
			if (filename != null)
			{
				if (!base.Host.IsRemote && this._streamInfoUpdates.Contains(filename))
				{
					throw new ArgumentException(SR.GetString("Filename_in_SaveAs_is_used_already", new object[] { filename }));
				}
				if (string.IsNullOrEmpty(base.ConfigStreamInfo.StreamName))
				{
					StreamInfo streamInfo = new StreamInfo(null, null, filename);
					this._streamInfoUpdates.Add(filename, streamInfo);
					base.ConfigStreamInfo.StreamName = filename;
					base.ConfigStreamInfo.StreamVersion = base.MonitorStream(null, null, base.ConfigStreamInfo.StreamName);
				}
				this.UpdateConfigHost.AddStreamname(base.ConfigStreamInfo.StreamName, filename, base.Host.IsRemote);
				foreach (object obj in this._streamInfoUpdates.Values)
				{
					StreamInfo streamInfo2 = (StreamInfo)obj;
					if (!string.IsNullOrEmpty(streamInfo2.SectionName))
					{
						string text = InternalConfigHost.StaticGetStreamNameForConfigSource(filename, streamInfo2.ConfigSource);
						this.UpdateConfigHost.AddStreamname(streamInfo2.StreamName, text, base.Host.IsRemote);
					}
				}
			}
			if (!flag2)
			{
				flag2 = this.RecordItselfRequiresUpdates;
			}
			if (configDeclarationUpdates != null || configDefinitionUpdates != null || flag2)
			{
				byte[] array = null;
				if (base.ConfigStreamInfo.HasStream)
				{
					using (Stream stream = base.Host.OpenStreamForRead(base.ConfigStreamInfo.StreamName))
					{
						if (stream == null)
						{
							throw new ConfigurationErrorsException(SR.GetString("Config_file_has_changed"), base.ConfigStreamInfo.StreamName, 0);
						}
						array = new byte[stream.Length];
						int num = stream.Read(array, 0, (int)stream.Length);
						if ((long)num != stream.Length)
						{
							throw new ConfigurationErrorsException(SR.GetString("Config_data_read_count_mismatch"));
						}
					}
				}
				string text2 = base.FindChangedConfigurationStream();
				if (text2 != null)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_file_has_changed"), text2, 0);
				}
				flag = true;
				object obj2 = null;
				bool flag3 = false;
				try
				{
					try
					{
						using (Stream stream2 = base.Host.OpenStreamForWrite(base.ConfigStreamInfo.StreamName, null, ref obj2))
						{
							flag3 = true;
							using (StreamWriter streamWriter = new StreamWriter(stream2))
							{
								XmlUtilWriter xmlUtilWriter = new XmlUtilWriter(streamWriter, true);
								if (base.ConfigStreamInfo.HasStream)
								{
									this.CopyConfig(configDeclarationUpdates, configDefinitionUpdates, array, base.ConfigStreamInfo.StreamName, this.NamespaceChangeNeeded, xmlUtilWriter);
								}
								else
								{
									this.CreateNewConfig(configDeclarationUpdates, configDefinitionUpdates, this.NamespaceChangeNeeded, xmlUtilWriter);
								}
							}
						}
					}
					catch
					{
						if (flag3)
						{
							base.Host.WriteCompleted(base.ConfigStreamInfo.StreamName, false, obj2);
						}
						throw;
					}
				}
				catch (Exception ex)
				{
					throw ExceptionUtil.WrapAsConfigException(SR.GetString("Config_error_loading_XML_file"), ex, base.ConfigStreamInfo.StreamName, 0);
				}
				catch
				{
					throw ExceptionUtil.WrapAsConfigException(SR.GetString("Config_error_loading_XML_file"), null, base.ConfigStreamInfo.StreamName, 0);
				}
				base.Host.WriteCompleted(base.ConfigStreamInfo.StreamName, true, obj2);
				base.ConfigStreamInfo.HasStream = true;
				base.ConfigStreamInfo.ClearStreamInfos();
				base.ConfigStreamInfo.StreamVersion = base.MonitorStream(null, null, base.ConfigStreamInfo.StreamName);
			}
			if (arrayList != null)
			{
				if (!flag)
				{
					string text3 = base.FindChangedConfigurationStream();
					if (text3 != null)
					{
						throw new ConfigurationErrorsException(SR.GetString("Config_file_has_changed"), text3, 0);
					}
				}
				foreach (object obj3 in arrayList)
				{
					DefinitionUpdate definitionUpdate = (DefinitionUpdate)obj3;
					this.SaveConfigSource(definitionUpdate);
				}
			}
			this.UpdateRecords();
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00015EB8 File Offset: 0x00014EB8
		private bool AreDeclarationAttributesModified(FactoryRecord factoryRecord, ConfigurationSection configSection)
		{
			return factoryRecord.FactoryTypeName != configSection.SectionInformation.Type || factoryRecord.AllowLocation != configSection.SectionInformation.AllowLocation || factoryRecord.RestartOnExternalChanges != configSection.SectionInformation.RestartOnExternalChanges || factoryRecord.RequirePermission != configSection.SectionInformation.RequirePermission || factoryRecord.AllowDefinition != configSection.SectionInformation.AllowDefinition || factoryRecord.AllowExeDefinition != configSection.SectionInformation.AllowExeDefinition || factoryRecord.OverrideModeDefault.OverrideMode != configSection.SectionInformation.OverrideModeDefaultSetting.OverrideMode || configSection.SectionInformation.IsModifiedFlags();
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x00015F6F File Offset: 0x00014F6F
		private void AppendAttribute(StringBuilder sb, string key, string value)
		{
			sb.Append(key);
			sb.Append("=\"");
			sb.Append(value);
			sb.Append("\" ");
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00015F9C File Offset: 0x00014F9C
		private string GetUpdatedSectionDeclarationXml(FactoryRecord factoryRecord, ConfigurationSection configSection, ConfigurationSaveMode saveMode)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('<');
			stringBuilder.Append("section");
			stringBuilder.Append(' ');
			this.AppendAttribute(stringBuilder, "name", configSection.SectionInformation.Name);
			this.AppendAttribute(stringBuilder, "type", (configSection.SectionInformation.Type != null) ? configSection.SectionInformation.Type : factoryRecord.FactoryTypeName);
			if (!configSection.SectionInformation.AllowLocation || saveMode == ConfigurationSaveMode.Full || (saveMode == ConfigurationSaveMode.Modified && configSection.SectionInformation.AllowLocationModified))
			{
				this.AppendAttribute(stringBuilder, "allowLocation", configSection.SectionInformation.AllowLocation ? "true" : "false");
			}
			if (configSection.SectionInformation.AllowDefinition != ConfigurationAllowDefinition.Everywhere || saveMode == ConfigurationSaveMode.Full || (saveMode == ConfigurationSaveMode.Modified && configSection.SectionInformation.AllowDefinitionModified))
			{
				string text = null;
				ConfigurationAllowDefinition allowDefinition = configSection.SectionInformation.AllowDefinition;
				if (allowDefinition <= ConfigurationAllowDefinition.MachineToWebRoot)
				{
					if (allowDefinition != ConfigurationAllowDefinition.MachineOnly)
					{
						if (allowDefinition == ConfigurationAllowDefinition.MachineToWebRoot)
						{
							text = "MachineToWebRoot";
						}
					}
					else
					{
						text = "MachineOnly";
					}
				}
				else if (allowDefinition != ConfigurationAllowDefinition.MachineToApplication)
				{
					if (allowDefinition == ConfigurationAllowDefinition.Everywhere)
					{
						text = "Everywhere";
					}
				}
				else
				{
					text = "MachineToApplication";
				}
				this.AppendAttribute(stringBuilder, "allowDefinition", text);
			}
			if (configSection.SectionInformation.AllowExeDefinition != ConfigurationAllowExeDefinition.MachineToApplication || saveMode == ConfigurationSaveMode.Full || (saveMode == ConfigurationSaveMode.Modified && configSection.SectionInformation.AllowExeDefinitionModified))
			{
				this.AppendAttribute(stringBuilder, "allowExeDefinition", this.ExeDefinitionToString(configSection.SectionInformation.AllowExeDefinition));
			}
			if (!configSection.SectionInformation.OverrideModeDefaultSetting.IsDefaultForSection || saveMode == ConfigurationSaveMode.Full || (saveMode == ConfigurationSaveMode.Modified && configSection.SectionInformation.OverrideModeDefaultModified))
			{
				this.AppendAttribute(stringBuilder, "overrideModeDefault", configSection.SectionInformation.OverrideModeDefaultSetting.OverrideModeXmlValue);
			}
			if (!configSection.SectionInformation.RestartOnExternalChanges)
			{
				this.AppendAttribute(stringBuilder, "restartOnExternalChanges", "false");
			}
			else if (saveMode == ConfigurationSaveMode.Full || (saveMode == ConfigurationSaveMode.Modified && configSection.SectionInformation.RestartOnExternalChangesModified))
			{
				this.AppendAttribute(stringBuilder, "restartOnExternalChanges", "true");
			}
			if (!configSection.SectionInformation.RequirePermission)
			{
				this.AppendAttribute(stringBuilder, "requirePermission", "false");
			}
			else if (saveMode == ConfigurationSaveMode.Full || (saveMode == ConfigurationSaveMode.Modified && configSection.SectionInformation.RequirePermissionModified))
			{
				this.AppendAttribute(stringBuilder, "requirePermission", "true");
			}
			stringBuilder.Append("/>");
			return stringBuilder.ToString();
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x000161FC File Offset: 0x000151FC
		private string ExeDefinitionToString(ConfigurationAllowExeDefinition allowDefinition)
		{
			if (allowDefinition <= ConfigurationAllowExeDefinition.MachineToApplication)
			{
				if (allowDefinition == ConfigurationAllowExeDefinition.MachineOnly)
				{
					return "MachineOnly";
				}
				if (allowDefinition == ConfigurationAllowExeDefinition.MachineToApplication)
				{
					return "MachineToApplication";
				}
			}
			else
			{
				if (allowDefinition == ConfigurationAllowExeDefinition.MachineToRoamingUser)
				{
					return "MachineToRoamingUser";
				}
				if (allowDefinition == ConfigurationAllowExeDefinition.MachineToLocalUser)
				{
					return "MachineToLocalUser";
				}
			}
			throw ExceptionUtil.PropertyInvalid("AllowExeDefinition");
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00016250 File Offset: 0x00015250
		private string GetUpdatedSectionGroupDeclarationXml(FactoryRecord factoryRecord, ConfigurationSectionGroup configSectionGroup)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('<');
			stringBuilder.Append("sectionGroup");
			stringBuilder.Append(' ');
			this.AppendAttribute(stringBuilder, "name", configSectionGroup.Name);
			this.AppendAttribute(stringBuilder, "type", (configSectionGroup.Type != null) ? configSectionGroup.Type : factoryRecord.FactoryTypeName);
			stringBuilder.Append('>');
			return stringBuilder.ToString();
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x000162C4 File Offset: 0x000152C4
		private bool HasRemovedSectionsOrGroups
		{
			get
			{
				return (this._removedSections != null && this._removedSections.Count > 0) || (this._removedSectionGroups != null && this._removedSectionGroups.Count > 0);
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x000162F6 File Offset: 0x000152F6
		private bool HasRemovedSections
		{
			get
			{
				return this._removedSections != null && this._removedSections.Count > 0;
			}
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x00016310 File Offset: 0x00015310
		private SectionUpdates GetConfigDeclarationUpdates(ConfigurationSaveMode saveMode, bool forceUpdateAll)
		{
			if (base.IsLocationConfig)
			{
				return null;
			}
			bool flag = this.HasRemovedSectionsOrGroups;
			SectionUpdates sectionUpdates = new SectionUpdates(string.Empty);
			if (this._factoryRecords != null)
			{
				foreach (object obj in this._factoryRecords.Values)
				{
					FactoryRecord factoryRecord = (FactoryRecord)obj;
					if (!factoryRecord.IsGroup)
					{
						string text = null;
						if (!factoryRecord.IsUndeclared)
						{
							ConfigurationSection configSection = this.GetConfigSection(factoryRecord.ConfigKey);
							if (configSection != null)
							{
								if (!configSection.SectionInformation.IsDeclared && !this.MgmtParent.IsRootConfig && this.MgmtParent.FindFactoryRecord(factoryRecord.ConfigKey, false) != null)
								{
									if (factoryRecord.HasFile)
									{
										flag = true;
										continue;
									}
									continue;
								}
								else if (this.AreDeclarationAttributesModified(factoryRecord, configSection) || !factoryRecord.HasFile)
								{
									flag = true;
									text = this.GetUpdatedSectionDeclarationXml(factoryRecord, configSection, saveMode);
								}
							}
							DeclarationUpdate declarationUpdate = new DeclarationUpdate(factoryRecord.ConfigKey, !factoryRecord.HasFile, text);
							sectionUpdates.AddSection(declarationUpdate);
						}
					}
					else
					{
						bool flag2 = false;
						ConfigurationSectionGroup configurationSectionGroup = this.LookupSectionGroup(factoryRecord.ConfigKey);
						if (!factoryRecord.HasFile)
						{
							flag2 = true;
						}
						else if (configurationSectionGroup != null && configurationSectionGroup.IsDeclarationRequired)
						{
							flag2 = true;
						}
						else if (factoryRecord.FactoryTypeName != null || configurationSectionGroup != null)
						{
							FactoryRecord factoryRecord2 = null;
							if (!this.MgmtParent.IsRootConfig)
							{
								factoryRecord2 = this.MgmtParent.FindFactoryRecord(factoryRecord.ConfigKey, false);
							}
							flag2 = factoryRecord2 == null || factoryRecord2.FactoryTypeName == null;
						}
						if (flag2)
						{
							string text2 = null;
							if (!factoryRecord.HasFile || (configurationSectionGroup != null && configurationSectionGroup.Type != factoryRecord.FactoryTypeName))
							{
								flag = true;
								text2 = this.GetUpdatedSectionGroupDeclarationXml(factoryRecord, configurationSectionGroup);
							}
							DeclarationUpdate declarationUpdate2 = new DeclarationUpdate(factoryRecord.ConfigKey, !factoryRecord.HasFile, text2);
							sectionUpdates.AddSectionGroup(declarationUpdate2);
						}
					}
				}
			}
			if (this._sectionRecords != null)
			{
				foreach (object obj2 in this._sectionRecords.Values)
				{
					SectionRecord sectionRecord = (SectionRecord)obj2;
					if (base.GetFactoryRecord(sectionRecord.ConfigKey, false) == null && sectionRecord.HasResult)
					{
						ConfigurationSection configurationSection = (ConfigurationSection)sectionRecord.Result;
						FactoryRecord factoryRecord3 = this.MgmtParent.FindFactoryRecord(sectionRecord.ConfigKey, false);
						if (configurationSection.SectionInformation.IsDeclared)
						{
							flag = true;
							string updatedSectionDeclarationXml = this.GetUpdatedSectionDeclarationXml(factoryRecord3, configurationSection, saveMode);
							DeclarationUpdate declarationUpdate3 = new DeclarationUpdate(factoryRecord3.ConfigKey, true, updatedSectionDeclarationXml);
							sectionUpdates.AddSection(declarationUpdate3);
						}
					}
				}
			}
			if (this._sectionGroups != null)
			{
				foreach (object obj3 in this._sectionGroups.Values)
				{
					ConfigurationSectionGroup configurationSectionGroup2 = (ConfigurationSectionGroup)obj3;
					if (base.GetFactoryRecord(configurationSectionGroup2.SectionGroupName, false) == null)
					{
						FactoryRecord factoryRecord4 = this.MgmtParent.FindFactoryRecord(configurationSectionGroup2.SectionGroupName, false);
						if (configurationSectionGroup2.IsDeclared || (factoryRecord4 != null && configurationSectionGroup2.Type != factoryRecord4.FactoryTypeName))
						{
							flag = true;
							string updatedSectionGroupDeclarationXml = this.GetUpdatedSectionGroupDeclarationXml(factoryRecord4, configurationSectionGroup2);
							DeclarationUpdate declarationUpdate4 = new DeclarationUpdate(factoryRecord4.ConfigKey, true, updatedSectionGroupDeclarationXml);
							sectionUpdates.AddSectionGroup(declarationUpdate4);
						}
					}
				}
			}
			if (flag)
			{
				return sectionUpdates;
			}
			return null;
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x000166D8 File Offset: 0x000156D8
		private bool AreLocationAttributesModified(SectionRecord sectionRecord, ConfigurationSection configSection)
		{
			OverrideModeSetting overrideModeSetting = OverrideModeSetting.LocationDefault;
			bool flag = true;
			if (sectionRecord.HasFileInput)
			{
				SectionXmlInfo sectionXmlInfo = sectionRecord.FileInput.SectionXmlInfo;
				overrideModeSetting = sectionXmlInfo.OverrideModeSetting;
				flag = !sectionXmlInfo.SkipInChildApps;
			}
			return !OverrideModeSetting.CanUseSameLocationTag(overrideModeSetting, configSection.SectionInformation.OverrideModeSetting) || flag != configSection.SectionInformation.InheritInChildApplications;
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00016738 File Offset: 0x00015738
		private bool AreSectionAttributesModified(SectionRecord sectionRecord, ConfigurationSection configSection)
		{
			string text;
			string text2;
			if (sectionRecord.HasFileInput)
			{
				SectionXmlInfo sectionXmlInfo = sectionRecord.FileInput.SectionXmlInfo;
				text = sectionXmlInfo.ConfigSource;
				text2 = sectionXmlInfo.ProtectionProviderName;
			}
			else
			{
				text = null;
				text2 = null;
			}
			return !StringUtil.EqualsNE(text, configSection.SectionInformation.ConfigSource) || !StringUtil.EqualsNE(text2, configSection.SectionInformation.ProtectionProviderName) || this.AreLocationAttributesModified(sectionRecord, configSection);
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0001679D File Offset: 0x0001579D
		private bool IsConfigSectionMoved(SectionRecord sectionRecord, ConfigurationSection configSection)
		{
			return !sectionRecord.HasFileInput || this.AreLocationAttributesModified(sectionRecord, configSection);
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x000167B4 File Offset: 0x000157B4
		private void GetConfigDefinitionUpdates(bool requireUpdates, ConfigurationSaveMode saveMode, bool forceSaveAll, out ConfigDefinitionUpdates definitionUpdates, out ArrayList configSourceUpdates)
		{
			definitionUpdates = new ConfigDefinitionUpdates();
			configSourceUpdates = null;
			bool flag = this.HasRemovedSections;
			if (this._sectionRecords != null)
			{
				base.InitProtectedConfigurationSection();
				foreach (object obj in this._sectionRecords)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					string text = (string)dictionaryEntry.Key;
					SectionRecord sectionRecord = (SectionRecord)dictionaryEntry.Value;
					sectionRecord.AddUpdate = false;
					bool flag2 = sectionRecord.HasFileInput;
					OverrideModeSetting overrideModeSetting = OverrideModeSetting.LocationDefault;
					bool flag3 = true;
					bool flag4 = false;
					string text2 = null;
					bool flag5 = false;
					if (!sectionRecord.HasResult)
					{
						if (sectionRecord.HasFileInput)
						{
							SectionXmlInfo sectionXmlInfo = sectionRecord.FileInput.SectionXmlInfo;
							overrideModeSetting = sectionXmlInfo.OverrideModeSetting;
							flag3 = !sectionXmlInfo.SkipInChildApps;
							flag5 = requireUpdates && !string.IsNullOrEmpty(sectionXmlInfo.ConfigSource);
						}
					}
					else
					{
						ConfigurationSection configurationSection = (ConfigurationSection)sectionRecord.Result;
						overrideModeSetting = configurationSection.SectionInformation.OverrideModeSetting;
						flag3 = configurationSection.SectionInformation.InheritInChildApplications;
						if (!configurationSection.SectionInformation.AllowLocation && (!overrideModeSetting.IsDefaultForLocationTag || !flag3))
						{
							throw new ConfigurationErrorsException(SR.GetString("Config_inconsistent_location_attributes", new object[] { text }));
						}
						flag5 = requireUpdates && !string.IsNullOrEmpty(configurationSection.SectionInformation.ConfigSource);
						try
						{
							bool flag6 = configurationSection.SectionInformation.ForceSave || configurationSection.IsModified() || (forceSaveAll && !configurationSection.SectionInformation.IsLocked);
							bool flag7 = this.AreSectionAttributesModified(sectionRecord, configurationSection);
							bool flag8 = flag6 || configurationSection.SectionInformation.RawXml != null;
							if (flag8 || flag7)
							{
								configurationSection.SectionInformation.VerifyIsEditable();
								configurationSection.SectionInformation.Removed = false;
								flag2 = true;
								flag4 = this.IsConfigSectionMoved(sectionRecord, configurationSection);
								if (!flag5)
								{
									flag5 = !string.IsNullOrEmpty(configurationSection.SectionInformation.ConfigSource) && (flag8 || configurationSection.SectionInformation.ConfigSourceModified);
								}
								if (flag6 || configurationSection.SectionInformation.RawXml == null || saveMode == ConfigurationSaveMode.Full)
								{
									ConfigurationSection configurationSection2 = this.FindImmediateParentSection(configurationSection);
									text2 = configurationSection.SerializeSection(configurationSection2, configurationSection.SectionInformation.Name, saveMode);
									this.ValidateSectionXml(text2, text);
								}
								else
								{
									text2 = configurationSection.SectionInformation.RawXml;
								}
								if (string.IsNullOrEmpty(text2) && (!string.IsNullOrEmpty(configurationSection.SectionInformation.ConfigSource) || !configurationSection.SectionInformation.LocationAttributesAreDefault || configurationSection.SectionInformation.ProtectionProvider != null))
								{
									text2 = this.WriteEmptyElement(configurationSection.SectionInformation.Name);
								}
								if (string.IsNullOrEmpty(text2))
								{
									configurationSection.SectionInformation.Removed = true;
									text2 = null;
									flag2 = false;
									if (sectionRecord.HasFileInput)
									{
										flag = true;
										sectionRecord.RemoveFileInput();
										goto IL_0412;
									}
									goto IL_0412;
								}
								else
								{
									if (flag7 || flag4 || string.IsNullOrEmpty(configurationSection.SectionInformation.ConfigSource))
									{
										flag = true;
									}
									if (configurationSection.SectionInformation.ProtectionProvider == null)
									{
										goto IL_0412;
									}
									ProtectedConfigurationSection protectedConfigurationSection = base.GetSection("configProtectedData") as ProtectedConfigurationSection;
									try
									{
										string text3 = base.Host.EncryptSection(text2, configurationSection.SectionInformation.ProtectionProvider, protectedConfigurationSection);
										text2 = ProtectedConfigurationSection.FormatEncryptedSection(text3, configurationSection.SectionInformation.Name, configurationSection.SectionInformation.ProtectionProvider.Name);
										goto IL_0412;
									}
									catch (Exception ex)
									{
										throw new ConfigurationErrorsException(SR.GetString("Encryption_failed", new object[]
										{
											configurationSection.SectionInformation.SectionName,
											configurationSection.SectionInformation.ProtectionProvider.Name,
											ex.Message
										}), ex);
									}
									catch
									{
										throw new ConfigurationErrorsException(SR.GetString("Encryption_failed", new object[]
										{
											configurationSection.SectionInformation.SectionName,
											configurationSection.SectionInformation.ProtectionProvider.Name,
											ExceptionUtil.NoExceptionInformation
										}));
									}
								}
							}
							if (configurationSection.SectionInformation.Removed)
							{
								flag2 = false;
								if (sectionRecord.HasFileInput)
								{
									flag = true;
								}
							}
							IL_0412:;
						}
						catch (Exception ex2)
						{
							throw new ConfigurationErrorsException(SR.GetString("Config_exception_in_config_section_handler", new object[] { configurationSection.SectionInformation.SectionName }), ex2);
						}
						catch
						{
							throw new ConfigurationErrorsException(SR.GetString("Config_exception_in_config_section_handler", new object[] { configurationSection.SectionInformation.SectionName }));
						}
					}
					if (flag2)
					{
						if (base.GetSectionLockedMode(sectionRecord.ConfigKey) == OverrideMode.Deny)
						{
							throw new ConfigurationErrorsException(SR.GetString("Config_section_locked"), null);
						}
						sectionRecord.AddUpdate = true;
						DefinitionUpdate definitionUpdate = definitionUpdates.AddUpdate(overrideModeSetting, flag3, flag4, text2, sectionRecord);
						if (flag5)
						{
							if (configSourceUpdates == null)
							{
								configSourceUpdates = new ArrayList();
							}
							configSourceUpdates.Add(definitionUpdate);
						}
					}
				}
			}
			if (this._flags[16777216])
			{
				flag = true;
				definitionUpdates.RequireLocation = true;
			}
			if (this._flags[33554432])
			{
				flag = true;
			}
			if (flag)
			{
				definitionUpdates.CompleteUpdates();
				return;
			}
			definitionUpdates = null;
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x00016D68 File Offset: 0x00015D68
		private string WriteEmptyElement(string ElementName)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('<');
			stringBuilder.Append(ElementName);
			stringBuilder.Append(" />");
			return stringBuilder.ToString();
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x00016DA0 File Offset: 0x00015DA0
		private void UpdateRecords()
		{
			if (this._factoryRecords != null)
			{
				foreach (object obj in this._factoryRecords.Values)
				{
					FactoryRecord factoryRecord = (FactoryRecord)obj;
					if (string.IsNullOrEmpty(factoryRecord.Filename))
					{
						factoryRecord.Filename = base.ConfigStreamInfo.StreamName;
					}
					factoryRecord.LineNumber = 0;
					ConfigurationSection configSection = this.GetConfigSection(factoryRecord.ConfigKey);
					if (configSection != null)
					{
						if (configSection.SectionInformation.Type != null)
						{
							factoryRecord.FactoryTypeName = configSection.SectionInformation.Type;
						}
						factoryRecord.AllowLocation = configSection.SectionInformation.AllowLocation;
						factoryRecord.RestartOnExternalChanges = configSection.SectionInformation.RestartOnExternalChanges;
						factoryRecord.RequirePermission = configSection.SectionInformation.RequirePermission;
						factoryRecord.AllowDefinition = configSection.SectionInformation.AllowDefinition;
						factoryRecord.AllowExeDefinition = configSection.SectionInformation.AllowExeDefinition;
					}
				}
			}
			if (this._sectionRecords != null)
			{
				string text = (base.IsLocationConfig ? this._parent.ConfigPath : base.ConfigPath);
				foreach (object obj2 in this._sectionRecords.Values)
				{
					SectionRecord sectionRecord = (SectionRecord)obj2;
					ConfigurationSection configurationSection;
					string text2;
					string text3;
					if (sectionRecord.HasResult)
					{
						configurationSection = (ConfigurationSection)sectionRecord.Result;
						text2 = configurationSection.SectionInformation.ConfigSource;
						if (string.IsNullOrEmpty(text2))
						{
							text2 = null;
						}
						text3 = configurationSection.SectionInformation.ConfigSourceStreamName;
						if (string.IsNullOrEmpty(text3))
						{
							text3 = null;
						}
					}
					else
					{
						configurationSection = null;
						text2 = null;
						text3 = null;
						if (sectionRecord.HasFileInput)
						{
							SectionXmlInfo sectionXmlInfo = sectionRecord.FileInput.SectionXmlInfo;
							text2 = sectionXmlInfo.ConfigSource;
							text3 = sectionXmlInfo.ConfigSourceStreamName;
						}
					}
					object obj3;
					if (!string.IsNullOrEmpty(text2))
					{
						obj3 = base.MonitorStream(sectionRecord.ConfigKey, text2, text3);
					}
					else
					{
						obj3 = null;
					}
					if (!sectionRecord.HasResult)
					{
						if (sectionRecord.HasFileInput)
						{
							SectionXmlInfo sectionXmlInfo2 = sectionRecord.FileInput.SectionXmlInfo;
							sectionXmlInfo2.StreamVersion = base.ConfigStreamInfo.StreamVersion;
							sectionXmlInfo2.ConfigSourceStreamVersion = obj3;
						}
					}
					else
					{
						configurationSection.SectionInformation.RawXml = null;
						bool addUpdate = sectionRecord.AddUpdate;
						sectionRecord.AddUpdate = false;
						if (addUpdate)
						{
							SectionInput sectionInput = sectionRecord.FileInput;
							if (sectionInput == null)
							{
								SectionXmlInfo sectionXmlInfo3 = new SectionXmlInfo(sectionRecord.ConfigKey, text, this._configPath, this._locationSubPath, base.ConfigStreamInfo.StreamName, 0, base.ConfigStreamInfo.StreamVersion, null, text2, text3, obj3, configurationSection.SectionInformation.ProtectionProviderName, configurationSection.SectionInformation.OverrideModeSetting, !configurationSection.SectionInformation.InheritInChildApplications);
								sectionInput = new SectionInput(sectionXmlInfo3, null);
								sectionInput.Result = configurationSection;
								sectionInput.ResultRuntimeObject = configurationSection;
								sectionRecord.AddFileInput(sectionInput);
							}
							else
							{
								SectionXmlInfo sectionXmlInfo4 = sectionInput.SectionXmlInfo;
								sectionXmlInfo4.LineNumber = 0;
								sectionXmlInfo4.StreamVersion = base.ConfigStreamInfo.StreamVersion;
								sectionXmlInfo4.RawXml = null;
								sectionXmlInfo4.ConfigSource = text2;
								sectionXmlInfo4.ConfigSourceStreamName = text3;
								sectionXmlInfo4.ConfigSourceStreamVersion = obj3;
								sectionXmlInfo4.ProtectionProviderName = configurationSection.SectionInformation.ProtectionProviderName;
								sectionXmlInfo4.OverrideModeSetting = configurationSection.SectionInformation.OverrideModeSetting;
								sectionXmlInfo4.SkipInChildApps = !configurationSection.SectionInformation.InheritInChildApplications;
							}
							sectionInput.ProtectionProvider = configurationSection.SectionInformation.ProtectionProvider;
						}
						try
						{
							configurationSection.ResetModified();
						}
						catch (Exception ex)
						{
							throw new ConfigurationErrorsException(SR.GetString("Config_exception_in_config_section_handler", new object[] { sectionRecord.ConfigKey }), ex, base.ConfigStreamInfo.StreamName, 0);
						}
						catch
						{
							throw new ConfigurationErrorsException(SR.GetString("Config_exception_in_config_section_handler", new object[] { sectionRecord.ConfigKey }), null, base.ConfigStreamInfo.StreamName, 0);
						}
					}
				}
			}
			foreach (object obj4 in this._streamInfoUpdates.Values)
			{
				StreamInfo streamInfo = (StreamInfo)obj4;
				if (!base.ConfigStreamInfo.StreamInfos.Contains(streamInfo.StreamName))
				{
					base.MonitorStream(streamInfo.SectionName, streamInfo.ConfigSource, streamInfo.StreamName);
				}
			}
			this.InitStreamInfoUpdates();
			this._flags[512] = this._flags[67108864];
			this._flags[16777216] = false;
			this._flags[33554432] = false;
			if (!base.IsLocationConfig && this._locationSections != null && this._removedSections != null && this._removedSections.Count > 0)
			{
				int i = 0;
				while (i < this._locationSections.Count)
				{
					LocationSectionRecord locationSectionRecord = (LocationSectionRecord)this._locationSections[i];
					if (this._removedSections.Contains(locationSectionRecord.ConfigKey))
					{
						this._locationSections.RemoveAt(i);
					}
					else
					{
						i++;
					}
				}
			}
			this._removedSections = null;
			this._removedSectionGroups = null;
		}

		// Token: 0x0600049E RID: 1182 RVA: 0x00017370 File Offset: 0x00016370
		private void CreateNewConfig(SectionUpdates declarationUpdates, ConfigDefinitionUpdates definitionUpdates, NamespaceChange namespaceChange, XmlUtilWriter utilWriter)
		{
			int num = 5;
			int num2 = 4;
			utilWriter.Write(string.Format(CultureInfo.InvariantCulture, "<?xml version=\"1.0\" encoding=\"{0}\"?>\r\n", new object[] { base.ConfigStreamInfo.StreamEncoding.WebName }));
			if (namespaceChange == NamespaceChange.Add)
			{
				utilWriter.Write(string.Format(CultureInfo.InvariantCulture, "<configuration xmlns=\"{0}\">\r\n", new object[] { "http://schemas.microsoft.com/.NetConfiguration/v2.0" }));
			}
			else
			{
				utilWriter.Write("<configuration>\r\n");
			}
			if (declarationUpdates != null)
			{
				this.WriteNewConfigDeclarations(declarationUpdates, utilWriter, num, num2, false);
			}
			this.WriteNewConfigDefinitions(definitionUpdates, utilWriter, num, num2);
			utilWriter.Write("</configuration>");
		}

		// Token: 0x0600049F RID: 1183 RVA: 0x00017414 File Offset: 0x00016414
		private void WriteNewConfigDeclarations(SectionUpdates declarationUpdates, XmlUtilWriter utilWriter, int linePosition, int indent, bool skipFirstIndent)
		{
			if (!skipFirstIndent)
			{
				utilWriter.AppendSpacesToLinePosition(linePosition);
			}
			utilWriter.Write("<configSections>\r\n");
			this.WriteUnwrittenConfigDeclarations(declarationUpdates, utilWriter, linePosition + indent, indent, false);
			utilWriter.AppendSpacesToLinePosition(linePosition);
			utilWriter.Write("</configSections>\r\n");
			if (skipFirstIndent)
			{
				utilWriter.AppendSpacesToLinePosition(linePosition);
			}
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00017468 File Offset: 0x00016468
		private void WriteUnwrittenConfigDeclarations(SectionUpdates declarationUpdates, XmlUtilWriter utilWriter, int linePosition, int indent, bool skipFirstIndent)
		{
			this.WriteUnwrittenConfigDeclarationsRecursive(declarationUpdates, utilWriter, linePosition, indent, skipFirstIndent);
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x00017478 File Offset: 0x00016478
		private void WriteUnwrittenConfigDeclarationsRecursive(SectionUpdates declarationUpdates, XmlUtilWriter utilWriter, int linePosition, int indent, bool skipFirstIndent)
		{
			string[] unretrievedSectionNames = declarationUpdates.GetUnretrievedSectionNames();
			if (unretrievedSectionNames != null)
			{
				foreach (string text in unretrievedSectionNames)
				{
					if (!skipFirstIndent)
					{
						utilWriter.AppendSpacesToLinePosition(linePosition);
					}
					skipFirstIndent = false;
					DeclarationUpdate declarationUpdate = declarationUpdates.GetDeclarationUpdate(text);
					utilWriter.Write(declarationUpdate.UpdatedXml);
					utilWriter.AppendNewLine();
				}
			}
			string[] unretrievedGroupNames = declarationUpdates.GetUnretrievedGroupNames();
			if (unretrievedGroupNames != null)
			{
				foreach (string text2 in unretrievedGroupNames)
				{
					if (!skipFirstIndent)
					{
						utilWriter.AppendSpacesToLinePosition(linePosition);
					}
					skipFirstIndent = false;
					SectionUpdates sectionUpdatesForGroup = declarationUpdates.GetSectionUpdatesForGroup(text2);
					DeclarationUpdate sectionGroupUpdate = sectionUpdatesForGroup.GetSectionGroupUpdate();
					if (sectionGroupUpdate == null)
					{
						utilWriter.Write("<sectionGroup name=\"" + text2 + "\">");
					}
					else
					{
						utilWriter.Write(sectionGroupUpdate.UpdatedXml);
					}
					utilWriter.AppendNewLine();
					this.WriteUnwrittenConfigDeclarationsRecursive(sectionUpdatesForGroup, utilWriter, linePosition + indent, indent, false);
					utilWriter.AppendSpacesToLinePosition(linePosition);
					utilWriter.Write("</sectionGroup>\r\n");
				}
			}
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00017580 File Offset: 0x00016580
		private void WriteNewConfigDefinitions(ConfigDefinitionUpdates configDefinitionUpdates, XmlUtilWriter utilWriter, int linePosition, int indent)
		{
			if (configDefinitionUpdates == null)
			{
				return;
			}
			foreach (object obj in configDefinitionUpdates.LocationUpdatesList)
			{
				LocationUpdates locationUpdates = (LocationUpdates)obj;
				SectionUpdates sectionUpdates = locationUpdates.SectionUpdates;
				if (!sectionUpdates.IsEmpty && sectionUpdates.IsNew)
				{
					configDefinitionUpdates.FlagLocationWritten();
					bool flag = this._locationSubPath != null || !locationUpdates.IsDefault;
					int num = linePosition;
					utilWriter.AppendSpacesToLinePosition(linePosition);
					if (flag)
					{
						if (this._locationSubPath == null)
						{
							utilWriter.Write(string.Format(CultureInfo.InvariantCulture, "<location {0} inheritInChildApplications=\"{1}\">\r\n", new object[]
							{
								locationUpdates.OverrideMode.LocationTagXmlString,
								MgmtConfigurationRecord.BoolToString(locationUpdates.InheritInChildApps)
							}));
						}
						else
						{
							utilWriter.Write(string.Format(CultureInfo.InvariantCulture, "<location path=\"{2}\" {0} inheritInChildApplications=\"{1}\">\r\n", new object[]
							{
								locationUpdates.OverrideMode.LocationTagXmlString,
								MgmtConfigurationRecord.BoolToString(locationUpdates.InheritInChildApps),
								this._locationSubPath
							}));
						}
						num += indent;
						utilWriter.AppendSpacesToLinePosition(num);
					}
					this.WriteNewConfigDefinitionsRecursive(utilWriter, locationUpdates.SectionUpdates, num, indent, true);
					if (flag)
					{
						utilWriter.AppendSpacesToLinePosition(linePosition);
						utilWriter.Write("</location>");
						utilWriter.AppendNewLine();
					}
				}
			}
			if (configDefinitionUpdates.RequireLocation)
			{
				configDefinitionUpdates.FlagLocationWritten();
				utilWriter.AppendSpacesToLinePosition(linePosition);
				utilWriter.Write(string.Format(CultureInfo.InvariantCulture, "<location path=\"{2}\" {0} inheritInChildApplications=\"{1}\">\r\n", new object[]
				{
					OverrideModeSetting.LocationDefault.LocationTagXmlString,
					"true",
					this._locationSubPath
				}));
				utilWriter.AppendSpacesToLinePosition(linePosition);
				utilWriter.Write("</location>");
				utilWriter.AppendNewLine();
			}
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00017780 File Offset: 0x00016780
		private bool WriteNewConfigDefinitionsRecursive(XmlUtilWriter utilWriter, SectionUpdates sectionUpdates, int linePosition, int indent, bool skipFirstIndent)
		{
			bool flag = false;
			string[] movedSectionNames = sectionUpdates.GetMovedSectionNames();
			if (movedSectionNames != null)
			{
				flag = true;
				foreach (string text in movedSectionNames)
				{
					DefinitionUpdate definitionUpdate = sectionUpdates.GetDefinitionUpdate(text);
					this.WriteSectionUpdate(utilWriter, definitionUpdate, linePosition, indent, skipFirstIndent);
					utilWriter.AppendNewLine();
					skipFirstIndent = false;
				}
			}
			string[] newGroupNames = sectionUpdates.GetNewGroupNames();
			if (newGroupNames != null)
			{
				foreach (string text2 in newGroupNames)
				{
					if (!skipFirstIndent)
					{
						utilWriter.AppendSpacesToLinePosition(linePosition);
					}
					skipFirstIndent = false;
					utilWriter.Write("<" + text2 + ">\r\n");
					bool flag2 = this.WriteNewConfigDefinitionsRecursive(utilWriter, sectionUpdates.GetSectionUpdatesForGroup(text2), linePosition + indent, indent, false);
					if (flag2)
					{
						flag = true;
					}
					utilWriter.AppendSpacesToLinePosition(linePosition);
					utilWriter.Write("</" + text2 + ">\r\n");
				}
			}
			sectionUpdates.IsNew = false;
			return flag;
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00017870 File Offset: 0x00016870
		private void CheckPreamble(byte[] preamble, XmlUtilWriter utilWriter, byte[] buffer)
		{
			bool flag = false;
			using (Stream stream = new MemoryStream(buffer))
			{
				byte[] array = new byte[preamble.Length];
				if (stream.Read(array, 0, array.Length) == array.Length)
				{
					flag = true;
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i] != preamble[i])
						{
							flag = false;
							break;
						}
					}
				}
			}
			if (!flag)
			{
				object obj = utilWriter.CreateStreamCheckpoint();
				utilWriter.Write('x');
				utilWriter.RestoreStreamCheckpoint(obj);
			}
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x000178F4 File Offset: 0x000168F4
		private int UpdateIndent(int oldIndent, XmlUtil xmlUtil, XmlUtilWriter utilWriter, int parentLinePosition)
		{
			int num = oldIndent;
			if (xmlUtil.Reader.NodeType == XmlNodeType.Element && utilWriter.IsLastLineBlank)
			{
				int trueLinePosition = xmlUtil.TrueLinePosition;
				if (parentLinePosition < trueLinePosition && trueLinePosition <= parentLinePosition + 10)
				{
					num = trueLinePosition - parentLinePosition;
				}
			}
			return num;
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00017934 File Offset: 0x00016934
		private void CopyConfig(SectionUpdates declarationUpdates, ConfigDefinitionUpdates definitionUpdates, byte[] buffer, string filename, NamespaceChange namespaceChange, XmlUtilWriter utilWriter)
		{
			this.CheckPreamble(base.ConfigStreamInfo.StreamEncoding.GetPreamble(), utilWriter, buffer);
			using (Stream stream = new MemoryStream(buffer))
			{
				using (XmlUtil xmlUtil = new XmlUtil(stream, filename, false))
				{
					XmlTextReader reader = xmlUtil.Reader;
					reader.WhitespaceHandling = WhitespaceHandling.All;
					reader.Read();
					xmlUtil.CopyReaderToNextElement(utilWriter, false);
					int num = 4;
					int trueLinePosition = xmlUtil.TrueLinePosition;
					bool isEmptyElement = reader.IsEmptyElement;
					string text;
					if (namespaceChange == NamespaceChange.Add)
					{
						text = string.Format(CultureInfo.InvariantCulture, "<configuration xmlns=\"{0}\">\r\n", new object[] { "http://schemas.microsoft.com/.NetConfiguration/v2.0" });
					}
					else if (namespaceChange == NamespaceChange.Remove)
					{
						text = "<configuration>\r\n";
					}
					else
					{
						text = null;
					}
					bool flag = declarationUpdates != null || definitionUpdates != null;
					string text2 = xmlUtil.UpdateStartElement(utilWriter, text, flag, trueLinePosition, num);
					bool flag2 = false;
					if (!isEmptyElement)
					{
						xmlUtil.CopyReaderToNextElement(utilWriter, true);
						num = this.UpdateIndent(num, xmlUtil, utilWriter, trueLinePosition);
						if (reader.NodeType == XmlNodeType.Element && reader.Name == "configSections")
						{
							flag2 = true;
							int trueLinePosition2 = xmlUtil.TrueLinePosition;
							bool isEmptyElement2 = reader.IsEmptyElement;
							if (declarationUpdates == null)
							{
								xmlUtil.CopyOuterXmlToNextElement(utilWriter, true);
							}
							else
							{
								string text3 = xmlUtil.UpdateStartElement(utilWriter, null, true, trueLinePosition2, num);
								if (!isEmptyElement2)
								{
									xmlUtil.CopyReaderToNextElement(utilWriter, true);
									this.CopyConfigDeclarationsRecursive(declarationUpdates, xmlUtil, utilWriter, string.Empty, trueLinePosition2, num);
								}
								if (declarationUpdates.HasUnretrievedSections())
								{
									int num2 = 0;
									if (text3 == null)
									{
										num2 = xmlUtil.TrueLinePosition;
									}
									if (!utilWriter.IsLastLineBlank)
									{
										utilWriter.AppendNewLine();
									}
									this.WriteUnwrittenConfigDeclarations(declarationUpdates, utilWriter, trueLinePosition2 + num, num, false);
									if (text3 == null)
									{
										utilWriter.AppendSpacesToLinePosition(num2);
									}
								}
								if (text3 == null)
								{
									xmlUtil.CopyXmlNode(utilWriter);
								}
								else
								{
									utilWriter.Write(text3);
								}
								xmlUtil.CopyReaderToNextElement(utilWriter, true);
							}
						}
					}
					if (!flag2 && declarationUpdates != null)
					{
						bool flag3 = reader.Depth > 0 && reader.NodeType == XmlNodeType.Element;
						int num3;
						if (flag3)
						{
							num3 = xmlUtil.TrueLinePosition;
						}
						else
						{
							num3 = trueLinePosition + num;
						}
						this.WriteNewConfigDeclarations(declarationUpdates, utilWriter, num3, num, flag3);
					}
					if (definitionUpdates != null)
					{
						bool flag4 = false;
						LocationUpdates locationUpdates = null;
						SectionUpdates sectionUpdates = null;
						if (!base.IsLocationConfig)
						{
							flag4 = true;
							locationUpdates = definitionUpdates.FindLocationUpdates(OverrideModeSetting.LocationDefault, true);
							if (locationUpdates != null)
							{
								sectionUpdates = locationUpdates.SectionUpdates;
							}
						}
						this.CopyConfigDefinitionsRecursive(definitionUpdates, xmlUtil, utilWriter, flag4, locationUpdates, sectionUpdates, true, string.Empty, trueLinePosition, num);
						this.WriteNewConfigDefinitions(definitionUpdates, utilWriter, trueLinePosition + num, num);
					}
					if (text2 != null)
					{
						if (!utilWriter.IsLastLineBlank)
						{
							utilWriter.AppendNewLine();
						}
						utilWriter.Write(text2);
					}
					while (xmlUtil.CopyXmlNode(utilWriter))
					{
					}
				}
			}
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00017C08 File Offset: 0x00016C08
		private bool CopyConfigDeclarationsRecursive(SectionUpdates declarationUpdates, XmlUtil xmlUtil, XmlUtilWriter utilWriter, string group, int parentLinePosition, int parentIndent)
		{
			bool flag = false;
			XmlTextReader reader = xmlUtil.Reader;
			int num = this.UpdateIndent(parentIndent, xmlUtil, utilWriter, parentLinePosition);
			int num2;
			int num3;
			if (reader.NodeType == XmlNodeType.Element)
			{
				num2 = xmlUtil.TrueLinePosition;
				num3 = num2;
			}
			else if (reader.NodeType == XmlNodeType.EndElement)
			{
				num2 = parentLinePosition + num;
				if (utilWriter.IsLastLineBlank)
				{
					num3 = xmlUtil.TrueLinePosition;
				}
				else
				{
					num3 = parentLinePosition;
				}
			}
			else
			{
				num2 = parentLinePosition + num;
				num3 = 0;
			}
			if (declarationUpdates != null)
			{
				string[] movedSectionNames = declarationUpdates.GetMovedSectionNames();
				if (movedSectionNames != null)
				{
					if (!utilWriter.IsLastLineBlank)
					{
						utilWriter.AppendNewLine();
					}
					foreach (string text in movedSectionNames)
					{
						DeclarationUpdate declarationUpdate = declarationUpdates.GetDeclarationUpdate(text);
						utilWriter.AppendSpacesToLinePosition(num2);
						utilWriter.Write(declarationUpdate.UpdatedXml);
						utilWriter.AppendNewLine();
						flag = true;
					}
					utilWriter.AppendSpacesToLinePosition(num3);
				}
			}
			if (reader.NodeType == XmlNodeType.Element)
			{
				int depth = reader.Depth;
				while (reader.Depth == depth)
				{
					bool flag2 = false;
					DeclarationUpdate declarationUpdate2 = null;
					DeclarationUpdate declarationUpdate3 = null;
					SectionUpdates sectionUpdates = declarationUpdates;
					string text2 = group;
					num = this.UpdateIndent(num, xmlUtil, utilWriter, parentLinePosition);
					num2 = xmlUtil.TrueLinePosition;
					string name = reader.Name;
					string attribute = reader.GetAttribute("name");
					string text3 = BaseConfigurationRecord.CombineConfigKey(group, attribute);
					if (name == "sectionGroup")
					{
						SectionUpdates sectionUpdatesForGroup = declarationUpdates.GetSectionUpdatesForGroup(attribute);
						if (sectionUpdatesForGroup != null)
						{
							declarationUpdate3 = sectionUpdatesForGroup.GetSectionGroupUpdate();
							if (sectionUpdatesForGroup.HasUnretrievedSections())
							{
								flag2 = true;
								text2 = text3;
								sectionUpdates = sectionUpdatesForGroup;
							}
						}
					}
					else
					{
						declarationUpdate2 = declarationUpdates.GetDeclarationUpdate(text3);
					}
					bool flag3 = declarationUpdate3 != null && declarationUpdate3.UpdatedXml != null;
					if (flag2)
					{
						object obj = utilWriter.CreateStreamCheckpoint();
						string text4 = null;
						if (flag3)
						{
							utilWriter.Write(declarationUpdate3.UpdatedXml);
							reader.Read();
						}
						else
						{
							text4 = xmlUtil.UpdateStartElement(utilWriter, null, true, num2, num);
						}
						if (text4 == null)
						{
							xmlUtil.CopyReaderToNextElement(utilWriter, true);
						}
						bool flag4 = this.CopyConfigDeclarationsRecursive(sectionUpdates, xmlUtil, utilWriter, text2, num2, num);
						if (text4 != null)
						{
							utilWriter.AppendSpacesToLinePosition(num2);
							utilWriter.Write(text4);
							utilWriter.AppendSpacesToLinePosition(parentLinePosition);
						}
						else
						{
							xmlUtil.CopyXmlNode(utilWriter);
						}
						if (flag4 || flag3)
						{
							flag = true;
						}
						else
						{
							utilWriter.RestoreStreamCheckpoint(obj);
						}
						xmlUtil.CopyReaderToNextElement(utilWriter, true);
					}
					else
					{
						bool flag5 = false;
						bool flag6;
						if (declarationUpdate2 == null)
						{
							flag6 = true;
							if (flag3)
							{
								flag = true;
								utilWriter.Write(declarationUpdate3.UpdatedXml);
								utilWriter.AppendNewLine();
								utilWriter.AppendSpacesToLinePosition(num2);
								utilWriter.Write("</sectionGroup>");
								utilWriter.AppendNewLine();
								utilWriter.AppendSpacesToLinePosition(num2);
							}
							else if (declarationUpdate3 != null)
							{
								flag = true;
								flag6 = false;
								flag5 = true;
							}
						}
						else
						{
							flag = true;
							if (declarationUpdate2.UpdatedXml == null)
							{
								flag6 = false;
							}
							else
							{
								flag6 = true;
								utilWriter.Write(declarationUpdate2.UpdatedXml);
							}
						}
						if (flag6)
						{
							xmlUtil.SkipAndCopyReaderToNextElement(utilWriter, true);
						}
						else if (flag5)
						{
							xmlUtil.SkipChildElementsAndCopyOuterXmlToNextElement(utilWriter);
						}
						else
						{
							xmlUtil.CopyOuterXmlToNextElement(utilWriter, true);
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00017EE8 File Offset: 0x00016EE8
		private bool CopyConfigDefinitionsRecursive(ConfigDefinitionUpdates configDefinitionUpdates, XmlUtil xmlUtil, XmlUtilWriter utilWriter, bool locationPathApplies, LocationUpdates locationUpdates, SectionUpdates sectionUpdates, bool addNewSections, string group, int parentLinePosition, int parentIndent)
		{
			bool flag = false;
			XmlTextReader reader = xmlUtil.Reader;
			int num = this.UpdateIndent(parentIndent, xmlUtil, utilWriter, parentLinePosition);
			int num2;
			int num3;
			if (reader.NodeType == XmlNodeType.Element)
			{
				num2 = xmlUtil.TrueLinePosition;
				num3 = num2;
			}
			else if (reader.NodeType == XmlNodeType.EndElement)
			{
				num2 = parentLinePosition + num;
				if (utilWriter.IsLastLineBlank)
				{
					num3 = xmlUtil.TrueLinePosition;
				}
				else
				{
					num3 = parentLinePosition;
				}
			}
			else
			{
				num2 = parentLinePosition + num;
				num3 = 0;
			}
			if (sectionUpdates != null && addNewSections)
			{
				sectionUpdates.IsNew = false;
				string[] movedSectionNames = sectionUpdates.GetMovedSectionNames();
				if (movedSectionNames != null)
				{
					if (!utilWriter.IsLastLineBlank)
					{
						utilWriter.AppendNewLine();
					}
					utilWriter.AppendSpacesToLinePosition(num2);
					bool flag2 = true;
					foreach (string text in movedSectionNames)
					{
						DefinitionUpdate definitionUpdate = sectionUpdates.GetDefinitionUpdate(text);
						this.WriteSectionUpdate(utilWriter, definitionUpdate, num2, num, flag2);
						flag2 = false;
						utilWriter.AppendNewLine();
						flag = true;
					}
					utilWriter.AppendSpacesToLinePosition(num3);
				}
			}
			if (reader.NodeType == XmlNodeType.Element)
			{
				int depth = reader.Depth;
				while (reader.Depth == depth)
				{
					bool flag3 = false;
					DefinitionUpdate definitionUpdate2 = null;
					bool flag4 = locationPathApplies;
					LocationUpdates locationUpdates2 = locationUpdates;
					SectionUpdates sectionUpdates2 = sectionUpdates;
					bool flag5 = addNewSections;
					string text2 = group;
					bool flag6 = false;
					num = this.UpdateIndent(num, xmlUtil, utilWriter, parentLinePosition);
					num2 = xmlUtil.TrueLinePosition;
					string name = reader.Name;
					if (name == "location")
					{
						string text3 = reader.GetAttribute("path");
						text3 = BaseConfigurationRecord.NormalizeLocationSubPath(text3, xmlUtil);
						OverrideModeSetting overrideModeSetting = OverrideModeSetting.LocationDefault;
						bool flag7 = true;
						if (base.IsLocationConfig)
						{
							flag4 = text3 != null && StringUtil.EqualsIgnoreCase(base.ConfigPath, base.Host.GetConfigPathFromLocationSubPath(base.Parent.ConfigPath, text3));
						}
						else
						{
							flag4 = text3 == null;
						}
						if (flag4)
						{
							string attribute = reader.GetAttribute("allowOverride");
							if (attribute != null)
							{
								overrideModeSetting = OverrideModeSetting.CreateFromXmlReadValue(bool.Parse(attribute));
							}
							string attribute2 = reader.GetAttribute("overrideMode");
							if (attribute2 != null)
							{
								overrideModeSetting = OverrideModeSetting.CreateFromXmlReadValue(OverrideModeSetting.ParseOverrideModeXmlValue(attribute2, null));
							}
							string attribute3 = reader.GetAttribute("inheritInChildApplications");
							if (attribute3 != null)
							{
								flag7 = bool.Parse(attribute3);
							}
							configDefinitionUpdates.FlagLocationWritten();
						}
						if (reader.IsEmptyElement)
						{
							flag4 = flag4 && configDefinitionUpdates.FindLocationUpdates(overrideModeSetting, flag7) != null;
						}
						else if (flag4)
						{
							if (configDefinitionUpdates != null)
							{
								locationUpdates2 = configDefinitionUpdates.FindLocationUpdates(overrideModeSetting, flag7);
								if (locationUpdates2 != null)
								{
									flag3 = true;
									sectionUpdates2 = locationUpdates2.SectionUpdates;
									if (this._locationSubPath == null && locationUpdates2.IsDefault)
									{
										flag5 = false;
									}
								}
							}
						}
						else if (this.HasRemovedSectionsOrGroups && !base.IsLocationConfig && base.Host.SupportsLocation)
						{
							flag3 = true;
							locationUpdates2 = null;
							sectionUpdates2 = null;
							flag5 = false;
						}
					}
					else
					{
						string text4 = BaseConfigurationRecord.CombineConfigKey(group, name);
						FactoryRecord factoryRecord = base.FindFactoryRecord(text4, false);
						if (factoryRecord == null)
						{
							if (!flag4 && !base.IsLocationConfig)
							{
								flag6 = true;
							}
						}
						else if (factoryRecord.IsGroup)
						{
							if (reader.IsEmptyElement)
							{
								if (!flag4 && !base.IsLocationConfig)
								{
									flag6 = true;
								}
							}
							else if (sectionUpdates != null)
							{
								SectionUpdates sectionUpdatesForGroup = sectionUpdates.GetSectionUpdatesForGroup(name);
								if (sectionUpdatesForGroup != null)
								{
									flag3 = true;
									text2 = text4;
									sectionUpdates2 = sectionUpdatesForGroup;
								}
							}
							else if (!flag4 && !base.IsLocationConfig)
							{
								if (this._removedSectionGroups != null && this._removedSectionGroups.Contains(text4))
								{
									flag6 = true;
								}
								else
								{
									flag3 = true;
									text2 = text4;
									locationUpdates2 = null;
									sectionUpdates2 = null;
									flag5 = false;
								}
							}
						}
						else if (sectionUpdates != null)
						{
							definitionUpdate2 = sectionUpdates.GetDefinitionUpdate(text4);
						}
						else if (!flag4 && !base.IsLocationConfig && this._removedSections != null && this._removedSections.Contains(text4))
						{
							flag6 = true;
						}
					}
					if (flag3)
					{
						object obj = utilWriter.CreateStreamCheckpoint();
						xmlUtil.CopyXmlNode(utilWriter);
						xmlUtil.CopyReaderToNextElement(utilWriter, true);
						bool flag8 = this.CopyConfigDefinitionsRecursive(configDefinitionUpdates, xmlUtil, utilWriter, flag4, locationUpdates2, sectionUpdates2, flag5, text2, num2, num);
						xmlUtil.CopyXmlNode(utilWriter);
						if (flag8)
						{
							flag = true;
						}
						else
						{
							utilWriter.RestoreStreamCheckpoint(obj);
						}
						xmlUtil.CopyReaderToNextElement(utilWriter, true);
					}
					else
					{
						bool flag9;
						if (definitionUpdate2 == null)
						{
							flag9 = flag4 || flag6;
						}
						else
						{
							flag9 = false;
							if (definitionUpdate2.UpdatedXml != null)
							{
								ConfigurationSection configurationSection = (ConfigurationSection)definitionUpdate2.SectionRecord.Result;
								if (string.IsNullOrEmpty(configurationSection.SectionInformation.ConfigSource) || configurationSection.SectionInformation.ConfigSourceModified)
								{
									flag9 = true;
									this.WriteSectionUpdate(utilWriter, definitionUpdate2, num2, num, true);
									flag = true;
								}
							}
						}
						if (flag9)
						{
							xmlUtil.SkipAndCopyReaderToNextElement(utilWriter, true);
						}
						else
						{
							xmlUtil.CopyOuterXmlToNextElement(utilWriter, true);
							flag = true;
						}
					}
				}
			}
			if (sectionUpdates != null && addNewSections && sectionUpdates.HasNewSectionGroups())
			{
				num2 = parentLinePosition + num;
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					if (utilWriter.IsLastLineBlank)
					{
						num3 = xmlUtil.TrueLinePosition;
					}
					else
					{
						num3 = parentLinePosition;
					}
				}
				else
				{
					num3 = 0;
				}
				utilWriter.AppendSpacesToLinePosition(num2);
				bool flag10 = this.WriteNewConfigDefinitionsRecursive(utilWriter, sectionUpdates, num2, num, true);
				if (flag10)
				{
					flag = true;
				}
				utilWriter.AppendSpacesToLinePosition(num3);
			}
			return flag;
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x000183D4 File Offset: 0x000173D4
		private void WriteSectionUpdate(XmlUtilWriter utilWriter, DefinitionUpdate update, int linePosition, int indent, bool skipFirstIndent)
		{
			ConfigurationSection configurationSection = (ConfigurationSection)update.SectionRecord.Result;
			string text;
			if (!string.IsNullOrEmpty(configurationSection.SectionInformation.ConfigSource))
			{
				text = string.Format(CultureInfo.InvariantCulture, "<{0} configSource=\"{1}\" />", new object[]
				{
					configurationSection.SectionInformation.Name,
					configurationSection.SectionInformation.ConfigSource
				});
			}
			else
			{
				text = update.UpdatedXml;
			}
			string text2 = XmlUtil.FormatXmlElement(text, linePosition, indent, skipFirstIndent);
			utilWriter.Write(text2);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00018458 File Offset: 0x00017458
		private void SaveConfigSource(DefinitionUpdate update)
		{
			string text;
			if (update.SectionRecord.HasResult)
			{
				ConfigurationSection configurationSection = (ConfigurationSection)update.SectionRecord.Result;
				text = configurationSection.SectionInformation.ConfigSourceStreamName;
			}
			else
			{
				SectionInput fileInput = update.SectionRecord.FileInput;
				text = fileInput.SectionXmlInfo.ConfigSourceStreamName;
			}
			byte[] array = null;
			using (Stream stream = base.Host.OpenStreamForRead(text))
			{
				if (stream != null)
				{
					array = new byte[stream.Length];
					int num = stream.Read(array, 0, (int)stream.Length);
					if ((long)num != stream.Length)
					{
						throw new ConfigurationErrorsException();
					}
				}
			}
			bool flag = array != null;
			object obj = null;
			bool flag2 = false;
			try
			{
				try
				{
					string text2;
					if (base.Host.IsRemote)
					{
						text2 = null;
					}
					else
					{
						text2 = base.ConfigStreamInfo.StreamName;
					}
					using (Stream stream2 = base.Host.OpenStreamForWrite(text, text2, ref obj))
					{
						flag2 = true;
						if (update.UpdatedXml == null)
						{
							if (flag)
							{
								stream2.Write(array, 0, array.Length);
							}
						}
						else
						{
							using (StreamWriter streamWriter = new StreamWriter(stream2))
							{
								XmlUtilWriter xmlUtilWriter = new XmlUtilWriter(streamWriter, true);
								if (flag)
								{
									this.CopyConfigSource(xmlUtilWriter, update.UpdatedXml, text, array);
								}
								else
								{
									this.CreateNewConfigSource(xmlUtilWriter, update.UpdatedXml, 4);
								}
							}
						}
					}
				}
				catch
				{
					if (flag2)
					{
						base.Host.WriteCompleted(text, false, obj);
					}
					throw;
				}
			}
			catch (Exception ex)
			{
				throw ExceptionUtil.WrapAsConfigException(SR.GetString("Config_error_loading_XML_file"), ex, text, 0);
			}
			catch
			{
				throw ExceptionUtil.WrapAsConfigException(SR.GetString("Config_error_loading_XML_file"), null, text, 0);
			}
			base.Host.WriteCompleted(text, true, obj);
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00018650 File Offset: 0x00017650
		private void CopyConfigSource(XmlUtilWriter utilWriter, string updatedXml, string configSourceStreamName, byte[] buffer)
		{
			byte[] preamble;
			using (Stream stream = new MemoryStream(buffer))
			{
				using (new XmlUtil(stream, configSourceStreamName, true))
				{
					preamble = base.ConfigStreamInfo.StreamEncoding.GetPreamble();
				}
			}
			this.CheckPreamble(preamble, utilWriter, buffer);
			using (Stream stream2 = new MemoryStream(buffer))
			{
				using (XmlUtil xmlUtil2 = new XmlUtil(stream2, configSourceStreamName, false))
				{
					XmlTextReader reader = xmlUtil2.Reader;
					reader.WhitespaceHandling = WhitespaceHandling.All;
					reader.Read();
					int num = 4;
					int num2 = 1;
					bool flag = xmlUtil2.CopyReaderToNextElement(utilWriter, false);
					if (flag)
					{
						int lineNumber = reader.LineNumber;
						num2 = reader.LinePosition - 1;
						int num3 = 0;
						while (reader.MoveToNextAttribute())
						{
							if (reader.LineNumber > lineNumber)
							{
								num3 = reader.LinePosition - num2;
								break;
							}
						}
						int num4 = 0;
						reader.Read();
						while (reader.Depth >= 1)
						{
							if (reader.NodeType == XmlNodeType.Element)
							{
								num4 = reader.LinePosition - 1 - num2;
								break;
							}
							reader.Read();
						}
						if (num4 > 0)
						{
							num = num4;
						}
						else if (num3 > 0)
						{
							num = num3;
						}
					}
					string text = XmlUtil.FormatXmlElement(updatedXml, num2, num, true);
					utilWriter.Write(text);
					if (flag)
					{
						while (reader.Depth > 0)
						{
							reader.Read();
						}
						if (reader.IsEmptyElement || reader.NodeType == XmlNodeType.EndElement)
						{
							reader.Read();
						}
						while (xmlUtil2.CopyXmlNode(utilWriter))
						{
						}
					}
				}
			}
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00018840 File Offset: 0x00017840
		private void CreateNewConfigSource(XmlUtilWriter utilWriter, string updatedXml, int indent)
		{
			string text = XmlUtil.FormatXmlElement(updatedXml, 0, indent, true);
			utilWriter.Write(string.Format(CultureInfo.InvariantCulture, "<?xml version=\"1.0\" encoding=\"{0}\"?>\r\n", new object[] { base.ConfigStreamInfo.StreamEncoding.WebName }));
			utilWriter.Write(text + "\r\n");
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0001889A File Offset: 0x0001789A
		private static string BoolToString(bool v)
		{
			if (!v)
			{
				return "false";
			}
			return "true";
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x000188AA File Offset: 0x000178AA
		internal void RemoveLocationWriteRequirement()
		{
			if (base.IsLocationConfig)
			{
				this._flags[16777216] = false;
				this._flags[33554432] = true;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x000188D6 File Offset: 0x000178D6
		// (set) Token: 0x060004B0 RID: 1200 RVA: 0x000188E8 File Offset: 0x000178E8
		internal bool NamespacePresent
		{
			get
			{
				return this._flags[67108864];
			}
			set
			{
				this._flags[67108864] = value;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x000188FB File Offset: 0x000178FB
		private NamespaceChange NamespaceChangeNeeded
		{
			get
			{
				if (this._flags[67108864] == this._flags[512])
				{
					return NamespaceChange.None;
				}
				if (this._flags[67108864])
				{
					return NamespaceChange.Add;
				}
				return NamespaceChange.Remove;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x00018936 File Offset: 0x00017936
		private bool RecordItselfRequiresUpdates
		{
			get
			{
				return this.NamespaceChangeNeeded != NamespaceChange.None;
			}
		}

		// Token: 0x0400033E RID: 830
		private const int DEFAULT_INDENT = 4;

		// Token: 0x0400033F RID: 831
		private const int MAX_INDENT = 10;

		// Token: 0x04000340 RID: 832
		private Hashtable _sectionGroups;

		// Token: 0x04000341 RID: 833
		private Hashtable _sectionFactories;

		// Token: 0x04000342 RID: 834
		private Hashtable _sectionGroupFactories;

		// Token: 0x04000343 RID: 835
		private Hashtable _removedSections;

		// Token: 0x04000344 RID: 836
		private Hashtable _removedSectionGroups;

		// Token: 0x04000345 RID: 837
		private Hashtable _locationTags;

		// Token: 0x04000346 RID: 838
		private HybridDictionary _streamInfoUpdates;

		// Token: 0x04000347 RID: 839
		private static readonly SimpleBitVector32 MgmtClassFlags = new SimpleBitVector32(80);
	}
}
