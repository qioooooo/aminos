using System;
using System.Collections.Specialized;

namespace System.Configuration.Provider
{
	// Token: 0x0200005D RID: 93
	public abstract class ProviderBase
	{
		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000398 RID: 920 RVA: 0x000128E5 File Offset: 0x000118E5
		public virtual string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000399 RID: 921 RVA: 0x000128ED File Offset: 0x000118ED
		public virtual string Description
		{
			get
			{
				if (!string.IsNullOrEmpty(this._Description))
				{
					return this._Description;
				}
				return this.Name;
			}
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0001290C File Offset: 0x0001190C
		public virtual void Initialize(string name, NameValueCollection config)
		{
			lock (this)
			{
				if (this._Initialized)
				{
					throw new InvalidOperationException(SR.GetString("Provider_Already_Initialized"));
				}
				this._Initialized = true;
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(SR.GetString("Config_provider_name_null_or_empty"), "name");
			}
			this._name = name;
			if (config != null)
			{
				this._Description = config["description"];
				config.Remove("description");
			}
		}

		// Token: 0x040002E7 RID: 743
		private string _name;

		// Token: 0x040002E8 RID: 744
		private string _Description;

		// Token: 0x040002E9 RID: 745
		private bool _Initialized;
	}
}
