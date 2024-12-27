using System;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000E7 RID: 231
	[XmlFormatExtensionPoint("Extensions")]
	public sealed class OutputBinding : MessageBinding
	{
		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x0001D367 File Offset: 0x0001C367
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

		// Token: 0x0400045E RID: 1118
		private ServiceDescriptionFormatExtensionCollection extensions;
	}
}
