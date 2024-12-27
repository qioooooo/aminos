using System;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000ED RID: 237
	[XmlFormatExtensionPoint("Extensions")]
	public sealed class OperationOutput : OperationMessage
	{
		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x0001D546 File Offset: 0x0001C546
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

		// Token: 0x0400046C RID: 1132
		private ServiceDescriptionFormatExtensionCollection extensions;
	}
}
