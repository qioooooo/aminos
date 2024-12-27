using System;
using System.Web.Services.Configuration;

namespace System.Web.Services.Description
{
	// Token: 0x02000113 RID: 275
	[XmlFormatExtension("header", "http://schemas.xmlsoap.org/wsdl/soap12/", typeof(InputBinding), typeof(OutputBinding))]
	public sealed class Soap12HeaderBinding : SoapHeaderBinding
	{
	}
}
