using System;
using System.Configuration.Internal;

namespace System.Configuration
{
	// Token: 0x02000095 RID: 149
	public sealed class SectionInformation
	{
		// Token: 0x06000569 RID: 1385 RVA: 0x0001AB74 File Offset: 0x00019B74
		internal SectionInformation(ConfigurationSection associatedConfigurationSection)
		{
			this._configKey = string.Empty;
			this._group = string.Empty;
			this._name = string.Empty;
			this._configurationSection = associatedConfigurationSection;
			this._allowDefinition = ConfigurationAllowDefinition.Everywhere;
			this._allowExeDefinition = ConfigurationAllowExeDefinition.MachineToApplication;
			this._overrideModeDefault = OverrideModeSetting.SectionDefault;
			this._overrideMode = OverrideModeSetting.LocationDefault;
			this._flags[8] = true;
			this._flags[16] = true;
			this._flags[32] = true;
			this._flags[256] = true;
			this._flags[4096] = false;
			this._modifiedFlags = default(SimpleBitVector32);
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x0001AC2F File Offset: 0x00019C2F
		internal void ResetModifiedFlags()
		{
			this._modifiedFlags = default(SimpleBitVector32);
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x0001AC3D File Offset: 0x00019C3D
		internal bool IsModifiedFlags()
		{
			return this._modifiedFlags.Data != 0;
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x0001AC50 File Offset: 0x00019C50
		internal void AttachToConfigurationRecord(MgmtConfigurationRecord configRecord, FactoryRecord factoryRecord, SectionRecord sectionRecord)
		{
			this.SetRuntimeConfigurationInformation(configRecord, factoryRecord, sectionRecord);
			this._configRecord = configRecord;
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x0001AC64 File Offset: 0x00019C64
		internal void SetRuntimeConfigurationInformation(BaseConfigurationRecord configRecord, FactoryRecord factoryRecord, SectionRecord sectionRecord)
		{
			this._flags[1] = true;
			this._configKey = factoryRecord.ConfigKey;
			this._group = factoryRecord.Group;
			this._name = factoryRecord.Name;
			this._typeName = factoryRecord.FactoryTypeName;
			this._allowDefinition = factoryRecord.AllowDefinition;
			this._allowExeDefinition = factoryRecord.AllowExeDefinition;
			this._flags[8] = factoryRecord.AllowLocation;
			this._flags[16] = factoryRecord.RestartOnExternalChanges;
			this._flags[32] = factoryRecord.RequirePermission;
			this._overrideModeDefault = factoryRecord.OverrideModeDefault;
			if (factoryRecord.IsUndeclared)
			{
				this._flags[8192] = true;
				this._flags[2] = false;
				this._flags[4] = false;
			}
			else
			{
				this._flags[8192] = false;
				this._flags[2] = configRecord.GetFactoryRecord(factoryRecord.ConfigKey, false) != null;
				this._flags[4] = configRecord.IsRootDeclaration(factoryRecord.ConfigKey, false);
			}
			this._flags[64] = sectionRecord.Locked;
			this._flags[128] = sectionRecord.LockChildren;
			this._flags[16384] = sectionRecord.LockChildrenWithoutFileInput;
			if (sectionRecord.HasFileInput)
			{
				SectionInput fileInput = sectionRecord.FileInput;
				this._flags[2048] = fileInput.IsProtectionProviderDetermined;
				this._protectionProvider = fileInput.ProtectionProvider;
				SectionXmlInfo sectionXmlInfo = fileInput.SectionXmlInfo;
				this._configSource = sectionXmlInfo.ConfigSource;
				this._configSourceStreamName = sectionXmlInfo.ConfigSourceStreamName;
				this._overrideMode = sectionXmlInfo.OverrideModeSetting;
				this._flags[256] = !sectionXmlInfo.SkipInChildApps;
				this._protectionProviderName = sectionXmlInfo.ProtectionProviderName;
			}
			else
			{
				this._flags[2048] = false;
				this._protectionProvider = null;
			}
			this._configurationSection.AssociateContext(configRecord);
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0001AE6E File Offset: 0x00019E6E
		internal void DetachFromConfigurationRecord()
		{
			this.RevertToParent();
			this._flags[1] = false;
			this._configRecord = null;
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x0001AE8A File Offset: 0x00019E8A
		private bool IsRuntime
		{
			get
			{
				return this._flags[1] && this._configRecord == null;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x0001AEA5 File Offset: 0x00019EA5
		internal bool Attached
		{
			get
			{
				return this._flags[1];
			}
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0001AEB3 File Offset: 0x00019EB3
		private void VerifyDesigntime()
		{
			if (this.IsRuntime)
			{
				throw new InvalidOperationException(SR.GetString("Config_operation_not_runtime"));
			}
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0001AECD File Offset: 0x00019ECD
		private void VerifyIsAttachedToConfigRecord()
		{
			if (this._configRecord == null)
			{
				throw new InvalidOperationException(SR.GetString("Config_cannot_edit_configurationsection_when_not_attached"));
			}
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x0001AEE8 File Offset: 0x00019EE8
		internal void VerifyIsEditable()
		{
			this.VerifyDesigntime();
			if (this.IsLocked)
			{
				throw new InvalidOperationException(SR.GetString("Config_cannot_edit_configurationsection_when_locked"));
			}
			if (this._flags[512])
			{
				throw new InvalidOperationException(SR.GetString("Config_cannot_edit_configurationsection_parentsection"));
			}
			if (!this._flags[8] && this._configRecord != null && this._configRecord.IsLocationConfig)
			{
				throw new InvalidOperationException(SR.GetString("Config_cannot_edit_configurationsection_when_location_locked"));
			}
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0001AF68 File Offset: 0x00019F68
		private void VerifyNotParentSection()
		{
			if (this._flags[512])
			{
				throw new InvalidOperationException(SR.GetString("Config_configsection_parentnotvalid"));
			}
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x0001AF8C File Offset: 0x00019F8C
		private void VerifySupportsLocation()
		{
			if (this._configRecord != null && !this._configRecord.RecordSupportsLocation)
			{
				throw new InvalidOperationException(SR.GetString("Config_cannot_edit_locationattriubtes"));
			}
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x0001AFB4 File Offset: 0x00019FB4
		internal void VerifyIsEditableFactory()
		{
			if (this._configRecord != null && this._configRecord.IsLocationConfig)
			{
				throw new InvalidOperationException(SR.GetString("Config_cannot_edit_configurationsection_in_location_config"));
			}
			if (BaseConfigurationRecord.IsImplicitSection(this.ConfigKey))
			{
				throw new InvalidOperationException(SR.GetString("Config_cannot_edit_configurationsection_when_it_is_implicit"));
			}
			if (this._flags[8192])
			{
				throw new InvalidOperationException(SR.GetString("Config_cannot_edit_configurationsection_when_it_is_undeclared"));
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x0001B025 File Offset: 0x0001A025
		internal string ConfigKey
		{
			get
			{
				return this._configKey;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x0001B02D File Offset: 0x0001A02D
		// (set) Token: 0x06000579 RID: 1401 RVA: 0x0001B03F File Offset: 0x0001A03F
		internal bool Removed
		{
			get
			{
				return this._flags[1024];
			}
			set
			{
				this._flags[1024] = value;
			}
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0001B054 File Offset: 0x0001A054
		private FactoryRecord FindParentFactoryRecord(bool permitErrors)
		{
			FactoryRecord factoryRecord = null;
			if (this._configRecord != null && !this._configRecord.Parent.IsRootConfig)
			{
				factoryRecord = this._configRecord.Parent.FindFactoryRecord(this._configKey, permitErrors);
			}
			return factoryRecord;
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x0001B096 File Offset: 0x0001A096
		public string SectionName
		{
			get
			{
				return this._configKey;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x0600057C RID: 1404 RVA: 0x0001B09E File Offset: 0x0001A09E
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x0001B0A6 File Offset: 0x0001A0A6
		// (set) Token: 0x0600057E RID: 1406 RVA: 0x0001B0B0 File Offset: 0x0001A0B0
		public ConfigurationAllowDefinition AllowDefinition
		{
			get
			{
				return this._allowDefinition;
			}
			set
			{
				this.VerifyIsEditable();
				this.VerifyIsEditableFactory();
				FactoryRecord factoryRecord = this.FindParentFactoryRecord(false);
				if (factoryRecord != null && factoryRecord.AllowDefinition != value)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_tag_name_already_defined", new object[] { this._configKey }));
				}
				this._allowDefinition = value;
				this._modifiedFlags[131072] = true;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x0600057F RID: 1407 RVA: 0x0001B116 File Offset: 0x0001A116
		internal bool AllowDefinitionModified
		{
			get
			{
				return this._modifiedFlags[131072];
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x0001B128 File Offset: 0x0001A128
		// (set) Token: 0x06000581 RID: 1409 RVA: 0x0001B130 File Offset: 0x0001A130
		public ConfigurationAllowExeDefinition AllowExeDefinition
		{
			get
			{
				return this._allowExeDefinition;
			}
			set
			{
				this.VerifyIsEditable();
				this.VerifyIsEditableFactory();
				FactoryRecord factoryRecord = this.FindParentFactoryRecord(false);
				if (factoryRecord != null && factoryRecord.AllowExeDefinition != value)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_tag_name_already_defined", new object[] { this._configKey }));
				}
				this._allowExeDefinition = value;
				this._modifiedFlags[65536] = true;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x0001B196 File Offset: 0x0001A196
		internal bool AllowExeDefinitionModified
		{
			get
			{
				return this._modifiedFlags[65536];
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x0001B1A8 File Offset: 0x0001A1A8
		// (set) Token: 0x06000584 RID: 1412 RVA: 0x0001B1B8 File Offset: 0x0001A1B8
		public OverrideMode OverrideModeDefault
		{
			get
			{
				return this._overrideModeDefault.OverrideMode;
			}
			set
			{
				this.VerifyIsEditable();
				this.VerifyIsEditableFactory();
				FactoryRecord factoryRecord = this.FindParentFactoryRecord(false);
				if (factoryRecord != null && factoryRecord.OverrideModeDefault.OverrideMode != value)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_tag_name_already_defined", new object[] { this._configKey }));
				}
				if (value == OverrideMode.Inherit)
				{
					value = OverrideMode.Allow;
				}
				this._overrideModeDefault.OverrideMode = value;
				this._modifiedFlags[1048576] = true;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x0001B231 File Offset: 0x0001A231
		internal OverrideModeSetting OverrideModeDefaultSetting
		{
			get
			{
				return this._overrideModeDefault;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000586 RID: 1414 RVA: 0x0001B239 File Offset: 0x0001A239
		internal bool OverrideModeDefaultModified
		{
			get
			{
				return this._modifiedFlags[1048576];
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x0001B24B File Offset: 0x0001A24B
		// (set) Token: 0x06000588 RID: 1416 RVA: 0x0001B25C File Offset: 0x0001A25C
		public bool AllowLocation
		{
			get
			{
				return this._flags[8];
			}
			set
			{
				this.VerifyIsEditable();
				this.VerifyIsEditableFactory();
				FactoryRecord factoryRecord = this.FindParentFactoryRecord(false);
				if (factoryRecord != null && factoryRecord.AllowLocation != value)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_tag_name_already_defined", new object[] { this._configKey }));
				}
				this._flags[8] = value;
				this._modifiedFlags[8] = true;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000589 RID: 1417 RVA: 0x0001B2C4 File Offset: 0x0001A2C4
		internal bool AllowLocationModified
		{
			get
			{
				return this._modifiedFlags[8];
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x0001B2D2 File Offset: 0x0001A2D2
		// (set) Token: 0x0600058B RID: 1419 RVA: 0x0001B2DF File Offset: 0x0001A2DF
		public bool AllowOverride
		{
			get
			{
				return this._overrideMode.AllowOverride;
			}
			set
			{
				this.VerifyIsEditable();
				this.VerifySupportsLocation();
				this._overrideMode.AllowOverride = value;
				this._modifiedFlags[2097152] = true;
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x0600058C RID: 1420 RVA: 0x0001B30A File Offset: 0x0001A30A
		// (set) Token: 0x0600058D RID: 1421 RVA: 0x0001B318 File Offset: 0x0001A318
		public OverrideMode OverrideMode
		{
			get
			{
				return this._overrideMode.OverrideMode;
			}
			set
			{
				this.VerifyIsEditable();
				this.VerifySupportsLocation();
				this._overrideMode.OverrideMode = value;
				this._modifiedFlags[2097152] = true;
				switch (value)
				{
				case OverrideMode.Inherit:
					this._flags[128] = this._flags[16384];
					return;
				case OverrideMode.Allow:
					this._flags[128] = false;
					return;
				case OverrideMode.Deny:
					this._flags[128] = true;
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x0600058E RID: 1422 RVA: 0x0001B3A7 File Offset: 0x0001A3A7
		public OverrideMode OverrideModeEffective
		{
			get
			{
				if (!this._flags[128])
				{
					return OverrideMode.Allow;
				}
				return OverrideMode.Deny;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x0600058F RID: 1423 RVA: 0x0001B3BE File Offset: 0x0001A3BE
		internal OverrideModeSetting OverrideModeSetting
		{
			get
			{
				return this._overrideMode;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06000590 RID: 1424 RVA: 0x0001B3C6 File Offset: 0x0001A3C6
		internal bool LocationAttributesAreDefault
		{
			get
			{
				return this._overrideMode.IsDefaultForLocationTag && this._flags[256];
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x0001B3E7 File Offset: 0x0001A3E7
		// (set) Token: 0x06000592 RID: 1426 RVA: 0x0001B400 File Offset: 0x0001A400
		public string ConfigSource
		{
			get
			{
				if (this._configSource != null)
				{
					return this._configSource;
				}
				return string.Empty;
			}
			set
			{
				this.VerifyIsEditable();
				string text;
				if (!string.IsNullOrEmpty(value))
				{
					text = BaseConfigurationRecord.NormalizeConfigSource(value, null);
				}
				else
				{
					text = null;
				}
				if (text == this._configSource)
				{
					return;
				}
				if (this._configRecord != null)
				{
					this._configRecord.ChangeConfigSource(this, this._configSource, this._configSourceStreamName, text);
				}
				this._configSource = text;
				this._modifiedFlags[262144] = true;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000593 RID: 1427 RVA: 0x0001B46F File Offset: 0x0001A46F
		internal bool ConfigSourceModified
		{
			get
			{
				return this._modifiedFlags[262144];
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000594 RID: 1428 RVA: 0x0001B481 File Offset: 0x0001A481
		// (set) Token: 0x06000595 RID: 1429 RVA: 0x0001B489 File Offset: 0x0001A489
		internal string ConfigSourceStreamName
		{
			get
			{
				return this._configSourceStreamName;
			}
			set
			{
				this._configSourceStreamName = value;
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000596 RID: 1430 RVA: 0x0001B492 File Offset: 0x0001A492
		// (set) Token: 0x06000597 RID: 1431 RVA: 0x0001B4A4 File Offset: 0x0001A4A4
		public bool InheritInChildApplications
		{
			get
			{
				return this._flags[256];
			}
			set
			{
				this.VerifyIsEditable();
				this.VerifySupportsLocation();
				this._flags[256] = value;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000598 RID: 1432 RVA: 0x0001B4C3 File Offset: 0x0001A4C3
		public bool IsDeclared
		{
			get
			{
				this.VerifyNotParentSection();
				return this._flags[2];
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x0001B4D7 File Offset: 0x0001A4D7
		public bool IsDeclarationRequired
		{
			get
			{
				this.VerifyNotParentSection();
				return this._flags[4];
			}
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0001B4EB File Offset: 0x0001A4EB
		public void ForceDeclaration()
		{
			this.ForceDeclaration(true);
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0001B4F4 File Offset: 0x0001A4F4
		public void ForceDeclaration(bool force)
		{
			this.VerifyIsEditable();
			if (!force && this._flags[4])
			{
				return;
			}
			if (force && BaseConfigurationRecord.IsImplicitSection(this.SectionName))
			{
				throw new ConfigurationErrorsException(SR.GetString("Cannot_declare_or_remove_implicit_section"));
			}
			if (force && this._flags[8192])
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_cannot_edit_configurationsection_when_it_is_undeclared"));
			}
			this._flags[2] = force;
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x0600059C RID: 1436 RVA: 0x0001B56B File Offset: 0x0001A56B
		private bool IsDefinitionAllowed
		{
			get
			{
				return this._configRecord == null || this._configRecord.IsDefinitionAllowed(this._allowDefinition, this._allowExeDefinition);
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x0001B58E File Offset: 0x0001A58E
		public bool IsLocked
		{
			get
			{
				return this._flags[64] || !this.IsDefinitionAllowed || this._configurationSection.ElementInformation.IsLocked;
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x0600059E RID: 1438 RVA: 0x0001B5B9 File Offset: 0x0001A5B9
		public bool IsProtected
		{
			get
			{
				return this.ProtectionProvider != null;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x0001B5C8 File Offset: 0x0001A5C8
		public ProtectedConfigurationProvider ProtectionProvider
		{
			get
			{
				if (!this._flags[2048] && this._configRecord != null)
				{
					this._protectionProvider = this._configRecord.GetProtectionProviderFromName(this._protectionProviderName, false);
					this._flags[2048] = true;
				}
				return this._protectionProvider;
			}
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x0001B620 File Offset: 0x0001A620
		public void ProtectSection(string protectionProvider)
		{
			this.VerifyIsEditable();
			if (!this.AllowLocation || this._configKey == "configProtectedData")
			{
				throw new InvalidOperationException(SR.GetString("Config_not_allowed_to_encrypt_this_section"));
			}
			if (this._configRecord != null)
			{
				if (string.IsNullOrEmpty(protectionProvider))
				{
					protectionProvider = this._configRecord.DefaultProviderName;
				}
				ProtectedConfigurationProvider protectionProviderFromName = this._configRecord.GetProtectionProviderFromName(protectionProvider, true);
				this._protectionProviderName = protectionProvider;
				this._protectionProvider = protectionProviderFromName;
				this._flags[2048] = true;
				this._modifiedFlags[524288] = true;
				return;
			}
			throw new InvalidOperationException(SR.GetString("Must_add_to_config_before_protecting_it"));
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x0001B6CC File Offset: 0x0001A6CC
		public void UnprotectSection()
		{
			this.VerifyIsEditable();
			this._protectionProvider = null;
			this._protectionProviderName = null;
			this._flags[2048] = true;
			this._modifiedFlags[524288] = true;
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060005A2 RID: 1442 RVA: 0x0001B704 File Offset: 0x0001A704
		internal string ProtectionProviderName
		{
			get
			{
				return this._protectionProviderName;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x0001B70C File Offset: 0x0001A70C
		// (set) Token: 0x060005A4 RID: 1444 RVA: 0x0001B71C File Offset: 0x0001A71C
		public bool RestartOnExternalChanges
		{
			get
			{
				return this._flags[16];
			}
			set
			{
				this.VerifyIsEditable();
				this.VerifyIsEditableFactory();
				FactoryRecord factoryRecord = this.FindParentFactoryRecord(false);
				if (factoryRecord != null && factoryRecord.RestartOnExternalChanges != value)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_tag_name_already_defined", new object[] { this._configKey }));
				}
				this._flags[16] = value;
				this._modifiedFlags[16] = true;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060005A5 RID: 1445 RVA: 0x0001B786 File Offset: 0x0001A786
		internal bool RestartOnExternalChangesModified
		{
			get
			{
				return this._modifiedFlags[16];
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060005A6 RID: 1446 RVA: 0x0001B795 File Offset: 0x0001A795
		// (set) Token: 0x060005A7 RID: 1447 RVA: 0x0001B7A4 File Offset: 0x0001A7A4
		public bool RequirePermission
		{
			get
			{
				return this._flags[32];
			}
			set
			{
				this.VerifyIsEditable();
				this.VerifyIsEditableFactory();
				FactoryRecord factoryRecord = this.FindParentFactoryRecord(false);
				if (factoryRecord != null && factoryRecord.RequirePermission != value)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_tag_name_already_defined", new object[] { this._configKey }));
				}
				this._flags[32] = value;
				this._modifiedFlags[32] = true;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060005A8 RID: 1448 RVA: 0x0001B80E File Offset: 0x0001A80E
		internal bool RequirePermissionModified
		{
			get
			{
				return this._modifiedFlags[32];
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060005A9 RID: 1449 RVA: 0x0001B81D File Offset: 0x0001A81D
		// (set) Token: 0x060005AA RID: 1450 RVA: 0x0001B828 File Offset: 0x0001A828
		public string Type
		{
			get
			{
				return this._typeName;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw ExceptionUtil.PropertyNullOrEmpty("Type");
				}
				this.VerifyIsEditable();
				this.VerifyIsEditableFactory();
				FactoryRecord factoryRecord = this.FindParentFactoryRecord(false);
				if (factoryRecord != null)
				{
					IInternalConfigHost internalConfigHost = null;
					if (this._configRecord != null)
					{
						internalConfigHost = this._configRecord.Host;
					}
					if (!factoryRecord.IsEquivalentType(internalConfigHost, value))
					{
						throw new ConfigurationErrorsException(SR.GetString("Config_tag_name_already_defined", new object[] { this._configKey }));
					}
				}
				this._typeName = value;
			}
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0001B8A8 File Offset: 0x0001A8A8
		public ConfigurationSection GetParentSection()
		{
			this.VerifyDesigntime();
			if (this._flags[512])
			{
				throw new InvalidOperationException(SR.GetString("Config_getparentconfigurationsection_first_instance"));
			}
			ConfigurationSection configurationSection = null;
			if (this._configRecord != null)
			{
				configurationSection = this._configRecord.FindAndCloneImmediateParentSection(this._configurationSection);
				if (configurationSection != null)
				{
					configurationSection.SectionInformation._flags[512] = true;
					configurationSection.SetReadOnly();
				}
			}
			return configurationSection;
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0001B919 File Offset: 0x0001A919
		public string GetRawXml()
		{
			this.VerifyDesigntime();
			this.VerifyNotParentSection();
			if (this.RawXml != null)
			{
				return this.RawXml;
			}
			if (this._configRecord != null)
			{
				return this._configRecord.GetRawXml(this._configKey);
			}
			return null;
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0001B951 File Offset: 0x0001A951
		public void SetRawXml(string rawXml)
		{
			this.VerifyIsEditable();
			if (this._configRecord != null)
			{
				this._configRecord.SetRawXml(this._configurationSection, rawXml);
				return;
			}
			this.RawXml = (string.IsNullOrEmpty(rawXml) ? null : rawXml);
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x0001B986 File Offset: 0x0001A986
		// (set) Token: 0x060005AF RID: 1455 RVA: 0x0001B98E File Offset: 0x0001A98E
		internal string RawXml
		{
			get
			{
				return this._rawXml;
			}
			set
			{
				this._rawXml = value;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060005B0 RID: 1456 RVA: 0x0001B997 File Offset: 0x0001A997
		// (set) Token: 0x060005B1 RID: 1457 RVA: 0x0001B9A9 File Offset: 0x0001A9A9
		public bool ForceSave
		{
			get
			{
				return this._flags[4096];
			}
			set
			{
				this.VerifyIsEditable();
				this._flags[4096] = value;
			}
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0001B9C2 File Offset: 0x0001A9C2
		public void RevertToParent()
		{
			this.VerifyIsEditable();
			this.VerifyIsAttachedToConfigRecord();
			this._configRecord.RevertToParent(this._configurationSection);
		}

		// Token: 0x04000388 RID: 904
		private const int Flag_Attached = 1;

		// Token: 0x04000389 RID: 905
		private const int Flag_Declared = 2;

		// Token: 0x0400038A RID: 906
		private const int Flag_DeclarationRequired = 4;

		// Token: 0x0400038B RID: 907
		private const int Flag_AllowLocation = 8;

		// Token: 0x0400038C RID: 908
		private const int Flag_RestartOnExternalChanges = 16;

		// Token: 0x0400038D RID: 909
		private const int Flag_RequirePermission = 32;

		// Token: 0x0400038E RID: 910
		private const int Flag_LocationLocked = 64;

		// Token: 0x0400038F RID: 911
		private const int Flag_ChildrenLocked = 128;

		// Token: 0x04000390 RID: 912
		private const int Flag_InheritInChildApps = 256;

		// Token: 0x04000391 RID: 913
		private const int Flag_IsParentSection = 512;

		// Token: 0x04000392 RID: 914
		private const int Flag_Removed = 1024;

		// Token: 0x04000393 RID: 915
		private const int Flag_ProtectionProviderDetermined = 2048;

		// Token: 0x04000394 RID: 916
		private const int Flag_ForceSave = 4096;

		// Token: 0x04000395 RID: 917
		private const int Flag_IsUndeclared = 8192;

		// Token: 0x04000396 RID: 918
		private const int Flag_ChildrenLockWithoutFileInput = 16384;

		// Token: 0x04000397 RID: 919
		private const int Flag_AllowExeDefinitionModified = 65536;

		// Token: 0x04000398 RID: 920
		private const int Flag_AllowDefinitionModified = 131072;

		// Token: 0x04000399 RID: 921
		private const int Flag_ConfigSourceModified = 262144;

		// Token: 0x0400039A RID: 922
		private const int Flag_ProtectionProviderModified = 524288;

		// Token: 0x0400039B RID: 923
		private const int Flag_OverrideModeDefaultModified = 1048576;

		// Token: 0x0400039C RID: 924
		private const int Flag_OverrideModeModified = 2097152;

		// Token: 0x0400039D RID: 925
		private ConfigurationSection _configurationSection;

		// Token: 0x0400039E RID: 926
		private SafeBitVector32 _flags;

		// Token: 0x0400039F RID: 927
		private SimpleBitVector32 _modifiedFlags;

		// Token: 0x040003A0 RID: 928
		private ConfigurationAllowDefinition _allowDefinition;

		// Token: 0x040003A1 RID: 929
		private ConfigurationAllowExeDefinition _allowExeDefinition;

		// Token: 0x040003A2 RID: 930
		private MgmtConfigurationRecord _configRecord;

		// Token: 0x040003A3 RID: 931
		private string _configKey;

		// Token: 0x040003A4 RID: 932
		private string _group;

		// Token: 0x040003A5 RID: 933
		private string _name;

		// Token: 0x040003A6 RID: 934
		private string _typeName;

		// Token: 0x040003A7 RID: 935
		private string _rawXml;

		// Token: 0x040003A8 RID: 936
		private string _configSource;

		// Token: 0x040003A9 RID: 937
		private string _configSourceStreamName;

		// Token: 0x040003AA RID: 938
		private ProtectedConfigurationProvider _protectionProvider;

		// Token: 0x040003AB RID: 939
		private string _protectionProviderName;

		// Token: 0x040003AC RID: 940
		private OverrideModeSetting _overrideModeDefault;

		// Token: 0x040003AD RID: 941
		private OverrideModeSetting _overrideMode;
	}
}
