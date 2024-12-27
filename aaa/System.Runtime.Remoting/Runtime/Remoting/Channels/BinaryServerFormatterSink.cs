using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Configuration;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters;
using System.Security;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x02000064 RID: 100
	public class BinaryServerFormatterSink : IServerChannelSink, IChannelSinkBase
	{
		// Token: 0x0600032C RID: 812 RVA: 0x0000ED55 File Offset: 0x0000DD55
		public BinaryServerFormatterSink(BinaryServerFormatterSink.Protocol protocol, IServerChannelSink nextSink, IChannelReceiver receiver)
		{
			if (receiver == null)
			{
				throw new ArgumentNullException("receiver");
			}
			this._nextSink = nextSink;
			this._protocol = protocol;
			this._receiver = receiver;
		}

		// Token: 0x170000BA RID: 186
		// (set) Token: 0x0600032D RID: 813 RVA: 0x0000ED8E File Offset: 0x0000DD8E
		internal bool IncludeVersioning
		{
			set
			{
				this._includeVersioning = value;
			}
		}

		// Token: 0x170000BB RID: 187
		// (set) Token: 0x0600032E RID: 814 RVA: 0x0000ED97 File Offset: 0x0000DD97
		internal bool StrictBinding
		{
			set
			{
				this._strictBinding = value;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600032F RID: 815 RVA: 0x0000EDA0 File Offset: 0x0000DDA0
		// (set) Token: 0x06000330 RID: 816 RVA: 0x0000EDA8 File Offset: 0x0000DDA8
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

		// Token: 0x06000331 RID: 817 RVA: 0x0000EDB4 File Offset: 0x0000DDB4
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
			if (text != null && string.CompareOrdinal(text, "application/octet-stream") != 0)
			{
				flag = false;
			}
			if (this._protocol == BinaryServerFormatterSink.Protocol.Http)
			{
				string text4 = (string)requestHeaders["__RequestVerb"];
				if (!text4.Equals("POST") && !text4.Equals("M-POST"))
				{
					flag = false;
				}
			}
			if (flag)
			{
				ServerProcessing serverProcessing;
				try
				{
					bool flag2 = true;
					object obj = requestHeaders["__CustomErrorsEnabled"];
					if (obj != null && obj is bool)
					{
						flag2 = (bool)obj;
					}
					CallContext.SetData("__CustomErrorsEnabled", flag2);
					string text5;
					if (baseTransportHeaders != null)
					{
						text5 = baseTransportHeaders.RequestUri;
					}
					else
					{
						text5 = (string)requestHeaders["__RequestUri"];
					}
					if (text5 != this.lastUri && RemotingServices.GetServerTypeForUri(text5) == null)
					{
						throw new RemotingException(CoreChannel.GetResourceString("Remoting_ChnlSink_UriNotPublished"));
					}
					this.lastUri = text5;
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
						requestMsg = CoreChannel.DeserializeBinaryRequestMessage(text5, requestStream, this._strictBinding, this.TypeFilterLevel);
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
					if (requestMsg is MarshalByRefObject && !AppSettings.AllowTransparentProxyMessage)
					{
						requestMsg = null;
						throw new RemotingException(CoreChannel.GetResourceString("Remoting_DeserializeMessage"), new NotSupportedException(AppSettings.AllowTransparentProxyMessageFwLink));
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
						this.SerializeResponse(sinkStack, responseMsg, ref responseHeaders, out responseStream);
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
					CallContext.SetData("__ClientIsClr", true);
					responseStream = (MemoryStream)CoreChannel.SerializeBinaryMessage(responseMsg, this._includeVersioning);
					CallContext.FreeNamedDataSlot("__ClientIsClr");
					responseStream.Position = 0L;
					responseHeaders = new TransportHeaders();
					if (this._protocol == BinaryServerFormatterSink.Protocol.Http)
					{
						responseHeaders["Content-Type"] = "application/octet-stream";
					}
				}
				catch
				{
					serverProcessing = ServerProcessing.Complete;
					responseMsg = new ReturnMessage(new Exception(CoreChannel.GetResourceString("Remoting_nonClsCompliantException")), (IMethodCallMessage)((requestMsg == null) ? new ErrorMessage() : requestMsg));
					CallContext.SetData("__ClientIsClr", true);
					responseStream = (MemoryStream)CoreChannel.SerializeBinaryMessage(responseMsg, this._includeVersioning);
					CallContext.FreeNamedDataSlot("__ClientIsClr");
					responseStream.Position = 0L;
					responseHeaders = new TransportHeaders();
					if (this._protocol == BinaryServerFormatterSink.Protocol.Http)
					{
						responseHeaders["Content-Type"] = "application/octet-stream";
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
			if (this._protocol == BinaryServerFormatterSink.Protocol.Http)
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

		// Token: 0x06000332 RID: 818 RVA: 0x0000F218 File Offset: 0x0000E218
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public void AsyncProcessResponse(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers, Stream stream)
		{
			this.SerializeResponse(sinkStack, msg, ref headers, out stream);
			sinkStack.AsyncProcessResponse(msg, headers, stream);
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000F234 File Offset: 0x0000E234
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		private void SerializeResponse(IServerResponseChannelSinkStack sinkStack, IMessage msg, ref ITransportHeaders headers, out Stream stream)
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
			if (this._protocol == BinaryServerFormatterSink.Protocol.Http)
			{
				baseTransportHeaders.ContentType = "application/octet-stream";
			}
			bool flag = false;
			stream = sinkStack.GetResponseStream(msg, headers);
			if (stream == null)
			{
				stream = new ChunkedMemoryStream(CoreChannel.BufferPool);
				flag = true;
			}
			bool flag2 = CoreChannel.SetupUrlBashingForIisSslIfNecessary();
			try
			{
				CallContext.SetData("__ClientIsClr", true);
				CoreChannel.SerializeBinaryMessage(msg, stream, this._includeVersioning);
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

		// Token: 0x06000334 RID: 820 RVA: 0x0000F330 File Offset: 0x0000E330
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public Stream GetResponseStream(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers)
		{
			throw new NotSupportedException();
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000335 RID: 821 RVA: 0x0000F337 File Offset: 0x0000E337
		public IServerChannelSink NextChannelSink
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._nextSink;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000336 RID: 822 RVA: 0x0000F33F File Offset: 0x0000E33F
		public IDictionary Properties
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return null;
			}
		}

		// Token: 0x04000255 RID: 597
		private IServerChannelSink _nextSink;

		// Token: 0x04000256 RID: 598
		private BinaryServerFormatterSink.Protocol _protocol;

		// Token: 0x04000257 RID: 599
		private IChannelReceiver _receiver;

		// Token: 0x04000258 RID: 600
		private bool _includeVersioning = true;

		// Token: 0x04000259 RID: 601
		private bool _strictBinding;

		// Token: 0x0400025A RID: 602
		private TypeFilterLevel _formatterSecurityLevel = TypeFilterLevel.Full;

		// Token: 0x0400025B RID: 603
		private string lastUri;

		// Token: 0x02000065 RID: 101
		[Serializable]
		public enum Protocol
		{
			// Token: 0x0400025D RID: 605
			Http,
			// Token: 0x0400025E RID: 606
			Other
		}
	}
}
