using System;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x0200003F RID: 63
	[Serializable]
	public sealed class ConfigurationSectionGroupCollection : NameObjectCollectionBase
	{
		// Token: 0x060002F8 RID: 760 RVA: 0x000118F0 File Offset: 0x000108F0
		internal ConfigurationSectionGroupCollection(MgmtConfigurationRecord configRecord, ConfigurationSectionGroup configSectionGroup)
			: base(StringComparer.Ordinal)
		{
			this._configRecord = configRecord;
			this._configSectionGroup = configSectionGroup;
			foreach (object obj in this._configRecord.SectionGroupFactories)
			{
				FactoryId factoryId = (FactoryId)((DictionaryEntry)obj).Value;
				if (factoryId.Group == this._configSectionGroup.SectionGroupName)
				{
					base.BaseAdd(factoryId.Name, factoryId.Name);
				}
			}
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x00011998 File Offset: 0x00010998
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x000119A2 File Offset: 0x000109A2
		internal void DetachFromConfigurationRecord()
		{
			this._configRecord = null;
			base.BaseClear();
		}

		// Token: 0x060002FB RID: 763 RVA: 0x000119B1 File Offset: 0x000109B1
		private void VerifyIsAttachedToConfigRecord()
		{
			if (this._configRecord == null)
			{
				throw new InvalidOperationException(SR.GetString("Config_cannot_edit_configurationsectiongroup_when_not_attached"));
			}
		}

		// Token: 0x170000C4 RID: 196
		public ConfigurationSectionGroup this[string name]
		{
			get
			{
				return this.Get(name);
			}
		}

		// Token: 0x170000C5 RID: 197
		public ConfigurationSectionGroup this[int index]
		{
			get
			{
				return this.Get(index);
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x000119DD File Offset: 0x000109DD
		public void Add(string name, ConfigurationSectionGroup sectionGroup)
		{
			this.VerifyIsAttachedToConfigRecord();
			this._configRecord.AddConfigurationSectionGroup(this._configSectionGroup.SectionGroupName, name, sectionGroup);
			base.BaseAdd(name, name);
		}

		// Token: 0x060002FF RID: 767 RVA: 0x00011A08 File Offset: 0x00010A08
		public void Clear()
		{
			this.VerifyIsAttachedToConfigRecord();
			if (this._configSectionGroup.IsRoot)
			{
				this._configRecord.RemoveLocationWriteRequirement();
			}
			string[] array = base.BaseGetAllKeys();
			foreach (string text in array)
			{
				this.Remove(text);
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000300 RID: 768 RVA: 0x00011A55 File Offset: 0x00010A55
		public override int Count
		{
			get
			{
				return base.Count;
			}
		}

		// Token: 0x06000301 RID: 769 RVA: 0x00011A60 File Offset: 0x00010A60
		public void CopyTo(ConfigurationSectionGroup[] array, int index)
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

		// Token: 0x06000302 RID: 770 RVA: 0x00011AB1 File Offset: 0x00010AB1
		public ConfigurationSectionGroup Get(int index)
		{
			return this.Get(this.GetKey(index));
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00011AC0 File Offset: 0x00010AC0
		public ConfigurationSectionGroup Get(string name)
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
			return this._configRecord.GetSectionGroup(text);
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00011BC8 File Offset: 0x00010BC8
		public override IEnumerator GetEnumerator()
		{
			int c = this.Count;
			for (int i = 0; i < c; i++)
			{
				yield return this[i];
			}
			yield break;
		}

		// Token: 0x06000305 RID: 773 RVA: 0x00011BE4 File Offset: 0x00010BE4
		public string GetKey(int index)
		{
			return base.BaseGetKey(index);
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000306 RID: 774 RVA: 0x00011BED File Offset: 0x00010BED
		public override NameObjectCollectionBase.KeysCollection Keys
		{
			get
			{
				return base.Keys;
			}
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00011BF8 File Offset: 0x00010BF8
		public void Remove(string name)
		{
			this.VerifyIsAttachedToConfigRecord();
			this._configRecord.RemoveConfigurationSectionGroup(this._configSectionGroup.SectionGroupName, name);
			string text = BaseConfigurationRecord.CombineConfigKey(this._configSectionGroup.SectionGroupName, name);
			if (!this._configRecord.SectionFactories.Contains(text))
			{
				base.BaseRemove(name);
			}
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00011C4E File Offset: 0x00010C4E
		public void RemoveAt(int index)
		{
			this.VerifyIsAttachedToConfigRecord();
			this.Remove(this.GetKey(index));
		}

		// Token: 0x040002A2 RID: 674
		private MgmtConfigurationRecord _configRecord;

		// Token: 0x040002A3 RID: 675
		private ConfigurationSectionGroup _configSectionGroup;
	}
}
