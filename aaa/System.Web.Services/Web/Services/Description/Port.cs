using System;
using System.Web.Services.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000E2 RID: 226
	[XmlFormatExtensionPoint("Extensions")]
	public sealed class Port : NamedItem
	{
		// Token: 0x0600061E RID: 1566 RVA: 0x0001D264 File Offset: 0x0001C264
		internal void SetParent(Service parent)
		{
			this.parent = parent;
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x0001D26D File Offset: 0x0001C26D
		public Service Service
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000620 RID: 1568 RVA: 0x0001D275 File Offset: 0x0001C275
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

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000621 RID: 1569 RVA: 0x0001D291 File Offset: 0x0001C291
		// (set) Token: 0x06000622 RID: 1570 RVA: 0x0001D299 File Offset: 0x0001C299
		[XmlAttribute("binding")]
		public XmlQualifiedName Binding
		{
			get
			{
				return this.binding;
			}
			set
			{
				this.binding = value;
			}
		}

		// Token: 0x04000455 RID: 1109
		private ServiceDescriptionFormatExtensionCollection extensions;

		// Token: 0x04000456 RID: 1110
		private XmlQualifiedName binding = XmlQualifiedName.Empty;

		// Token: 0x04000457 RID: 1111
		private Service parent;
	}
}
