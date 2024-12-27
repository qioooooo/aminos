using System;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000E6 RID: 230
	[XmlFormatExtensionPoint("Extensions")]
	public sealed class InputBinding : MessageBinding
	{
		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x0600062E RID: 1582 RVA: 0x0001D343 File Offset: 0x0001C343
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

		// Token: 0x0400045D RID: 1117
		private ServiceDescriptionFormatExtensionCollection extensions;
	}
}
