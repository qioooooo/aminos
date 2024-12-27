using System;
using System.Xml.XPath;

namespace System.Xml
{
	// Token: 0x020000EC RID: 236
	public class XmlProcessingInstruction : XmlLinkedNode
	{
		// Token: 0x06000E80 RID: 3712 RVA: 0x000405B7 File Offset: 0x0003F5B7
		protected internal XmlProcessingInstruction(string target, string data, XmlDocument doc)
			: base(doc)
		{
			this.target = target;
			this.data = data;
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06000E81 RID: 3713 RVA: 0x000405CE File Offset: 0x0003F5CE
		public override string Name
		{
			get
			{
				if (this.target != null)
				{
					return this.target;
				}
				return string.Empty;
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06000E82 RID: 3714 RVA: 0x000405E4 File Offset: 0x0003F5E4
		public override string LocalName
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06000E83 RID: 3715 RVA: 0x000405EC File Offset: 0x0003F5EC
		// (set) Token: 0x06000E84 RID: 3716 RVA: 0x000405F4 File Offset: 0x0003F5F4
		public override string Value
		{
			get
			{
				return this.data;
			}
			set
			{
				this.Data = value;
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06000E85 RID: 3717 RVA: 0x000405FD File Offset: 0x0003F5FD
		public string Target
		{
			get
			{
				return this.target;
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x06000E86 RID: 3718 RVA: 0x00040605 File Offset: 0x0003F605
		// (set) Token: 0x06000E87 RID: 3719 RVA: 0x00040610 File Offset: 0x0003F610
		public string Data
		{
			get
			{
				return this.data;
			}
			set
			{
				XmlNode parentNode = this.ParentNode;
				XmlNodeChangedEventArgs eventArgs = this.GetEventArgs(this, parentNode, parentNode, this.data, value, XmlNodeChangedAction.Change);
				if (eventArgs != null)
				{
					this.BeforeEvent(eventArgs);
				}
				this.data = value;
				if (eventArgs != null)
				{
					this.AfterEvent(eventArgs);
				}
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x06000E88 RID: 3720 RVA: 0x00040651 File Offset: 0x0003F651
		// (set) Token: 0x06000E89 RID: 3721 RVA: 0x00040659 File Offset: 0x0003F659
		public override string InnerText
		{
			get
			{
				return this.data;
			}
			set
			{
				this.Data = value;
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x06000E8A RID: 3722 RVA: 0x00040662 File Offset: 0x0003F662
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.ProcessingInstruction;
			}
		}

		// Token: 0x06000E8B RID: 3723 RVA: 0x00040665 File Offset: 0x0003F665
		public override XmlNode CloneNode(bool deep)
		{
			return this.OwnerDocument.CreateProcessingInstruction(this.target, this.data);
		}

		// Token: 0x06000E8C RID: 3724 RVA: 0x0004067E File Offset: 0x0003F67E
		public override void WriteTo(XmlWriter w)
		{
			w.WriteProcessingInstruction(this.target, this.data);
		}

		// Token: 0x06000E8D RID: 3725 RVA: 0x00040692 File Offset: 0x0003F692
		public override void WriteContentTo(XmlWriter w)
		{
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06000E8E RID: 3726 RVA: 0x00040694 File Offset: 0x0003F694
		internal override string XPLocalName
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06000E8F RID: 3727 RVA: 0x0004069C File Offset: 0x0003F69C
		internal override XPathNodeType XPNodeType
		{
			get
			{
				return XPathNodeType.ProcessingInstruction;
			}
		}

		// Token: 0x040009A5 RID: 2469
		private string target;

		// Token: 0x040009A6 RID: 2470
		private string data;
	}
}
