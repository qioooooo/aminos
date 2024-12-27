using System;
using System.Collections;
using System.Configuration;

namespace System.Data
{
	// Token: 0x0200033D RID: 829
	internal sealed class LocalDBInstancesCollection : ConfigurationElementCollection
	{
		// Token: 0x06002B42 RID: 11074 RVA: 0x002A19A8 File Offset: 0x002A0DA8
		internal LocalDBInstancesCollection()
			: base(LocalDBInstancesCollection.s_comparer)
		{
		}

		// Token: 0x06002B43 RID: 11075 RVA: 0x002A19C0 File Offset: 0x002A0DC0
		protected override ConfigurationElement CreateNewElement()
		{
			return new LocalDBInstanceElement();
		}

		// Token: 0x06002B44 RID: 11076 RVA: 0x002A19D4 File Offset: 0x002A0DD4
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((LocalDBInstanceElement)element).Name;
		}

		// Token: 0x04001C4E RID: 7246
		private static readonly LocalDBInstancesCollection.TrimOrdinalIgnoreCaseStringComparer s_comparer = new LocalDBInstancesCollection.TrimOrdinalIgnoreCaseStringComparer();

		// Token: 0x0200033E RID: 830
		private class TrimOrdinalIgnoreCaseStringComparer : IComparer
		{
			// Token: 0x06002B46 RID: 11078 RVA: 0x002A1A04 File Offset: 0x002A0E04
			public int Compare(object x, object y)
			{
				string text = x as string;
				if (text != null)
				{
					x = text.Trim();
				}
				string text2 = y as string;
				if (text2 != null)
				{
					y = text2.Trim();
				}
				return StringComparer.OrdinalIgnoreCase.Compare(x, y);
			}
		}
	}
}
