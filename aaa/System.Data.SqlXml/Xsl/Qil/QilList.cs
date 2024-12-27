using System;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x02000053 RID: 83
	internal class QilList : QilNode
	{
		// Token: 0x06000565 RID: 1381 RVA: 0x000211A4 File Offset: 0x000201A4
		public QilList(QilNodeType nodeType)
			: base(nodeType)
		{
			this.members = new QilNode[4];
			this.xmlType = null;
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x000211C0 File Offset: 0x000201C0
		public override XmlQueryType XmlType
		{
			get
			{
				if (this.xmlType == null)
				{
					XmlQueryType xmlQueryType = XmlQueryTypeFactory.Empty;
					if (this.count > 0)
					{
						if (this.nodeType == QilNodeType.Sequence)
						{
							for (int i = 0; i < this.count; i++)
							{
								xmlQueryType = XmlQueryTypeFactory.Sequence(xmlQueryType, this.members[i].XmlType);
							}
							if (xmlQueryType.IsDod)
							{
								xmlQueryType = XmlQueryTypeFactory.PrimeProduct(XmlQueryTypeFactory.NodeNotRtfS, xmlQueryType.Cardinality);
							}
						}
						else if (this.nodeType == QilNodeType.BranchList)
						{
							xmlQueryType = this.members[0].XmlType;
							for (int j = 1; j < this.count; j++)
							{
								xmlQueryType = XmlQueryTypeFactory.Choice(xmlQueryType, this.members[j].XmlType);
							}
						}
					}
					this.xmlType = xmlQueryType;
				}
				return this.xmlType;
			}
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00021284 File Offset: 0x00020284
		public override QilNode ShallowClone(QilFactory f)
		{
			QilList qilList = (QilList)base.MemberwiseClone();
			qilList.members = (QilNode[])this.members.Clone();
			return qilList;
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x000212B4 File Offset: 0x000202B4
		public override int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x170000D4 RID: 212
		public override QilNode this[int index]
		{
			get
			{
				if (index >= 0 && index < this.count)
				{
					return this.members[index];
				}
				throw new IndexOutOfRangeException();
			}
			set
			{
				if (index >= 0 && index < this.count)
				{
					this.members[index] = value;
					this.xmlType = null;
					return;
				}
				throw new IndexOutOfRangeException();
			}
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00021300 File Offset: 0x00020300
		public override void Insert(int index, QilNode node)
		{
			if (index < 0 || index > this.count)
			{
				throw new IndexOutOfRangeException();
			}
			if (this.count == this.members.Length)
			{
				QilNode[] array = new QilNode[this.count * 2];
				Array.Copy(this.members, array, this.count);
				this.members = array;
			}
			if (index < this.count)
			{
				Array.Copy(this.members, index, this.members, index + 1, this.count - index);
			}
			this.count++;
			this.members[index] = node;
			this.xmlType = null;
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x0002139C File Offset: 0x0002039C
		public override void RemoveAt(int index)
		{
			if (index < 0 || index >= this.count)
			{
				throw new IndexOutOfRangeException();
			}
			this.count--;
			if (index < this.count)
			{
				Array.Copy(this.members, index + 1, this.members, index, this.count - index);
			}
			this.members[this.count] = null;
			this.xmlType = null;
		}

		// Token: 0x04000396 RID: 918
		private int count;

		// Token: 0x04000397 RID: 919
		private QilNode[] members;
	}
}
