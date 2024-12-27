using System;
using System.Web.Services.Configuration;

namespace System.Web.Services.Description
{
	// Token: 0x02000111 RID: 273
	[XmlFormatExtension("fault", "http://schemas.xmlsoap.org/wsdl/soap12/", typeof(FaultBinding))]
	public sealed class Soap12FaultBinding : SoapFaultBinding
	{
	}
}
