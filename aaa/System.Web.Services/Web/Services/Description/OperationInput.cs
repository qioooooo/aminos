using System;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000EC RID: 236
	[XmlFormatExtensionPoint("Extensions")]
	public sealed class OperationInput : OperationMessage
	{
		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x0001D522 File Offset: 0x0001C522
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

		// Token: 0x0400046B RID: 1131
		private ServiceDescriptionFormatExtensionCollection extensions;
	}
}
