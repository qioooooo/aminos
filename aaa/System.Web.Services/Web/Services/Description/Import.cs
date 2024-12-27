using System;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000E1 RID: 225
	[XmlFormatExtensionPoint("Extensions")]
	public sealed class Import : DocumentableItem
	{
		// Token: 0x06000616 RID: 1558 RVA: 0x0001D1F1 File Offset: 0x0001C1F1
		internal void SetParent(ServiceDescription parent)
		{
			this.parent = parent;
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000617 RID: 1559 RVA: 0x0001D1FA File Offset: 0x0001C1FA
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

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000618 RID: 1560 RVA: 0x0001D216 File Offset: 0x0001C216
		public ServiceDescription ServiceDescription
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000619 RID: 1561 RVA: 0x0001D21E File Offset: 0x0001C21E
		// (set) Token: 0x0600061A RID: 1562 RVA: 0x0001D234 File Offset: 0x0001C234
		[XmlAttribute("namespace")]
		public string Namespace
		{
			get
			{
				if (this.ns != null)
				{
					return this.ns;
				}
				return string.Empty;
			}
			set
			{
				this.ns = value;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x0600061B RID: 1563 RVA: 0x0001D23D File Offset: 0x0001C23D
		// (set) Token: 0x0600061C RID: 1564 RVA: 0x0001D253 File Offset: 0x0001C253
		[XmlAttribute("location")]
		public string Location
		{
			get
			{
				if (this.location != null)
				{
					return this.location;
				}
				return string.Empty;
			}
			set
			{
				this.location = value;
			}
		}

		// Token: 0x04000451 RID: 1105
		private string ns;

		// Token: 0x04000452 RID: 1106
		private string location;

		// Token: 0x04000453 RID: 1107
		private ServiceDescription parent;

		// Token: 0x04000454 RID: 1108
		private ServiceDescriptionFormatExtensionCollection extensions;
	}
}
