using System;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x02000219 RID: 537
	[ConfigurationCollection(typeof(OutputCacheProfile))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class OutputCacheProfileCollection : ConfigurationElementCollection
	{
		// Token: 0x170005A7 RID: 1447
		// (get) Token: 0x06001CCF RID: 7375 RVA: 0x0008380D File Offset: 0x0008280D
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return OutputCacheProfileCollection._properties;
			}
		}

		// Token: 0x06001CD0 RID: 7376 RVA: 0x00083814 File Offset: 0x00082814
		public OutputCacheProfileCollection()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		// Token: 0x170005A8 RID: 1448
		// (get) Token: 0x06001CD1 RID: 7377 RVA: 0x00083821 File Offset: 0x00082821
		public string[] AllKeys
		{
			get
			{
				return StringUtil.ObjectArrayToStringArray(base.BaseGetAllKeys());
			}
		}

		// Token: 0x170005A9 RID: 1449
		public OutputCacheProfile this[string name]
		{
			get
			{
				return (OutputCacheProfile)base.BaseGet(name);
			}
		}

		// Token: 0x170005AA RID: 1450
		public OutputCacheProfile this[int index]
		{
			get
			{
				return (OutputCacheProfile)base.BaseGet(index);
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

		// Token: 0x06001CD5 RID: 7381 RVA: 0x00083864 File Offset: 0x00082864
		protected override ConfigurationElement CreateNewElement()
		{
			return new OutputCacheProfile();
		}

		// Token: 0x06001CD6 RID: 7382 RVA: 0x0008386B File Offset: 0x0008286B
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((OutputCacheProfile)element).Name;
		}

		// Token: 0x06001CD7 RID: 7383 RVA: 0x00083878 File Offset: 0x00082878
		public void Add(OutputCacheProfile name)
		{
			this.BaseAdd(name);
		}

		// Token: 0x06001CD8 RID: 7384 RVA: 0x00083881 File Offset: 0x00082881
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x06001CD9 RID: 7385 RVA: 0x00083889 File Offset: 0x00082889
		public OutputCacheProfile Get(int index)
		{
			return (OutputCacheProfile)base.BaseGet(index);
		}

		// Token: 0x06001CDA RID: 7386 RVA: 0x00083897 File Offset: 0x00082897
		public OutputCacheProfile Get(string name)
		{
			return (OutputCacheProfile)base.BaseGet(name);
		}

		// Token: 0x06001CDB RID: 7387 RVA: 0x000838A5 File Offset: 0x000828A5
		public string GetKey(int index)
		{
			return (string)base.BaseGetKey(index);
		}

		// Token: 0x06001CDC RID: 7388 RVA: 0x000838B3 File Offset: 0x000828B3
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x000838BC File Offset: 0x000828BC
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x06001CDE RID: 7390 RVA: 0x000838C5 File Offset: 0x000828C5
		public void Set(OutputCacheProfile user)
		{
			base.BaseAdd(user, false);
		}

		// Token: 0x0400190B RID: 6411
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
	}
}
