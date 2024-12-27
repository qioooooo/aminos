using System;
using System.Collections;

namespace System.Xml.Serialization
{
	// Token: 0x020002FE RID: 766
	public class XmlAnyElementAttributes : CollectionBase
	{
		// Token: 0x170008C8 RID: 2248
		public XmlAnyElementAttribute this[int index]
		{
			get
			{
				return (XmlAnyElementAttribute)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060023DC RID: 9180 RVA: 0x000AA323 File Offset: 0x000A9323
		public int Add(XmlAnyElementAttribute attribute)
		{
			return base.List.Add(attribute);
		}

		// Token: 0x060023DD RID: 9181 RVA: 0x000AA331 File Offset: 0x000A9331
		public void Insert(int index, XmlAnyElementAttribute attribute)
		{
			base.List.Insert(index, attribute);
		}

		// Token: 0x060023DE RID: 9182 RVA: 0x000AA340 File Offset: 0x000A9340
		public int IndexOf(XmlAnyElementAttribute attribute)
		{
			return base.List.IndexOf(attribute);
		}

		// Token: 0x060023DF RID: 9183 RVA: 0x000AA34E File Offset: 0x000A934E
		public bool Contains(XmlAnyElementAttribute attribute)
		{
			return base.List.Contains(attribute);
		}

		// Token: 0x060023E0 RID: 9184 RVA: 0x000AA35C File Offset: 0x000A935C
		public void Remove(XmlAnyElementAttribute attribute)
		{
			base.List.Remove(attribute);
		}

		// Token: 0x060023E1 RID: 9185 RVA: 0x000AA36A File Offset: 0x000A936A
		public void CopyTo(XmlAnyElementAttribute[] array, int index)
		{
			base.List.CopyTo(array, index);
		}
	}
}
