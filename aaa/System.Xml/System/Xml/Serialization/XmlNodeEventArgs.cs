using System;

namespace System.Xml.Serialization
{
	// Token: 0x02000344 RID: 836
	public class XmlNodeEventArgs : EventArgs
	{
		// Token: 0x060028BE RID: 10430 RVA: 0x000D1E2C File Offset: 0x000D0E2C
		internal XmlNodeEventArgs(XmlNode xmlNode, int lineNumber, int linePosition, object o)
		{
			this.o = o;
			this.xmlNode = xmlNode;
			this.lineNumber = lineNumber;
			this.linePosition = linePosition;
		}

		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x060028BF RID: 10431 RVA: 0x000D1E51 File Offset: 0x000D0E51
		public object ObjectBeingDeserialized
		{
			get
			{
				return this.o;
			}
		}

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x060028C0 RID: 10432 RVA: 0x000D1E59 File Offset: 0x000D0E59
		public XmlNodeType NodeType
		{
			get
			{
				return this.xmlNode.NodeType;
			}
		}

		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x060028C1 RID: 10433 RVA: 0x000D1E66 File Offset: 0x000D0E66
		public string Name
		{
			get
			{
				return this.xmlNode.Name;
			}
		}

		// Token: 0x170009AC RID: 2476
		// (get) Token: 0x060028C2 RID: 10434 RVA: 0x000D1E73 File Offset: 0x000D0E73
		public string LocalName
		{
			get
			{
				return this.xmlNode.LocalName;
			}
		}

		// Token: 0x170009AD RID: 2477
		// (get) Token: 0x060028C3 RID: 10435 RVA: 0x000D1E80 File Offset: 0x000D0E80
		public string NamespaceURI
		{
			get
			{
				return this.xmlNode.NamespaceURI;
			}
		}

		// Token: 0x170009AE RID: 2478
		// (get) Token: 0x060028C4 RID: 10436 RVA: 0x000D1E8D File Offset: 0x000D0E8D
		public string Text
		{
			get
			{
				return this.xmlNode.Value;
			}
		}

		// Token: 0x170009AF RID: 2479
		// (get) Token: 0x060028C5 RID: 10437 RVA: 0x000D1E9A File Offset: 0x000D0E9A
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
		}

		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x060028C6 RID: 10438 RVA: 0x000D1EA2 File Offset: 0x000D0EA2
		public int LinePosition
		{
			get
			{
				return this.linePosition;
			}
		}

		// Token: 0x04001696 RID: 5782
		private object o;

		// Token: 0x04001697 RID: 5783
		private XmlNode xmlNode;

		// Token: 0x04001698 RID: 5784
		private int lineNumber;

		// Token: 0x04001699 RID: 5785
		private int linePosition;
	}
}
