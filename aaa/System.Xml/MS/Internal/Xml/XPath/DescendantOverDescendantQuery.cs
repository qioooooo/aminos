using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000136 RID: 310
	internal sealed class DescendantOverDescendantQuery : DescendantBaseQuery
	{
		// Token: 0x060011DF RID: 4575 RVA: 0x0004EC60 File Offset: 0x0004DC60
		public DescendantOverDescendantQuery(Query qyParent, bool matchSelf, string name, string prefix, XPathNodeType typeTest, bool abbrAxis)
			: base(qyParent, name, prefix, typeTest, matchSelf, abbrAxis)
		{
		}

		// Token: 0x060011E0 RID: 4576 RVA: 0x0004EC71 File Offset: 0x0004DC71
		private DescendantOverDescendantQuery(DescendantOverDescendantQuery other)
			: base(other)
		{
			this.level = other.level;
		}

		// Token: 0x060011E1 RID: 4577 RVA: 0x0004EC86 File Offset: 0x0004DC86
		public override void Reset()
		{
			this.level = 0;
			base.Reset();
		}

		// Token: 0x060011E2 RID: 4578 RVA: 0x0004EC98 File Offset: 0x0004DC98
		public override XPathNavigator Advance()
		{
			for (;;)
			{
				IL_0000:
				if (this.level == 0)
				{
					this.currentNode = this.qyInput.Advance();
					this.position = 0;
					if (this.currentNode == null)
					{
						break;
					}
					if (this.matchSelf && this.matches(this.currentNode))
					{
						goto Block_3;
					}
					this.currentNode = this.currentNode.Clone();
					if (!this.MoveToFirstChild())
					{
						continue;
					}
				}
				else if (!this.MoveUpUntillNext())
				{
					continue;
				}
				while (!this.matches(this.currentNode))
				{
					if (!this.MoveToFirstChild())
					{
						goto IL_0000;
					}
				}
				goto Block_5;
			}
			return null;
			Block_3:
			this.position = 1;
			return this.currentNode;
			Block_5:
			this.position++;
			return this.currentNode;
		}

		// Token: 0x060011E3 RID: 4579 RVA: 0x0004ED45 File Offset: 0x0004DD45
		private bool MoveToFirstChild()
		{
			if (this.currentNode.MoveToFirstChild())
			{
				this.level++;
				return true;
			}
			return false;
		}

		// Token: 0x060011E4 RID: 4580 RVA: 0x0004ED65 File Offset: 0x0004DD65
		private bool MoveUpUntillNext()
		{
			while (!this.currentNode.MoveToNext())
			{
				this.level--;
				if (this.level == 0)
				{
					return false;
				}
				this.currentNode.MoveToParent();
			}
			return true;
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x0004ED9B File Offset: 0x0004DD9B
		public override XPathNodeIterator Clone()
		{
			return new DescendantOverDescendantQuery(this);
		}

		// Token: 0x04000B55 RID: 2901
		private int level;
	}
}
