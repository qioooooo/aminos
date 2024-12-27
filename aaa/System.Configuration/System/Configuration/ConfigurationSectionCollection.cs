using System;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x0200003D RID: 61
	[Serializable]
	public sealed class ConfigurationSectionCollection : NameObjectCollectionBase
	{
		// Token: 0x060002D5 RID: 725 RVA: 0x00011294 File Offset: 0x00010294
		internal ConfigurationSectionCollection(MgmtConfigurationRecord configRecord, ConfigurationSectionGroup configSectionGroup)
			: base(StringComparer.Ordinal)
		{
			this._configRecord = configRecord;
			this._configSectionGroup = configSectionGroup;
			foreach (object obj in this._configRecord.SectionFactories)
			{
				FactoryId factoryId = (FactoryId)((DictionaryEntry)obj).Value;
				if (factoryId.Group == this._configSectionGroup.SectionGroupName)
				{
					base.BaseAdd(factoryId.Name, factoryId.Name);
				}
			}
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0001133C File Offset: 0x0001033C
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x00011346 File Offset: 0x00010346
		internal void DetachFromConfigurationRecord()
		{
			this._configRecord = null;
			base.BaseClear();
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00011355 File Offset: 0x00010355
		private void VerifyIsAttachedToConfigRecord()
		{
			if (this._configRecord == null)
			{
				throw new InvalidOperationException(SR.GetString("Config_cannot_edit_configurationsectiongroup_when_not_attached"));
			}
		}

		// Token: 0x170000B7 RID: 183
		public ConfigurationSection this[string name]
		{
			get
			{
				return this.Get(name);
			}
		}

		// Token: 0x170000B8 RID: 184
		public ConfigurationSection this[int index]
		{
			get
			{
				return this.Get(index);
			}
		}

		// Token: 0x060002DB RID: 731 RVA: 0x00011381 File Offset: 0x00010381
		public void Add(string name, ConfigurationSection section)
		{
			this.VerifyIsAttachedToConfigRecord();
			this._configRecord.AddConfigurationSection(this._configSectionGroup.SectionGroupName, name, section);
			base.BaseAdd(name, name);
		}

		// Token: 0x060002DC RID: 732 RVA: 0x000113AC File Offset: 0x000103AC
		public void Clear()
		{
			this.VerifyIsAttachedToConfigRecord();
			string[] array = base.BaseGetAllKeys();
			foreach (string text in array)
			{
				this.Remove(text);
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060002DD RID: 733 RVA: 0x000113E1 File Offset: 0x000103E1
		public override int Count
		{
			get
			{
				return base.Count;
			}
		}

		// Token: 0x060002DE RID: 734 RVA: 0x000113EC File Offset: 0x000103EC
		public void CopyTo(ConfigurationSection[] array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			int count = this.Count;
			if (array.Length < count + index)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			int i = 0;
			int num = index;
			while (i < count)
			{
				array[num] = this.Get(i);
				i++;
				num++;
			}
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0001143D File Offset: 0x0001043D
		public ConfigurationSection Get(int index)
		{
			return this.Get(this.GetKey(index));
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0001144C File Offset: 0x0001044C
		public ConfigurationSection Get(string name)
		{
			this.VerifyIsAttachedToConfigRecord();
			if (string.IsNullOrEmpty(name))
			{
				throw ExceptionUtil.ParameterNullOrEmpty("name");
			}
			if (name.IndexOf('/') >= 0)
			{
				return null;
			}
			string text = BaseConfigurationRecord.CombineConfigKey(this._configSectionGroup.SectionGroupName, name);
			return (ConfigurationSection)this._configRecord.GetSection(text);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00011558 File Offset: 0x00010558
		public override IEnumerator GetEnumerator()
		{
			int c = this.Count;
			for (int i = 0; i < c; i++)
			{
				yield return this[i];
			}
			yield break;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00011574 File Offset: 0x00010574
		public string GetKey(int index)
		{
			return base.BaseGetKey(index);
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x0001157D File Offset: 0x0001057D
		public override NameObjectCollectionBase.KeysCollection Keys
		{
			get
			{
				return base.Keys;
			}
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x00011588 File Offset: 0x00010588
		public void Remove(string name)
		{
			this.VerifyIsAttachedToConfigRecord();
			this._configRecord.RemoveConfigurationSection(this._configSectionGroup.SectionGroupName, name);
			string text = BaseConfigurationRecord.CombineConfigKey(this._configSectionGroup.SectionGroupName, name);
			if (!this._configRecord.SectionFactories.Contains(text))
			{
				base.BaseRemove(name);
			}
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x000115DE File Offset: 0x000105DE
		public void RemoveAt(int index)
		{
			this.VerifyIsAttachedToConfigRecord();
			this.Remove(this.GetKey(index));
		}

		// Token: 0x04000296 RID: 662
		private MgmtConfigurationRecord _configRecord;

		// Token: 0x04000297 RID: 663
		private ConfigurationSectionGroup _configSectionGroup;
	}
}
