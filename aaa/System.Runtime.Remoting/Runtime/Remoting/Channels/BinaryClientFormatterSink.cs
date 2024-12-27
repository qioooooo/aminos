using System;
using System.Collections;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x02000062 RID: 98
	public class BinaryClientFormatterSink : IClientFormatterSink, IMessageSink, IClientChannelSink, IChannelSinkBase
	{
		// Token: 0x06000315 RID: 789 RVA: 0x0000E92A File Offset: 0x0000D92A
		public BinaryClientFormatterSink(IClientChannelSink nextSink)
		{
			this._nextSink = nextSink;
		}

		// Token: 0x170000B2 RID: 178
		// (set) Token: 0x06000316 RID: 790 RVA: 0x0000E947 File Offset: 0x0000D947
		internal bool IncludeVersioning
		{
			set
			{
				this._includeVersioning = value;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (set) Token: 0x06000317 RID: 791 RVA: 0x0000E950 File Offset: 0x0000D950
		internal bool StrictBinding
		{
			set
			{
				this._strictBinding = value;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (set) Token: 0x06000318 RID: 792 RVA: 0x0000E959 File Offset: 0x0000D959
		internal SinkChannelProtocol ChannelProtocol
		{
			set
			{
				this._channelProtocol = value;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000319 RID: 793 RVA: 0x0000E962 File Offset: 0x0000D962
		public IMessageSink NextSink
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0000E96C File Offset: 0x0000D96C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public IMessage SyncProcessMessage(IMessage msg)
		{
			IMethodCallMessage methodCallMessage = msg as IMethodCallMessage;
			IMessage message;
			try
			{
				ITransportHeaders transportHeaders;
				Stream stream;
				this.SerializeMessage(msg, out transportHeaders, out stream);
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

		// Token: 0x0600031B RID: 795 RVA: 0x0000EA00 File Offset: 0x0000DA00
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			IMethodCallMessage methodCallMessage = (IMethodCallMessage)msg;
			try
			{
				ITransportHeaders transportHeaders;
				Stream stream;
				this.SerializeMessage(msg, out transportHeaders, out stream);
				ClientChannelSinkStack clientChannelSinkStack = new ClientChannelSinkStack(replySink);
				clientChannelSinkStack.Push(this, msg);
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

		// Token: 0x0600031C RID: 796 RVA: 0x0000EA9C File Offset: 0x0000DA9C
		private void SerializeMessage(IMessage msg, out ITransportHeaders headers, out Stream stream)
		{
			BaseTransportHeaders baseTransportHeaders = new BaseTransportHeaders();
			headers = baseTransportHeaders;
			baseTransportHeaders.ContentType = "application/octet-stream";
			if (this._channelProtocol == SinkChannelProtocol.Http)
			{
				headers["__RequestVerb"] = "POST";
			}
			bool flag = false;
			stream = this._nextSink.GetRequestStream(msg, headers);
			if (stream == null)
			{
				stream = new ChunkedMemoryStream(CoreChannel.BufferPool);
				flag = true;
			}
			CoreChannel.SerializeBinaryMessage(msg, stream, this._includeVersioning);
			if (flag)
			{
				stream.Position = 0L;
			}
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000EB14 File Offset: 0x0000DB14
		private IMessage DeserializeMessage(IMethodCallMessage mcm, ITransportHeaders headers, Stream stream)
		{
			IMessage message = CoreChannel.DeserializeBinaryResponseMessage(stream, mcm, this._strictBinding);
			stream.Close();
			return message;
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000EB36 File Offset: 0x0000DB36
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public void ProcessMessage(IMessage msg, ITransportHeaders requestHeaders, Stream requestStream, out ITransportHeaders responseHeaders, out Stream responseStream)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000EB3D File Offset: 0x0000DB3D
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public void AsyncProcessRequest(IClientChannelSinkStack sinkStack, IMessage msg, ITransportHeaders headers, Stream stream)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000EB44 File Offset: 0x0000DB44
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public void AsyncProcessResponse(IClientResponseChannelSinkStack sinkStack, object state, ITransportHeaders headers, Stream stream)
		{
			IMethodCallMessage methodCallMessage = (IMethodCallMessage)state;
			IMessage message = this.DeserializeMessage(methodCallMessage, headers, stream);
			sinkStack.DispatchReplyMessage(message);
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000EB6A File Offset: 0x0000DB6A
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public Stream GetRequestStream(IMessage msg, ITransportHeaders headers)
		{
			throw new NotSupportedException();
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000322 RID: 802 RVA: 0x0000EB71 File Offset: 0x0000DB71
		public IClientChannelSink NextChannelSink
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._nextSink;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000323 RID: 803 RVA: 0x0000EB79 File Offset: 0x0000DB79
		public IDictionary Properties
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return null;
			}
		}

		// Token: 0x0400024D RID: 589
		private IClientChannelSink _nextSink;

		// Token: 0x0400024E RID: 590
		private bool _includeVersioning = true;

		// Token: 0x0400024F RID: 591
		private bool _strictBinding;

		// Token: 0x04000250 RID: 592
		private SinkChannelProtocol _channelProtocol = SinkChannelProtocol.Other;
	}
}
