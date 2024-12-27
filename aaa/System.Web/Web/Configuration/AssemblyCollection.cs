using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x0200019D RID: 413
	[ConfigurationCollection(typeof(AssemblyInfo))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class AssemblyCollection : ConfigurationElementCollection
	{
		// Token: 0x1700043A RID: 1082
		// (get) Token: 0x06001153 RID: 4435 RVA: 0x0004D91F File Offset: 0x0004C91F
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return AssemblyCollection._properties;
			}
		}

		// Token: 0x1700043B RID: 1083
		public AssemblyInfo this[int index]
		{
			get
			{
				return (AssemblyInfo)base.BaseGet(index);
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

		// Token: 0x1700043C RID: 1084
		public AssemblyInfo this[string assemblyName]
		{
			get
			{
				return (AssemblyInfo)base.BaseGet(assemblyName);
			}
		}

		// Token: 0x06001157 RID: 4439 RVA: 0x0004D95C File Offset: 0x0004C95C
		public void Add(AssemblyInfo assemblyInformation)
		{
			this.BaseAdd(assemblyInformation);
		}

		// Token: 0x06001158 RID: 4440 RVA: 0x0004D965 File Offset: 0x0004C965
		public void Remove(string key)
		{
			base.BaseRemove(key);
		}

		// Token: 0x06001159 RID: 4441 RVA: 0x0004D96E File Offset: 0x0004C96E
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x0600115A RID: 4442 RVA: 0x0004D977 File Offset: 0x0004C977
		protected override ConfigurationElement CreateNewElement()
		{
			return new AssemblyInfo();
		}

		// Token: 0x0600115B RID: 4443 RVA: 0x0004D97E File Offset: 0x0004C97E
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((AssemblyInfo)element).Assembly;
		}

		// Token: 0x0600115C RID: 4444 RVA: 0x0004D98B File Offset: 0x0004C98B
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x0600115D RID: 4445 RVA: 0x0004D993 File Offset: 0x0004C993
		internal bool IsRemoved(string key)
		{
			return base.BaseIsRemoved(key);
		}

		// Token: 0x040016A3 RID: 5795
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
	}
}
