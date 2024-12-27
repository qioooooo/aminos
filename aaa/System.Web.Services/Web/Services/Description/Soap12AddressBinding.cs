using System;
using System.Web.Services.Configuration;

namespace System.Web.Services.Description
{
	// Token: 0x02000115 RID: 277
	[XmlFormatExtension("address", "http://schemas.xmlsoap.org/wsdl/soap12/", typeof(Port))]
	public sealed class Soap12AddressBinding : SoapAddressBinding
	{
	}
}
