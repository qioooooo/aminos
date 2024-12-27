using System;
using System.Web.Services.Configuration;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x02000114 RID: 276
	[XmlFormatExtension("address", "http://schemas.xmlsoap.org/wsdl/soap/", typeof(Port))]
	public class SoapAddressBinding : ServiceDescriptionFormatExtension
	{
		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000862 RID: 2146 RVA: 0x0003D744 File Offset: 0x0003C744
		// (set) Token: 0x06000863 RID: 2147 RVA: 0x0003D75A File Offset: 0x0003C75A
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

		// Token: 0x040005A1 RID: 1441
		private string location;
	}
}
