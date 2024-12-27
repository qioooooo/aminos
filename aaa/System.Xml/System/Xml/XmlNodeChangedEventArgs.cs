using System;

namespace System.Xml
{
	// Token: 0x020000E6 RID: 230
	public class XmlNodeChangedEventArgs : EventArgs
	{
		// Token: 0x06000DFB RID: 3579 RVA: 0x0003E36D File Offset: 0x0003D36D
		public XmlNodeChangedEventArgs(XmlNode node, XmlNode oldParent, XmlNode newParent, string oldValue, string newValue, XmlNodeChangedAction action)
		{
			this.node = node;
			this.oldParent = oldParent;
			this.newParent = newParent;
			this.action = action;
			this.oldValue = oldValue;
			this.newValue = newValue;
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06000DFC RID: 3580 RVA: 0x0003E3A2 File Offset: 0x0003D3A2
		public XmlNodeChangedAction Action
		{
			get
			{
				return this.action;
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06000DFD RID: 3581 RVA: 0x0003E3AA File Offset: 0x0003D3AA
		public XmlNode Node
		{
			get
			{
				return this.node;
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06000DFE RID: 3582 RVA: 0x0003E3B2 File Offset: 0x0003D3B2
		public XmlNode OldParent
		{
			get
			{
				return this.oldParent;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06000DFF RID: 3583 RVA: 0x0003E3BA File Offset: 0x0003D3BA
		public XmlNode NewParent
		{
			get
			{
				return this.newParent;
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000E00 RID: 3584 RVA: 0x0003E3C2 File Offset: 0x0003D3C2
		public string OldValue
		{
			get
			{
				return this.oldValue;
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000E01 RID: 3585 RVA: 0x0003E3CA File Offset: 0x0003D3CA
		public string NewValue
		{
			get
			{
				return this.newValue;
			}
		}

		// Token: 0x0400097B RID: 2427
		private XmlNodeChangedAction action;

		// Token: 0x0400097C RID: 2428
		private XmlNode node;

		// Token: 0x0400097D RID: 2429
		private XmlNode oldParent;

		// Token: 0x0400097E RID: 2430
		private XmlNode newParent;

		// Token: 0x0400097F RID: 2431
		private string oldValue;

		// Token: 0x04000980 RID: 2432
		private string newValue;
	}
}
