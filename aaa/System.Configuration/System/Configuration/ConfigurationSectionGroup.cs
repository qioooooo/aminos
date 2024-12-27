using System;

namespace System.Configuration
{
	// Token: 0x0200003E RID: 62
	public class ConfigurationSectionGroup
	{
		// Token: 0x060002E7 RID: 743 RVA: 0x0001161C File Offset: 0x0001061C
		internal void AttachToConfigurationRecord(MgmtConfigurationRecord configRecord, FactoryRecord factoryRecord)
		{
			this._configRecord = configRecord;
			this._configKey = factoryRecord.ConfigKey;
			this._group = factoryRecord.Group;
			this._name = factoryRecord.Name;
			this._typeName = factoryRecord.FactoryTypeName;
			if (this._typeName != null)
			{
				FactoryRecord factoryRecord2 = null;
				if (!configRecord.Parent.IsRootConfig)
				{
					factoryRecord2 = configRecord.Parent.FindFactoryRecord(factoryRecord.ConfigKey, true);
				}
				this._declarationRequired = factoryRecord2 == null || factoryRecord2.FactoryTypeName == null;
				this._declared = configRecord.GetFactoryRecord(factoryRecord.ConfigKey, true) != null;
			}
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x000116B8 File Offset: 0x000106B8
		internal void RootAttachToConfigurationRecord(MgmtConfigurationRecord configRecord)
		{
			this._configRecord = configRecord;
			this._isRoot = true;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x000116C8 File Offset: 0x000106C8
		internal void DetachFromConfigurationRecord()
		{
			if (this._configSections != null)
			{
				this._configSections.DetachFromConfigurationRecord();
			}
			if (this._configSectionGroups != null)
			{
				this._configSectionGroups.DetachFromConfigurationRecord();
			}
			this._configRecord = null;
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060002EA RID: 746 RVA: 0x000116F7 File Offset: 0x000106F7
		internal bool Attached
		{
			get
			{
				return this._configRecord != null;
			}
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00011708 File Offset: 0x00010708
		private FactoryRecord FindParentFactoryRecord(bool permitErrors)
		{
			FactoryRecord factoryRecord = null;
			if (this._configRecord != null && !this._configRecord.Parent.IsRootConfig)
			{
				factoryRecord = this._configRecord.Parent.FindFactoryRecord(this._configKey, permitErrors);
			}
			return factoryRecord;
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0001174A File Offset: 0x0001074A
		private void VerifyIsAttachedToConfigRecord()
		{
			if (this._configRecord == null)
			{
				throw new InvalidOperationException(SR.GetString("Config_cannot_edit_configurationsectiongroup_when_not_attached"));
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060002ED RID: 749 RVA: 0x00011764 File Offset: 0x00010764
		public bool IsDeclared
		{
			get
			{
				return this._declared;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060002EE RID: 750 RVA: 0x0001176C File Offset: 0x0001076C
		public bool IsDeclarationRequired
		{
			get
			{
				return this._declarationRequired;
			}
		}

		// Token: 0x060002EF RID: 751 RVA: 0x00011774 File Offset: 0x00010774
		public void ForceDeclaration()
		{
			this.ForceDeclaration(true);
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00011780 File Offset: 0x00010780
		public void ForceDeclaration(bool force)
		{
			if (this._isRoot)
			{
				throw new InvalidOperationException(SR.GetString("Config_root_section_group_cannot_be_edited"));
			}
			if (this._configRecord != null && this._configRecord.IsLocationConfig)
			{
				throw new InvalidOperationException(SR.GetString("Config_cannot_edit_configurationsectiongroup_in_location_config"));
			}
			if (!force && this._declarationRequired)
			{
				return;
			}
			this._declared = force;
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060002F1 RID: 753 RVA: 0x000117DD File Offset: 0x000107DD
		public string SectionGroupName
		{
			get
			{
				return this._configKey;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x000117E5 File Offset: 0x000107E5
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x000117ED File Offset: 0x000107ED
		// (set) Token: 0x060002F4 RID: 756 RVA: 0x000117F8 File Offset: 0x000107F8
		public string Type
		{
			get
			{
				return this._typeName;
			}
			set
			{
				if (this._isRoot)
				{
					throw new InvalidOperationException(SR.GetString("Config_root_section_group_cannot_be_edited"));
				}
				string text = value;
				if (string.IsNullOrEmpty(text))
				{
					text = null;
				}
				if (this._configRecord != null)
				{
					if (this._configRecord.IsLocationConfig)
					{
						throw new InvalidOperationException(SR.GetString("Config_cannot_edit_configurationsectiongroup_in_location_config"));
					}
					if (text != null)
					{
						FactoryRecord factoryRecord = this.FindParentFactoryRecord(false);
						if (factoryRecord != null && !factoryRecord.IsEquivalentType(this._configRecord.Host, text))
						{
							throw new ConfigurationErrorsException(SR.GetString("Config_tag_name_already_defined", new object[] { this._configKey }));
						}
					}
				}
				this._typeName = text;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x00011898 File Offset: 0x00010898
		public ConfigurationSectionCollection Sections
		{
			get
			{
				if (this._configSections == null)
				{
					this.VerifyIsAttachedToConfigRecord();
					this._configSections = new ConfigurationSectionCollection(this._configRecord, this);
				}
				return this._configSections;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x000118C0 File Offset: 0x000108C0
		public ConfigurationSectionGroupCollection SectionGroups
		{
			get
			{
				if (this._configSectionGroups == null)
				{
					this.VerifyIsAttachedToConfigRecord();
					this._configSectionGroups = new ConfigurationSectionGroupCollection(this._configRecord, this);
				}
				return this._configSectionGroups;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060002F7 RID: 759 RVA: 0x000118E8 File Offset: 0x000108E8
		internal bool IsRoot
		{
			get
			{
				return this._isRoot;
			}
		}

		// Token: 0x04000298 RID: 664
		private string _configKey = string.Empty;

		// Token: 0x04000299 RID: 665
		private string _group = string.Empty;

		// Token: 0x0400029A RID: 666
		private string _name = string.Empty;

		// Token: 0x0400029B RID: 667
		private ConfigurationSectionCollection _configSections;

		// Token: 0x0400029C RID: 668
		private ConfigurationSectionGroupCollection _configSectionGroups;

		// Token: 0x0400029D RID: 669
		private MgmtConfigurationRecord _configRecord;

		// Token: 0x0400029E RID: 670
		private string _typeName;

		// Token: 0x0400029F RID: 671
		private bool _declared;

		// Token: 0x040002A0 RID: 672
		private bool _declarationRequired;

		// Token: 0x040002A1 RID: 673
		private bool _isRoot;
	}
}
