using System;

namespace System.Xml
{
	// Token: 0x02000382 RID: 898
	internal abstract class BaseTreeIterator
	{
		// Token: 0x06002F7E RID: 12158 RVA: 0x002B0B7C File Offset: 0x002AFF7C
		internal BaseTreeIterator(DataSetMapper mapper)
		{
			this.mapper = mapper;
		}

		// Token: 0x06002F7F RID: 12159
		internal abstract void Reset();

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06002F80 RID: 12160
		internal abstract XmlNode CurrentNode { get; }

		// Token: 0x06002F81 RID: 12161
		internal abstract bool Next();

		// Token: 0x06002F82 RID: 12162
		internal abstract bool NextRight();

		// Token: 0x06002F83 RID: 12163 RVA: 0x002B0B98 File Offset: 0x002AFF98
		internal bool NextRowElement()
		{
			while (this.Next())
			{
				if (this.OnRowElement())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002F84 RID: 12164 RVA: 0x002B0BBC File Offset: 0x002AFFBC
		internal bool NextRightRowElement()
		{
			return this.NextRight() && (this.OnRowElement() || this.NextRowElement());
		}

		// Token: 0x06002F85 RID: 12165 RVA: 0x002B0BE4 File Offset: 0x002AFFE4
		internal bool OnRowElement()
		{
			XmlBoundElement xmlBoundElement = this.CurrentNode as XmlBoundElement;
			return xmlBoundElement != null && xmlBoundElement.Row != null;
		}

		// Token: 0x04001D88 RID: 7560
		protected DataSetMapper mapper;
	}
}
