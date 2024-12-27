using System;
using System.Collections;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006C2 RID: 1730
	internal class DispatchChannelSink : IServerChannelSink, IChannelSinkBase
	{
		// Token: 0x06003EBD RID: 16061 RVA: 0x000D7BE5 File Offset: 0x000D6BE5
		internal DispatchChannelSink()
		{
		}

		// Token: 0x06003EBE RID: 16062 RVA: 0x000D7BED File Offset: 0x000D6BED
		public ServerProcessing ProcessMessage(IServerChannelSinkStack sinkStack, IMessage requestMsg, ITransportHeaders requestHeaders, Stream requestStream, out IMessage responseMsg, out ITransportHeaders responseHeaders, out Stream responseStream)
		{
			if (requestMsg == null)
			{
				throw new ArgumentNullException("requestMsg", Environment.GetResourceString("Remoting_Channel_DispatchSinkMessageMissing"));
			}
			if (requestStream != null)
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_Channel_DispatchSinkWantsNullRequestStream"));
			}
			responseHeaders = null;
			responseStream = null;
			return ChannelServices.DispatchMessage(sinkStack, requestMsg, out responseMsg);
		}

		// Token: 0x06003EBF RID: 16063 RVA: 0x000D7C2C File Offset: 0x000D6C2C
		public void AsyncProcessResponse(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers, Stream stream)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06003EC0 RID: 16064 RVA: 0x000D7C33 File Offset: 0x000D6C33
		public Stream GetResponseStream(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000A87 RID: 2695
		// (get) Token: 0x06003EC1 RID: 16065 RVA: 0x000D7C3A File Offset: 0x000D6C3A
		public IServerChannelSink NextChannelSink
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x06003EC2 RID: 16066 RVA: 0x000D7C3D File Offset: 0x000D6C3D
		public IDictionary Properties
		{
			get
			{
				return null;
			}
		}
	}
}
