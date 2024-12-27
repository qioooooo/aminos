using System;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000EA RID: 234
	public abstract class OperationMessage : NamedItem
	{
		// Token: 0x06000642 RID: 1602 RVA: 0x0001D4C9 File Offset: 0x0001C4C9
		internal void SetParent(Operation parent)
		{
			this.parent = parent;
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x0001D4D2 File Offset: 0x0001C4D2
		public Operation Operation
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x0001D4DA File Offset: 0x0001C4DA
		// (set) Token: 0x06000645 RID: 1605 RVA: 0x0001D4E2 File Offset: 0x0001C4E2
		[XmlAttribute("message")]
		public XmlQualifiedName Message
		{
			get
			{
				return this.message;
			}
			set
			{
				this.message = value;
			}
		}

		// Token: 0x04000468 RID: 1128
		private XmlQualifiedName message = XmlQualifiedName.Empty;

		// Token: 0x04000469 RID: 1129
		private Operation parent;
	}
}
