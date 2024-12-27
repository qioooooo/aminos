using System;
using System.Collections.Specialized;

namespace System.Configuration
{
	// Token: 0x02000075 RID: 117
	internal class KeyValueInternalCollection : NameValueCollection
	{
		// Token: 0x0600044A RID: 1098 RVA: 0x00014304 File Offset: 0x00013304
		public KeyValueInternalCollection(AppSettingsSection root)
		{
			this._root = root;
			foreach (object obj in this._root.Settings)
			{
				KeyValueConfigurationElement keyValueConfigurationElement = (KeyValueConfigurationElement)obj;
				base.Add(keyValueConfigurationElement.Key, keyValueConfigurationElement.Value);
			}
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x0001437C File Offset: 0x0001337C
		public override void Add(string key, string value)
		{
			this._root.Settings.Add(new KeyValueConfigurationElement(key, value));
			base.Add(key, value);
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x0001439D File Offset: 0x0001339D
		public override void Clear()
		{
			this._root.Settings.Clear();
			base.Clear();
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x000143B5 File Offset: 0x000133B5
		public override void Remove(string key)
		{
			this._root.Settings.Remove(key);
			base.Remove(key);
		}

		// Token: 0x0400032E RID: 814
		private AppSettingsSection _root;
	}
}
