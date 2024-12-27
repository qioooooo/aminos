using System;

namespace System.Xml.Serialization
{
	// Token: 0x02000335 RID: 821
	public struct XmlDeserializationEvents
	{
		// Token: 0x17000983 RID: 2435
		// (get) Token: 0x06002827 RID: 10279 RVA: 0x000D0504 File Offset: 0x000CF504
		// (set) Token: 0x06002828 RID: 10280 RVA: 0x000D050C File Offset: 0x000CF50C
		public XmlNodeEventHandler OnUnknownNode
		{
			get
			{
				return this.onUnknownNode;
			}
			set
			{
				this.onUnknownNode = value;
			}
		}

		// Token: 0x17000984 RID: 2436
		// (get) Token: 0x06002829 RID: 10281 RVA: 0x000D0515 File Offset: 0x000CF515
		// (set) Token: 0x0600282A RID: 10282 RVA: 0x000D051D File Offset: 0x000CF51D
		public XmlAttributeEventHandler OnUnknownAttribute
		{
			get
			{
				return this.onUnknownAttribute;
			}
			set
			{
				this.onUnknownAttribute = value;
			}
		}

		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x0600282B RID: 10283 RVA: 0x000D0526 File Offset: 0x000CF526
		// (set) Token: 0x0600282C RID: 10284 RVA: 0x000D052E File Offset: 0x000CF52E
		public XmlElementEventHandler OnUnknownElement
		{
			get
			{
				return this.onUnknownElement;
			}
			set
			{
				this.onUnknownElement = value;
			}
		}

		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x0600282D RID: 10285 RVA: 0x000D0537 File Offset: 0x000CF537
		// (set) Token: 0x0600282E RID: 10286 RVA: 0x000D053F File Offset: 0x000CF53F
		public UnreferencedObjectEventHandler OnUnreferencedObject
		{
			get
			{
				return this.onUnreferencedObject;
			}
			set
			{
				this.onUnreferencedObject = value;
			}
		}

		// Token: 0x04001672 RID: 5746
		private XmlNodeEventHandler onUnknownNode;

		// Token: 0x04001673 RID: 5747
		private XmlAttributeEventHandler onUnknownAttribute;

		// Token: 0x04001674 RID: 5748
		private XmlElementEventHandler onUnknownElement;

		// Token: 0x04001675 RID: 5749
		private UnreferencedObjectEventHandler onUnreferencedObject;

		// Token: 0x04001676 RID: 5750
		internal object sender;
	}
}
