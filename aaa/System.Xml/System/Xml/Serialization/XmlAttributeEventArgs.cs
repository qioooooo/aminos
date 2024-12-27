using System;

namespace System.Xml.Serialization
{
	// Token: 0x02000340 RID: 832
	public class XmlAttributeEventArgs : EventArgs
	{
		// Token: 0x060028AA RID: 10410 RVA: 0x000D1D66 File Offset: 0x000D0D66
		internal XmlAttributeEventArgs(XmlAttribute attr, int lineNumber, int linePosition, object o, string qnames)
		{
			this.attr = attr;
			this.o = o;
			this.qnames = qnames;
			this.lineNumber = lineNumber;
			this.linePosition = linePosition;
		}

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x060028AB RID: 10411 RVA: 0x000D1D93 File Offset: 0x000D0D93
		public object ObjectBeingDeserialized
		{
			get
			{
				return this.o;
			}
		}

		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x060028AC RID: 10412 RVA: 0x000D1D9B File Offset: 0x000D0D9B
		public XmlAttribute Attr
		{
			get
			{
				return this.attr;
			}
		}

		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x060028AD RID: 10413 RVA: 0x000D1DA3 File Offset: 0x000D0DA3
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x060028AE RID: 10414 RVA: 0x000D1DAB File Offset: 0x000D0DAB
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x060028AF RID: 10415 RVA: 0x000D1DB3 File Offset: 0x000D0DB3
		public string ExpectedAttributes
		{
			get
			{
				if (this.qnames != null)
				{
					return this.qnames;
				}
				return string.Empty;
			}
		}

		// Token: 0x0400168C RID: 5772
		private object o;

		// Token: 0x0400168D RID: 5773
		private XmlAttribute attr;

		// Token: 0x0400168E RID: 5774
		private string qnames;

		// Token: 0x0400168F RID: 5775
		private int lineNumber;

		// Token: 0x04001690 RID: 5776
		private int linePosition;
	}
}
