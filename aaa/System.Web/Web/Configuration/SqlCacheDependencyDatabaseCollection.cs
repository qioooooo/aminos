using System;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x0200024C RID: 588
	[ConfigurationCollection(typeof(SqlCacheDependencyDatabase))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class SqlCacheDependencyDatabaseCollection : ConfigurationElementCollection
	{
		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x06001F1E RID: 7966 RVA: 0x0008A918 File Offset: 0x00089918
		public string[] AllKeys
		{
			get
			{
				return StringUtil.ObjectArrayToStringArray(base.BaseGetAllKeys());
			}
		}

		// Token: 0x17000676 RID: 1654
		public SqlCacheDependencyDatabase this[string name]
		{
			get
			{
				return (SqlCacheDependencyDatabase)base.BaseGet(name);
			}
		}

		// Token: 0x17000677 RID: 1655
		public SqlCacheDependencyDatabase this[int index]
		{
			get
			{
				return (SqlCacheDependencyDatabase)base.BaseGet(index);
			}
			set
			{
				if (base.BaseGet(index) != null)
				{
					base.BaseRemoveAt(index);
				}
				this.BaseAdd(index, value);
			}
		}

		// Token: 0x06001F22 RID: 7970 RVA: 0x0008A95B File Offset: 0x0008995B
		protected override ConfigurationElement CreateNewElement()
		{
			return new SqlCacheDependencyDatabase();
		}

		// Token: 0x06001F23 RID: 7971 RVA: 0x0008A962 File Offset: 0x00089962
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((SqlCacheDependencyDatabase)element).Name;
		}

		// Token: 0x06001F24 RID: 7972 RVA: 0x0008A96F File Offset: 0x0008996F
		public void Add(SqlCacheDependencyDatabase name)
		{
			this.BaseAdd(name);
		}

		// Token: 0x06001F25 RID: 7973 RVA: 0x0008A978 File Offset: 0x00089978
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x06001F26 RID: 7974 RVA: 0x0008A980 File Offset: 0x00089980
		public SqlCacheDependencyDatabase Get(int index)
		{
			return (SqlCacheDependencyDatabase)base.BaseGet(index);
		}

		// Token: 0x06001F27 RID: 7975 RVA: 0x0008A98E File Offset: 0x0008998E
		public SqlCacheDependencyDatabase Get(string name)
		{
			return (SqlCacheDependencyDatabase)base.BaseGet(name);
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x0008A99C File Offset: 0x0008999C
		public string GetKey(int index)
		{
			return (string)base.BaseGetKey(index);
		}

		// Token: 0x06001F29 RID: 7977 RVA: 0x0008A9AA File Offset: 0x000899AA
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x06001F2A RID: 7978 RVA: 0x0008A9B3 File Offset: 0x000899B3
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x06001F2B RID: 7979 RVA: 0x0008A9BC File Offset: 0x000899BC
		public void Set(SqlCacheDependencyDatabase user)
		{
			base.BaseAdd(user, false);
		}

		// Token: 0x04001A42 RID: 6722
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
	}
}
