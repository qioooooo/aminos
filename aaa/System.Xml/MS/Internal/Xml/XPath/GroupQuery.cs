using System;
using System.Xml.XPath;

namespace MS.Internal.Xml.XPath
{
	// Token: 0x02000143 RID: 323
	internal sealed class GroupQuery : BaseAxisQuery
	{
		// Token: 0x06001237 RID: 4663 RVA: 0x0004FD0D File Offset: 0x0004ED0D
		public GroupQuery(Query qy)
			: base(qy)
		{
		}

		// Token: 0x06001238 RID: 4664 RVA: 0x0004FD16 File Offset: 0x0004ED16
		private GroupQuery(GroupQuery other)
			: base(other)
		{
		}

		// Token: 0x06001239 RID: 4665 RVA: 0x0004FD1F File Offset: 0x0004ED1F
		public override XPathNavigator Advance()
		{
			this.currentNode = this.qyInput.Advance();
			if (this.currentNode != null)
			{
				this.position++;
			}
			return this.currentNode;
		}

		// Token: 0x0600123A RID: 4666 RVA: 0x0004FD4E File Offset: 0x0004ED4E
		public override object Evaluate(XPathNodeIterator nodeIterator)
		{
			return this.qyInput.Evaluate(nodeIterator);
		}

		// Token: 0x0600123B RID: 4667 RVA: 0x0004FD5C File Offset: 0x0004ED5C
		public override XPathNodeIterator Clone()
		{
			return new GroupQuery(this);
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x0600123C RID: 4668 RVA: 0x0004FD64 File Offset: 0x0004ED64
		public override XPathResultType StaticType
		{
			get
			{
				return this.qyInput.StaticType;
			}
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x0600123D RID: 4669 RVA: 0x0004FD71 File Offset: 0x0004ED71
		public override QueryProps Properties
		{
			get
			{
				return QueryProps.Position;
			}
		}
	}
}
