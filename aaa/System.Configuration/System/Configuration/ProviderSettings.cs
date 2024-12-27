using System;
using System.Collections;
using System.Collections.Specialized;

namespace System.Configuration
{
	// Token: 0x0200008D RID: 141
	public sealed class ProviderSettings : ConfigurationElement
	{
		// Token: 0x06000521 RID: 1313 RVA: 0x00019A74 File Offset: 0x00018A74
		public ProviderSettings()
		{
			this._properties = new ConfigurationPropertyCollection();
			this._properties.Add(this._propName);
			this._properties.Add(this._propType);
			this._PropertyNameCollection = null;
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00019AFD File Offset: 0x00018AFD
		public ProviderSettings(string name, string type)
			: this()
		{
			this.Name = name;
			this.Type = type;
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x00019B13 File Offset: 0x00018B13
		protected internal override ConfigurationPropertyCollection Properties
		{
			get
			{
				this.UpdatePropertyCollection();
				return this._properties;
			}
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00019B24 File Offset: 0x00018B24
		protected internal override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
		{
			ProviderSettings providerSettings = parentElement as ProviderSettings;
			if (providerSettings != null)
			{
				providerSettings.UpdatePropertyCollection();
			}
			ProviderSettings providerSettings2 = sourceElement as ProviderSettings;
			if (providerSettings2 != null)
			{
				providerSettings2.UpdatePropertyCollection();
			}
			base.Unmerge(sourceElement, parentElement, saveMode);
			this.UpdatePropertyCollection();
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00019B64 File Offset: 0x00018B64
		protected internal override void Reset(ConfigurationElement parentElement)
		{
			ProviderSettings providerSettings = parentElement as ProviderSettings;
			if (providerSettings != null)
			{
				providerSettings.UpdatePropertyCollection();
			}
			base.Reset(parentElement);
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00019B8C File Offset: 0x00018B8C
		internal bool UpdatePropertyCollection()
		{
			bool flag = false;
			ArrayList arrayList = null;
			if (this._PropertyNameCollection != null)
			{
				foreach (object obj in this._properties)
				{
					ConfigurationProperty configurationProperty = (ConfigurationProperty)obj;
					if (configurationProperty.Name != "name" && configurationProperty.Name != "type" && this._PropertyNameCollection.Get(configurationProperty.Name) == null)
					{
						if (arrayList == null)
						{
							arrayList = new ArrayList();
						}
						if ((base.Values.GetConfigValue(configurationProperty.Name).ValueFlags & ConfigurationValueFlags.Locked) == ConfigurationValueFlags.Default)
						{
							arrayList.Add(configurationProperty.Name);
							flag = true;
						}
					}
				}
				if (arrayList != null)
				{
					foreach (object obj2 in arrayList)
					{
						string text = (string)obj2;
						this._properties.Remove(text);
					}
				}
				foreach (object obj3 in this._PropertyNameCollection)
				{
					string text2 = (string)obj3;
					string text3 = this._PropertyNameCollection[text2];
					string property = this.GetProperty(text2);
					if (property == null || text3 != property)
					{
						this.SetProperty(text2, text3);
						flag = true;
					}
				}
			}
			this._PropertyNameCollection = null;
			return flag;
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00019D38 File Offset: 0x00018D38
		protected internal override bool IsModified()
		{
			return this.UpdatePropertyCollection() || base.IsModified();
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x00019D4A File Offset: 0x00018D4A
		// (set) Token: 0x06000529 RID: 1321 RVA: 0x00019D5D File Offset: 0x00018D5D
		[ConfigurationProperty("name", IsRequired = true, IsKey = true)]
		public string Name
		{
			get
			{
				return (string)base[this._propName];
			}
			set
			{
				base[this._propName] = value;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x00019D6C File Offset: 0x00018D6C
		// (set) Token: 0x0600052B RID: 1323 RVA: 0x00019D7F File Offset: 0x00018D7F
		[ConfigurationProperty("type", IsRequired = true)]
		public string Type
		{
			get
			{
				return (string)base[this._propType];
			}
			set
			{
				base[this._propType] = value;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x00019D90 File Offset: 0x00018D90
		public NameValueCollection Parameters
		{
			get
			{
				if (this._PropertyNameCollection == null)
				{
					lock (this)
					{
						if (this._PropertyNameCollection == null)
						{
							this._PropertyNameCollection = new NameValueCollection(StringComparer.Ordinal);
							foreach (object obj in this._properties)
							{
								ConfigurationProperty configurationProperty = (ConfigurationProperty)obj;
								if (configurationProperty.Name != "name" && configurationProperty.Name != "type")
								{
									this._PropertyNameCollection.Add(configurationProperty.Name, (string)base[configurationProperty]);
								}
							}
						}
					}
				}
				return this._PropertyNameCollection;
			}
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00019E74 File Offset: 0x00018E74
		private string GetProperty(string PropName)
		{
			if (this._properties.Contains(PropName))
			{
				ConfigurationProperty configurationProperty = this._properties[PropName];
				if (configurationProperty != null)
				{
					return (string)base[configurationProperty];
				}
			}
			return null;
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00019EB0 File Offset: 0x00018EB0
		private bool SetProperty(string PropName, string value)
		{
			ConfigurationProperty configurationProperty;
			if (this._properties.Contains(PropName))
			{
				configurationProperty = this._properties[PropName];
			}
			else
			{
				configurationProperty = new ConfigurationProperty(PropName, typeof(string), null);
				this._properties.Add(configurationProperty);
			}
			if (configurationProperty != null)
			{
				base[configurationProperty] = value;
				return true;
			}
			return false;
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x00019F08 File Offset: 0x00018F08
		protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
		{
			ConfigurationProperty configurationProperty = new ConfigurationProperty(name, typeof(string), value);
			this._properties.Add(configurationProperty);
			base[configurationProperty] = value;
			this.Parameters[name] = value;
			return true;
		}

		// Token: 0x04000374 RID: 884
		private readonly ConfigurationProperty _propName = new ConfigurationProperty("name", typeof(string), null, null, ConfigurationProperty.NonEmptyStringValidator, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x04000375 RID: 885
		private readonly ConfigurationProperty _propType = new ConfigurationProperty("type", typeof(string), "", ConfigurationPropertyOptions.IsRequired);

		// Token: 0x04000376 RID: 886
		private ConfigurationPropertyCollection _properties;

		// Token: 0x04000377 RID: 887
		private NameValueCollection _PropertyNameCollection;
	}
}
