using System;
using System.Collections.Generic;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200013E RID: 318
	internal sealed class FollSiblingQuery : BaseAxisQuery
	{
		// Token: 0x0600121C RID: 4636 RVA: 0x0004F954 File Offset: 0x0004E954
		public FollSiblingQuery(Query qyInput, string name, string prefix, XPathNodeType type)
			: base(qyInput, name, prefix, type)
		{
			this.elementStk = new ClonableStack<XPathNavigator>();
			this.parentStk = new List<XPathNavigator>();
		}

		// Token: 0x0600121D RID: 4637 RVA: 0x0004F977 File Offset: 0x0004E977
		private FollSiblingQuery(FollSiblingQuery other)
			: base(other)
		{
			this.elementStk = other.elementStk.Clone();
			this.parentStk = new List<XPathNavigator>(other.parentStk);
			this.nextInput = Query.Clone(other.nextInput);
		}

		// Token: 0x0600121E RID: 4638 RVA: 0x0004F9B3 File Offset: 0x0004E9B3
		public override void Reset()
		{
			this.elementStk.Clear();
			this.parentStk.Clear();
			this.nextInput = null;
			base.Reset();
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x0004F9D8 File Offset: 0x0004E9D8
		private bool Visited(XPathNavigator nav)
		{
			XPathNavigator xpathNavigator = nav.Clone();
			xpathNavigator.MoveToParent();
			for (int i = 0; i < this.parentStk.Count; i++)
			{
				if (xpathNavigator.IsSamePosition(this.parentStk[i]))
				{
					return true;
				}
			}
			this.parentStk.Add(xpathNavigator);
			return false;
		}

		// Token: 0x06001220 RID: 4640 RVA: 0x0004FA2C File Offset: 0x0004EA2C
		private XPathNavigator FetchInput()
		{
			XPathNavigator xpathNavigator;
			for (;;)
			{
				xpathNavigator = this.qyInput.Advance();
				if (xpathNavigator == null)
				{
					break;
				}
				if (!this.Visited(xpathNavigator))
				{
					goto Block_1;
				}
			}
			return null;
			Block_1:
			return xpathNavigator.Clone();
		}

		// Token: 0x06001221 RID: 4641 RVA: 0x0004FA5C File Offset: 0x0004EA5C
		public override XPathNavigator Advance()
		{
			for (;;)
			{
				if (this.currentNode == null)
				{
					if (this.nextInput == null)
					{
						this.nextInput = this.FetchInput();
					}
					if (this.elementStk.Count == 0)
					{
						if (this.nextInput == null)
						{
							break;
						}
						this.currentNode = this.nextInput;
						this.nextInput = this.FetchInput();
					}
					else
					{
						this.currentNode = this.elementStk.Pop();
					}
				}
				while (this.currentNode.IsDescendant(this.nextInput))
				{
					this.elementStk.Push(this.currentNode);
					this.currentNode = this.nextInput;
					this.nextInput = this.qyInput.Advance();
					if (this.nextInput != null)
					{
						this.nextInput = this.nextInput.Clone();
					}
				}
				while (this.currentNode.MoveToNext())
				{
					if (this.matches(this.currentNode))
					{
						goto Block_6;
					}
				}
				this.currentNode = null;
			}
			return null;
			Block_6:
			this.position++;
			return this.currentNode;
		}

		// Token: 0x06001222 RID: 4642 RVA: 0x0004FB63 File Offset: 0x0004EB63
		public override XPathNodeIterator Clone()
		{
			return new FollSiblingQuery(this);
		}

		// Token: 0x04000B62 RID: 2914
		private ClonableStack<XPathNavigator> elementStk;

		// Token: 0x04000B63 RID: 2915
		private List<XPathNavigator> parentStk;

		// Token: 0x04000B64 RID: 2916
		private XPathNavigator nextInput;
	}
}
