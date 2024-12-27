using System;
using System.Net;
using System.Web.Services.Configuration;
using System.Xml;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200005B RID: 91
	internal abstract class SoapServerProtocolHelper
	{
		// Token: 0x06000210 RID: 528 RVA: 0x0000A1F1 File Offset: 0x000091F1
		protected SoapServerProtocolHelper(SoapServerProtocol protocol)
		{
			this.protocol = protocol;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000A200 File Offset: 0x00009200
		protected SoapServerProtocolHelper(SoapServerProtocol protocol, string requestNamespace)
		{
			this.protocol = protocol;
			this.requestNamespace = requestNamespace;
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000A218 File Offset: 0x00009218
		internal static SoapServerProtocolHelper GetHelper(SoapServerProtocol protocol, string envelopeNs)
		{
			SoapServerProtocolHelper soapServerProtocolHelper;
			if (envelopeNs == "http://schemas.xmlsoap.org/soap/envelope/")
			{
				soapServerProtocolHelper = new Soap11ServerProtocolHelper(protocol, envelopeNs);
			}
			else if (envelopeNs == "http://www.w3.org/2003/05/soap-envelope")
			{
				soapServerProtocolHelper = new Soap12ServerProtocolHelper(protocol, envelopeNs);
			}
			else
			{
				soapServerProtocolHelper = new Soap11ServerProtocolHelper(protocol, envelopeNs);
			}
			return soapServerProtocolHelper;
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000A25C File Offset: 0x0000925C
		internal HttpStatusCode SetResponseErrorCode(HttpResponse response, SoapException soapException)
		{
			if (soapException.SubCode != null && soapException.SubCode.Code == Soap12FaultCodes.UnsupportedMediaTypeFaultCode)
			{
				response.StatusCode = 415;
				soapException.ClearSubCode();
			}
			else if (SoapException.IsClientFaultCode(soapException.Code))
			{
				global::System.Web.Services.Protocols.ServerProtocol.SetHttpResponseStatusCode(response, 500);
				for (Exception ex = soapException; ex != null; ex = ex.InnerException)
				{
					if (ex is XmlException)
					{
						response.StatusCode = 400;
					}
				}
			}
			else
			{
				global::System.Web.Services.Protocols.ServerProtocol.SetHttpResponseStatusCode(response, 500);
			}
			response.StatusDescription = HttpWorkerRequest.GetStatusDescription(response.StatusCode);
			return (HttpStatusCode)response.StatusCode;
		}

		// Token: 0x06000214 RID: 532
		internal abstract void WriteFault(XmlWriter writer, SoapException soapException, HttpStatusCode statusCode);

		// Token: 0x06000215 RID: 533
		internal abstract SoapServerMethod RouteRequest();

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000216 RID: 534
		internal abstract SoapProtocolVersion Version { get; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000217 RID: 535
		internal abstract WebServiceProtocols Protocol { get; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000218 RID: 536
		internal abstract string EnvelopeNs { get; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000219 RID: 537
		internal abstract string EncodingNs { get; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600021A RID: 538
		internal abstract string HttpContentType { get; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600021B RID: 539 RVA: 0x0000A2F8 File Offset: 0x000092F8
		internal string RequestNamespace
		{
			get
			{
				return this.requestNamespace;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600021C RID: 540 RVA: 0x0000A300 File Offset: 0x00009300
		protected SoapServerProtocol ServerProtocol
		{
			get
			{
				return this.protocol;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x0600021D RID: 541 RVA: 0x0000A308 File Offset: 0x00009308
		protected SoapServerType ServerType
		{
			get
			{
				return (SoapServerType)this.protocol.ServerType;
			}
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000A31C File Offset: 0x0000931C
		protected XmlQualifiedName GetRequestElement()
		{
			SoapServerMessage message = this.ServerProtocol.Message;
			long position = message.Stream.Position;
			XmlReader xmlReader = this.protocol.GetXmlReader();
			xmlReader.MoveToContent();
			this.requestNamespace = xmlReader.NamespaceURI;
			if (!xmlReader.IsStartElement("Envelope", this.requestNamespace))
			{
				throw new InvalidOperationException(Res.GetString("WebMissingEnvelopeElement"));
			}
			if (xmlReader.IsEmptyElement)
			{
				throw new InvalidOperationException(Res.GetString("WebMissingBodyElement"));
			}
			xmlReader.ReadStartElement("Envelope", this.requestNamespace);
			xmlReader.MoveToContent();
			while (!xmlReader.EOF && !xmlReader.IsStartElement("Body", this.requestNamespace))
			{
				xmlReader.Skip();
			}
			if (xmlReader.EOF)
			{
				throw new InvalidOperationException(Res.GetString("WebMissingBodyElement"));
			}
			XmlQualifiedName xmlQualifiedName;
			if (xmlReader.IsEmptyElement)
			{
				xmlQualifiedName = XmlQualifiedName.Empty;
			}
			else
			{
				xmlReader.ReadStartElement("Body", this.requestNamespace);
				xmlReader.MoveToContent();
				xmlQualifiedName = new XmlQualifiedName(xmlReader.LocalName, xmlReader.NamespaceURI);
			}
			message.Stream.Position = position;
			return xmlQualifiedName;
		}

		// Token: 0x040002D2 RID: 722
		private SoapServerProtocol protocol;

		// Token: 0x040002D3 RID: 723
		private string requestNamespace;
	}
}
