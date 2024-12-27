using System;
using System.Xml.XPath;

namespace System.Xml.Xsl.Runtime
{
	// Token: 0x0200007D RID: 125
	internal sealed class RtfTextNavigator : RtfNavigator
	{
		// Token: 0x06000719 RID: 1817 RVA: 0x00025792 File Offset: 0x00024792
		public RtfTextNavigator(string text, string baseUri)
		{
			this.text = text;
			this.baseUri = baseUri;
			this.constr = new NavigatorConstructor();
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x000257B3 File Offset: 0x000247B3
		public RtfTextNavigator(RtfTextNavigator that)
		{
			this.text = that.text;
			this.baseUri = that.baseUri;
			this.constr = that.constr;
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x000257DF File Offset: 0x000247DF
		public override void CopyToWriter(XmlWriter writer)
		{
			writer.WriteString(this.Value);
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x000257ED File Offset: 0x000247ED
		public override XPathNavigator ToNavigator()
		{
			return this.constr.GetNavigator(this.text, this.baseUri, new NameTable());
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600071D RID: 1821 RVA: 0x0002580B File Offset: 0x0002480B
		public override string Value
		{
			get
			{
				return this.text;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600071E RID: 1822 RVA: 0x00025813 File Offset: 0x00024813
		public override string BaseURI
		{
			get
			{
				return this.baseUri;
			}
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0002581B File Offset: 0x0002481B
		public override XPathNavigator Clone()
		{
			return new RtfTextNavigator(this);
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x00025824 File Offset: 0x00024824
		public override bool MoveTo(XPathNavigator other)
		{
			RtfTextNavigator rtfTextNavigator = other as RtfTextNavigator;
			if (rtfTextNavigator != null)
			{
				this.text = rtfTextNavigator.text;
				this.baseUri = rtfTextNavigator.baseUri;
				this.constr = rtfTextNavigator.constr;
				return true;
			}
			return false;
		}

		// Token: 0x04000499 RID: 1177
		private string text;

		// Token: 0x0400049A RID: 1178
		private string baseUri;

		// Token: 0x0400049B RID: 1179
		private NavigatorConstructor constr;
	}
}
