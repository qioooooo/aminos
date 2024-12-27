using System;
using System.Net;
using System.Threading;
using System.Web.Services.Configuration;
using System.Web.Services.Description;
using System.Web.Services.Diagnostics;
using System.Xml;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200005C RID: 92
	internal class Soap11ServerProtocolHelper : SoapServerProtocolHelper
	{
		// Token: 0x0600021F RID: 543 RVA: 0x0000A435 File Offset: 0x00009435
		internal Soap11ServerProtocolHelper(SoapServerProtocol protocol)
			: base(protocol)
		{
		}

		// Token: 0x06000220 RID: 544 RVA: 0x0000A43E File Offset: 0x0000943E
		internal Soap11ServerProtocolHelper(SoapServerProtocol protocol, string requestNamespace)
			: base(protocol, requestNamespace)
		{
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000221 RID: 545 RVA: 0x0000A448 File Offset: 0x00009448
		internal override SoapProtocolVersion Version
		{
			get
			{
				return SoapProtocolVersion.Soap11;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000222 RID: 546 RVA: 0x0000A44B File Offset: 0x0000944B
		internal override WebServiceProtocols Protocol
		{
			get
			{
				return WebServiceProtocols.HttpSoap;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000223 RID: 547 RVA: 0x0000A44E File Offset: 0x0000944E
		internal override string EnvelopeNs
		{
			get
			{
				return "http://schemas.xmlsoap.org/soap/envelope/";
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000224 RID: 548 RVA: 0x0000A455 File Offset: 0x00009455
		internal override string EncodingNs
		{
			get
			{
				return "http://schemas.xmlsoap.org/soap/encoding/";
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000225 RID: 549 RVA: 0x0000A45C File Offset: 0x0000945C
		internal override string HttpContentType
		{
			get
			{
				return "text/xml";
			}
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000A464 File Offset: 0x00009464
		internal override SoapServerMethod RouteRequest()
		{
			string text = base.ServerProtocol.Request.Headers["SOAPAction"];
			if (text == null)
			{
				throw new SoapException(Res.GetString("UnableToHandleRequestActionRequired0"), new XmlQualifiedName("Client", "http://schemas.xmlsoap.org/soap/envelope/"));
			}
			object obj;
			if (base.ServerType.routingOnSoapAction)
			{
				if (text.StartsWith("\"", StringComparison.Ordinal) && text.EndsWith("\"", StringComparison.Ordinal))
				{
					text = text.Substring(1, text.Length - 2);
				}
				obj = HttpUtility.UrlDecode(text);
			}
			else
			{
				try
				{
					obj = base.GetRequestElement();
				}
				catch (SoapException)
				{
					throw;
				}
				catch (Exception ex)
				{
					if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
					{
						throw;
					}
					throw new SoapException(Res.GetString("TheRootElementForTheRequestCouldNotBeDetermined0"), new XmlQualifiedName("Server", "http://schemas.xmlsoap.org/soap/envelope/"), ex);
				}
				catch
				{
					throw new SoapException(Res.GetString("TheRootElementForTheRequestCouldNotBeDetermined0"), new XmlQualifiedName("Server", "http://schemas.xmlsoap.org/soap/envelope/"), null);
				}
			}
			TraceMethod traceMethod = (Tracing.On ? new TraceMethod(this, "RouteRequest", new object[0]) : null);
			if (Tracing.On)
			{
				Tracing.Enter("RouteRequest", traceMethod, new TraceMethod(base.ServerType, "GetMethod", new object[] { obj }), Tracing.Details(base.ServerProtocol.Request));
			}
			SoapServerMethod method = base.ServerType.GetMethod(obj);
			if (Tracing.On)
			{
				Tracing.Exit("RouteRequest", traceMethod);
			}
			if (method != null)
			{
				return method;
			}
			if (base.ServerType.routingOnSoapAction)
			{
				throw new SoapException(Res.GetString("WebHttpHeader", new object[]
				{
					"SOAPAction",
					(string)obj
				}), new XmlQualifiedName("Client", "http://schemas.xmlsoap.org/soap/envelope/"));
			}
			throw new SoapException(Res.GetString("TheRequestElementXmlnsWasNotRecognized2", new object[]
			{
				((XmlQualifiedName)obj).Name,
				((XmlQualifiedName)obj).Namespace
			}), new XmlQualifiedName("Client", "http://schemas.xmlsoap.org/soap/envelope/"));
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000A698 File Offset: 0x00009698
		internal override void WriteFault(XmlWriter writer, SoapException soapException, HttpStatusCode statusCode)
		{
			if (statusCode != HttpStatusCode.InternalServerError)
			{
				return;
			}
			if (soapException == null)
			{
				return;
			}
			SoapServerMessage message = base.ServerProtocol.Message;
			writer.WriteStartDocument();
			writer.WriteStartElement("soap", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
			writer.WriteAttributeString("xmlns", "soap", null, "http://schemas.xmlsoap.org/soap/envelope/");
			writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
			writer.WriteAttributeString("xmlns", "xsd", null, "http://www.w3.org/2001/XMLSchema");
			if (base.ServerProtocol.ServerMethod != null)
			{
				SoapHeaderHandling.WriteHeaders(writer, base.ServerProtocol.ServerMethod.outHeaderSerializer, message.Headers, base.ServerProtocol.ServerMethod.outHeaderMappings, SoapHeaderDirection.Fault, base.ServerProtocol.ServerMethod.use == SoapBindingUse.Encoded, base.ServerType.serviceNamespace, base.ServerType.serviceDefaultIsEncoded, "http://schemas.xmlsoap.org/soap/envelope/");
			}
			else
			{
				SoapHeaderHandling.WriteUnknownHeaders(writer, message.Headers, "http://schemas.xmlsoap.org/soap/envelope/");
			}
			writer.WriteStartElement("Body", "http://schemas.xmlsoap.org/soap/envelope/");
			writer.WriteStartElement("Fault", "http://schemas.xmlsoap.org/soap/envelope/");
			writer.WriteStartElement("faultcode", "");
			XmlQualifiedName xmlQualifiedName = Soap11ServerProtocolHelper.TranslateFaultCode(soapException.Code);
			if (xmlQualifiedName.Namespace != null && xmlQualifiedName.Namespace.Length > 0 && writer.LookupPrefix(xmlQualifiedName.Namespace) == null)
			{
				writer.WriteAttributeString("xmlns", "q0", null, xmlQualifiedName.Namespace);
			}
			writer.WriteQualifiedName(xmlQualifiedName.Name, xmlQualifiedName.Namespace);
			writer.WriteEndElement();
			writer.WriteStartElement("faultstring", "");
			if (soapException.Lang != null && soapException.Lang.Length != 0)
			{
				writer.WriteAttributeString("xml", "lang", "http://www.w3.org/XML/1998/namespace", soapException.Lang);
			}
			writer.WriteString(base.ServerProtocol.GenerateFaultString(soapException));
			writer.WriteEndElement();
			string actor = soapException.Actor;
			if (actor.Length > 0)
			{
				writer.WriteElementString("faultactor", "", actor);
			}
			if (!(soapException is SoapHeaderException))
			{
				if (soapException.Detail == null)
				{
					writer.WriteStartElement("detail", "");
					writer.WriteEndElement();
				}
				else
				{
					soapException.Detail.WriteTo(writer);
				}
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
			writer.WriteEndElement();
			writer.Flush();
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000A8EC File Offset: 0x000098EC
		private static XmlQualifiedName TranslateFaultCode(XmlQualifiedName code)
		{
			if (code.Namespace == "http://schemas.xmlsoap.org/soap/envelope/")
			{
				return code;
			}
			if (code.Namespace == "http://www.w3.org/2003/05/soap-envelope")
			{
				if (code.Name == "Receiver")
				{
					return SoapException.ServerFaultCode;
				}
				if (code.Name == "Sender")
				{
					return SoapException.ClientFaultCode;
				}
				if (code.Name == "MustUnderstand")
				{
					return SoapException.MustUnderstandFaultCode;
				}
				if (code.Name == "VersionMismatch")
				{
					return SoapException.VersionMismatchFaultCode;
				}
			}
			return code;
		}
	}
}
