using System;
using System.Collections.Generic;
using System.Xml.Xsl.Qil;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x020000EF RID: 239
	internal class InvokeGenerator : QilCloneVisitor
	{
		// Token: 0x06000AC8 RID: 2760 RVA: 0x00033B8C File Offset: 0x00032B8C
		public InvokeGenerator(XsltQilFactory f, bool debug)
			: base(f.BaseFactory)
		{
			this.debug = debug;
			this.fac = f;
			this.iterStack = new Stack<QilIterator>();
		}

		// Token: 0x06000AC9 RID: 2761 RVA: 0x00033BB4 File Offset: 0x00032BB4
		public QilNode GenerateInvoke(QilFunction func, IList<XslNode> actualArgs)
		{
			this.iterStack.Clear();
			this.formalArgs = func.Arguments;
			this.invokeArgs = this.fac.ActualParameterList();
			this.curArg = 0;
			while (this.curArg < this.formalArgs.Count)
			{
				QilParameter qilParameter = (QilParameter)this.formalArgs[this.curArg];
				QilNode qilNode = this.FindActualArg(qilParameter, actualArgs);
				if (qilNode == null)
				{
					if (this.debug)
					{
						if (qilParameter.Name.NamespaceUri == "urn:schemas-microsoft-com:xslt-debug")
						{
							qilNode = base.Clone(qilParameter.DefaultValue);
						}
						else
						{
							qilNode = this.fac.DefaultValueMarker();
						}
					}
					else
					{
						qilNode = base.Clone(qilParameter.DefaultValue);
					}
				}
				XmlQueryType xmlType = qilParameter.XmlType;
				XmlQueryType xmlType2 = qilNode.XmlType;
				if (!xmlType2.IsSubtypeOf(xmlType))
				{
					qilNode = this.fac.TypeAssert(qilNode, xmlType);
				}
				this.invokeArgs.Add(qilNode);
				this.curArg++;
			}
			QilNode qilNode2 = this.fac.Invoke(func, this.invokeArgs);
			while (this.iterStack.Count != 0)
			{
				qilNode2 = this.fac.Loop(this.iterStack.Pop(), qilNode2);
			}
			return qilNode2;
		}

		// Token: 0x06000ACA RID: 2762 RVA: 0x00033CF8 File Offset: 0x00032CF8
		private QilNode FindActualArg(QilParameter formalArg, IList<XslNode> actualArgs)
		{
			QilName name = formalArg.Name;
			foreach (XslNode xslNode in actualArgs)
			{
				if (xslNode.Name.Equals(name))
				{
					return ((VarPar)xslNode).Value;
				}
			}
			return null;
		}

		// Token: 0x06000ACB RID: 2763 RVA: 0x00033D60 File Offset: 0x00032D60
		protected override QilNode VisitReference(QilNode n)
		{
			QilNode qilNode = base.FindClonedReference(n);
			if (qilNode != null)
			{
				return qilNode;
			}
			int i = 0;
			while (i < this.curArg)
			{
				if (n == this.formalArgs[i])
				{
					if (this.invokeArgs[i] is QilLiteral)
					{
						return this.invokeArgs[i].ShallowClone(this.fac.BaseFactory);
					}
					if (!(this.invokeArgs[i] is QilIterator))
					{
						QilIterator qilIterator = this.fac.BaseFactory.Let(this.invokeArgs[i]);
						this.iterStack.Push(qilIterator);
						this.invokeArgs[i] = qilIterator;
					}
					return this.invokeArgs[i];
				}
				else
				{
					i++;
				}
			}
			return n;
		}

		// Token: 0x06000ACC RID: 2764 RVA: 0x00033E2A File Offset: 0x00032E2A
		protected override QilNode VisitFunction(QilFunction n)
		{
			return n;
		}

		// Token: 0x04000732 RID: 1842
		private bool debug;

		// Token: 0x04000733 RID: 1843
		private Stack<QilIterator> iterStack;

		// Token: 0x04000734 RID: 1844
		private QilList formalArgs;

		// Token: 0x04000735 RID: 1845
		private QilList invokeArgs;

		// Token: 0x04000736 RID: 1846
		private int curArg;

		// Token: 0x04000737 RID: 1847
		private XsltQilFactory fac;
	}
}
