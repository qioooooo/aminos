using System;
using System.Collections;

namespace System.Xml.Serialization
{
	// Token: 0x0200030B RID: 779
	public class XmlElementAttributes : CollectionBase
	{
		// Token: 0x17000919 RID: 2329
		public XmlElementAttribute this[int index]
		{
			get
			{
				return (XmlElementAttribute)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060024FD RID: 9469 RVA: 0x000ADEA3 File Offset: 0x000ACEA3
		public int Add(XmlElementAttribute attribute)
		{
			return base.List.Add(attribute);
		}

		// Token: 0x060024FE RID: 9470 RVA: 0x000ADEB1 File Offset: 0x000ACEB1
		public void Insert(int index, XmlElementAttribute attribute)
		{
			base.List.Insert(index, attribute);
		}

		// Token: 0x060024FF RID: 9471 RVA: 0x000ADEC0 File Offset: 0x000ACEC0
		public int IndexOf(XmlElementAttribute attribute)
		{
			return base.List.IndexOf(attribute);
		}

		// Token: 0x06002500 RID: 9472 RVA: 0x000ADECE File Offset: 0x000ACECE
		public bool Contains(XmlElementAttribute attribute)
		{
			return base.List.Contains(attribute);
		}

		// Token: 0x06002501 RID: 9473 RVA: 0x000ADEDC File Offset: 0x000ACEDC
		public void Remove(XmlElementAttribute attribute)
		{
			base.List.Remove(attribute);
		}

		// Token: 0x06002502 RID: 9474 RVA: 0x000ADEEA File Offset: 0x000ACEEA
		public void CopyTo(XmlElementAttribute[] array, int index)
		{
			base.List.CopyTo(array, index);
		}
	}
}
