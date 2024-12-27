using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;

namespace System.Configuration
{
	// Token: 0x02000030 RID: 48
	public sealed class ConfigurationLockCollection : ICollection, IEnumerable
	{
		// Token: 0x06000259 RID: 601 RVA: 0x0000F671 File Offset: 0x0000E671
		internal ConfigurationLockCollection(ConfigurationElement thisElement)
			: this(thisElement, ConfigurationLockCollectionType.LockedAttributes)
		{
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000F67B File Offset: 0x0000E67B
		internal ConfigurationLockCollection(ConfigurationElement thisElement, ConfigurationLockCollectionType lockType)
			: this(thisElement, lockType, string.Empty)
		{
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000F68A File Offset: 0x0000E68A
		internal ConfigurationLockCollection(ConfigurationElement thisElement, ConfigurationLockCollectionType lockType, string ignoreName)
			: this(thisElement, lockType, ignoreName, null)
		{
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000F698 File Offset: 0x0000E698
		internal ConfigurationLockCollection(ConfigurationElement thisElement, ConfigurationLockCollectionType lockType, string ignoreName, ConfigurationLockCollection parentCollection)
		{
			this._thisElement = thisElement;
			this._lockType = lockType;
			this.internalDictionary = new HybridDictionary();
			this.internalArraylist = new ArrayList();
			this._bModified = false;
			this._bExceptionList = this._lockType == ConfigurationLockCollectionType.LockedExceptionList || this._lockType == ConfigurationLockCollectionType.LockedElementsExceptionList;
			this._ignoreName = ignoreName;
			if (parentCollection != null)
			{
				foreach (object obj in parentCollection)
				{
					string text = (string)obj;
					this.Add(text, ConfigurationValueFlags.Inherited);
					if (this._bExceptionList)
					{
						if (this.SeedList.Length != 0)
						{
							this.SeedList += ",";
						}
						this.SeedList += text;
					}
				}
			}
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000F798 File Offset: 0x0000E798
		internal void ClearSeedList()
		{
			this.SeedList = string.Empty;
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600025E RID: 606 RVA: 0x0000F7A5 File Offset: 0x0000E7A5
		internal ConfigurationLockCollectionType LockType
		{
			get
			{
				return this._lockType;
			}
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000F7B0 File Offset: 0x0000E7B0
		public void Add(string name)
		{
			if ((this._thisElement.ItemLocked & ConfigurationValueFlags.Locked) != ConfigurationValueFlags.Default && (this._thisElement.ItemLocked & ConfigurationValueFlags.Inherited) != ConfigurationValueFlags.Default)
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_attribute_locked", new object[] { name }));
			}
			ConfigurationValueFlags configurationValueFlags = ConfigurationValueFlags.Modified;
			string text = name.Trim();
			ConfigurationProperty configurationProperty = this._thisElement.Properties[text];
			if (configurationProperty == null && text != "*")
			{
				ConfigurationElementCollection configurationElementCollection = this._thisElement as ConfigurationElementCollection;
				if (configurationElementCollection == null && this._thisElement.Properties.DefaultCollectionProperty != null)
				{
					configurationElementCollection = this._thisElement[this._thisElement.Properties.DefaultCollectionProperty] as ConfigurationElementCollection;
				}
				if (configurationElementCollection == null || this._lockType == ConfigurationLockCollectionType.LockedAttributes || this._lockType == ConfigurationLockCollectionType.LockedExceptionList)
				{
					this._thisElement.ReportInvalidLock(text, this._lockType, null, null);
				}
				else if (!configurationElementCollection.IsLockableElement(text))
				{
					this._thisElement.ReportInvalidLock(text, this._lockType, null, configurationElementCollection.LockableElements);
				}
			}
			else
			{
				if (configurationProperty != null && configurationProperty.IsRequired)
				{
					throw new ConfigurationErrorsException(SR.GetString("Config_base_required_attribute_lock_attempt", new object[] { configurationProperty.Name }));
				}
				if (text != "*")
				{
					if (this._lockType == ConfigurationLockCollectionType.LockedElements || this._lockType == ConfigurationLockCollectionType.LockedElementsExceptionList)
					{
						if (!typeof(ConfigurationElement).IsAssignableFrom(configurationProperty.Type))
						{
							this._thisElement.ReportInvalidLock(text, this._lockType, null, null);
						}
					}
					else if (typeof(ConfigurationElement).IsAssignableFrom(configurationProperty.Type))
					{
						this._thisElement.ReportInvalidLock(text, this._lockType, null, null);
					}
				}
			}
			if (this.internalDictionary.Contains(name))
			{
				configurationValueFlags = ConfigurationValueFlags.Modified | (ConfigurationValueFlags)this.internalDictionary[name];
				this.internalDictionary.Remove(name);
				this.internalArraylist.Remove(name);
			}
			this.internalDictionary.Add(name, configurationValueFlags);
			this.internalArraylist.Add(name);
			this._bModified = true;
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000F9CC File Offset: 0x0000E9CC
		internal void Add(string name, ConfigurationValueFlags flags)
		{
			if (flags != ConfigurationValueFlags.Inherited && this.internalDictionary.Contains(name))
			{
				flags = ConfigurationValueFlags.Modified | (ConfigurationValueFlags)this.internalDictionary[name];
				this.internalDictionary.Remove(name);
				this.internalArraylist.Remove(name);
			}
			this.internalDictionary.Add(name, flags);
			this.internalArraylist.Add(name);
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000FA38 File Offset: 0x0000EA38
		internal bool DefinedInParent(string name)
		{
			if (name == null)
			{
				return false;
			}
			if (this._bExceptionList)
			{
				string text = "," + this.SeedList + ",";
				if (name.Equals(this._ignoreName) || text.IndexOf("," + name + ",", StringComparison.Ordinal) >= 0)
				{
					return true;
				}
			}
			return this.internalDictionary.Contains(name) && ((ConfigurationValueFlags)this.internalDictionary[name] & ConfigurationValueFlags.Inherited) != ConfigurationValueFlags.Default;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000FABB File Offset: 0x0000EABB
		internal bool IsValueModified(string name)
		{
			return this.internalDictionary.Contains(name) && ((ConfigurationValueFlags)this.internalDictionary[name] & ConfigurationValueFlags.Modified) != ConfigurationValueFlags.Default;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000FAE8 File Offset: 0x0000EAE8
		internal void RemoveInheritedLocks()
		{
			StringCollection stringCollection = new StringCollection();
			foreach (object obj in this)
			{
				string text = (string)obj;
				if (this.DefinedInParent(text))
				{
					stringCollection.Add(text);
				}
			}
			foreach (string text2 in stringCollection)
			{
				this.internalDictionary.Remove(text2);
				this.internalArraylist.Remove(text2);
			}
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000FBA8 File Offset: 0x0000EBA8
		public void Remove(string name)
		{
			if (!this.internalDictionary.Contains(name))
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_collection_entry_not_found", new object[] { name }));
			}
			if (this._bExceptionList || ((ConfigurationValueFlags)this.internalDictionary[name] & ConfigurationValueFlags.Inherited) == ConfigurationValueFlags.Default)
			{
				this.internalDictionary.Remove(name);
				this.internalArraylist.Remove(name);
				this._bModified = true;
				return;
			}
			if (((ConfigurationValueFlags)this.internalDictionary[name] & ConfigurationValueFlags.Modified) != ConfigurationValueFlags.Default)
			{
				ConfigurationValueFlags configurationValueFlags = (ConfigurationValueFlags)this.internalDictionary[name];
				configurationValueFlags &= ~ConfigurationValueFlags.Modified;
				this.internalDictionary[name] = configurationValueFlags;
				this._bModified = true;
				return;
			}
			throw new ConfigurationErrorsException(SR.GetString("Config_base_attribute_locked", new object[] { name }));
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000FC7D File Offset: 0x0000EC7D
		public IEnumerator GetEnumerator()
		{
			return this.internalArraylist.GetEnumerator();
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000FC8C File Offset: 0x0000EC8C
		internal void ClearInternal(bool useSeedIfAvailble)
		{
			ArrayList arrayList = new ArrayList();
			foreach (object obj in this.internalDictionary)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				if (((ConfigurationValueFlags)dictionaryEntry.Value & ConfigurationValueFlags.Inherited) == ConfigurationValueFlags.Default || this._bExceptionList)
				{
					arrayList.Add(dictionaryEntry.Key);
				}
			}
			foreach (object obj2 in arrayList)
			{
				this.internalDictionary.Remove(obj2);
				this.internalArraylist.Remove(obj2);
			}
			if (useSeedIfAvailble && !string.IsNullOrEmpty(this.SeedList))
			{
				string[] array = this.SeedList.Split(new char[] { ',' });
				foreach (string text in array)
				{
					this.Add(text, ConfigurationValueFlags.Inherited);
				}
			}
			this._bModified = true;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000FDC0 File Offset: 0x0000EDC0
		public void Clear()
		{
			this.ClearInternal(true);
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000FDC9 File Offset: 0x0000EDC9
		public bool Contains(string name)
		{
			return (this._bExceptionList && name.Equals(this._ignoreName)) || this.internalDictionary.Contains(name);
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000269 RID: 617 RVA: 0x0000FDEF File Offset: 0x0000EDEF
		public int Count
		{
			get
			{
				return this.internalDictionary.Count;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600026A RID: 618 RVA: 0x0000FDFC File Offset: 0x0000EDFC
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600026B RID: 619 RVA: 0x0000FDFF File Offset: 0x0000EDFF
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000FE02 File Offset: 0x0000EE02
		public void CopyTo(string[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000FE0C File Offset: 0x0000EE0C
		void ICollection.CopyTo(Array array, int index)
		{
			this.internalArraylist.CopyTo(array, index);
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x0600026E RID: 622 RVA: 0x0000FE1B File Offset: 0x0000EE1B
		public bool IsModified
		{
			get
			{
				return this._bModified;
			}
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000FE23 File Offset: 0x0000EE23
		internal void ResetModified()
		{
			this._bModified = false;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000FE2C File Offset: 0x0000EE2C
		public bool IsReadOnly(string name)
		{
			if (!this.internalDictionary.Contains(name))
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_base_collection_entry_not_found", new object[] { name }));
			}
			return ((ConfigurationValueFlags)this.internalDictionary[name] & ConfigurationValueFlags.Inherited) != ConfigurationValueFlags.Default;
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0000FE7C File Offset: 0x0000EE7C
		internal bool ExceptionList
		{
			get
			{
				return this._bExceptionList;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000272 RID: 626 RVA: 0x0000FE84 File Offset: 0x0000EE84
		public string AttributeList
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (object obj in this.internalDictionary)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(',');
					}
					stringBuilder.Append(dictionaryEntry.Key);
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000FF10 File Offset: 0x0000EF10
		public void SetFromList(string attributeList)
		{
			string[] array = attributeList.Split(new char[] { ',', ';', ':' });
			this.Clear();
			foreach (string text in array)
			{
				string text2 = text.Trim();
				if (!this.Contains(text2))
				{
					this.Add(text2);
				}
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000274 RID: 628 RVA: 0x0000FF6C File Offset: 0x0000EF6C
		public bool HasParentElements
		{
			get
			{
				bool flag = false;
				if (this.ExceptionList && this.internalDictionary.Count == 0 && !string.IsNullOrEmpty(this.SeedList))
				{
					return true;
				}
				foreach (object obj in this.internalDictionary)
				{
					if (((ConfigurationValueFlags)((DictionaryEntry)obj).Value & ConfigurationValueFlags.Inherited) != ConfigurationValueFlags.Default)
					{
						flag = true;
						break;
					}
				}
				return flag;
			}
		}

		// Token: 0x0400025D RID: 605
		private const string LockAll = "*";

		// Token: 0x0400025E RID: 606
		private HybridDictionary internalDictionary;

		// Token: 0x0400025F RID: 607
		private ArrayList internalArraylist;

		// Token: 0x04000260 RID: 608
		private bool _bModified;

		// Token: 0x04000261 RID: 609
		private bool _bExceptionList;

		// Token: 0x04000262 RID: 610
		private string _ignoreName = string.Empty;

		// Token: 0x04000263 RID: 611
		private ConfigurationElement _thisElement;

		// Token: 0x04000264 RID: 612
		private ConfigurationLockCollectionType _lockType;

		// Token: 0x04000265 RID: 613
		private string SeedList = string.Empty;
	}
}
