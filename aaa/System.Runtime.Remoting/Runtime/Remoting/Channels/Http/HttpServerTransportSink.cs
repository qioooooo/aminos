using System;
using System.Collections;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Channels.Http
{
	// Token: 0x0200002D RID: 45
	internal class HttpServerTransportSink : IServerChannelSink, IChannelSinkBase
	{
		// Token: 0x06000172 RID: 370 RVA: 0x00008100 File Offset: 0x00007100
		public HttpServerTransportSink(IServerChannelSink nextSink)
		{
			this._nextSink = nextSink;
		}

		// Token: 0x06000173 RID: 371 RVA: 0x00008110 File Offset: 0x00007110
		internal void ServiceRequest(object state)
		{
			HttpServerSocketHandler httpServerSocketHandler = (HttpServerSocketHandler)state;
			ITransportHeaders transportHeaders = httpServerSocketHandler.ReadHeaders();
			Stream requestStream = httpServerSocketHandler.GetRequestStream();
			transportHeaders["__CustomErrorsEnabled"] = httpServerSocketHandler.CustomErrorsEnabled();
			ServerChannelSinkStack serverChannelSinkStack = new ServerChannelSinkStack();
			serverChannelSinkStack.Push(this, httpServerSocketHandler);
			IMessage message;
			ITransportHeaders transportHeaders2;
			Stream stream;
			ServerProcessing serverProcessing = this._nextSink.ProcessMessage(serverChannelSinkStack, null, transportHeaders, requestStream, out message, out transportHeaders2, out stream);
			switch (serverProcessing)
			{
			case ServerProcessing.Complete:
				serverChannelSinkStack.Pop(this);
				httpServerSocketHandler.SendResponse(stream, "200", "OK", transportHeaders2);
				break;
			case ServerProcessing.OneWay:
				httpServerSocketHandler.SendResponse(null, "202", "Accepted", transportHeaders2);
				break;
			case ServerProcessing.Async:
				serverChannelSinkStack.StoreAndDispatch(this, httpServerSocketHandler);
				break;
			}
			if (serverProcessing != ServerProcessing.Async)
			{
				if (httpServerSocketHandler.CanServiceAnotherRequest())
				{
					httpServerSocketHandler.BeginReadMessage();
					return;
				}
				httpServerSocketHandler.Close();
			}
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000081DB File Offset: 0x000071DB
		public ServerProcessing ProcessMessage(IServerChannelSinkStack sinkStack, IMessage requestMsg, ITransportHeaders requestHeaders, Stream requestStream, out IMessage responseMsg, out ITransportHeaders responseHeaders, out Stream responseStream)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000175 RID: 373 RVA: 0x000081E4 File Offset: 0x000071E4
		public void AsyncProcessResponse(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers, Stream stream)
		{
			HttpServerSocketHandler httpServerSocketHandler = (HttpServerSocketHandler)state;
			httpServerSocketHandler.SendResponse(stream, "200", "OK", headers);
			if (httpServerSocketHandler.CanServiceAnotherRequest())
			{
				httpServerSocketHandler.BeginReadMessage();
				return;
			}
			httpServerSocketHandler.Close();
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00008224 File Offset: 0x00007224
		public Stream GetResponseStream(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers)
		{
			HttpServerSocketHandler httpServerSocketHandler = (HttpServerSocketHandler)state;
			if (httpServerSocketHandler.AllowChunkedResponse)
			{
				return httpServerSocketHandler.GetResponseStream("200", "OK", headers);
			}
			return null;
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000177 RID: 375 RVA: 0x00008254 File Offset: 0x00007254
		public IServerChannelSink NextChannelSink
		{
			get
			{
				return this._nextSink;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000178 RID: 376 RVA: 0x0000825C File Offset: 0x0000725C
		public IDictionary Properties
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000179 RID: 377 RVA: 0x0000825F File Offset: 0x0000725F
		internal static string ServerHeader
		{
			get
			{
				return HttpServerTransportSink.s_serverHeader;
			}
		}

		// Token: 0x0400012C RID: 300
		private static string s_serverHeader = "MS .NET Remoting, MS .NET CLR " + Environment.Version.ToString();

		// Token: 0x0400012D RID: 301
		private IServerChannelSink _nextSink;
	}
}
