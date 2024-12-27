using System;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000EF RID: 239
	[XmlFormatExtensionPoint("Extensions")]
	public sealed class PortType : NamedItem
	{
		// Token: 0x06000659 RID: 1625 RVA: 0x0001D7B4 File Offset: 0x0001C7B4
		internal void SetParent(ServiceDescription parent)
		{
			this.parent = parent;
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x0001D7BD File Offset: 0x0001C7BD
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

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x0600065B RID: 1627 RVA: 0x0001D7D9 File Offset: 0x0001C7D9
		public ServiceDescription ServiceDescription
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x0001D7E1 File Offset: 0x0001C7E1
		[XmlElement("operation")]
		public OperationCollection Operations
		{
			get
			{
				if (this.operations == null)
				{
					this.operations = new OperationCollection(this);
				}
				return this.operations;
			}
		}

		// Token: 0x04000472 RID: 1138
		private OperationCollection operations;

		// Token: 0x04000473 RID: 1139
		private ServiceDescription parent;

		// Token: 0x04000474 RID: 1140
		private ServiceDescriptionFormatExtensionCollection extensions;
	}
}
