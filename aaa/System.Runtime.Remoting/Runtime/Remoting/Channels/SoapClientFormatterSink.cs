using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;
using System.Text;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x02000067 RID: 103
	public class SoapClientFormatterSink : IClientFormatterSink, IMessageSink, IClientChannelSink, IChannelSinkBase
	{
		// Token: 0x0600033C RID: 828 RVA: 0x0000F492 File Offset: 0x0000E492
		public SoapClientFormatterSink(IClientChannelSink nextSink)
		{
			this._nextSink = nextSink;
		}

		// Token: 0x170000C0 RID: 192
		// (set) Token: 0x0600033D RID: 829 RVA: 0x0000F4AF File Offset: 0x0000E4AF
		internal bool IncludeVersioning
		{
			set
			{
				this._includeVersioning = value;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (set) Token: 0x0600033E RID: 830 RVA: 0x0000F4B8 File Offset: 0x0000E4B8
		internal bool StrictBinding
		{
			set
			{
				this._strictBinding = value;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (set) Token: 0x0600033F RID: 831 RVA: 0x0000F4C1 File Offset: 0x0000E4C1
		internal SinkChannelProtocol ChannelProtocol
		{
			set
			{
				this._channelProtocol = value;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000340 RID: 832 RVA: 0x0000F4CA File Offset: 0x0000E4CA
		public IMessageSink NextSink
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000F4D4 File Offset: 0x0000E4D4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public IMessage SyncProcessMessage(IMessage msg)
		{
			IMethodCallMessage methodCallMessage = (IMethodCallMessage)msg;
			IMessage message;
			try
			{
				ITransportHeaders transportHeaders;
				Stream stream;
				this.SerializeMessage(methodCallMessage, out transportHeaders, out stream);
				ITransportHeaders transportHeaders2;
				Stream stream2;
				this._nextSink.ProcessMessage(msg, transportHeaders, stream, out transportHeaders2, out stream2);
				if (transportHeaders2 == null)
				{
					throw new ArgumentNullException("returnHeaders");
				}
				message = this.DeserializeMessage(methodCallMessage, transportHeaders2, stream2);
			}
			catch (Exception ex)
			{
				message = new ReturnMessage(ex, methodCallMessage);
			}
			catch
			{
				message = new ReturnMessage(new Exception(CoreChannel.GetResourceString("Remoting_nonClsCompliantException")), methodCallMessage);
			}
			return message;
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000F568 File Offset: 0x0000E568
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			IMethodCallMessage methodCallMessage = (IMethodCallMessage)msg;
			try
			{
				ITransportHeaders transportHeaders;
				Stream stream;
				this.SerializeMessage(methodCallMessage, out transportHeaders, out stream);
				ClientChannelSinkStack clientChannelSinkStack = new ClientChannelSinkStack(replySink);
				clientChannelSinkStack.Push(this, methodCallMessage);
				this._nextSink.AsyncProcessRequest(clientChannelSinkStack, msg, transportHeaders, stream);
			}
			catch (Exception ex)
			{
				IMessage message = new ReturnMessage(ex, methodCallMessage);
				if (replySink != null)
				{
					replySink.SyncProcessMessage(message);
				}
			}
			catch
			{
				IMessage message = new ReturnMessage(new Exception(CoreChannel.GetResourceString("Remoting_nonClsCompliantException")), methodCallMessage);
				if (replySink != null)
				{
					replySink.SyncProcessMessage(message);
				}
			}
			return null;
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000F604 File Offset: 0x0000E604
		private void SerializeMessage(IMethodCallMessage mcm, out ITransportHeaders headers, out Stream stream)
		{
			BaseTransportHeaders baseTransportHeaders = new BaseTransportHeaders();
			headers = baseTransportHeaders;
			MethodBase methodBase = mcm.MethodBase;
			headers["SOAPAction"] = '"' + HttpEncodingHelper.EncodeUriAsXLinkHref(SoapServices.GetSoapActionFromMethodBase(methodBase)) + '"';
			baseTransportHeaders.ContentType = "text/xml; charset=\"utf-8\"";
			if (this._channelProtocol == SinkChannelProtocol.Http)
			{
				headers["__RequestVerb"] = "POST";
			}
			bool flag = false;
			stream = this._nextSink.GetRequestStream(mcm, headers);
			if (stream == null)
			{
				stream = new ChunkedMemoryStream(CoreChannel.BufferPool);
				flag = true;
			}
			CoreChannel.SerializeSoapMessage(mcm, stream, this._includeVersioning);
			if (flag)
			{
				stream.Position = 0L;
			}
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000F6B0 File Offset: 0x0000E6B0
		private IMessage DeserializeMessage(IMethodCallMessage mcm, ITransportHeaders headers, Stream stream)
		{
			Header[] array = new Header[]
			{
				new Header("__TypeName", mcm.TypeName),
				new Header("__MethodName", mcm.MethodName),
				new Header("__MethodSignature", mcm.MethodSignature)
			};
			string text = headers["Content-Type"] as string;
			string text2;
			string text3;
			HttpChannelHelper.ParseContentType(text, out text2, out text3);
			IMessage message;
			if (string.Compare(text2, "text/xml", StringComparison.Ordinal) == 0)
			{
				message = CoreChannel.DeserializeSoapResponseMessage(stream, mcm, array, this._strictBinding);
			}
			else
			{
				int num = 1024;
				byte[] array2 = new byte[num];
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = stream.Read(array2, 0, num); i > 0; i = stream.Read(array2, 0, num))
				{
					stringBuilder.Append(Encoding.ASCII.GetString(array2, 0, i));
				}
				message = new ReturnMessage(new RemotingException(stringBuilder.ToString()), mcm);
			}
			stream.Close();
			return message;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000F7A0 File Offset: 0x0000E7A0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public void ProcessMessage(IMessage msg, ITransportHeaders requestHeaders, Stream requestStream, out ITransportHeaders responseHeaders, out Stream responseStream)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000F7A7 File Offset: 0x0000E7A7
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public void AsyncProcessRequest(IClientChannelSinkStack sinkStack, IMessage msg, ITransportHeaders headers, Stream stream)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000F7B0 File Offset: 0x0000E7B0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public void AsyncProcessResponse(IClientResponseChannelSinkStack sinkStack, object state, ITransportHeaders headers, Stream stream)
		{
			IMethodCallMessage methodCallMessage = (IMethodCallMessage)state;
			IMessage message = this.DeserializeMessage(methodCallMessage, headers, stream);
			sinkStack.DispatchReplyMessage(message);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000F7D6 File Offset: 0x0000E7D6
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public Stream GetRequestStream(IMessage msg, ITransportHeaders headers)
		{
			throw new NotSupportedException();
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000349 RID: 841 RVA: 0x0000F7DD File Offset: 0x0000E7DD
		public IClientChannelSink NextChannelSink
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._nextSink;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600034A RID: 842 RVA: 0x0000F7E5 File Offset: 0x0000E7E5
		public IDictionary Properties
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return null;
			}
		}

		// Token: 0x04000262 RID: 610
		private IClientChannelSink _nextSink;

		// Token: 0x04000263 RID: 611
		private bool _includeVersioning = true;

		// Token: 0x04000264 RID: 612
		private bool _strictBinding;

		// Token: 0x04000265 RID: 613
		private SinkChannelProtocol _channelProtocol = SinkChannelProtocol.Other;
	}
}
