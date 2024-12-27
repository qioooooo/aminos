using System;
using System.Collections;

namespace System.Xml.Serialization
{
	// Token: 0x02000301 RID: 769
	public class XmlArrayItemAttributes : CollectionBase
	{
		// Token: 0x170008D6 RID: 2262
		public XmlArrayItemAttribute this[int index]
		{
			get
			{
				return (XmlArrayItemAttribute)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06002404 RID: 9220 RVA: 0x000AA522 File Offset: 0x000A9522
		public int Add(XmlArrayItemAttribute attribute)
		{
			return base.List.Add(attribute);
		}

		// Token: 0x06002405 RID: 9221 RVA: 0x000AA530 File Offset: 0x000A9530
		public void Insert(int index, XmlArrayItemAttribute attribute)
		{
			base.List.Insert(index, attribute);
		}

		// Token: 0x06002406 RID: 9222 RVA: 0x000AA53F File Offset: 0x000A953F
		public int IndexOf(XmlArrayItemAttribute attribute)
		{
			return base.List.IndexOf(attribute);
		}

		// Token: 0x06002407 RID: 9223 RVA: 0x000AA54D File Offset: 0x000A954D
		public bool Contains(XmlArrayItemAttribute attribute)
		{
			return base.List.Contains(attribute);
		}

		// Token: 0x06002408 RID: 9224 RVA: 0x000AA55B File Offset: 0x000A955B
		public void Remove(XmlArrayItemAttribute attribute)
		{
			base.List.Remove(attribute);
		}

		// Token: 0x06002409 RID: 9225 RVA: 0x000AA569 File Offset: 0x000A9569
		public void CopyTo(XmlArrayItemAttribute[] array, int index)
		{
			base.List.CopyTo(array, index);
		}
	}
}
