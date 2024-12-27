using System;
using System.Web.Services.Configuration;

namespace System.Web.Services.Description
{
	// Token: 0x0200010B RID: 267
	[XmlFormatExtension("binding", "http://schemas.xmlsoap.org/wsdl/soap12/", typeof(Binding))]
	[XmlFormatExtensionPrefix("soap12", "http://schemas.xmlsoap.org/wsdl/soap12/")]
	public sealed class Soap12Binding : SoapBinding
	{
		// Token: 0x0400058A RID: 1418
		public new const string Namespace = "http://schemas.xmlsoap.org/wsdl/soap12/";

		// Token: 0x0400058B RID: 1419
		public new const string HttpTransport = "http://schemas.xmlsoap.org/soap/http";
	}
}
