using System;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000C8 RID: 200
	[XmlFormatExtensionPoint("Extensions")]
	public sealed class MimePart : ServiceDescriptionFormatExtension
	{
		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x0001B20F File Offset: 0x0001A20F
		[XmlIgnore]
		public ServiceDescriptionFormatExtensionCollection Extensions
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

		// Token: 0x04000418 RID: 1048
		private ServiceDescriptionFormatExtensionCollection extensions;
	}
}
