using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Web.Services.Configuration;
using System.Web.Services.Description;
using System.Web.Services.Diagnostics;
using System.Web.Services.Discovery;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000062 RID: 98
	[ComVisible(true)]
	public class SoapHttpClientProtocol : HttpWebClientProtocol
	{
		// Token: 0x06000261 RID: 609 RVA: 0x0000B8C8 File Offset: 0x0000A8C8
		public SoapHttpClientProtocol()
		{
			Type type = base.GetType();
			this.clientType = (SoapClientType)WebClientProtocol.GetFromCache(type);
			if (this.clientType == null)
			{
				lock (WebClientProtocol.InternalSyncObject)
				{
					this.clientType = (SoapClientType)WebClientProtocol.GetFromCache(type);
					if (this.clientType == null)
					{
						this.clientType = new SoapClientType(type);
						WebClientProtocol.AddToCache(type, this.clientType);
					}
				}
			}
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000B954 File Offset: 0x0000A954
		public void Discover()
		{
			if (this.clientType.Binding == null)
			{
				throw new InvalidOperationException(Res.GetString("DiscoveryIsNotPossibleBecauseTypeIsMissing1", new object[] { base.GetType().FullName }));
			}
			DiscoveryClientProtocol discoveryClientProtocol = new DiscoveryClientProtocol(this);
			DiscoveryDocument discoveryDocument = discoveryClientProtocol.Discover(base.Url);
			foreach (object obj in discoveryDocument.References)
			{
				global::System.Web.Services.Discovery.SoapBinding soapBinding = obj as global::System.Web.Services.Discovery.SoapBinding;
				if (soapBinding != null && this.clientType.Binding.Name == soapBinding.Binding.Name && this.clientType.Binding.Namespace == soapBinding.Binding.Namespace)
				{
					base.Url = soapBinding.Address;
					return;
				}
			}
			throw new InvalidOperationException(Res.GetString("TheBindingNamedFromNamespaceWasNotFoundIn3", new object[]
			{
				this.clientType.Binding.Name,
				this.clientType.Binding.Namespace,
				base.Url
			}));
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000BA9C File Offset: 0x0000AA9C
		protected override WebRequest GetWebRequest(Uri uri)
		{
			return base.GetWebRequest(uri);
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000264 RID: 612 RVA: 0x0000BAB2 File Offset: 0x0000AAB2
		// (set) Token: 0x06000265 RID: 613 RVA: 0x0000BABA File Offset: 0x0000AABA
		[DefaultValue(SoapProtocolVersion.Default)]
		[ComVisible(false)]
		[WebServicesDescription("ClientProtocolSoapVersion")]
		public SoapProtocolVersion SoapVersion
		{
			get
			{
				return this.version;
			}
			set
			{
				this.version = value;
			}
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000BAC4 File Offset: 0x0000AAC4
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		protected virtual XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
		{
			if (bufferSize < 512)
			{
				bufferSize = 512;
			}
			return new XmlTextWriter(new StreamWriter(message.Stream, (base.RequestEncoding != null) ? base.RequestEncoding : new UTF8Encoding(false), bufferSize));
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000BB0C File Offset: 0x0000AB0C
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		protected virtual XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
		{
			Encoding encoding = ((message.SoapVersion == SoapProtocolVersion.Soap12) ? RequestResponseUtils.GetEncoding2(message.ContentType) : RequestResponseUtils.GetEncoding(message.ContentType));
			if (bufferSize < 512)
			{
				bufferSize = 512;
			}
			XmlTextReader xmlTextReader;
			if (encoding != null)
			{
				xmlTextReader = new XmlTextReader(new StreamReader(message.Stream, encoding, true, bufferSize));
			}
			else
			{
				xmlTextReader = new XmlTextReader(message.Stream);
			}
			xmlTextReader.ProhibitDtd = true;
			xmlTextReader.Normalization = true;
			xmlTextReader.XmlResolver = null;
			return xmlTextReader;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000BB88 File Offset: 0x0000AB88
		protected object[] Invoke(string methodName, object[] parameters)
		{
			WebRequest webRequest = null;
			object[] array;
			try
			{
				webRequest = this.GetWebRequest(base.Uri);
				base.NotifyClientCallOut(webRequest);
				base.PendingSyncRequest = webRequest;
				SoapClientMessage soapClientMessage = this.BeforeSerialize(webRequest, methodName, parameters);
				Stream requestStream = webRequest.GetRequestStream();
				try
				{
					soapClientMessage.SetStream(requestStream);
					this.Serialize(soapClientMessage);
				}
				finally
				{
					requestStream.Close();
				}
				WebResponse webResponse = this.GetWebResponse(webRequest);
				Stream stream = null;
				try
				{
					stream = webResponse.GetResponseStream();
					array = this.ReadResponse(soapClientMessage, webResponse, stream, false);
				}
				catch (XmlException ex)
				{
					throw new InvalidOperationException(Res.GetString("WebResponseBadXml"), ex);
				}
				finally
				{
					if (stream != null)
					{
						stream.Close();
					}
				}
			}
			finally
			{
				if (webRequest == base.PendingSyncRequest)
				{
					base.PendingSyncRequest = null;
				}
			}
			return array;
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000BC68 File Offset: 0x0000AC68
		protected IAsyncResult BeginInvoke(string methodName, object[] parameters, AsyncCallback callback, object asyncState)
		{
			SoapHttpClientProtocol.InvokeAsyncState invokeAsyncState = new SoapHttpClientProtocol.InvokeAsyncState(methodName, parameters);
			WebClientAsyncResult webClientAsyncResult = new WebClientAsyncResult(this, invokeAsyncState, null, callback, asyncState);
			return base.BeginSend(base.Uri, webClientAsyncResult, true);
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000BC98 File Offset: 0x0000AC98
		internal override void InitializeAsyncRequest(WebRequest request, object internalAsyncState)
		{
			SoapHttpClientProtocol.InvokeAsyncState invokeAsyncState = (SoapHttpClientProtocol.InvokeAsyncState)internalAsyncState;
			invokeAsyncState.Message = this.BeforeSerialize(request, invokeAsyncState.MethodName, invokeAsyncState.Parameters);
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000BCC8 File Offset: 0x0000ACC8
		internal override void AsyncBufferedSerialize(WebRequest request, Stream requestStream, object internalAsyncState)
		{
			SoapHttpClientProtocol.InvokeAsyncState invokeAsyncState = (SoapHttpClientProtocol.InvokeAsyncState)internalAsyncState;
			SoapClientMessage message = invokeAsyncState.Message;
			message.SetStream(requestStream);
			this.Serialize(invokeAsyncState.Message);
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000BCF8 File Offset: 0x0000ACF8
		protected object[] EndInvoke(IAsyncResult asyncResult)
		{
			object obj = null;
			Stream stream = null;
			object[] array;
			try
			{
				WebResponse webResponse = base.EndSend(asyncResult, ref obj, ref stream);
				SoapHttpClientProtocol.InvokeAsyncState invokeAsyncState = (SoapHttpClientProtocol.InvokeAsyncState)obj;
				array = this.ReadResponse(invokeAsyncState.Message, webResponse, stream, true);
			}
			catch (XmlException ex)
			{
				throw new InvalidOperationException(Res.GetString("WebResponseBadXml"), ex);
			}
			finally
			{
				if (stream != null)
				{
					stream.Close();
				}
			}
			return array;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000BD6C File Offset: 0x0000AD6C
		private void InvokeAsyncCallback(IAsyncResult result)
		{
			object[] array = null;
			Exception ex = null;
			WebClientAsyncResult webClientAsyncResult = (WebClientAsyncResult)result;
			if (webClientAsyncResult.Request != null)
			{
				object obj = null;
				Stream stream = null;
				try
				{
					WebResponse webResponse = base.EndSend(webClientAsyncResult, ref obj, ref stream);
					SoapHttpClientProtocol.InvokeAsyncState invokeAsyncState = (SoapHttpClientProtocol.InvokeAsyncState)obj;
					array = this.ReadResponse(invokeAsyncState.Message, webResponse, stream, true);
				}
				catch (XmlException ex2)
				{
					if (Tracing.On)
					{
						Tracing.ExceptionCatch(TraceEventType.Warning, this, "InvokeAsyncCallback", ex2);
					}
					ex = new InvalidOperationException(Res.GetString("WebResponseBadXml"), ex2);
				}
				catch (Exception ex3)
				{
					if (ex3 is ThreadAbortException || ex3 is StackOverflowException || ex3 is OutOfMemoryException)
					{
						throw;
					}
					if (Tracing.On)
					{
						Tracing.ExceptionCatch(TraceEventType.Warning, this, "InvokeAsyncCallback", ex3);
					}
					ex = ex3;
				}
				catch
				{
					ex = new Exception(Res.GetString("NonClsCompliantException"));
				}
				finally
				{
					if (stream != null)
					{
						stream.Close();
					}
				}
			}
			AsyncOperation asyncOperation = (AsyncOperation)result.AsyncState;
			UserToken userToken = (UserToken)asyncOperation.UserSuppliedState;
			base.OperationCompleted(userToken.UserState, array, ex, false);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000BEA8 File Offset: 0x0000AEA8
		protected void InvokeAsync(string methodName, object[] parameters, SendOrPostCallback callback)
		{
			this.InvokeAsync(methodName, parameters, callback, null);
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000BEB4 File Offset: 0x0000AEB4
		protected void InvokeAsync(string methodName, object[] parameters, SendOrPostCallback callback, object userState)
		{
			if (userState == null)
			{
				userState = base.NullToken;
			}
			SoapHttpClientProtocol.InvokeAsyncState invokeAsyncState = new SoapHttpClientProtocol.InvokeAsyncState(methodName, parameters);
			AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(new UserToken(callback, userState));
			WebClientAsyncResult webClientAsyncResult = new WebClientAsyncResult(this, invokeAsyncState, null, new AsyncCallback(this.InvokeAsyncCallback), asyncOperation);
			try
			{
				base.AsyncInvokes.Add(userState, webClientAsyncResult);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (Tracing.On)
				{
					Tracing.ExceptionCatch(TraceEventType.Warning, this, "InvokeAsync", ex);
				}
				Exception ex2 = new ArgumentException(Res.GetString("AsyncDuplicateUserState"), ex);
				object[] array = new object[1];
				InvokeCompletedEventArgs invokeCompletedEventArgs = new InvokeCompletedEventArgs(array, ex2, false, userState);
				asyncOperation.PostOperationCompleted(callback, invokeCompletedEventArgs);
				return;
			}
			catch
			{
				Exception ex3 = new ArgumentException(Res.GetString("AsyncDuplicateUserState"), new Exception(Res.GetString("NonClsCompliantException")));
				object[] array2 = new object[1];
				InvokeCompletedEventArgs invokeCompletedEventArgs2 = new InvokeCompletedEventArgs(array2, ex3, false, userState);
				asyncOperation.PostOperationCompleted(callback, invokeCompletedEventArgs2);
				return;
			}
			try
			{
				base.BeginSend(base.Uri, webClientAsyncResult, true);
			}
			catch (Exception ex4)
			{
				if (ex4 is ThreadAbortException || ex4 is StackOverflowException || ex4 is OutOfMemoryException)
				{
					throw;
				}
				if (Tracing.On)
				{
					Tracing.ExceptionCatch(TraceEventType.Warning, this, "InvokeAsync", ex4);
				}
				object obj = userState;
				object[] array3 = new object[1];
				base.OperationCompleted(obj, array3, ex4, false);
			}
			catch
			{
				object obj2 = userState;
				object[] array4 = new object[1];
				base.OperationCompleted(obj2, array4, new Exception(Res.GetString("NonClsCompliantException")), false);
			}
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000C070 File Offset: 0x0000B070
		private static Array CombineExtensionsHelper(Array array1, Array array2, Array array3, Type elementType)
		{
			int num = array1.Length + array2.Length + array3.Length;
			if (num == 0)
			{
				return null;
			}
			Array array4;
			if (elementType == typeof(SoapReflectedExtension))
			{
				array4 = new SoapReflectedExtension[num];
			}
			else
			{
				if (elementType != typeof(object))
				{
					throw new ArgumentException(Res.GetString("ElementTypeMustBeObjectOrSoapReflectedException"), "elementType");
				}
				array4 = new object[num];
			}
			int num2 = 0;
			Array.Copy(array1, 0, array4, num2, array1.Length);
			num2 += array1.Length;
			Array.Copy(array2, 0, array4, num2, array2.Length);
			num2 += array2.Length;
			Array.Copy(array3, 0, array4, num2, array3.Length);
			return array4;
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0000C11C File Offset: 0x0000B11C
		private string EnvelopeNs
		{
			get
			{
				if (this.version != SoapProtocolVersion.Soap12)
				{
					return "http://schemas.xmlsoap.org/soap/envelope/";
				}
				return "http://www.w3.org/2003/05/soap-envelope";
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000272 RID: 626 RVA: 0x0000C132 File Offset: 0x0000B132
		private string EncodingNs
		{
			get
			{
				if (this.version != SoapProtocolVersion.Soap12)
				{
					return "http://schemas.xmlsoap.org/soap/encoding/";
				}
				return "http://www.w3.org/2003/05/soap-encoding";
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000273 RID: 627 RVA: 0x0000C148 File Offset: 0x0000B148
		private string HttpContentType
		{
			get
			{
				if (this.version != SoapProtocolVersion.Soap12)
				{
					return "text/xml";
				}
				return "application/soap+xml";
			}
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000C160 File Offset: 0x0000B160
		private SoapClientMessage BeforeSerialize(WebRequest request, string methodName, object[] parameters)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException("parameters");
			}
			SoapClientMethod method = this.clientType.GetMethod(methodName);
			if (method == null)
			{
				throw new ArgumentException(Res.GetString("WebInvalidMethodName", new object[] { methodName }));
			}
			SoapReflectedExtension[] array = (SoapReflectedExtension[])SoapHttpClientProtocol.CombineExtensionsHelper(this.clientType.HighPriExtensions, method.extensions, this.clientType.LowPriExtensions, typeof(SoapReflectedExtension));
			object[] array2 = (object[])SoapHttpClientProtocol.CombineExtensionsHelper(this.clientType.HighPriExtensionInitializers, method.extensionInitializers, this.clientType.LowPriExtensionInitializers, typeof(object));
			SoapExtension[] array3 = SoapMessage.InitializeExtensions(array, array2);
			SoapClientMessage soapClientMessage = new SoapClientMessage(this, method, base.Url);
			soapClientMessage.initializedExtensions = array3;
			if (array3 != null)
			{
				soapClientMessage.SetExtensionStream(new SoapExtensionStream());
			}
			soapClientMessage.InitExtensionStreamChain(soapClientMessage.initializedExtensions);
			string text = UrlEncoder.EscapeString(method.action, Encoding.UTF8);
			soapClientMessage.SetStage(SoapMessageStage.BeforeSerialize);
			if (this.version == SoapProtocolVersion.Soap12)
			{
				soapClientMessage.ContentType = ContentType.Compose("application/soap+xml", (base.RequestEncoding != null) ? base.RequestEncoding : Encoding.UTF8, text);
			}
			else
			{
				soapClientMessage.ContentType = ContentType.Compose("text/xml", (base.RequestEncoding != null) ? base.RequestEncoding : Encoding.UTF8);
			}
			soapClientMessage.SetParameterValues(parameters);
			SoapHeaderHandling.GetHeaderMembers(soapClientMessage.Headers, this, method.inHeaderMappings, SoapHeaderDirection.In, true);
			soapClientMessage.RunExtensions(soapClientMessage.initializedExtensions, true);
			request.ContentType = soapClientMessage.ContentType;
			if (soapClientMessage.ContentEncoding != null && soapClientMessage.ContentEncoding.Length > 0)
			{
				request.Headers["Content-Encoding"] = soapClientMessage.ContentEncoding;
			}
			request.Method = "POST";
			if (this.version != SoapProtocolVersion.Soap12 && request.Headers["SOAPAction"] == null)
			{
				StringBuilder stringBuilder = new StringBuilder(text.Length + 2);
				stringBuilder.Append('"');
				stringBuilder.Append(text);
				stringBuilder.Append('"');
				request.Headers.Add("SOAPAction", stringBuilder.ToString());
			}
			return soapClientMessage;
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000C390 File Offset: 0x0000B390
		private void Serialize(SoapClientMessage message)
		{
			Stream stream = message.Stream;
			SoapClientMethod method = message.Method;
			bool flag = method.use == SoapBindingUse.Encoded;
			string envelopeNs = this.EnvelopeNs;
			string encodingNs = this.EncodingNs;
			XmlWriter writerForMessage = this.GetWriterForMessage(message, 1024);
			if (writerForMessage == null)
			{
				throw new InvalidOperationException(Res.GetString("WebNullWriterForMessage"));
			}
			writerForMessage.WriteStartDocument();
			writerForMessage.WriteStartElement("soap", "Envelope", envelopeNs);
			writerForMessage.WriteAttributeString("xmlns", "soap", null, envelopeNs);
			if (flag)
			{
				writerForMessage.WriteAttributeString("xmlns", "soapenc", null, encodingNs);
				writerForMessage.WriteAttributeString("xmlns", "tns", null, this.clientType.serviceNamespace);
				writerForMessage.WriteAttributeString("xmlns", "types", null, SoapReflector.GetEncodedNamespace(this.clientType.serviceNamespace, this.clientType.serviceDefaultIsEncoded));
			}
			writerForMessage.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
			writerForMessage.WriteAttributeString("xmlns", "xsd", null, "http://www.w3.org/2001/XMLSchema");
			SoapHeaderHandling.WriteHeaders(writerForMessage, method.inHeaderSerializer, message.Headers, method.inHeaderMappings, SoapHeaderDirection.In, flag, this.clientType.serviceNamespace, this.clientType.serviceDefaultIsEncoded, envelopeNs);
			writerForMessage.WriteStartElement("Body", envelopeNs);
			if (flag && this.version != SoapProtocolVersion.Soap12)
			{
				writerForMessage.WriteAttributeString("soap", "encodingStyle", null, encodingNs);
			}
			object[] parameterValues = message.GetParameterValues();
			TraceMethod traceMethod = (Tracing.On ? new TraceMethod(this, "Serialize", new object[0]) : null);
			if (Tracing.On)
			{
				Tracing.Enter(Tracing.TraceId("TraceWriteRequest"), traceMethod, new TraceMethod(method.parameterSerializer, "Serialize", new object[]
				{
					writerForMessage,
					parameterValues,
					null,
					flag ? encodingNs : null
				}));
			}
			method.parameterSerializer.Serialize(writerForMessage, parameterValues, null, flag ? encodingNs : null);
			if (Tracing.On)
			{
				Tracing.Exit(Tracing.TraceId("TraceWriteRequest"), traceMethod);
			}
			writerForMessage.WriteEndElement();
			writerForMessage.WriteEndElement();
			writerForMessage.Flush();
			message.SetStage(SoapMessageStage.AfterSerialize);
			message.RunExtensions(message.initializedExtensions, true);
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000C5C4 File Offset: 0x0000B5C4
		private object[] ReadResponse(SoapClientMessage message, WebResponse response, Stream responseStream, bool asyncCall)
		{
			SoapClientMethod method = message.Method;
			HttpWebResponse httpWebResponse = response as HttpWebResponse;
			int num = (int)((httpWebResponse != null) ? httpWebResponse.StatusCode : ((HttpStatusCode)(-1)));
			if (num >= 300 && num != 500 && num != 400)
			{
				throw new WebException(RequestResponseUtils.CreateResponseExceptionString(httpWebResponse, responseStream), null, WebExceptionStatus.ProtocolError, httpWebResponse);
			}
			message.Headers.Clear();
			message.SetStream(responseStream);
			message.InitExtensionStreamChain(message.initializedExtensions);
			message.SetStage(SoapMessageStage.BeforeDeserialize);
			message.ContentType = response.ContentType;
			message.ContentEncoding = response.Headers["Content-Encoding"];
			message.RunExtensions(message.initializedExtensions, false);
			if (method.oneWay && (httpWebResponse == null || httpWebResponse.StatusCode != HttpStatusCode.InternalServerError))
			{
				return new object[0];
			}
			bool flag = ContentType.IsSoap(message.ContentType);
			if (!flag || (flag && httpWebResponse != null && httpWebResponse.ContentLength == 0L))
			{
				if (num == 400)
				{
					throw new WebException(RequestResponseUtils.CreateResponseExceptionString(httpWebResponse, responseStream), null, WebExceptionStatus.ProtocolError, httpWebResponse);
				}
				throw new InvalidOperationException(Res.GetString("WebResponseContent", new object[] { message.ContentType, this.HttpContentType }) + Environment.NewLine + RequestResponseUtils.CreateResponseExceptionString(response, responseStream));
			}
			else
			{
				if (message.Exception != null)
				{
					throw message.Exception;
				}
				int num2;
				if (asyncCall || httpWebResponse == null)
				{
					num2 = 512;
				}
				else
				{
					num2 = RequestResponseUtils.GetBufferSize((int)httpWebResponse.ContentLength);
				}
				XmlReader readerForMessage = this.GetReaderForMessage(message, num2);
				if (readerForMessage == null)
				{
					throw new InvalidOperationException(Res.GetString("WebNullReaderForMessage"));
				}
				readerForMessage.MoveToContent();
				int depth = readerForMessage.Depth;
				string encodingNs = this.EncodingNs;
				string namespaceURI = readerForMessage.NamespaceURI;
				if (namespaceURI == null || namespaceURI.Length == 0)
				{
					readerForMessage.ReadStartElement("Envelope");
				}
				else if (readerForMessage.NamespaceURI == "http://schemas.xmlsoap.org/soap/envelope/")
				{
					readerForMessage.ReadStartElement("Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
				}
				else
				{
					if (!(readerForMessage.NamespaceURI == "http://www.w3.org/2003/05/soap-envelope"))
					{
						throw new SoapException(Res.GetString("WebInvalidEnvelopeNamespace", new object[] { namespaceURI, this.EnvelopeNs }), SoapException.VersionMismatchFaultCode);
					}
					readerForMessage.ReadStartElement("Envelope", "http://www.w3.org/2003/05/soap-envelope");
				}
				readerForMessage.MoveToContent();
				SoapHeaderHandling soapHeaderHandling = new SoapHeaderHandling();
				soapHeaderHandling.ReadHeaders(readerForMessage, method.outHeaderSerializer, message.Headers, method.outHeaderMappings, SoapHeaderDirection.Out | SoapHeaderDirection.Fault, namespaceURI, (method.use == SoapBindingUse.Encoded) ? encodingNs : null, false);
				readerForMessage.MoveToContent();
				readerForMessage.ReadStartElement("Body", namespaceURI);
				readerForMessage.MoveToContent();
				if (readerForMessage.IsStartElement("Fault", namespaceURI))
				{
					message.Exception = this.ReadSoapException(readerForMessage);
				}
				else if (method.oneWay)
				{
					readerForMessage.Skip();
					message.SetParameterValues(new object[0]);
				}
				else
				{
					TraceMethod traceMethod = (Tracing.On ? new TraceMethod(this, "ReadResponse", new object[0]) : null);
					bool flag2 = method.use == SoapBindingUse.Encoded;
					if (Tracing.On)
					{
						Tracing.Enter(Tracing.TraceId("TraceReadResponse"), traceMethod, new TraceMethod(method.returnSerializer, "Deserialize", new object[]
						{
							readerForMessage,
							flag2 ? encodingNs : null
						}));
					}
					bool flag3 = !flag2 && (WebServicesSection.Current.SoapEnvelopeProcessing.IsStrict || Tracing.On);
					if (flag3)
					{
						XmlDeserializationEvents xmlDeserializationEvents = (Tracing.On ? Tracing.GetDeserializationEvents() : RuntimeUtils.GetDeserializationEvents());
						message.SetParameterValues((object[])method.returnSerializer.Deserialize(readerForMessage, null, xmlDeserializationEvents));
					}
					else
					{
						message.SetParameterValues((object[])method.returnSerializer.Deserialize(readerForMessage, flag2 ? encodingNs : null));
					}
					if (Tracing.On)
					{
						Tracing.Exit(Tracing.TraceId("TraceReadResponse"), traceMethod);
					}
				}
				while (depth < readerForMessage.Depth && readerForMessage.Read())
				{
				}
				if (readerForMessage.NodeType == XmlNodeType.EndElement)
				{
					readerForMessage.Read();
				}
				message.SetStage(SoapMessageStage.AfterDeserialize);
				message.RunExtensions(message.initializedExtensions, false);
				SoapHeaderHandling.SetHeaderMembers(message.Headers, this, method.outHeaderMappings, SoapHeaderDirection.Out | SoapHeaderDirection.Fault, true);
				if (message.Exception != null)
				{
					throw message.Exception;
				}
				return message.GetParameterValues();
			}
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000CA04 File Offset: 0x0000BA04
		private SoapException ReadSoapException(XmlReader reader)
		{
			XmlQualifiedName xmlQualifiedName = XmlQualifiedName.Empty;
			string text = null;
			string text2 = null;
			string text3 = null;
			XmlNode xmlNode = null;
			SoapFaultSubCode soapFaultSubCode = null;
			string text4 = null;
			bool flag = reader.NamespaceURI == "http://www.w3.org/2003/05/soap-envelope";
			if (reader.IsEmptyElement)
			{
				reader.Skip();
			}
			else
			{
				reader.ReadStartElement();
				reader.MoveToContent();
				int depth = reader.Depth;
				while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
				{
					if (reader.NamespaceURI == "http://schemas.xmlsoap.org/soap/envelope/" || reader.NamespaceURI == "http://www.w3.org/2003/05/soap-envelope" || reader.NamespaceURI == null || reader.NamespaceURI.Length == 0)
					{
						if (reader.LocalName == "faultcode" || reader.LocalName == "Code")
						{
							if (flag)
							{
								xmlQualifiedName = this.ReadSoap12FaultCode(reader, out soapFaultSubCode);
							}
							else
							{
								xmlQualifiedName = this.ReadFaultCode(reader);
							}
						}
						else if (reader.LocalName == "faultstring")
						{
							text4 = reader.GetAttribute("lang", "http://www.w3.org/XML/1998/namespace");
							reader.MoveToElement();
							text = reader.ReadElementString();
						}
						else if (reader.LocalName == "Reason")
						{
							if (reader.IsEmptyElement)
							{
								reader.Skip();
							}
							else
							{
								reader.ReadStartElement();
								reader.MoveToContent();
								while (reader.NodeType != XmlNodeType.EndElement)
								{
									if (reader.NodeType == XmlNodeType.None)
									{
										break;
									}
									if (reader.LocalName == "Text" && reader.NamespaceURI == "http://www.w3.org/2003/05/soap-envelope")
									{
										text = reader.ReadElementString();
									}
									else
									{
										reader.Skip();
									}
									reader.MoveToContent();
								}
								while (reader.NodeType == XmlNodeType.Whitespace)
								{
									reader.Skip();
								}
								if (reader.NodeType == XmlNodeType.None)
								{
									reader.Skip();
								}
								else
								{
									reader.ReadEndElement();
								}
							}
						}
						else if (reader.LocalName == "faultactor" || reader.LocalName == "Node")
						{
							text2 = reader.ReadElementString();
						}
						else if (reader.LocalName == "detail" || reader.LocalName == "Detail")
						{
							xmlNode = new XmlDocument().ReadNode(reader);
						}
						else if (reader.LocalName == "Role")
						{
							text3 = reader.ReadElementString();
						}
						else
						{
							reader.Skip();
						}
					}
					else
					{
						reader.Skip();
					}
					reader.MoveToContent();
				}
				while (reader.Read() && depth < reader.Depth)
				{
				}
				if (reader.NodeType == XmlNodeType.EndElement)
				{
					reader.Read();
				}
			}
			if (xmlNode != null || flag)
			{
				return new SoapException(text, xmlQualifiedName, text2, text3, text4, xmlNode, soapFaultSubCode, null);
			}
			return new SoapHeaderException(text, xmlQualifiedName, text2, text3, text4, soapFaultSubCode, null);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000CCC0 File Offset: 0x0000BCC0
		private XmlQualifiedName ReadSoap12FaultCode(XmlReader reader, out SoapFaultSubCode subcode)
		{
			SoapFaultSubCode soapFaultSubCode = this.ReadSoap12FaultCodesRecursive(reader, 0);
			if (soapFaultSubCode == null)
			{
				subcode = null;
				return null;
			}
			subcode = soapFaultSubCode.SubCode;
			return soapFaultSubCode.Code;
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000CCEC File Offset: 0x0000BCEC
		private SoapFaultSubCode ReadSoap12FaultCodesRecursive(XmlReader reader, int depth)
		{
			if (depth > 100)
			{
				return null;
			}
			if (reader.IsEmptyElement)
			{
				reader.Skip();
				return null;
			}
			XmlQualifiedName xmlQualifiedName = null;
			SoapFaultSubCode soapFaultSubCode = null;
			int depth2 = reader.Depth;
			reader.ReadStartElement();
			reader.MoveToContent();
			while (reader.NodeType != XmlNodeType.EndElement && reader.NodeType != XmlNodeType.None)
			{
				if (reader.NamespaceURI == "http://www.w3.org/2003/05/soap-envelope" || reader.NamespaceURI == null || reader.NamespaceURI.Length == 0)
				{
					if (reader.LocalName == "Value")
					{
						xmlQualifiedName = this.ReadFaultCode(reader);
					}
					else if (reader.LocalName == "Subcode")
					{
						soapFaultSubCode = this.ReadSoap12FaultCodesRecursive(reader, depth + 1);
					}
					else
					{
						reader.Skip();
					}
				}
				else
				{
					reader.Skip();
				}
				reader.MoveToContent();
			}
			while (depth2 < reader.Depth && reader.Read())
			{
			}
			if (reader.NodeType == XmlNodeType.EndElement)
			{
				reader.Read();
			}
			return new SoapFaultSubCode(xmlQualifiedName, soapFaultSubCode);
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000CDE0 File Offset: 0x0000BDE0
		private XmlQualifiedName ReadFaultCode(XmlReader reader)
		{
			if (reader.IsEmptyElement)
			{
				reader.Skip();
				return null;
			}
			reader.ReadStartElement();
			string text = reader.ReadString();
			int num = text.IndexOf(":", StringComparison.Ordinal);
			string text2 = reader.NamespaceURI;
			if (num >= 0)
			{
				string text3 = text.Substring(0, num);
				text2 = reader.LookupNamespace(text3);
				if (text2 == null)
				{
					throw new InvalidOperationException(Res.GetString("WebQNamePrefixUndefined", new object[] { text3 }));
				}
			}
			reader.ReadEndElement();
			return new XmlQualifiedName(text.Substring(num + 1), text2);
		}

		// Token: 0x040002F6 RID: 758
		private SoapClientType clientType;

		// Token: 0x040002F7 RID: 759
		private SoapProtocolVersion version;

		// Token: 0x02000063 RID: 99
		private class InvokeAsyncState
		{
			// Token: 0x0600027B RID: 635 RVA: 0x0000CE6A File Offset: 0x0000BE6A
			public InvokeAsyncState(string methodName, object[] parameters)
			{
				this.MethodName = methodName;
				this.Parameters = parameters;
			}

			// Token: 0x040002F8 RID: 760
			public string MethodName;

			// Token: 0x040002F9 RID: 761
			public object[] Parameters;

			// Token: 0x040002FA RID: 762
			public SoapClientMessage Message;
		}
	}
}
