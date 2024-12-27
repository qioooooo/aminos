using System;
using System.Xml.XPath;

namespace System.Xml
{
	// Token: 0x020000BF RID: 191
	internal sealed class DocumentXPathNodeIterator_Empty : XPathNodeIterator
	{
		// Token: 0x06000B5F RID: 2911 RVA: 0x00034D81 File Offset: 0x00033D81
		internal DocumentXPathNodeIterator_Empty(DocumentXPathNavigator nav)
		{
			this.nav = nav.Clone();
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x00034D95 File Offset: 0x00033D95
		internal DocumentXPathNodeIterator_Empty(DocumentXPathNodeIterator_Empty other)
		{
			this.nav = other.nav.Clone();
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x00034DAE File Offset: 0x00033DAE
		public override XPathNodeIterator Clone()
		{
			return new DocumentXPathNodeIterator_Empty(this);
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x00034DB6 File Offset: 0x00033DB6
		public override bool MoveNext()
		{
			return false;
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000B63 RID: 2915 RVA: 0x00034DB9 File Offset: 0x00033DB9
		public override XPathNavigator Current
		{
			get
			{
				return this.nav;
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000B64 RID: 2916 RVA: 0x00034DC1 File Offset: 0x00033DC1
		public override int CurrentPosition
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000B65 RID: 2917 RVA: 0x00034DC4 File Offset: 0x00033DC4
		public override int Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x040008DE RID: 2270
		private XPathNavigator nav;
	}
}
