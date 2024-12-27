using System;

namespace System.ComponentModel.Design.Data
{
	// Token: 0x02000144 RID: 324
	public sealed class DesignerDataConnection
	{
		// Token: 0x06000C87 RID: 3207 RVA: 0x00030904 File Offset: 0x0002F904
		public DesignerDataConnection(string name, string providerName, string connectionString)
			: this(name, providerName, connectionString, false)
		{
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x00030910 File Offset: 0x0002F910
		public DesignerDataConnection(string name, string providerName, string connectionString, bool isConfigured)
		{
			this._name = name;
			this._providerName = providerName;
			this._connectionString = connectionString;
			this._isConfigured = isConfigured;
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000C89 RID: 3209 RVA: 0x00030935 File Offset: 0x0002F935
		public string ConnectionString
		{
			get
			{
				return this._connectionString;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000C8A RID: 3210 RVA: 0x0003093D File Offset: 0x0002F93D
		public bool IsConfigured
		{
			get
			{
				return this._isConfigured;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000C8B RID: 3211 RVA: 0x00030945 File Offset: 0x0002F945
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000C8C RID: 3212 RVA: 0x0003094D File Offset: 0x0002F94D
		public string ProviderName
		{
			get
			{
				return this._providerName;
			}
		}

		// Token: 0x04000EA3 RID: 3747
		private string _connectionString;

		// Token: 0x04000EA4 RID: 3748
		private bool _isConfigured;

		// Token: 0x04000EA5 RID: 3749
		private string _name;

		// Token: 0x04000EA6 RID: 3750
		private string _providerName;
	}
}
