using System;
using System.Configuration.Internal;
using System.IO;

namespace System.Configuration
{
	// Token: 0x02000021 RID: 33
	public sealed class Configuration
	{
		// Token: 0x060001BF RID: 447 RVA: 0x0000C788 File Offset: 0x0000B788
		internal Configuration(string locationSubPath, Type typeConfigHost, params object[] hostInitConfigurationParams)
		{
			this._typeConfigHost = typeConfigHost;
			this._hostInitConfigurationParams = hostInitConfigurationParams;
			this._configRoot = new InternalConfigRoot();
			IInternalConfigHost internalConfigHost = (IInternalConfigHost)TypeUtil.CreateInstanceWithReflectionPermission(typeConfigHost);
			IInternalConfigHost internalConfigHost2 = new UpdateConfigHost(internalConfigHost);
			this._configRoot.Init(internalConfigHost2, true);
			string text;
			string text2;
			internalConfigHost.InitForConfiguration(ref locationSubPath, out text, out text2, this._configRoot, hostInitConfigurationParams);
			if (!string.IsNullOrEmpty(locationSubPath) && !internalConfigHost2.SupportsLocation)
			{
				throw ExceptionUtil.UnexpectedError("Configuration::ctor");
			}
			if (string.IsNullOrEmpty(locationSubPath) != string.IsNullOrEmpty(text2))
			{
				throw ExceptionUtil.UnexpectedError("Configuration::ctor");
			}
			this._configRecord = (MgmtConfigurationRecord)this._configRoot.GetConfigRecord(text);
			if (!string.IsNullOrEmpty(locationSubPath))
			{
				this._configRecord = MgmtConfigurationRecord.Create(this._configRoot, this._configRecord, text2, locationSubPath);
			}
			this._configRecord.ThrowIfInitErrors();
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000C85E File Offset: 0x0000B85E
		internal Configuration OpenLocationConfiguration(string locationSubPath)
		{
			return new Configuration(locationSubPath, this._typeConfigHost, this._hostInitConfigurationParams);
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x0000C872 File Offset: 0x0000B872
		public AppSettingsSection AppSettings
		{
			get
			{
				return (AppSettingsSection)this.GetSection("appSettings");
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x0000C884 File Offset: 0x0000B884
		public ConnectionStringsSection ConnectionStrings
		{
			get
			{
				return (ConnectionStringsSection)this.GetSection("connectionStrings");
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x0000C896 File Offset: 0x0000B896
		public string FilePath
		{
			get
			{
				return this._configRecord.ConfigurationFilePath;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x0000C8A3 File Offset: 0x0000B8A3
		public bool HasFile
		{
			get
			{
				return this._configRecord.HasStream;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x0000C8B0 File Offset: 0x0000B8B0
		public ConfigurationLocationCollection Locations
		{
			get
			{
				if (this._locations == null)
				{
					this._locations = this._configRecord.GetLocationCollection(this);
				}
				return this._locations;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x0000C8D2 File Offset: 0x0000B8D2
		public ContextInformation EvaluationContext
		{
			get
			{
				if (this._evalContext == null)
				{
					this._evalContext = new ContextInformation(this._configRecord);
				}
				return this._evalContext;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x0000C8F3 File Offset: 0x0000B8F3
		public ConfigurationSectionGroup RootSectionGroup
		{
			get
			{
				if (this._rootSectionGroup == null)
				{
					this._rootSectionGroup = new ConfigurationSectionGroup();
					this._rootSectionGroup.RootAttachToConfigurationRecord(this._configRecord);
				}
				return this._rootSectionGroup;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x0000C91F File Offset: 0x0000B91F
		public ConfigurationSectionCollection Sections
		{
			get
			{
				return this.RootSectionGroup.Sections;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x0000C92C File Offset: 0x0000B92C
		public ConfigurationSectionGroupCollection SectionGroups
		{
			get
			{
				return this.RootSectionGroup.SectionGroups;
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000C93C File Offset: 0x0000B93C
		public ConfigurationSection GetSection(string sectionName)
		{
			return (ConfigurationSection)this._configRecord.GetSection(sectionName);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000C95C File Offset: 0x0000B95C
		public ConfigurationSectionGroup GetSectionGroup(string sectionGroupName)
		{
			return this._configRecord.GetSectionGroup(sectionGroupName);
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001CC RID: 460 RVA: 0x0000C977 File Offset: 0x0000B977
		// (set) Token: 0x060001CD RID: 461 RVA: 0x0000C984 File Offset: 0x0000B984
		public bool NamespaceDeclared
		{
			get
			{
				return this._configRecord.NamespacePresent;
			}
			set
			{
				this._configRecord.NamespacePresent = value;
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000C992 File Offset: 0x0000B992
		public void Save()
		{
			this.SaveAsImpl(null, ConfigurationSaveMode.Modified, false);
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000C99D File Offset: 0x0000B99D
		public void Save(ConfigurationSaveMode saveMode)
		{
			this.SaveAsImpl(null, saveMode, false);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000C9A8 File Offset: 0x0000B9A8
		public void Save(ConfigurationSaveMode saveMode, bool forceSaveAll)
		{
			this.SaveAsImpl(null, saveMode, forceSaveAll);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000C9B3 File Offset: 0x0000B9B3
		public void SaveAs(string filename)
		{
			this.SaveAs(filename, ConfigurationSaveMode.Modified, false);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000C9BE File Offset: 0x0000B9BE
		public void SaveAs(string filename, ConfigurationSaveMode saveMode)
		{
			this.SaveAs(filename, saveMode, false);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000C9C9 File Offset: 0x0000B9C9
		public void SaveAs(string filename, ConfigurationSaveMode saveMode, bool forceSaveAll)
		{
			if (string.IsNullOrEmpty(filename))
			{
				throw ExceptionUtil.ParameterNullOrEmpty("filename");
			}
			this.SaveAsImpl(filename, saveMode, forceSaveAll);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000C9E7 File Offset: 0x0000B9E7
		private void SaveAsImpl(string filename, ConfigurationSaveMode saveMode, bool forceSaveAll)
		{
			if (string.IsNullOrEmpty(filename))
			{
				filename = null;
			}
			else
			{
				filename = Path.GetFullPath(filename);
			}
			if (forceSaveAll)
			{
				this.ForceGroupsRecursive(this.RootSectionGroup);
			}
			this._configRecord.SaveAs(filename, saveMode, forceSaveAll);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000CA1C File Offset: 0x0000BA1C
		private void ForceGroupsRecursive(ConfigurationSectionGroup group)
		{
			foreach (object obj in group.Sections)
			{
				ConfigurationSection configurationSection = (ConfigurationSection)obj;
				ConfigurationSection configurationSection2 = group.Sections[configurationSection.SectionInformation.Name];
			}
			foreach (object obj2 in group.SectionGroups)
			{
				ConfigurationSectionGroup configurationSectionGroup = (ConfigurationSectionGroup)obj2;
				this.ForceGroupsRecursive(group.SectionGroups[configurationSectionGroup.Name]);
			}
		}

		// Token: 0x04000212 RID: 530
		private Type _typeConfigHost;

		// Token: 0x04000213 RID: 531
		private object[] _hostInitConfigurationParams;

		// Token: 0x04000214 RID: 532
		private IInternalConfigRoot _configRoot;

		// Token: 0x04000215 RID: 533
		private MgmtConfigurationRecord _configRecord;

		// Token: 0x04000216 RID: 534
		private ConfigurationSectionGroup _rootSectionGroup;

		// Token: 0x04000217 RID: 535
		private ConfigurationLocationCollection _locations;

		// Token: 0x04000218 RID: 536
		private ContextInformation _evalContext;
	}
}
