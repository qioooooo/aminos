using System;
using System.Collections.Generic;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x02000047 RID: 71
	internal class QilScopedVisitor : QilVisitor
	{
		// Token: 0x0600048E RID: 1166 RVA: 0x0001F3A0 File Offset: 0x0001E3A0
		protected virtual void BeginScope(QilNode node)
		{
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x0001F3A2 File Offset: 0x0001E3A2
		protected virtual void EndScope(QilNode node)
		{
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x0001F3A4 File Offset: 0x0001E3A4
		protected virtual void BeforeVisit(QilNode node)
		{
			QilNodeType nodeType = node.NodeType;
			if (nodeType != QilNodeType.QilExpression)
			{
				switch (nodeType)
				{
				case QilNodeType.Loop:
				case QilNodeType.Filter:
				case QilNodeType.Sort:
					goto IL_0112;
				case QilNodeType.SortKey:
				case QilNodeType.DocOrderDistinct:
					return;
				case QilNodeType.Function:
					break;
				default:
					return;
				}
			}
			else
			{
				QilExpression qilExpression = (QilExpression)node;
				foreach (QilNode qilNode in qilExpression.GlobalParameterList)
				{
					this.BeginScope(qilNode);
				}
				foreach (QilNode qilNode2 in qilExpression.GlobalVariableList)
				{
					this.BeginScope(qilNode2);
				}
				using (IEnumerator<QilNode> enumerator3 = qilExpression.FunctionList.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						QilNode qilNode3 = enumerator3.Current;
						this.BeginScope(qilNode3);
					}
					return;
				}
			}
			using (IEnumerator<QilNode> enumerator4 = ((QilFunction)node).Arguments.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					QilNode qilNode4 = enumerator4.Current;
					this.BeginScope(qilNode4);
				}
				return;
			}
			IL_0112:
			this.BeginScope(((QilLoop)node).Variable);
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x0001F508 File Offset: 0x0001E508
		protected virtual void AfterVisit(QilNode node)
		{
			QilNodeType nodeType = node.NodeType;
			if (nodeType != QilNodeType.QilExpression)
			{
				switch (nodeType)
				{
				case QilNodeType.Loop:
				case QilNodeType.Filter:
				case QilNodeType.Sort:
					goto IL_0112;
				case QilNodeType.SortKey:
				case QilNodeType.DocOrderDistinct:
					return;
				case QilNodeType.Function:
					break;
				default:
					return;
				}
			}
			else
			{
				QilExpression qilExpression = (QilExpression)node;
				foreach (QilNode qilNode in qilExpression.FunctionList)
				{
					this.EndScope(qilNode);
				}
				foreach (QilNode qilNode2 in qilExpression.GlobalVariableList)
				{
					this.EndScope(qilNode2);
				}
				using (IEnumerator<QilNode> enumerator3 = qilExpression.GlobalParameterList.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						QilNode qilNode3 = enumerator3.Current;
						this.EndScope(qilNode3);
					}
					return;
				}
			}
			using (IEnumerator<QilNode> enumerator4 = ((QilFunction)node).Arguments.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					QilNode qilNode4 = enumerator4.Current;
					this.EndScope(qilNode4);
				}
				return;
			}
			IL_0112:
			this.EndScope(((QilLoop)node).Variable);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x0001F66C File Offset: 0x0001E66C
		protected override QilNode Visit(QilNode n)
		{
			this.BeforeVisit(n);
			QilNode qilNode = base.Visit(n);
			this.AfterVisit(n);
			return qilNode;
		}
	}
}
