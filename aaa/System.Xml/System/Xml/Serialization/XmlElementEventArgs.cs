using System;

namespace System.Xml.Serialization
{
	// Token: 0x02000342 RID: 834
	public class XmlElementEventArgs : EventArgs
	{
		// Token: 0x060028B4 RID: 10420 RVA: 0x000D1DC9 File Offset: 0x000D0DC9
		internal XmlElementEventArgs(XmlElement elem, int lineNumber, int linePosition, object o, string qnames)
		{
			this.elem = elem;
			this.o = o;
			this.qnames = qnames;
			this.lineNumber = lineNumber;
			this.linePosition = linePosition;
		}

		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x060028B5 RID: 10421 RVA: 0x000D1DF6 File Offset: 0x000D0DF6
		public object ObjectBeingDeserialized
		{
			get
			{
				return this.o;
			}
		}

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x060028B6 RID: 10422 RVA: 0x000D1DFE File Offset: 0x000D0DFE
		public XmlElement Element
		{
			get
			{
				return this.elem;
			}
		}

		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x060028B7 RID: 10423 RVA: 0x000D1E06 File Offset: 0x000D0E06
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x060028B8 RID: 10424 RVA: 0x000D1E0E File Offset: 0x000D0E0E
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x060028B9 RID: 10425 RVA: 0x000D1E16 File Offset: 0x000D0E16
		public string ExpectedElements
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

		// Token: 0x04001691 RID: 5777
		private object o;

		// Token: 0x04001692 RID: 5778
		private XmlElement elem;

		// Token: 0x04001693 RID: 5779
		private string qnames;

		// Token: 0x04001694 RID: 5780
		private int lineNumber;

		// Token: 0x04001695 RID: 5781
		private int linePosition;
	}
}
