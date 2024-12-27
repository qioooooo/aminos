using System;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x0200003A RID: 58
	internal abstract class QilReplaceVisitor : QilVisitor
	{
		// Token: 0x06000353 RID: 851 RVA: 0x000164B9 File Offset: 0x000154B9
		public QilReplaceVisitor(QilFactory f)
		{
			this.f = f;
		}

		// Token: 0x06000354 RID: 852 RVA: 0x000164C8 File Offset: 0x000154C8
		protected override QilNode VisitChildren(QilNode parent)
		{
			XmlQueryType xmlType = parent.XmlType;
			bool flag = false;
			for (int i = 0; i < parent.Count; i++)
			{
				QilNode qilNode = parent[i];
				XmlQueryType xmlQueryType = ((qilNode != null) ? qilNode.XmlType : null);
				QilNode qilNode2;
				if (this.IsReference(parent, i))
				{
					qilNode2 = this.VisitReference(qilNode);
				}
				else
				{
					qilNode2 = this.Visit(qilNode);
				}
				if (!object.Equals(qilNode, qilNode2) || (qilNode2 != null && !object.Equals(xmlQueryType, qilNode2.XmlType)))
				{
					flag = true;
					parent[i] = qilNode2;
				}
			}
			if (flag)
			{
				this.RecalculateType(parent, xmlType);
			}
			return parent;
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00016558 File Offset: 0x00015558
		protected virtual void RecalculateType(QilNode node, XmlQueryType oldType)
		{
			XmlQueryType xmlQueryType = this.f.TypeChecker.Check(node);
			node.XmlType = xmlQueryType;
		}

		// Token: 0x04000360 RID: 864
		protected QilFactory f;
	}
}
