using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Web.Services.Configuration;
using System.Web.Services.Description;
using System.Web.Services.Diagnostics;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000082 RID: 130
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public class SoapServerProtocol : ServerProtocol
	{
		// Token: 0x06000365 RID: 869 RVA: 0x00010112 File Offset: 0x0000F112
		protected internal SoapServerProtocol()
		{
			this.protocolsSupported = WebServicesSection.Current.EnabledProtocols;
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0001012A File Offset: 0x0000F12A
		internal override string GetUrlKeyComponent()
		{
			return string.Empty;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00010131 File Offset: 0x0000F131
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		protected virtual XmlWriter GetWriterForMessage(SoapServerMessage message, int bufferSize)
		{
			if (bufferSize < 512)
			{
				bufferSize = 512;
			}
			return new XmlTextWriter(new StreamWriter(message.Stream, new UTF8Encoding(false), bufferSize));
		}

		// Token: 0x06000368 RID: 872 RVA: 0x0001015C File Offset: 0x0000F15C
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		protected virtual XmlReader GetReaderForMessage(SoapServerMessage message, int bufferSize)
		{
			Encoding encoding = RequestResponseUtils.GetEncoding2(message.ContentType);
			if (bufferSize < 512)
			{
				bufferSize = 512;
			}
			int readTimeout = WebServicesSection.Current.SoapEnvelopeProcessing.ReadTimeout;
			long num = ((readTimeout < 0) ? 0L : ((long)readTimeout * 10000000L));
			long ticks = DateTime.UtcNow.Ticks;
			long num2 = ((long.MaxValue - num <= ticks) ? long.MaxValue : (ticks + num));
			XmlTextReader xmlTextReader;
			if (encoding != null)
			{
				if (num2 == 9223372036854775807L)
				{
					xmlTextReader = new XmlTextReader(new StreamReader(message.Stream, encoding, true, bufferSize));
				}
				else
				{
					xmlTextReader = new SoapServerProtocol.SoapEnvelopeReader(new StreamReader(message.Stream, encoding, true, bufferSize), num2);
				}
			}
			else if (num2 == 9223372036854775807L)
			{
				xmlTextReader = new XmlTextReader(message.Stream);
			}
			else
			{
				xmlTextReader = new SoapServerProtocol.SoapEnvelopeReader(message.Stream, num2);
			}
			xmlTextReader.ProhibitDtd = true;
			xmlTextReader.Normalization = true;
			xmlTextReader.XmlResolver = null;
			return xmlTextReader;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00010258 File Offset: 0x0000F258
		internal override bool Initialize()
		{
			this.GuessVersion();
			this.message = new SoapServerMessage(this);
			this.onewayInitException = null;
			this.serverType = (SoapServerType)base.GetFromCache(typeof(SoapServerProtocol), base.Type);
			if (this.serverType == null)
			{
				lock (ServerProtocol.InternalSyncObject)
				{
					this.serverType = (SoapServerType)base.GetFromCache(typeof(SoapServerProtocol), base.Type);
					if (this.serverType == null)
					{
						this.serverType = new SoapServerType(base.Type, this.protocolsSupported);
						base.AddToCache(typeof(SoapServerProtocol), base.Type, this.serverType);
					}
				}
			}
			Exception ex = null;
			try
			{
				this.message.highPriConfigExtensions = SoapMessage.InitializeExtensions(this.serverType.HighPriExtensions, this.serverType.HighPriExtensionInitializers);
				this.message.highPriConfigExtensions = this.ModifyInitializedExtensions(PriorityGroup.High, this.message.highPriConfigExtensions);
				this.message.SetStream(base.Request.InputStream);
				this.message.InitExtensionStreamChain(this.message.highPriConfigExtensions);
				this.message.SetStage(SoapMessageStage.BeforeDeserialize);
				this.message.ContentType = base.Request.ContentType;
				this.message.ContentEncoding = base.Request.Headers["Content-Encoding"];
				this.message.RunExtensions(this.message.highPriConfigExtensions, false);
				ex = this.message.Exception;
			}
			catch (Exception ex2)
			{
				if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
				{
					throw;
				}
				if (Tracing.On)
				{
					Tracing.ExceptionCatch(TraceEventType.Warning, this, "Initialize", ex2);
				}
				ex = ex2;
			}
			catch
			{
				ex = new Exception(Res.GetString("NonClsCompliantException"));
			}
			this.message.allExtensions = this.message.highPriConfigExtensions;
			this.GuessVersion();
			try
			{
				this.serverMethod = this.RouteRequest(this.message);
				if (this.serverMethod == null)
				{
					throw new SoapException(Res.GetString("UnableToHandleRequest0"), new XmlQualifiedName("Server", "http://schemas.xmlsoap.org/soap/envelope/"));
				}
			}
			catch (Exception ex3)
			{
				if (ex3 is ThreadAbortException || ex3 is StackOverflowException || ex3 is OutOfMemoryException)
				{
					throw;
				}
				if (this.helper.RequestNamespace != null)
				{
					this.SetHelper(SoapServerProtocolHelper.GetHelper(this, this.helper.RequestNamespace));
				}
				this.CheckHelperVersion();
				throw;
			}
			catch
			{
				if (this.helper.RequestNamespace != null)
				{
					this.SetHelper(SoapServerProtocolHelper.GetHelper(this, this.helper.RequestNamespace));
				}
				this.CheckHelperVersion();
				throw;
			}
			this.isOneWay = this.serverMethod.oneWay;
			if (ex == null)
			{
				try
				{
					SoapReflectedExtension[] array = (SoapReflectedExtension[])SoapServerProtocol.CombineExtensionsHelper(this.serverMethod.extensions, this.serverType.LowPriExtensions, typeof(SoapReflectedExtension));
					object[] array2 = (object[])SoapServerProtocol.CombineExtensionsHelper(this.serverMethod.extensionInitializers, this.serverType.LowPriExtensionInitializers, typeof(object));
					this.message.otherExtensions = SoapMessage.InitializeExtensions(array, array2);
					this.message.otherExtensions = this.ModifyInitializedExtensions(PriorityGroup.Low, this.message.otherExtensions);
					this.message.allExtensions = (SoapExtension[])SoapServerProtocol.CombineExtensionsHelper(this.message.highPriConfigExtensions, this.message.otherExtensions, typeof(SoapExtension));
				}
				catch (Exception ex4)
				{
					if (ex4 is ThreadAbortException || ex4 is StackOverflowException || ex4 is OutOfMemoryException)
					{
						throw;
					}
					if (Tracing.On)
					{
						Tracing.ExceptionCatch(TraceEventType.Warning, this, "Initialize", ex4);
					}
					ex = ex4;
				}
				catch
				{
					ex = new Exception(Res.GetString("NonClsCompliantException"));
				}
			}
			if (ex != null)
			{
				if (this.isOneWay)
				{
					this.onewayInitException = ex;
				}
				else
				{
					if (ex is SoapException)
					{
						throw ex;
					}
					throw SoapException.Create(this.Version, Res.GetString("WebConfigExtensionError"), new XmlQualifiedName("Server", "http://schemas.xmlsoap.org/soap/envelope/"), ex);
				}
			}
			return true;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x000106C4 File Offset: 0x0000F6C4
		protected virtual SoapExtension[] ModifyInitializedExtensions(PriorityGroup group, SoapExtension[] extensions)
		{
			return extensions;
		}

		// Token: 0x0600036B RID: 875 RVA: 0x000106C7 File Offset: 0x0000F6C7
		protected virtual SoapServerMethod RouteRequest(SoapServerMessage message)
		{
			return this.helper.RouteRequest();
		}

		// Token: 0x0600036C RID: 876 RVA: 0x000106D4 File Offset: 0x0000F6D4
		private void GuessVersion()
		{
			if (this.IsSupported(WebServiceProtocols.AnyHttpSoap))
			{
				if (base.Request.Headers["SOAPAction"] == null || ContentType.MatchesBase(base.Request.ContentType, "application/soap+xml"))
				{
					this.SetHelper(new Soap12ServerProtocolHelper(this));
					return;
				}
				this.SetHelper(new Soap11ServerProtocolHelper(this));
				return;
			}
			else
			{
				if (this.IsSupported(WebServiceProtocols.HttpSoap))
				{
					this.SetHelper(new Soap11ServerProtocolHelper(this));
					return;
				}
				if (this.IsSupported(WebServiceProtocols.HttpSoap12))
				{
					this.SetHelper(new Soap12ServerProtocolHelper(this));
				}
				return;
			}
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0001075F File Offset: 0x0000F75F
		internal bool IsSupported(WebServiceProtocols protocol)
		{
			return (this.protocolsSupported & protocol) == protocol;
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x0600036E RID: 878 RVA: 0x0001076C File Offset: 0x0000F76C
		internal override ServerType ServerType
		{
			get
			{
				return this.serverType;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x0600036F RID: 879 RVA: 0x00010774 File Offset: 0x0000F774
		internal override LogicalMethodInfo MethodInfo
		{
			get
			{
				return this.serverMethod.methodInfo;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000370 RID: 880 RVA: 0x00010781 File Offset: 0x0000F781
		internal SoapServerMethod ServerMethod
		{
			get
			{
				return this.serverMethod;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000371 RID: 881 RVA: 0x00010789 File Offset: 0x0000F789
		internal SoapServerMessage Message
		{
			get
			{
				return this.message;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000372 RID: 882 RVA: 0x00010791 File Offset: 0x0000F791
		internal override bool IsOneWay
		{
			get
			{
				return this.isOneWay;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000373 RID: 883 RVA: 0x00010799 File Offset: 0x0000F799
		internal override Exception OnewayInitException
		{
			get
			{
				return this.onewayInitException;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000374 RID: 884 RVA: 0x000107A1 File Offset: 0x0000F7A1
		internal SoapProtocolVersion Version
		{
			get
			{
				return this.version;
			}
		}

		// Token: 0x06000375 RID: 885 RVA: 0x000107AC File Offset: 0x0000F7AC
		internal override void CreateServerInstance()
		{
			base.CreateServerInstance();
			this.message.SetStage(SoapMessageStage.AfterDeserialize);
			this.message.RunExtensions(this.message.allExtensions, true);
			SoapHeaderHandling.SetHeaderMembers(this.message.Headers, this.Target, this.serverMethod.inHeaderMappings, SoapHeaderDirection.In, false);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x00010805 File Offset: 0x0000F805
		private void SetHelper(SoapServerProtocolHelper helper)
		{
			this.helper = helper;
			this.version = helper.Version;
			base.Context.Items[WebService.SoapVersionContextSlot] = helper.Version;
		}

		// Token: 0x06000377 RID: 887 RVA: 0x0001083C File Offset: 0x0000F83C
		private static Array CombineExtensionsHelper(Array array1, Array array2, Type elementType)
		{
			if (array1 == null)
			{
				return array2;
			}
			if (array2 == null)
			{
				return array1;
			}
			int num = array1.Length + array2.Length;
			if (num == 0)
			{
				return null;
			}
			Array array3;
			if (elementType == typeof(SoapReflectedExtension))
			{
				array3 = new SoapReflectedExtension[num];
			}
			else if (elementType == typeof(SoapExtension))
			{
				array3 = new SoapExtension[num];
			}
			else
			{
				if (elementType != typeof(object))
				{
					throw new ArgumentException(Res.GetString("ElementTypeMustBeObjectOrSoapExtensionOrSoapReflectedException"), "elementType");
				}
				array3 = new object[num];
			}
			Array.Copy(array1, 0, array3, 0, array1.Length);
			Array.Copy(array2, 0, array3, array1.Length, array2.Length);
			return array3;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x000108E4 File Offset: 0x0000F8E4
		private void CheckHelperVersion()
		{
			if (this.helper.RequestNamespace == null)
			{
				return;
			}
			if (this.helper.RequestNamespace != this.helper.EnvelopeNs)
			{
				string requestNamespace = this.helper.RequestNamespace;
				if (this.IsSupported(WebServiceProtocols.HttpSoap))
				{
					this.SetHelper(new Soap11ServerProtocolHelper(this));
				}
				else
				{
					this.SetHelper(new Soap12ServerProtocolHelper(this));
				}
				throw new SoapException(Res.GetString("WebInvalidEnvelopeNamespace", new object[]
				{
					requestNamespace,
					this.helper.EnvelopeNs
				}), SoapException.VersionMismatchFaultCode);
			}
			if (!this.IsSupported(this.helper.Protocol))
			{
				string requestNamespace2 = this.helper.RequestNamespace;
				string text = (this.IsSupported(WebServiceProtocols.HttpSoap) ? "http://schemas.xmlsoap.org/soap/envelope/" : "http://www.w3.org/2003/05/soap-envelope");
				this.SetHelper(new Soap11ServerProtocolHelper(this));
				throw new SoapException(Res.GetString("WebInvalidEnvelopeNamespace", new object[] { requestNamespace2, text }), SoapException.VersionMismatchFaultCode);
			}
		}

		// Token: 0x06000379 RID: 889 RVA: 0x000109E4 File Offset: 0x0000F9E4
		internal override object[] ReadParameters()
		{
			this.message.InitExtensionStreamChain(this.message.otherExtensions);
			this.message.RunExtensions(this.message.otherExtensions, true);
			if (!ContentType.IsSoap(this.message.ContentType))
			{
				throw new SoapException(Res.GetString("WebRequestContent", new object[]
				{
					this.message.ContentType,
					this.helper.HttpContentType
				}), new XmlQualifiedName("Client", "http://schemas.xmlsoap.org/soap/envelope/"), new SoapFaultSubCode(Soap12FaultCodes.UnsupportedMediaTypeFaultCode));
			}
			XmlReader xmlReader = null;
			try
			{
				xmlReader = this.GetXmlReader();
				xmlReader.MoveToContent();
				this.SetHelper(SoapServerProtocolHelper.GetHelper(this, xmlReader.NamespaceURI));
			}
			catch (XmlException ex)
			{
				throw new SoapException(Res.GetString("WebRequestUnableToRead"), new XmlQualifiedName("Client", "http://schemas.xmlsoap.org/soap/envelope/"), ex);
			}
			this.CheckHelperVersion();
			if (this.version == SoapProtocolVersion.Soap11 && !ContentType.MatchesBase(this.message.ContentType, this.helper.HttpContentType))
			{
				throw new SoapException(Res.GetString("WebRequestContent", new object[]
				{
					this.message.ContentType,
					this.helper.HttpContentType
				}), new XmlQualifiedName("Client", "http://schemas.xmlsoap.org/soap/envelope/"), new SoapFaultSubCode(Soap12FaultCodes.UnsupportedMediaTypeFaultCode));
			}
			if (this.message.Exception != null)
			{
				throw this.message.Exception;
			}
			object[] array2;
			try
			{
				if (!xmlReader.IsStartElement("Envelope", this.helper.EnvelopeNs))
				{
					throw new InvalidOperationException(Res.GetString("WebMissingEnvelopeElement"));
				}
				if (xmlReader.IsEmptyElement)
				{
					throw new InvalidOperationException(Res.GetString("WebMissingBodyElement"));
				}
				int depth = xmlReader.Depth;
				xmlReader.ReadStartElement("Envelope", this.helper.EnvelopeNs);
				xmlReader.MoveToContent();
				bool flag = (this.serverMethod.wsiClaims & WsiProfiles.BasicProfile1_1) != WsiProfiles.None && this.version != SoapProtocolVersion.Soap12;
				string text = new SoapHeaderHandling().ReadHeaders(xmlReader, this.serverMethod.inHeaderSerializer, this.message.Headers, this.serverMethod.inHeaderMappings, SoapHeaderDirection.In, this.helper.EnvelopeNs, (this.serverMethod.use == SoapBindingUse.Encoded) ? this.helper.EncodingNs : null, flag);
				if (text != null)
				{
					throw new SoapHeaderException(Res.GetString("WebMissingHeader", new object[] { text }), new XmlQualifiedName("MustUnderstand", "http://schemas.xmlsoap.org/soap/envelope/"));
				}
				if (!xmlReader.IsStartElement("Body", this.helper.EnvelopeNs))
				{
					throw new InvalidOperationException(Res.GetString("WebMissingBodyElement"));
				}
				xmlReader.ReadStartElement("Body", this.helper.EnvelopeNs);
				xmlReader.MoveToContent();
				bool flag2 = this.serverMethod.use == SoapBindingUse.Encoded;
				TraceMethod traceMethod = (Tracing.On ? new TraceMethod(this, "ReadParameters", new object[0]) : null);
				if (Tracing.On)
				{
					Tracing.Enter(Tracing.TraceId("TraceReadRequest"), traceMethod, new TraceMethod(this.serverMethod.parameterSerializer, "Deserialize", new object[]
					{
						xmlReader,
						(this.serverMethod.use == SoapBindingUse.Encoded) ? this.helper.EncodingNs : null
					}));
				}
				bool flag3 = !flag2 && (WebServicesSection.Current.SoapEnvelopeProcessing.IsStrict || Tracing.On);
				object[] array;
				if (flag3)
				{
					XmlDeserializationEvents xmlDeserializationEvents = (Tracing.On ? Tracing.GetDeserializationEvents() : RuntimeUtils.GetDeserializationEvents());
					array = (object[])this.serverMethod.parameterSerializer.Deserialize(xmlReader, null, xmlDeserializationEvents);
				}
				else
				{
					array = (object[])this.serverMethod.parameterSerializer.Deserialize(xmlReader, flag2 ? this.helper.EncodingNs : null);
				}
				if (Tracing.On)
				{
					Tracing.Exit(Tracing.TraceId("TraceReadRequest"), traceMethod);
				}
				while (depth < xmlReader.Depth && xmlReader.Read())
				{
				}
				if (xmlReader.NodeType == XmlNodeType.EndElement)
				{
					xmlReader.Read();
				}
				this.message.SetParameterValues(array);
				array2 = array;
			}
			catch (SoapException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
				{
					throw;
				}
				throw new SoapException(Res.GetString("WebRequestUnableToRead"), new XmlQualifiedName("Client", "http://schemas.xmlsoap.org/soap/envelope/"), ex2);
			}
			catch
			{
				throw new SoapException(Res.GetString("WebRequestUnableToRead"), new XmlQualifiedName("Client", "http://schemas.xmlsoap.org/soap/envelope/"), new Exception(Res.GetString("NonClsCompliantException")));
			}
			return array2;
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00010EE0 File Offset: 0x0000FEE0
		internal override void WriteReturns(object[] returnValues, Stream outputStream)
		{
			if (this.serverMethod.oneWay)
			{
				return;
			}
			bool flag = this.serverMethod.use == SoapBindingUse.Encoded;
			SoapHeaderHandling.EnsureHeadersUnderstood(this.message.Headers);
			this.message.Headers.Clear();
			SoapHeaderHandling.GetHeaderMembers(this.message.Headers, this.Target, this.serverMethod.outHeaderMappings, SoapHeaderDirection.Out, false);
			if (this.message.allExtensions != null)
			{
				this.message.SetExtensionStream(new SoapExtensionStream());
			}
			this.message.InitExtensionStreamChain(this.message.allExtensions);
			this.message.SetStage(SoapMessageStage.BeforeSerialize);
			this.message.ContentType = ContentType.Compose(this.helper.HttpContentType, Encoding.UTF8);
			this.message.SetParameterValues(returnValues);
			this.message.RunExtensions(this.message.allExtensions, true);
			this.message.SetStream(outputStream);
			base.Response.ContentType = this.message.ContentType;
			if (this.message.ContentEncoding != null && this.message.ContentEncoding.Length > 0)
			{
				base.Response.AppendHeader("Content-Encoding", this.message.ContentEncoding);
			}
			XmlWriter writerForMessage = this.GetWriterForMessage(this.message, 1024);
			if (writerForMessage == null)
			{
				throw new InvalidOperationException(Res.GetString("WebNullWriterForMessage"));
			}
			writerForMessage.WriteStartDocument();
			writerForMessage.WriteStartElement("soap", "Envelope", this.helper.EnvelopeNs);
			writerForMessage.WriteAttributeString("xmlns", "soap", null, this.helper.EnvelopeNs);
			if (flag)
			{
				writerForMessage.WriteAttributeString("xmlns", "soapenc", null, this.helper.EncodingNs);
				writerForMessage.WriteAttributeString("xmlns", "tns", null, this.serverType.serviceNamespace);
				writerForMessage.WriteAttributeString("xmlns", "types", null, SoapReflector.GetEncodedNamespace(this.serverType.serviceNamespace, this.serverType.serviceDefaultIsEncoded));
			}
			if (this.serverMethod.rpc && this.version == SoapProtocolVersion.Soap12)
			{
				writerForMessage.WriteAttributeString("xmlns", "rpc", null, "http://www.w3.org/2003/05/soap-rpc");
			}
			writerForMessage.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
			writerForMessage.WriteAttributeString("xmlns", "xsd", null, "http://www.w3.org/2001/XMLSchema");
			SoapHeaderHandling.WriteHeaders(writerForMessage, this.serverMethod.outHeaderSerializer, this.message.Headers, this.serverMethod.outHeaderMappings, SoapHeaderDirection.Out, flag, this.serverType.serviceNamespace, this.serverType.serviceDefaultIsEncoded, this.helper.EnvelopeNs);
			writerForMessage.WriteStartElement("Body", this.helper.EnvelopeNs);
			if (flag && this.version != SoapProtocolVersion.Soap12)
			{
				writerForMessage.WriteAttributeString("soap", "encodingStyle", null, this.helper.EncodingNs);
			}
			TraceMethod traceMethod = (Tracing.On ? new TraceMethod(this, "WriteReturns", new object[0]) : null);
			if (Tracing.On)
			{
				Tracing.Enter(Tracing.TraceId("TraceWriteResponse"), traceMethod, new TraceMethod(this.serverMethod.returnSerializer, "Serialize", new object[]
				{
					writerForMessage,
					returnValues,
					null,
					flag ? this.helper.EncodingNs : null
				}));
			}
			this.serverMethod.returnSerializer.Serialize(writerForMessage, returnValues, null, flag ? this.helper.EncodingNs : null);
			if (Tracing.On)
			{
				Tracing.Exit(Tracing.TraceId("TraceWriteResponse"), traceMethod);
			}
			writerForMessage.WriteEndElement();
			writerForMessage.WriteEndElement();
			writerForMessage.Flush();
			this.message.SetStage(SoapMessageStage.AfterSerialize);
			this.message.RunExtensions(this.message.allExtensions, true);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x000112B4 File Offset: 0x000102B4
		internal override bool WriteException(Exception e, Stream outputStream)
		{
			if (this.message == null)
			{
				return false;
			}
			this.message.Headers.Clear();
			if (this.serverMethod != null && this.Target != null)
			{
				SoapHeaderHandling.GetHeaderMembers(this.message.Headers, this.Target, this.serverMethod.outHeaderMappings, SoapHeaderDirection.Fault, false);
			}
			SoapException ex;
			if (e is SoapException)
			{
				ex = (SoapException)e;
			}
			else if (this.serverMethod != null && this.serverMethod.rpc && this.helper.Version == SoapProtocolVersion.Soap12 && e is ArgumentException)
			{
				ex = SoapException.Create(this.Version, Res.GetString("WebRequestUnableToProcess"), new XmlQualifiedName("Client", "http://schemas.xmlsoap.org/soap/envelope/"), null, null, null, new SoapFaultSubCode(Soap12FaultCodes.RpcBadArgumentsFaultCode), e);
			}
			else
			{
				ex = SoapException.Create(this.Version, Res.GetString("WebRequestUnableToProcess"), new XmlQualifiedName("Server", "http://schemas.xmlsoap.org/soap/envelope/"), e);
			}
			if (SoapException.IsVersionMismatchFaultCode(ex.Code) && this.IsSupported(WebServiceProtocols.HttpSoap12))
			{
				SoapUnknownHeader soapUnknownHeader = this.CreateUpgradeHeader();
				if (soapUnknownHeader != null)
				{
					this.Message.Headers.Add(soapUnknownHeader);
				}
			}
			base.Response.ClearHeaders();
			base.Response.Clear();
			HttpStatusCode httpStatusCode = this.helper.SetResponseErrorCode(base.Response, ex);
			bool flag = false;
			SoapExtensionStream soapExtensionStream = new SoapExtensionStream();
			if (this.message.allExtensions != null)
			{
				this.message.SetExtensionStream(soapExtensionStream);
			}
			try
			{
				this.message.InitExtensionStreamChain(this.message.allExtensions);
			}
			catch (Exception ex2)
			{
				if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
				{
					throw;
				}
				if (Tracing.On)
				{
					Tracing.ExceptionCatch(TraceEventType.Warning, this, "WriteException", ex2);
				}
				flag = true;
			}
			catch
			{
				flag = true;
			}
			this.message.SetStage(SoapMessageStage.BeforeSerialize);
			this.message.ContentType = ContentType.Compose(this.helper.HttpContentType, Encoding.UTF8);
			this.message.Exception = ex;
			if (!flag)
			{
				try
				{
					this.message.RunExtensions(this.message.allExtensions, false);
				}
				catch (Exception ex3)
				{
					if (ex3 is ThreadAbortException || ex3 is StackOverflowException || ex3 is OutOfMemoryException)
					{
						throw;
					}
					if (Tracing.On)
					{
						Tracing.ExceptionCatch(TraceEventType.Warning, this, "WriteException", ex3);
					}
					flag = true;
				}
				catch
				{
					flag = true;
				}
			}
			this.message.SetStream(outputStream);
			base.Response.ContentType = this.message.ContentType;
			if (this.message.ContentEncoding != null && this.message.ContentEncoding.Length > 0)
			{
				base.Response.AppendHeader("Content-Encoding", this.message.ContentEncoding);
			}
			XmlWriter writerForMessage = this.GetWriterForMessage(this.message, 512);
			if (writerForMessage == null)
			{
				throw new InvalidOperationException(Res.GetString("WebNullWriterForMessage"));
			}
			this.helper.WriteFault(writerForMessage, this.message.Exception, httpStatusCode);
			if (!flag)
			{
				SoapException ex4 = null;
				try
				{
					this.message.SetStage(SoapMessageStage.AfterSerialize);
					this.message.RunExtensions(this.message.allExtensions, false);
				}
				catch (Exception ex5)
				{
					if (ex5 is ThreadAbortException || ex5 is StackOverflowException || ex5 is OutOfMemoryException)
					{
						throw;
					}
					if (Tracing.On)
					{
						Tracing.ExceptionCatch(TraceEventType.Warning, this, "WriteException", ex5);
					}
					if (!soapExtensionStream.HasWritten)
					{
						ex4 = SoapException.Create(this.Version, Res.GetString("WebExtensionError"), new XmlQualifiedName("Server", "http://schemas.xmlsoap.org/soap/envelope/"), ex5);
					}
				}
				catch
				{
					if (!soapExtensionStream.HasWritten)
					{
						ex4 = SoapException.Create(this.Version, Res.GetString("WebExtensionError"), new XmlQualifiedName("Server", "http://schemas.xmlsoap.org/soap/envelope/"), null);
					}
					return false;
				}
				if (ex4 != null)
				{
					base.Response.ContentType = ContentType.Compose("text/plain", Encoding.UTF8);
					StreamWriter streamWriter = new StreamWriter(outputStream, new UTF8Encoding(false));
					streamWriter.WriteLine(base.GenerateFaultString(this.message.Exception));
					streamWriter.Flush();
					return true;
				}
				return true;
			}
			return true;
		}

		// Token: 0x0600037C RID: 892 RVA: 0x00011718 File Offset: 0x00010718
		private bool WriteException_TryWriteFault(SoapServerMessage message, Stream outputStream, HttpStatusCode statusCode, bool disableExtensions)
		{
			return true;
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0001171C File Offset: 0x0001071C
		internal SoapUnknownHeader CreateUpgradeHeader()
		{
			XmlDocument xmlDocument = new XmlDocument();
			XmlElement xmlElement = xmlDocument.CreateElement("soap12", "Upgrade", "http://www.w3.org/2003/05/soap-envelope");
			if (this.IsSupported(WebServiceProtocols.HttpSoap))
			{
				xmlElement.AppendChild(SoapServerProtocol.CreateUpgradeEnvelope(xmlDocument, "soap", "http://schemas.xmlsoap.org/soap/envelope/"));
			}
			if (this.IsSupported(WebServiceProtocols.HttpSoap12))
			{
				xmlElement.AppendChild(SoapServerProtocol.CreateUpgradeEnvelope(xmlDocument, "soap12", "http://www.w3.org/2003/05/soap-envelope"));
			}
			return new SoapUnknownHeader
			{
				Element = xmlElement
			};
		}

		// Token: 0x0600037E RID: 894 RVA: 0x00011794 File Offset: 0x00010794
		private static XmlElement CreateUpgradeEnvelope(XmlDocument doc, string prefix, string envelopeNs)
		{
			XmlElement xmlElement = doc.CreateElement("soap12", "SupportedEnvelope", "http://www.w3.org/2003/05/soap-envelope");
			XmlAttribute xmlAttribute = doc.CreateAttribute("xmlns", prefix, "http://www.w3.org/2000/xmlns/");
			xmlAttribute.Value = envelopeNs;
			XmlAttribute xmlAttribute2 = doc.CreateAttribute("qname");
			xmlAttribute2.Value = prefix + ":Envelope";
			xmlElement.Attributes.Append(xmlAttribute2);
			xmlElement.Attributes.Append(xmlAttribute);
			return xmlElement;
		}

		// Token: 0x0600037F RID: 895 RVA: 0x00011808 File Offset: 0x00010808
		internal XmlReader GetXmlReader()
		{
			Encoding encoding = RequestResponseUtils.GetEncoding2(this.Message.ContentType);
			bool flag = this.serverMethod != null && (this.serverMethod.wsiClaims & WsiProfiles.BasicProfile1_1) != WsiProfiles.None && this.Version != SoapProtocolVersion.Soap12;
			if (flag && encoding != null && !(encoding is UTF8Encoding) && !(encoding is UnicodeEncoding))
			{
				throw new InvalidOperationException(Res.GetString("WebWsiContentTypeEncoding"));
			}
			XmlReader readerForMessage = this.GetReaderForMessage(this.Message, RequestResponseUtils.GetBufferSize(base.Request.ContentLength));
			if (readerForMessage == null)
			{
				throw new InvalidOperationException(Res.GetString("WebNullReaderForMessage"));
			}
			return readerForMessage;
		}

		// Token: 0x0400037E RID: 894
		private SoapServerType serverType;

		// Token: 0x0400037F RID: 895
		private SoapServerMethod serverMethod;

		// Token: 0x04000380 RID: 896
		private SoapServerMessage message;

		// Token: 0x04000381 RID: 897
		private bool isOneWay;

		// Token: 0x04000382 RID: 898
		private Exception onewayInitException;

		// Token: 0x04000383 RID: 899
		private SoapProtocolVersion version;

		// Token: 0x04000384 RID: 900
		private WebServiceProtocols protocolsSupported;

		// Token: 0x04000385 RID: 901
		private SoapServerProtocolHelper helper;

		// Token: 0x02000083 RID: 131
		internal class SoapEnvelopeReader : XmlTextReader
		{
			// Token: 0x06000380 RID: 896 RVA: 0x000118A4 File Offset: 0x000108A4
			internal SoapEnvelopeReader(TextReader input, long timeout)
				: base(input)
			{
				this.readerTimedout = timeout;
			}

			// Token: 0x06000381 RID: 897 RVA: 0x000118B4 File Offset: 0x000108B4
			internal SoapEnvelopeReader(Stream input, long timeout)
				: base(input)
			{
				this.readerTimedout = timeout;
			}

			// Token: 0x06000382 RID: 898 RVA: 0x000118C4 File Offset: 0x000108C4
			public override bool Read()
			{
				this.CheckTimeout();
				return base.Read();
			}

			// Token: 0x06000383 RID: 899 RVA: 0x000118D2 File Offset: 0x000108D2
			public override bool MoveToNextAttribute()
			{
				this.CheckTimeout();
				return base.MoveToNextAttribute();
			}

			// Token: 0x06000384 RID: 900 RVA: 0x000118E0 File Offset: 0x000108E0
			public override XmlNodeType MoveToContent()
			{
				this.CheckTimeout();
				return base.MoveToContent();
			}

			// Token: 0x06000385 RID: 901 RVA: 0x000118F0 File Offset: 0x000108F0
			private void CheckTimeout()
			{
				if (DateTime.UtcNow.Ticks > this.readerTimedout)
				{
					throw new InvalidOperationException(Res.GetString("WebTimeout"));
				}
			}

			// Token: 0x04000386 RID: 902
			private long readerTimedout;
		}
	}
}
