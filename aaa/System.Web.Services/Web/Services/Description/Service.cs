using System;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000E3 RID: 227
	[XmlFormatExtensionPoint("Extensions")]
	public sealed class Service : NamedItem
	{
		// Token: 0x06000624 RID: 1572 RVA: 0x0001D2B5 File Offset: 0x0001C2B5
		internal void SetParent(ServiceDescription parent)
		{
			this.parent = parent;
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000625 RID: 1573 RVA: 0x0001D2BE File Offset: 0x0001C2BE
		public ServiceDescription ServiceDescription
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000626 RID: 1574 RVA: 0x0001D2C6 File Offset: 0x0001C2C6
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

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000627 RID: 1575 RVA: 0x0001D2E2 File Offset: 0x0001C2E2
		[XmlElement("port")]
		public PortCollection Ports
		{
			get
			{
				if (this.ports == null)
				{
					this.ports = new PortCollection(this);
				}
				return this.ports;
			}
		}

		// Token: 0x04000458 RID: 1112
		private ServiceDescriptionFormatExtensionCollection extensions;

		// Token: 0x04000459 RID: 1113
		private PortCollection ports;

		// Token: 0x0400045A RID: 1114
		private ServiceDescription parent;
	}
}
