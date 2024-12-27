using System;
using System.Configuration;

namespace System.Diagnostics
{
	// Token: 0x020001CB RID: 459
	[ConfigurationCollection(typeof(SourceElement), AddItemName = "source", CollectionType = ConfigurationElementCollectionType.BasicMap)]
	internal class SourceElementsCollection : ConfigurationElementCollection
	{
		// Token: 0x170002E0 RID: 736
		public SourceElement this[string name]
		{
			get
			{
				return (SourceElement)base.BaseGet(name);
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000E50 RID: 3664 RVA: 0x0002D9BD File Offset: 0x0002C9BD
		protected override string ElementName
		{
			get
			{
				return "source";
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000E51 RID: 3665 RVA: 0x0002D9C4 File Offset: 0x0002C9C4
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.BasicMap;
			}
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x0002D9C8 File Offset: 0x0002C9C8
		protected override ConfigurationElement CreateNewElement()
		{
			SourceElement sourceElement = new SourceElement();
			sourceElement.Listeners.InitializeDefaultInternal();
			return sourceElement;
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x0002D9E7 File Offset: 0x0002C9E7
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((SourceElement)element).Name;
		}
	}
}
