using System;
using System.Web.Services.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000F1 RID: 241
	[XmlFormatExtensionPoint("Extensions")]
	public sealed class MessagePart : NamedItem
	{
		// Token: 0x06000665 RID: 1637 RVA: 0x0001D903 File Offset: 0x0001C903
		internal void SetParent(Message parent)
		{
			this.parent = parent;
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000666 RID: 1638 RVA: 0x0001D90C File Offset: 0x0001C90C
		[XmlIgnore]
		public override ServiceDescriptionFormatExtensionCollection Extensions
		{
			get
			{
				if (this.extensions == null)
				{
					this.extensions = new ServiceDescriptionFormatExtensionCollection(this);
				}
				return this.extensions;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000667 RID: 1639 RVA: 0x0001D928 File Offset: 0x0001C928
		public Message Message
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000668 RID: 1640 RVA: 0x0001D930 File Offset: 0x0001C930
		// (set) Token: 0x06000669 RID: 1641 RVA: 0x0001D938 File Offset: 0x0001C938
		[XmlAttribute("element")]
		public XmlQualifiedName Element
		{
			get
			{
				return this.element;
			}
			set
			{
				this.element = value;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x0600066A RID: 1642 RVA: 0x0001D941 File Offset: 0x0001C941
		// (set) Token: 0x0600066B RID: 1643 RVA: 0x0001D957 File Offset: 0x0001C957
		[XmlAttribute("type")]
		public XmlQualifiedName Type
		{
			get
			{
				if (this.type == null)
				{
					return XmlQualifiedName.Empty;
				}
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		// Token: 0x04000478 RID: 1144
		private XmlQualifiedName type = XmlQualifiedName.Empty;

		// Token: 0x04000479 RID: 1145
		private XmlQualifiedName element = XmlQualifiedName.Empty;

		// Token: 0x0400047A RID: 1146
		private Message parent;

		// Token: 0x0400047B RID: 1147
		private ServiceDescriptionFormatExtensionCollection extensions;
	}
}
