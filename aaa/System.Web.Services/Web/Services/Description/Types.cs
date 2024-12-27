using System;
using System.Web.Services.Configuration;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000F2 RID: 242
	[XmlFormatExtensionPoint("Extensions")]
	public sealed class Types : DocumentableItem
	{
		// Token: 0x0600066D RID: 1645 RVA: 0x0001D97E File Offset: 0x0001C97E
		internal bool HasItems()
		{
			return (this.schemas != null && this.schemas.Count > 0) || (this.extensions != null && this.extensions.Count > 0);
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x0600066E RID: 1646 RVA: 0x0001D9B0 File Offset: 0x0001C9B0
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

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x0600066F RID: 1647 RVA: 0x0001D9CC File Offset: 0x0001C9CC
		[XmlElement("schema", typeof(XmlSchema), Namespace = "http://www.w3.org/2001/XMLSchema")]
		public XmlSchemas Schemas
		{
			get
			{
				if (this.schemas == null)
				{
					this.schemas = new XmlSchemas();
				}
				return this.schemas;
			}
		}

		// Token: 0x0400047C RID: 1148
		private XmlSchemas schemas;

		// Token: 0x0400047D RID: 1149
		private ServiceDescriptionFormatExtensionCollection extensions;
	}
}
