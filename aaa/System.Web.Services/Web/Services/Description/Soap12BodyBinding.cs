using System;
using System.Web.Services.Configuration;

namespace System.Web.Services.Description
{
	// Token: 0x0200010F RID: 271
	[XmlFormatExtension("body", "http://schemas.xmlsoap.org/wsdl/soap12/", typeof(InputBinding), typeof(OutputBinding), typeof(MimePart))]
	public sealed class Soap12BodyBinding : SoapBodyBinding
	{
	}
}
