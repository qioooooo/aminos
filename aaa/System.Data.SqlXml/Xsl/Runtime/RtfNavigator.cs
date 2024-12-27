using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x0200007B RID: 123
	internal abstract class RtfNavigator : XPathNavigator
	{
		// Token: 0x060006FD RID: 1789
		public abstract void CopyToWriter(XmlWriter writer);

		// Token: 0x060006FE RID: 1790
		public abstract XPathNavigator ToNavigator();

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x00025645 File Offset: 0x00024645
		public override XPathNodeType NodeType
		{
			get
			{
				return XPathNodeType.Root;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x00025648 File Offset: 0x00024648
		public override string LocalName
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x0002564F File Offset: 0x0002464F
		public override string NamespaceURI
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x00025656 File Offset: 0x00024656
		public override string Name
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x0002565D File Offset: 0x0002465D
		public override string Prefix
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000704 RID: 1796 RVA: 0x00025664 File Offset: 0x00024664
		public override bool IsEmptyElement
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000705 RID: 1797 RVA: 0x00025667 File Offset: 0x00024667
		public override XmlNameTable NameTable
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0002566E File Offset: 0x0002466E
		public override bool MoveToFirstAttribute()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x00025675 File Offset: 0x00024675
		public override bool MoveToNextAttribute()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x0002567C File Offset: 0x0002467C
		public override bool MoveToFirstNamespace(XPathNamespaceScope namespaceScope)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x00025683 File Offset: 0x00024683
		public override bool MoveToNextNamespace(XPathNamespaceScope namespaceScope)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0002568A File Offset: 0x0002468A
		public override bool MoveToNext()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x00025691 File Offset: 0x00024691
		public override bool MoveToPrevious()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x00025698 File Offset: 0x00024698
		public override bool MoveToFirstChild()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0002569F File Offset: 0x0002469F
		public override bool MoveToParent()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x000256A6 File Offset: 0x000246A6
		public override bool MoveToId(string id)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x000256AD File Offset: 0x000246AD
		public override bool IsSamePosition(XPathNavigator other)
		{
			throw new NotSupportedException();
		}
	}
}
