using System;
using System.Configuration;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.Configuration
{
	// Token: 0x020001DF RID: 479
	[ConfigurationCollection(typeof(FormsAuthenticationUser), AddItemName = "user", CollectionType = ConfigurationElementCollectionType.BasicMap)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class FormsAuthenticationUserCollection : ConfigurationElementCollection
	{
		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06001AB8 RID: 6840 RVA: 0x0007C0E4 File Offset: 0x0007B0E4
		public string[] AllKeys
		{
			get
			{
				return StringUtil.ObjectArrayToStringArray(base.BaseGetAllKeys());
			}
		}

		// Token: 0x1700051F RID: 1311
		public FormsAuthenticationUser this[string name]
		{
			get
			{
				return (FormsAuthenticationUser)base.BaseGet(name);
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06001ABA RID: 6842 RVA: 0x0007C0FF File Offset: 0x0007B0FF
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return FormsAuthenticationUserCollection._properties;
			}
		}

		// Token: 0x17000521 RID: 1313
		public FormsAuthenticationUser this[int index]
		{
			get
			{
				return (FormsAuthenticationUser)base.BaseGet(index);
			}
			set
			{
				this.BaseAdd(index, value);
			}
		}

		// Token: 0x06001ABD RID: 6845 RVA: 0x0007C11E File Offset: 0x0007B11E
		protected override ConfigurationElement CreateNewElement()
		{
			return new FormsAuthenticationUser();
		}

		// Token: 0x06001ABE RID: 6846 RVA: 0x0007C125 File Offset: 0x0007B125
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((FormsAuthenticationUser)element).Name;
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06001ABF RID: 6847 RVA: 0x0007C132 File Offset: 0x0007B132
		protected override string ElementName
		{
			get
			{
				return "user";
			}
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06001AC0 RID: 6848 RVA: 0x0007C139 File Offset: 0x0007B139
		protected override bool ThrowOnDuplicate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x06001AC1 RID: 6849 RVA: 0x0007C13C File Offset: 0x0007B13C
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.BasicMap;
			}
		}

		// Token: 0x06001AC2 RID: 6850 RVA: 0x0007C13F File Offset: 0x0007B13F
		public void Add(FormsAuthenticationUser user)
		{
			this.BaseAdd(user);
		}

		// Token: 0x06001AC3 RID: 6851 RVA: 0x0007C148 File Offset: 0x0007B148
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x06001AC4 RID: 6852 RVA: 0x0007C150 File Offset: 0x0007B150
		public FormsAuthenticationUser Get(int index)
		{
			return (FormsAuthenticationUser)base.BaseGet(index);
		}

		// Token: 0x06001AC5 RID: 6853 RVA: 0x0007C15E File Offset: 0x0007B15E
		public FormsAuthenticationUser Get(string name)
		{
			return (FormsAuthenticationUser)base.BaseGet(name);
		}

		// Token: 0x06001AC6 RID: 6854 RVA: 0x0007C16C File Offset: 0x0007B16C
		public string GetKey(int index)
		{
			return (string)base.BaseGetKey(index);
		}

		// Token: 0x06001AC7 RID: 6855 RVA: 0x0007C17A File Offset: 0x0007B17A
		public void Remove(string name)
		{
			base.BaseRemove(name);
		}

		// Token: 0x06001AC8 RID: 6856 RVA: 0x0007C183 File Offset: 0x0007B183
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x06001AC9 RID: 6857 RVA: 0x0007C18C File Offset: 0x0007B18C
		public void Set(FormsAuthenticationUser user)
		{
			base.BaseAdd(user, false);
		}

		// Token: 0x040017F6 RID: 6134
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
	}
}
