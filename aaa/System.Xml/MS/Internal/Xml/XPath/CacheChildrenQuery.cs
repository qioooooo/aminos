using System;
using System.Xml;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x0200012D RID: 301
	internal sealed class CacheChildrenQuery : ChildrenQuery
	{
		// Token: 0x060011A0 RID: 4512 RVA: 0x0004E2B9 File Offset: 0x0004D2B9
		public CacheChildrenQuery(Query qyInput, string name, string prefix, XPathNodeType type)
			: base(qyInput, name, prefix, type)
		{
			this.elementStk = new ClonableStack<XPathNavigator>();
			this.positionStk = new ClonableStack<int>();
			this.needInput = true;
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x0004E2E4 File Offset: 0x0004D2E4
		private CacheChildrenQuery(CacheChildrenQuery other)
			: base(other)
		{
			this.nextInput = Query.Clone(other.nextInput);
			this.elementStk = other.elementStk.Clone();
			this.positionStk = other.positionStk.Clone();
			this.needInput = other.needInput;
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x0004E337 File Offset: 0x0004D337
		public override void Reset()
		{
			this.nextInput = null;
			this.elementStk.Clear();
			this.positionStk.Clear();
			this.needInput = true;
			base.Reset();
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x0004E364 File Offset: 0x0004D364
		public override XPathNavigator Advance()
		{
			for (;;)
			{
				if (this.needInput)
				{
					if (this.elementStk.Count == 0)
					{
						this.currentNode = this.GetNextInput();
						if (this.currentNode == null)
						{
							break;
						}
						if (!this.currentNode.MoveToFirstChild())
						{
							continue;
						}
						this.position = 0;
					}
					else
					{
						this.currentNode = this.elementStk.Pop();
						this.position = this.positionStk.Pop();
						if (!this.DecideNextNode())
						{
							continue;
						}
					}
					this.needInput = false;
				}
				else if (!this.currentNode.MoveToNext() || !this.DecideNextNode())
				{
					this.needInput = true;
					continue;
				}
				if (this.matches(this.currentNode))
				{
					goto Block_5;
				}
			}
			return null;
			Block_5:
			this.position++;
			return this.currentNode;
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x0004E42C File Offset: 0x0004D42C
		private bool DecideNextNode()
		{
			this.nextInput = this.GetNextInput();
			if (this.nextInput != null && Query.CompareNodes(this.currentNode, this.nextInput) == XmlNodeOrder.After)
			{
				this.elementStk.Push(this.currentNode);
				this.positionStk.Push(this.position);
				this.currentNode = this.nextInput;
				this.nextInput = null;
				if (!this.currentNode.MoveToFirstChild())
				{
					return false;
				}
				this.position = 0;
			}
			return true;
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x0004E4B0 File Offset: 0x0004D4B0
		private XPathNavigator GetNextInput()
		{
			XPathNavigator xpathNavigator;
			if (this.nextInput != null)
			{
				xpathNavigator = this.nextInput;
				this.nextInput = null;
			}
			else
			{
				xpathNavigator = this.qyInput.Advance();
				if (xpathNavigator != null)
				{
					xpathNavigator = xpathNavigator.Clone();
				}
			}
			return xpathNavigator;
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x0004E4EC File Offset: 0x0004D4EC
		public override XPathNodeIterator Clone()
		{
			return new CacheChildrenQuery(this);
		}

		// Token: 0x04000B44 RID: 2884
		private XPathNavigator nextInput;

		// Token: 0x04000B45 RID: 2885
		private ClonableStack<XPathNavigator> elementStk;

		// Token: 0x04000B46 RID: 2886
		private ClonableStack<int> positionStk;

		// Token: 0x04000B47 RID: 2887
		private bool needInput;
	}
}
