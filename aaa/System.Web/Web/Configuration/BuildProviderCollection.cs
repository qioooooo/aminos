using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x020001B0 RID: 432
	[ConfigurationCollection(typeof(BuildProvider))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class BuildProviderCollection : ConfigurationElementCollection
	{
		// Token: 0x06001909 RID: 6409 RVA: 0x00077FBA File Offset: 0x00076FBA
		public BuildProviderCollection()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x0600190A RID: 6410 RVA: 0x00077FC7 File Offset: 0x00076FC7
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return BuildProviderCollection._properties;
			}
		}

		// Token: 0x1700047C RID: 1148
		public BuildProvider this[string name]
		{
			get
			{
				return (BuildProvider)base.BaseGet(name);
			}
		}

		// Token: 0x1700047D RID: 1149
		public BuildProvider this[int index]
		{
			get
			{
				return (BuildProvider)base.BaseGet(index);
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

		// Token: 0x0600190E RID: 6414 RVA: 0x00078004 File Offset: 0x00077004
		public void Add(BuildProvider buildProvider)
		{
			this.BaseAdd(buildProvider);
		}

		// Token: 0x0600190F RID: 6415 RVA: 0x0007800D File Offset: 0x0007700D
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x06001910 RID: 6416 RVA: 0x00078016 File Offset: 0x00077016
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x06001911 RID: 6417 RVA: 0x0007801F File Offset: 0x0007701F
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x06001912 RID: 6418 RVA: 0x00078027 File Offset: 0x00077027
		protected override ConfigurationElement CreateNewElement()
		{
			return new BuildProvider();
		}

		// Token: 0x06001913 RID: 6419 RVA: 0x0007802E File Offset: 0x0007702E
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((BuildProvider)element).Extension;
		}

		// Token: 0x04001700 RID: 5888
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
	}
}
