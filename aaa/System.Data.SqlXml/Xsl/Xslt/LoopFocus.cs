using System;
using System.Xml.Xsl.Qil;
using System.Xml.Xsl.XPath;

namespace System.Xml.Xsl.Xslt
{
	// Token: 0x020000ED RID: 237
	internal struct LoopFocus : IFocus
	{
		// Token: 0x06000ABD RID: 2749 RVA: 0x000339C4 File Offset: 0x000329C4
		public LoopFocus(XPathQilFactory f)
		{
			this.f = f;
			this.current = (this.cached = (this.last = null));
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x000339F4 File Offset: 0x000329F4
		public void SetFocus(QilIterator current)
		{
			this.current = current;
			this.cached = (this.last = null);
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000ABF RID: 2751 RVA: 0x00033A18 File Offset: 0x00032A18
		public bool IsFocusSet
		{
			get
			{
				return this.current != null;
			}
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x00033A26 File Offset: 0x00032A26
		public QilNode GetCurrent()
		{
			return this.current;
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x00033A2E File Offset: 0x00032A2E
		public QilNode GetPosition()
		{
			return this.f.XsltConvert(this.f.PositionOf(this.current), XmlQueryTypeFactory.DoubleX);
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x00033A51 File Offset: 0x00032A51
		public QilNode GetLast()
		{
			if (this.last == null)
			{
				this.last = this.f.Let(this.f.Double(0.0));
			}
			return this.last;
		}

		// Token: 0x06000AC3 RID: 2755 RVA: 0x00033A86 File Offset: 0x00032A86
		public void EnsureCache()
		{
			if (this.cached == null)
			{
				this.cached = this.f.Let(this.current.Binding);
				this.current.Binding = this.cached;
			}
		}

		// Token: 0x06000AC4 RID: 2756 RVA: 0x00033ABD File Offset: 0x00032ABD
		public void Sort(QilNode sortKeys)
		{
			if (sortKeys != null)
			{
				this.EnsureCache();
				this.current = this.f.For(this.f.Sort(this.current, sortKeys));
			}
		}

		// Token: 0x06000AC5 RID: 2757 RVA: 0x00033AEC File Offset: 0x00032AEC
		public QilLoop ConstructLoop(QilNode body)
		{
			if (this.last != null)
			{
				this.EnsureCache();
				this.last.Binding = this.f.XsltConvert(this.f.Length(this.cached), XmlQueryTypeFactory.DoubleX);
			}
			QilLoop qilLoop = this.f.BaseFactory.Loop(this.current, body);
			if (this.last != null)
			{
				qilLoop = this.f.BaseFactory.Loop(this.last, qilLoop);
			}
			if (this.cached != null)
			{
				qilLoop = this.f.BaseFactory.Loop(this.cached, qilLoop);
			}
			return qilLoop;
		}

		// Token: 0x0400072E RID: 1838
		private XPathQilFactory f;

		// Token: 0x0400072F RID: 1839
		private QilIterator current;

		// Token: 0x04000730 RID: 1840
		private QilIterator cached;

		// Token: 0x04000731 RID: 1841
		private QilIterator last;
	}
}
