using System;
using System.Collections;
using System.Collections.Specialized;

namespace System.Configuration
{
	// Token: 0x02000043 RID: 67
	internal class ConfigurationValues : NameObjectCollectionBase
	{
		// Token: 0x0600030A RID: 778 RVA: 0x00011C80 File Offset: 0x00010C80
		internal ConfigurationValues()
			: base(StringComparer.Ordinal)
		{
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00011C90 File Offset: 0x00010C90
		internal void AssociateContext(BaseConfigurationRecord configRecord)
		{
			this._configRecord = configRecord;
			foreach (object obj in this.ConfigurationElements)
			{
				ConfigurationElement configurationElement = (ConfigurationElement)obj;
				configurationElement.AssociateContext(this._configRecord);
			}
		}

		// Token: 0x0600030C RID: 780 RVA: 0x00011CF8 File Offset: 0x00010CF8
		internal bool Contains(string key)
		{
			return base.BaseGet(key) != null;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00011D07 File Offset: 0x00010D07
		internal string GetKey(int index)
		{
			return base.BaseGetKey(index);
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00011D10 File Offset: 0x00010D10
		internal ConfigurationValue GetConfigValue(string key)
		{
			return (ConfigurationValue)base.BaseGet(key);
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00011D1E File Offset: 0x00010D1E
		internal ConfigurationValue GetConfigValue(int index)
		{
			return (ConfigurationValue)base.BaseGet(index);
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00011D2C File Offset: 0x00010D2C
		internal PropertySourceInfo GetSourceInfo(string key)
		{
			ConfigurationValue configValue = this.GetConfigValue(key);
			if (configValue != null)
			{
				return configValue.SourceInfo;
			}
			return null;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00011D4C File Offset: 0x00010D4C
		internal void ChangeSourceInfo(string key, PropertySourceInfo sourceInfo)
		{
			ConfigurationValue configValue = this.GetConfigValue(key);
			if (configValue != null)
			{
				configValue.SourceInfo = sourceInfo;
			}
		}

		// Token: 0x06000312 RID: 786 RVA: 0x00011D6C File Offset: 0x00010D6C
		private ConfigurationValue CreateConfigValue(object value, ConfigurationValueFlags valueFlags, PropertySourceInfo sourceInfo)
		{
			if (value != null)
			{
				if (value is ConfigurationElement)
				{
					this._containsElement = true;
					((ConfigurationElement)value).AssociateContext(this._configRecord);
				}
				else if (value is InvalidPropValue)
				{
					this._containsInvalidValue = true;
				}
			}
			return new ConfigurationValue(value, valueFlags, sourceInfo);
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00011DB8 File Offset: 0x00010DB8
		internal void SetValue(string key, object value, ConfigurationValueFlags valueFlags, PropertySourceInfo sourceInfo)
		{
			ConfigurationValue configurationValue = this.CreateConfigValue(value, valueFlags, sourceInfo);
			base.BaseSet(key, configurationValue);
		}

		// Token: 0x170000C8 RID: 200
		internal object this[string key]
		{
			get
			{
				ConfigurationValue configValue = this.GetConfigValue(key);
				if (configValue != null)
				{
					return configValue.Value;
				}
				return null;
			}
			set
			{
				this.SetValue(key, value, ConfigurationValueFlags.Modified, null);
			}
		}

		// Token: 0x170000C9 RID: 201
		internal object this[int index]
		{
			get
			{
				ConfigurationValue configValue = this.GetConfigValue(index);
				if (configValue != null)
				{
					return configValue.Value;
				}
				return null;
			}
		}

		// Token: 0x06000317 RID: 791 RVA: 0x00011E24 File Offset: 0x00010E24
		internal void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000318 RID: 792 RVA: 0x00011E2C File Offset: 0x00010E2C
		internal object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06000319 RID: 793 RVA: 0x00011E30 File Offset: 0x00010E30
		internal ConfigurationValueFlags RetrieveFlags(string key)
		{
			ConfigurationValue configurationValue = (ConfigurationValue)base.BaseGet(key);
			if (configurationValue != null)
			{
				return configurationValue.ValueFlags;
			}
			return ConfigurationValueFlags.Default;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00011E58 File Offset: 0x00010E58
		internal bool IsModified(string key)
		{
			ConfigurationValue configurationValue = (ConfigurationValue)base.BaseGet(key);
			return configurationValue != null && (configurationValue.ValueFlags & ConfigurationValueFlags.Modified) != ConfigurationValueFlags.Default;
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00011E88 File Offset: 0x00010E88
		internal bool IsInherited(string key)
		{
			ConfigurationValue configurationValue = (ConfigurationValue)base.BaseGet(key);
			return configurationValue != null && (configurationValue.ValueFlags & ConfigurationValueFlags.Inherited) != ConfigurationValueFlags.Default;
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600031C RID: 796 RVA: 0x00011EB5 File Offset: 0x00010EB5
		internal IEnumerable ConfigurationElements
		{
			get
			{
				if (this._containsElement)
				{
					return new ConfigurationValues.ConfigurationElementsCollection(this);
				}
				return ConfigurationValues.EmptyCollectionInstance;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600031D RID: 797 RVA: 0x00011ECB File Offset: 0x00010ECB
		internal IEnumerable InvalidValues
		{
			get
			{
				if (this._containsInvalidValue)
				{
					return new ConfigurationValues.InvalidValuesCollection(this);
				}
				return ConfigurationValues.EmptyCollectionInstance;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600031E RID: 798 RVA: 0x00011EE1 File Offset: 0x00010EE1
		private static IEnumerable EmptyCollectionInstance
		{
			get
			{
				if (ConfigurationValues.s_emptyCollection == null)
				{
					ConfigurationValues.s_emptyCollection = new ConfigurationValues.EmptyCollection();
				}
				return ConfigurationValues.s_emptyCollection;
			}
		}

		// Token: 0x040002B1 RID: 689
		private BaseConfigurationRecord _configRecord;

		// Token: 0x040002B2 RID: 690
		private bool _containsElement;

		// Token: 0x040002B3 RID: 691
		private bool _containsInvalidValue;

		// Token: 0x040002B4 RID: 692
		private static IEnumerable s_emptyCollection;

		// Token: 0x02000044 RID: 68
		private class EmptyCollection : IEnumerable
		{
			// Token: 0x0600031F RID: 799 RVA: 0x00011EF9 File Offset: 0x00010EF9
			internal EmptyCollection()
			{
				this._emptyEnumerator = new ConfigurationValues.EmptyCollection.EmptyCollectionEnumerator();
			}

			// Token: 0x06000320 RID: 800 RVA: 0x00011F0C File Offset: 0x00010F0C
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this._emptyEnumerator;
			}

			// Token: 0x040002B5 RID: 693
			private IEnumerator _emptyEnumerator;

			// Token: 0x02000045 RID: 69
			private class EmptyCollectionEnumerator : IEnumerator
			{
				// Token: 0x06000321 RID: 801 RVA: 0x00011F14 File Offset: 0x00010F14
				bool IEnumerator.MoveNext()
				{
					return false;
				}

				// Token: 0x170000CE RID: 206
				// (get) Token: 0x06000322 RID: 802 RVA: 0x00011F17 File Offset: 0x00010F17
				object IEnumerator.Current
				{
					get
					{
						return null;
					}
				}

				// Token: 0x06000323 RID: 803 RVA: 0x00011F1A File Offset: 0x00010F1A
				void IEnumerator.Reset()
				{
				}
			}
		}

		// Token: 0x02000046 RID: 70
		private class ConfigurationElementsCollection : IEnumerable
		{
			// Token: 0x06000325 RID: 805 RVA: 0x00011F24 File Offset: 0x00010F24
			internal ConfigurationElementsCollection(ConfigurationValues values)
			{
				this._values = values;
			}

			// Token: 0x06000326 RID: 806 RVA: 0x00012014 File Offset: 0x00011014
			IEnumerator IEnumerable.GetEnumerator()
			{
				if (this._values._containsElement)
				{
					for (int index = 0; index < this._values.Count; index++)
					{
						object value = this._values[index];
						if (value is ConfigurationElement)
						{
							yield return value;
						}
					}
				}
				yield break;
			}

			// Token: 0x040002B6 RID: 694
			private ConfigurationValues _values;
		}

		// Token: 0x02000047 RID: 71
		private class InvalidValuesCollection : IEnumerable
		{
			// Token: 0x06000327 RID: 807 RVA: 0x00012030 File Offset: 0x00011030
			internal InvalidValuesCollection(ConfigurationValues values)
			{
				this._values = values;
			}

			// Token: 0x06000328 RID: 808 RVA: 0x00012120 File Offset: 0x00011120
			IEnumerator IEnumerable.GetEnumerator()
			{
				if (this._values._containsInvalidValue)
				{
					for (int index = 0; index < this._values.Count; index++)
					{
						object value = this._values[index];
						if (value is InvalidPropValue)
						{
							yield return value;
						}
					}
				}
				yield break;
			}

			// Token: 0x040002B7 RID: 695
			private ConfigurationValues _values;
		}
	}
}
