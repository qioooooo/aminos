using System;
using System.Configuration;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x0200025B RID: 603
	[ConfigurationCollection(typeof(TrustLevel), AddItemName = "trustLevel", CollectionType = ConfigurationElementCollectionType.BasicMap)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class TrustLevelCollection : ConfigurationElementCollection
	{
		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06001FD7 RID: 8151 RVA: 0x0008C20F File Offset: 0x0008B20F
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return TrustLevelCollection._properties;
			}
		}

		// Token: 0x170006D2 RID: 1746
		public TrustLevel this[int index]
		{
			get
			{
				return (TrustLevel)base.BaseGet(index);
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

		// Token: 0x170006D3 RID: 1747
		public TrustLevel this[string key]
		{
			get
			{
				return (TrustLevel)base.BaseGet(key);
			}
		}

		// Token: 0x06001FDB RID: 8155 RVA: 0x0008C24C File Offset: 0x0008B24C
		protected override ConfigurationElement CreateNewElement()
		{
			return new TrustLevel();
		}

		// Token: 0x06001FDC RID: 8156 RVA: 0x0008C253 File Offset: 0x0008B253
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((TrustLevel)element).Name;
		}

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06001FDD RID: 8157 RVA: 0x0008C260 File Offset: 0x0008B260
		protected override string ElementName
		{
			get
			{
				return "trustLevel";
			}
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06001FDE RID: 8158 RVA: 0x0008C267 File Offset: 0x0008B267
		protected override bool ThrowOnDuplicate
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x06001FDF RID: 8159 RVA: 0x0008C26A File Offset: 0x0008B26A
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.BasicMap;
			}
		}

		// Token: 0x06001FE0 RID: 8160 RVA: 0x0008C270 File Offset: 0x0008B270
		protected override bool IsElementName(string elementname)
		{
			bool flag = false;
			if (elementname != null && elementname == "trustLevel")
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x0008C294 File Offset: 0x0008B294
		public void Add(TrustLevel trustLevel)
		{
			this.BaseAdd(trustLevel);
		}

		// Token: 0x06001FE2 RID: 8162 RVA: 0x0008C29D File Offset: 0x0008B29D
		public void Clear()
		{
			base.BaseClear();
		}

		// Token: 0x06001FE3 RID: 8163 RVA: 0x0008C2A5 File Offset: 0x0008B2A5
		public TrustLevel Get(int index)
		{
			return (TrustLevel)base.BaseGet(index);
		}

		// Token: 0x06001FE4 RID: 8164 RVA: 0x0008C2B3 File Offset: 0x0008B2B3
		public void RemoveAt(int index)
		{
			base.BaseRemoveAt(index);
		}

		// Token: 0x06001FE5 RID: 8165 RVA: 0x0008C2BC File Offset: 0x0008B2BC
		public void Remove(TrustLevel trustLevel)
		{
			base.BaseRemove(this.GetElementKey(trustLevel));
		}

		// Token: 0x06001FE6 RID: 8166 RVA: 0x0008C2CB File Offset: 0x0008B2CB
		public void Set(int index, TrustLevel trustLevel)
		{
			this.BaseAdd(index, trustLevel);
		}

		// Token: 0x04001A73 RID: 6771
		private static ConfigurationPropertyCollection _properties = new ConfigurationPropertyCollection();
	}
}
