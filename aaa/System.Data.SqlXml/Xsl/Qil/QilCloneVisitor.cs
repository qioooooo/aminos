using System;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x02000048 RID: 72
	internal class QilCloneVisitor : QilScopedVisitor
	{
		// Token: 0x06000494 RID: 1172 RVA: 0x0001F698 File Offset: 0x0001E698
		public QilCloneVisitor(QilFactory fac)
			: this(fac, new SubstitutionList())
		{
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x0001F6A6 File Offset: 0x0001E6A6
		public QilCloneVisitor(QilFactory fac, SubstitutionList subs)
		{
			this.fac = fac;
			this.subs = subs;
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x0001F6BC File Offset: 0x0001E6BC
		public QilNode Clone(QilNode node)
		{
			QilDepthChecker.Check(node);
			return this.VisitAssumeReference(node);
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0001F6CC File Offset: 0x0001E6CC
		protected override QilNode Visit(QilNode oldNode)
		{
			QilNode qilNode = null;
			if (oldNode == null)
			{
				return null;
			}
			if (oldNode is QilReference)
			{
				qilNode = this.FindClonedReference(oldNode);
			}
			if (qilNode == null)
			{
				qilNode = oldNode.ShallowClone(this.fac);
			}
			return base.Visit(qilNode);
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0001F708 File Offset: 0x0001E708
		protected override QilNode VisitChildren(QilNode parent)
		{
			for (int i = 0; i < parent.Count; i++)
			{
				QilNode qilNode = parent[i];
				if (this.IsReference(parent, i))
				{
					parent[i] = this.VisitReference(qilNode);
					if (parent[i] == null)
					{
						parent[i] = qilNode;
					}
				}
				else
				{
					parent[i] = this.Visit(qilNode);
				}
			}
			return parent;
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x0001F768 File Offset: 0x0001E768
		protected override QilNode VisitReference(QilNode oldNode)
		{
			QilNode qilNode = this.FindClonedReference(oldNode);
			return base.VisitReference((qilNode == null) ? oldNode : qilNode);
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0001F78A File Offset: 0x0001E78A
		protected override void BeginScope(QilNode node)
		{
			this.subs.AddSubstitutionPair(node, node.ShallowClone(this.fac));
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0001F7A4 File Offset: 0x0001E7A4
		protected override void EndScope(QilNode node)
		{
			this.subs.RemoveLastSubstitutionPair();
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0001F7B1 File Offset: 0x0001E7B1
		protected QilNode FindClonedReference(QilNode node)
		{
			return this.subs.FindReplacement(node);
		}

		// Token: 0x04000381 RID: 897
		private QilFactory fac;

		// Token: 0x04000382 RID: 898
		private SubstitutionList subs;
	}
}
