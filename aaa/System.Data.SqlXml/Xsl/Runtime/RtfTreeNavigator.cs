using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x0200007C RID: 124
	internal sealed class RtfTreeNavigator : RtfNavigator
	{
		// Token: 0x06000711 RID: 1809 RVA: 0x000256BC File Offset: 0x000246BC
		public RtfTreeNavigator(XmlEventCache events, XmlNameTable nameTable)
		{
			this.events = events;
			this.constr = new NavigatorConstructor();
			this.nameTable = nameTable;
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x000256DD File Offset: 0x000246DD
		public RtfTreeNavigator(RtfTreeNavigator that)
		{
			this.events = that.events;
			this.constr = that.constr;
			this.nameTable = that.nameTable;
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x00025709 File Offset: 0x00024709
		public override void CopyToWriter(XmlWriter writer)
		{
			this.events.EventsToWriter(writer);
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x00025717 File Offset: 0x00024717
		public override XPathNavigator ToNavigator()
		{
			return this.constr.GetNavigator(this.events, this.nameTable);
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000715 RID: 1813 RVA: 0x00025730 File Offset: 0x00024730
		public override string Value
		{
			get
			{
				return this.events.EventsToString();
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000716 RID: 1814 RVA: 0x0002573D File Offset: 0x0002473D
		public override string BaseURI
		{
			get
			{
				return this.events.BaseUri;
			}
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x0002574A File Offset: 0x0002474A
		public override XPathNavigator Clone()
		{
			return new RtfTreeNavigator(this);
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x00025754 File Offset: 0x00024754
		public override bool MoveTo(XPathNavigator other)
		{
			RtfTreeNavigator rtfTreeNavigator = other as RtfTreeNavigator;
			if (rtfTreeNavigator != null)
			{
				this.events = rtfTreeNavigator.events;
				this.constr = rtfTreeNavigator.constr;
				this.nameTable = rtfTreeNavigator.nameTable;
				return true;
			}
			return false;
		}

		// Token: 0x04000496 RID: 1174
		private XmlEventCache events;

		// Token: 0x04000497 RID: 1175
		private NavigatorConstructor constr;

		// Token: 0x04000498 RID: 1176
		private XmlNameTable nameTable;
	}
}
