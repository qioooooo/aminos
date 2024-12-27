using System;
using System.Diagnostics;
using System.Xml.Xsl.Qil;
using System.Xml.Xsl.XPath;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x020000EB RID: 235
	internal struct SingletonFocus : IFocus
	{
		// Token: 0x06000AB0 RID: 2736 RVA: 0x0003383F File Offset: 0x0003283F
		public SingletonFocus(XPathQilFactory f)
		{
			this.f = f;
			this.focusType = SingletonFocusType.None;
			this.current = null;
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x00033856 File Offset: 0x00032856
		public void SetFocus(SingletonFocusType focusType)
		{
			this.focusType = focusType;
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x0003385F File Offset: 0x0003285F
		public void SetFocus(QilIterator current)
		{
			if (current != null)
			{
				this.focusType = SingletonFocusType.Iterator;
				this.current = current;
				return;
			}
			this.focusType = SingletonFocusType.None;
			this.current = null;
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x00033881 File Offset: 0x00032881
		[Conditional("DEBUG")]
		private void CheckFocus()
		{
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x00033884 File Offset: 0x00032884
		public QilNode GetCurrent()
		{
			switch (this.focusType)
			{
			case SingletonFocusType.InitialDocumentNode:
				return this.f.Root(this.f.XmlContext());
			case SingletonFocusType.InitialContextNode:
				return this.f.XmlContext();
			default:
				return this.current;
			}
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x000338D3 File Offset: 0x000328D3
		public QilNode GetPosition()
		{
			return this.f.Double(1.0);
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x000338E9 File Offset: 0x000328E9
		public QilNode GetLast()
		{
			return this.f.Double(1.0);
		}

		// Token: 0x04000727 RID: 1831
		private XPathQilFactory f;

		// Token: 0x04000728 RID: 1832
		private SingletonFocusType focusType;

		// Token: 0x04000729 RID: 1833
		private QilIterator current;
	}
}
