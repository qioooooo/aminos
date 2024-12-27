using System;
using System.Configuration;

namespace System.Diagnostics
{
	// Token: 0x020001C6 RID: 454
	[ConfigurationCollection(typeof(ListenerElement), AddItemName = "add", CollectionType = ConfigurationElementCollectionType.BasicMap)]
	internal class SharedListenerElementsCollection : ListenerElementsCollection
	{
		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000E2E RID: 3630 RVA: 0x0002D20C File Offset: 0x0002C20C
		public override ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return ConfigurationElementCollectionType.BasicMap;
			}
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x0002D20F File Offset: 0x0002C20F
		protected override ConfigurationElement CreateNewElement()
		{
			return new ListenerElement(false);
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000E30 RID: 3632 RVA: 0x0002D217 File Offset: 0x0002C217
		protected override string ElementName
		{
			get
			{
				return "add";
			}
		}
	}
}
