using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters;
using System.Security;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x02000069 RID: 105
	public class SoapServerFormatterSink : IServerChannelSink, IChannelSinkBase
	{
		// Token: 0x06000353 RID: 851 RVA: 0x0000F9C1 File Offset: 0x0000E9C1
		public SoapServerFormatterSink(SoapServerFormatterSink.Protocol protocol, IServerChannelSink nextSink, IChannelReceiver receiver)
		{
			if (receiver == null)
			{
				throw new ArgumentNullException("receiver");
			}
			this._nextSink = nextSink;
			this._protocol = protocol;
			this._receiver = receiver;
		}

		// Token: 0x170000C8 RID: 200
		// (set) Token: 0x06000354 RID: 852 RVA: 0x0000F9FA File Offset: 0x0000E9FA
		internal bool IncludeVersioning
		{
			set
			{
				this._includeVersioning = value;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (set) Token: 0x06000355 RID: 853 RVA: 0x0000FA03 File Offset: 0x0000EA03
		internal bool StrictBinding
		{
			set
			{
				this._strictBinding = value;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000356 RID: 854 RVA: 0x0000FA0C File Offset: 0x0000EA0C
		// (set) Token: 0x06000357 RID: 855 RVA: 0x0000FA14 File Offset: 0x0000EA14
		[ComVisible(false)]
		public TypeFilterLevel TypeFilterLevel
		{
			get
			{
				return this._formatterSecurityLevel;
			}
			set
			{
				this._formatterSecurityLevel = value;
			}
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0000FA20 File Offset: 0x0000EA20
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public ServerProcessing ProcessMessage(IServerChannelSinkStack sinkStack, IMessage requestMsg, ITransportHeaders requestHeaders, Stream requestStream, out IMessage responseMsg, out ITransportHeaders responseHeaders, out Stream responseStream)
		{
			if (requestMsg != null)
			{
				return this._nextSink.ProcessMessage(sinkStack, requestMsg, requestHeaders, requestStream, out responseMsg, out responseHeaders, out responseStream);
			}
			if (requestHeaders == null)
			{
				throw new ArgumentNullException("requestHeaders");
			}
			BaseTransportHeaders baseTransportHeaders = requestHeaders as BaseTransportHeaders;
			responseHeaders = null;
			responseStream = null;
			string text = null;
			bool flag = true;
			string text2;
			if (baseTransportHeaders != null)
			{
				text2 = baseTransportHeaders.ContentType;
			}
			else
			{
				text2 = requestHeaders["Content-Type"] as string;
			}
			if (text2 != null)
			{
				string text3;
				HttpChannelHelper.ParseContentType(text2, out text, out text3);
			}
			if (text != null && string.Compare(text, "text/xml", StringComparison.Ordinal) != 0)
			{
				flag = false;
			}
			if (this._protocol == SoapServerFormatterSink.Protocol.Http)
			{
				string text4 = (string)requestHeaders["__RequestVerb"];
				if (!text4.Equals("POST") && !text4.Equals("M-POST"))
				{
					flag = false;
				}
			}
			if (flag)
			{
				bool flag2 = true;
				ServerProcessing serverProcessing;
				try
				{
					string text5;
					if (baseTransportHeaders != null)
					{
						text5 = baseTransportHeaders.RequestUri;
					}
					else
					{
						text5 = (string)requestHeaders["__RequestUri"];
					}
					if (RemotingServices.GetServerTypeForUri(text5) == null)
					{
						throw new RemotingException(CoreChannel.GetResourceString("Remoting_ChnlSink_UriNotPublished"));
					}
					if (this._protocol == SoapServerFormatterSink.Protocol.Http)
					{
						string text6 = (string)requestHeaders["User-Agent"];
						if (text6 != null)
						{
							if (text6.IndexOf("MS .NET Remoting") == -1)
							{
								flag2 = false;
							}
						}
						else
						{
							flag2 = false;
						}
					}
					bool flag3 = true;
					object obj = requestHeaders["__CustomErrorsEnabled"];
					if (obj != null && obj is bool)
					{
						flag3 = (bool)obj;
					}
					CallContext.SetData("__CustomErrorsEnabled", flag3);
					string text7;
					Header[] channelHeaders = this.GetChannelHeaders(requestHeaders, out text7);
					PermissionSet permissionSet = null;
					if (this.TypeFilterLevel != TypeFilterLevel.Full)
					{
						permissionSet = new PermissionSet(PermissionState.None);
						permissionSet.SetPermission(new SecurityPermission(SecurityPermissionFlag.SerializationFormatter));
					}
					try
					{
						if (permissionSet != null)
						{
							permissionSet.PermitOnly();
						}
						requestMsg = CoreChannel.DeserializeSoapRequestMessage(requestStream, channelHeaders, this._strictBinding, this.TypeFilterLevel);
					}
					finally
					{
						if (permissionSet != null)
						{
							CodeAccessPermission.RevertPermitOnly();
						}
					}
					requestStream.Close();
					if (requestMsg == null)
					{
						throw new RemotingException(CoreChannel.GetResourceString("Remoting_DeserializeMessage"));
					}
					if (text7 != null && !SoapServices.IsSoapActionValidForMethodBase(text7, ((IMethodMessage)requestMsg).MethodBase))
					{
						throw new RemotingException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Soap_InvalidSoapAction"), new object[] { text7 }));
					}
					sinkStack.Push(this, null);
					serverProcessing = this._nextSink.ProcessMessage(sinkStack, requestMsg, requestHeaders, null, out responseMsg, out responseHeaders, out responseStream);
					if (responseStream != null)
					{
						throw new RemotingException(CoreChannel.GetResourceString("Remoting_ChnlSink_WantNullResponseStream"));
					}
					switch (serverProcessing)
					{
					case ServerProcessing.Complete:
						if (responseMsg == null)
						{
							throw new RemotingException(CoreChannel.GetResourceString("Remoting_DispatchMessage"));
						}
						sinkStack.Pop(this);
						this.SerializeResponse(sinkStack, responseMsg, flag2, ref responseHeaders, out responseStream);
						break;
					case ServerProcessing.OneWay:
						sinkStack.Pop(this);
						break;
					case ServerProcessing.Async:
						sinkStack.Store(this, null);
						break;
					}
				}
				catch (Exception ex)
				{
					serverProcessing = ServerProcessing.Complete;
					responseMsg = new ReturnMessage(ex, (IMethodCallMessage)((requestMsg == null) ? new ErrorMessage() : requestMsg));
					CallContext.SetData("__ClientIsClr", flag2);
					responseStream = (MemoryStream)CoreChannel.SerializeSoapMessage(responseMsg, this._includeVersioning);
					CallContext.FreeNamedDataSlot("__ClientIsClr");
					responseStream.Position = 0L;
					responseHeaders = new TransportHeaders();
					if (this._protocol == SoapServerFormatterSink.Protocol.Http)
					{
						responseHeaders["__HttpStatusCode"] = "500";
						responseHeaders["__HttpReasonPhrase"] = "Internal Server Error";
						responseHeaders["Content-Type"] = "text/xml; charset=\"utf-8\"";
					}
				}
				catch
				{
					serverProcessing = ServerProcessing.Complete;
					responseMsg = new ReturnMessage(new Exception(CoreChannel.GetResourceString("Remoting_nonClsCompliantException")), (IMethodCallMessage)((requestMsg == null) ? new ErrorMessage() : requestMsg));
					CallContext.SetData("__ClientIsClr", flag2);
					responseStream = (MemoryStream)CoreChannel.SerializeSoapMessage(responseMsg, this._includeVersioning);
					CallContext.FreeNamedDataSlot("__ClientIsClr");
					responseStream.Position = 0L;
					responseHeaders = new TransportHeaders();
					if (this._protocol == SoapServerFormatterSink.Protocol.Http)
					{
						responseHeaders["__HttpStatusCode"] = "500";
						responseHeaders["__HttpReasonPhrase"] = "Internal Server Error";
						responseHeaders["Content-Type"] = "text/xml; charset=\"utf-8\"";
					}
				}
				finally
				{
					CallContext.FreeNamedDataSlot("__CustomErrorsEnabled");
				}
				return serverProcessing;
			}
			if (this._nextSink != null)
			{
				return this._nextSink.ProcessMessage(sinkStack, null, requestHeaders, requestStream, out responseMsg, out responseHeaders, out responseStream);
			}
			if (this._protocol == SoapServerFormatterSink.Protocol.Http)
			{
				responseHeaders = new TransportHeaders();
				responseHeaders["__HttpStatusCode"] = "400";
				responseHeaders["__HttpReasonPhrase"] = "Bad Request";
				responseStream = null;
				responseMsg = null;
				return ServerProcessing.Complete;
			}
			throw new RemotingException(CoreChannel.GetResourceString("Remoting_Channels_InvalidRequestFormat"));
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0000FF10 File Offset: 0x0000EF10
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public void AsyncProcessResponse(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers, Stream stream)
		{
			this.SerializeResponse(sinkStack, msg, true, ref headers, out stream);
			sinkStack.AsyncProcessResponse(msg, headers, stream);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x0000FF2C File Offset: 0x0000EF2C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		private void SerializeResponse(IServerResponseChannelSinkStack sinkStack, IMessage msg, bool bClientIsClr, ref ITransportHeaders headers, out Stream stream)
		{
			BaseTransportHeaders baseTransportHeaders = new BaseTransportHeaders();
			if (headers != null)
			{
				foreach (object obj in headers)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					baseTransportHeaders[dictionaryEntry.Key] = dictionaryEntry.Value;
				}
			}
			headers = baseTransportHeaders;
			baseTransportHeaders.ContentType = "text/xml; charset=\"utf-8\"";
			if (this._protocol == SoapServerFormatterSink.Protocol.Http)
			{
				IMethodReturnMessage methodReturnMessage = msg as IMethodReturnMessage;
				if (methodReturnMessage != null && methodReturnMessage.Exception != null)
				{
					headers["__HttpStatusCode"] = "500";
					headers["__HttpReasonPhrase"] = "Internal Server Error";
				}
			}
			bool flag = false;
			stream = sinkStack.GetResponseStream(msg, headers);
			if (stream == null)
			{
				stream = new ChunkedMemoryStream(CoreChannel.BufferPool);
				flag = true;
			}
			bool flag2 = CoreChannel.SetupUrlBashingForIisSslIfNecessary();
			CallContext.SetData("__ClientIsClr", bClientIsClr);
			try
			{
				CoreChannel.SerializeSoapMessage(msg, stream, this._includeVersioning);
			}
			finally
			{
				CallContext.FreeNamedDataSlot("__ClientIsClr");
				CoreChannel.CleanupUrlBashingForIisSslIfNecessary(flag2);
			}
			if (flag)
			{
				stream.Position = 0L;
			}
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00010064 File Offset: 0x0000F064
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public Stream GetResponseStream(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers)
		{
			throw new NotSupportedException();
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x0600035C RID: 860 RVA: 0x0001006B File Offset: 0x0000F06B
		public IServerChannelSink NextChannelSink
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._nextSink;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x0600035D RID: 861 RVA: 0x00010073 File Offset: 0x0000F073
		public IDictionary Properties
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return null;
			}
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00010078 File Offset: 0x0000F078
		private Header[] GetChannelHeaders(ITransportHeaders requestHeaders, out string soapActionToVerify)
		{
			soapActionToVerify = null;
			string text = (string)requestHeaders["__RequestUri"];
			string text2 = (string)requestHeaders["SOAPAction"];
			if (text2 == null)
			{
				throw new RemotingException(CoreChannel.GetResourceString("Remoting_SoapActionMissing"));
			}
			text2 = HttpEncodingHelper.DecodeUri(text2);
			soapActionToVerify = text2;
			string text3;
			string text4;
			if (!SoapServices.GetTypeAndMethodNameFromSoapAction(text2, out text3, out text4))
			{
				Type serverTypeForUri = RemotingServices.GetServerTypeForUri(text);
				if (serverTypeForUri == null)
				{
					throw new RemotingException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_TypeNotFoundFromUri"), new object[] { text }));
				}
				text3 = "clr:" + serverTypeForUri.FullName + ", " + serverTypeForUri.Assembly.GetName().Name;
			}
			else
			{
				text3 = "clr:" + text3;
			}
			int num = 2;
			Header[] array = new Header[num];
			array[0] = new Header("__Uri", text);
			array[1] = new Header("__TypeName", text3);
			return array;
		}

		// Token: 0x0400026A RID: 618
		private IServerChannelSink _nextSink;

		// Token: 0x0400026B RID: 619
		private SoapServerFormatterSink.Protocol _protocol;

		// Token: 0x0400026C RID: 620
		private IChannelReceiver _receiver;

		// Token: 0x0400026D RID: 621
		private bool _includeVersioning = true;

		// Token: 0x0400026E RID: 622
		private bool _strictBinding;

		// Token: 0x0400026F RID: 623
		private TypeFilterLevel _formatterSecurityLevel = TypeFilterLevel.Full;

		// Token: 0x0200006A RID: 106
		[Serializable]
		public enum Protocol
		{
			// Token: 0x04000271 RID: 625
			Http,
			// Token: 0x04000272 RID: 626
			Other
		}
	}
}
