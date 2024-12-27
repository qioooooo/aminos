using System;
using System.ComponentModel;

namespace System.Configuration
{
	// Token: 0x020006E5 RID: 1765
	public class SettingChangingEventArgs : CancelEventArgs
	{
		// Token: 0x06003682 RID: 13954 RVA: 0x000E8BF4 File Offset: 0x000E7BF4
		public SettingChangingEventArgs(string settingName, string settingClass, string settingKey, object newValue, bool cancel)
			: base(cancel)
		{
			this._settingName = settingName;
			this._settingClass = settingClass;
			this._settingKey = settingKey;
			this._newValue = newValue;
		}

		// Token: 0x17000C98 RID: 3224
		// (get) Token: 0x06003683 RID: 13955 RVA: 0x000E8C1B File Offset: 0x000E7C1B
		public object NewValue
		{
			get
			{
				return this._newValue;
			}
		}

		// Token: 0x17000C99 RID: 3225
		// (get) Token: 0x06003684 RID: 13956 RVA: 0x000E8C23 File Offset: 0x000E7C23
		public string SettingClass
		{
			get
			{
				return this._settingClass;
			}
		}

		// Token: 0x17000C9A RID: 3226
		// (get) Token: 0x06003685 RID: 13957 RVA: 0x000E8C2B File Offset: 0x000E7C2B
		public string SettingName
		{
			get
			{
				return this._settingName;
			}
		}

		// Token: 0x17000C9B RID: 3227
		// (get) Token: 0x06003686 RID: 13958 RVA: 0x000E8C33 File Offset: 0x000E7C33
		public string SettingKey
		{
			get
			{
				return this._settingKey;
			}
		}

		// Token: 0x04003183 RID: 12675
		private string _settingClass;

		// Token: 0x04003184 RID: 12676
		private string _settingName;

		// Token: 0x04003185 RID: 12677
		private string _settingKey;

		// Token: 0x04003186 RID: 12678
		private object _newValue;
	}
}
