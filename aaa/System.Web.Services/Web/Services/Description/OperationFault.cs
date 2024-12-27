using System;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000EB RID: 235
	[XmlFormatExtensionPoint("Extensions")]
	public sealed class OperationFault : OperationMessage
	{
		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x0001D4FE File Offset: 0x0001C4FE
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

		// Token: 0x0400046A RID: 1130
		private ServiceDescriptionFormatExtensionCollection extensions;
	}
}
