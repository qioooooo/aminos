using System;
using System.Collections.Generic;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x020000F5 RID: 245
	internal class TemplateMatch
	{
		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000AE0 RID: 2784 RVA: 0x00034C2E File Offset: 0x00033C2E
		public XmlNodeKindFlags NodeKind
		{
			get
			{
				return this.nodeKind;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000AE1 RID: 2785 RVA: 0x00034C36 File Offset: 0x00033C36
		public QilName QName
		{
			get
			{
				return this.qname;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000AE2 RID: 2786 RVA: 0x00034C3E File Offset: 0x00033C3E
		public QilIterator Iterator
		{
			get
			{
				return this.iterator;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000AE3 RID: 2787 RVA: 0x00034C46 File Offset: 0x00033C46
		public QilNode Condition
		{
			get
			{
				return this.condition;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x00034C4E File Offset: 0x00033C4E
		public QilFunction TemplateFunction
		{
			get
			{
				return this.template.Function;
			}
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x00034C5C File Offset: 0x00033C5C
		public TemplateMatch(Template template, QilLoop filter)
		{
			this.template = template;
			this.priority = (double.IsNaN(template.Priority) ? XPathPatternBuilder.GetPriority(filter) : template.Priority);
			this.iterator = filter.Variable;
			this.condition = filter.Body;
			XPathPatternBuilder.CleanAnnotation(filter);
			this.NipOffTypeNameCheck();
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x00034CBC File Offset: 0x00033CBC
		private void NipOffTypeNameCheck()
		{
			QilBinary[] array = new QilBinary[4];
			int num = -1;
			QilNode left = this.condition;
			this.nodeKind = XmlNodeKindFlags.None;
			this.qname = null;
			while (left.NodeType == QilNodeType.And)
			{
				left = (array[++num & 3] = (QilBinary)left).Left;
			}
			if (left.NodeType != QilNodeType.IsType)
			{
				return;
			}
			QilBinary qilBinary = (QilBinary)left;
			if (qilBinary.Left != this.iterator || qilBinary.Right.NodeType != QilNodeType.LiteralType)
			{
				return;
			}
			XmlNodeKindFlags nodeKinds = qilBinary.Right.XmlType.NodeKinds;
			if (!Bits.ExactlyOne((uint)nodeKinds))
			{
				return;
			}
			this.nodeKind = nodeKinds;
			QilBinary qilBinary2 = array[num & 3];
			if (qilBinary2 != null && qilBinary2.Right.NodeType == QilNodeType.Eq)
			{
				QilBinary qilBinary3 = (QilBinary)qilBinary2.Right;
				if (qilBinary3.Left.NodeType == QilNodeType.NameOf && ((QilUnary)qilBinary3.Left).Child == this.iterator && qilBinary3.Right.NodeType == QilNodeType.LiteralQName)
				{
					this.qname = (QilName)((QilLiteral)qilBinary3.Right).Value;
					num--;
				}
			}
			QilBinary qilBinary4 = array[num & 3];
			QilBinary qilBinary5 = array[(num - 1) & 3];
			if (qilBinary5 != null)
			{
				qilBinary5.Left = qilBinary4.Right;
				return;
			}
			if (qilBinary4 != null)
			{
				this.condition = qilBinary4.Right;
				return;
			}
			this.condition = null;
		}

		// Token: 0x040007A0 RID: 1952
		public static readonly TemplateMatch.TemplateMatchComparer Comparer = new TemplateMatch.TemplateMatchComparer();

		// Token: 0x040007A1 RID: 1953
		private Template template;

		// Token: 0x040007A2 RID: 1954
		private double priority;

		// Token: 0x040007A3 RID: 1955
		private XmlNodeKindFlags nodeKind;

		// Token: 0x040007A4 RID: 1956
		private QilName qname;

		// Token: 0x040007A5 RID: 1957
		private QilIterator iterator;

		// Token: 0x040007A6 RID: 1958
		private QilNode condition;

		// Token: 0x020000F6 RID: 246
		internal class TemplateMatchComparer : IComparer<TemplateMatch>
		{
			// Token: 0x06000AE8 RID: 2792 RVA: 0x00034E2D File Offset: 0x00033E2D
			public int Compare(TemplateMatch x, TemplateMatch y)
			{
				if (x.priority > y.priority)
				{
					return 1;
				}
				if (x.priority >= y.priority)
				{
					return x.template.OrderNumber - y.template.OrderNumber;
				}
				return -1;
			}
		}
	}
}
