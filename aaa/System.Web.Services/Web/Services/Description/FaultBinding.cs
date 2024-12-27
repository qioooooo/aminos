using System;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000E5 RID: 229
	[XmlFormatExtensionPoint("Extensions")]
	public sealed class FaultBinding : MessageBinding
	{
		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x0001D31F File Offset: 0x0001C31F
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

		// Token: 0x0400045C RID: 1116
		private ServiceDescriptionFormatExtensionCollection extensions;
	}
}
