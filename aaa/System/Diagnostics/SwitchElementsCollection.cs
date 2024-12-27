using System;
using System.Configuration;

namespace System.Diagnostics
{
	// Token: 0x020001D1 RID: 465
	[ConfigurationCollection(typeof(SwitchElement))]
	internal class SwitchElementsCollection : ConfigurationElementCollection
	{
		// Token: 0x170002EF RID: 751
		public SwitchElement this[string name]
		{
			get
			{
				return (SwitchElement)base.BaseGet(name);
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000E76 RID: 3702 RVA: 0x0002DEAC File Offset: 0x0002CEAC
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.AddRemoveClearMap;
			}
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x0002DEAF File Offset: 0x0002CEAF
		protected override ConfigurationElement CreateNewElement()
		{
			return new SwitchElement();
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x0002DEB6 File Offset: 0x0002CEB6
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((SwitchElement)element).Name;
		}
	}
}
