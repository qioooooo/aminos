using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x020001AD RID: 429
	[ConfigurationCollection(typeof(BufferModeSettings))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class BufferModesCollection : ConfigurationElementCollection
	{
		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x060018DF RID: 6367 RVA: 0x0007780D File Offset: 0x0007680D
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return BufferModesCollection._properties;
			}
		}

		// Token: 0x060018E0 RID: 6368 RVA: 0x00077814 File Offset: 0x00076814
		public void Add(BufferModeSettings bufferModeSettings)
		{
			this.BaseAdd(bufferModeSettings);
		}

		// Token: 0x060018E1 RID: 6369 RVA: 0x0007781D File Offset: 0x0007681D
		public void Remove(string s)
		{
			base.BaseRemove(s);
		}

		// Token: 0x060018E2 RID: 6370 RVA: 0x00077826 File Offset: 0x00076826
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x060018E3 RID: 6371 RVA: 0x0007782E File Offset: 0x0007682E
		protected override ConfigurationElement CreateNewElement()
		{
			return new BufferModeSettings();
		}

		// Token: 0x060018E4 RID: 6372 RVA: 0x00077835 File Offset: 0x00076835
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((BufferModeSettings)element).Name;
		}

		// Token: 0x1700046B RID: 1131
		public BufferModeSettings this[string key]
		{
			get
			{
				return (BufferModeSettings)base.BaseGet(key);
			}
		}

		// Token: 0x1700046C RID: 1132
		public BufferModeSettings this[int index]
		{
			get
			{
				return (BufferModeSettings)base.BaseGet(index);
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

		// Token: 0x040016F0 RID: 5872
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
	}
}
