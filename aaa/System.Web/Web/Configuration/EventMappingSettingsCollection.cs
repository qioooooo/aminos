using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x020001D8 RID: 472
	[ConfigurationCollection(typeof(EventMappingSettings))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class EventMappingSettingsCollection : ConfigurationElementCollection
	{
		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06001A66 RID: 6758 RVA: 0x0007B667 File Offset: 0x0007A667
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return EventMappingSettingsCollection._properties;
			}
		}

		// Token: 0x17000501 RID: 1281
		public EventMappingSettings this[string key]
		{
			get
			{
				return (EventMappingSettings)base.BaseGet(key);
			}
		}

		// Token: 0x17000502 RID: 1282
		public EventMappingSettings this[int index]
		{
			get
			{
				return (EventMappingSettings)base.BaseGet(index);
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

		// Token: 0x06001A6B RID: 6763 RVA: 0x0007B6AC File Offset: 0x0007A6AC
		protected override ConfigurationElement CreateNewElement()
		{
			return new EventMappingSettings();
		}

		// Token: 0x06001A6C RID: 6764 RVA: 0x0007B6B3 File Offset: 0x0007A6B3
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((EventMappingSettings)element).Name;
		}

		// Token: 0x06001A6D RID: 6765 RVA: 0x0007B6C0 File Offset: 0x0007A6C0
		public void Add(EventMappingSettings eventMappingSettings)
		{
			this.BaseAdd(eventMappingSettings);
		}

		// Token: 0x06001A6E RID: 6766 RVA: 0x0007B6C9 File Offset: 0x0007A6C9
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x06001A6F RID: 6767 RVA: 0x0007B6D1 File Offset: 0x0007A6D1
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x06001A70 RID: 6768 RVA: 0x0007B6DA File Offset: 0x0007A6DA
		public void Insert(int index, EventMappingSettings eventMappingSettings)
		{
			this.BaseAdd(index, eventMappingSettings);
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x0007B6E4 File Offset: 0x0007A6E4
		public int IndexOf(string name)
		{
			ConfigurationElement configurationElement = base.BaseGet(name);
			if (configurationElement == null)
			{
				return -1;
			}
			return base.BaseIndexOf(configurationElement);
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x0007B705 File Offset: 0x0007A705
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x06001A73 RID: 6771 RVA: 0x0007B70E File Offset: 0x0007A70E
		public bool Contains(string name)
		{
			return this.IndexOf(name) != -1;
		}

		// Token: 0x040017D9 RID: 6105
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
	}
}
