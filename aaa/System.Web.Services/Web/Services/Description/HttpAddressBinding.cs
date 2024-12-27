using System;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000B6 RID: 182
	[XmlFormatExtension("address", "http://schemas.xmlsoap.org/wsdl/http/", typeof(Port))]
	public sealed class HttpAddressBinding : ServiceDescriptionFormatExtension
	{
		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060004C9 RID: 1225 RVA: 0x00017DE2 File Offset: 0x00016DE2
		// (set) Token: 0x060004CA RID: 1226 RVA: 0x00017DF8 File Offset: 0x00016DF8
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

		// Token: 0x040003E1 RID: 993
		private string location;
	}
}
