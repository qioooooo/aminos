using System;
using System.Collections;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Security.Principal;
using System.Threading;

namespace System.Runtime.Remoting.Channels.Tcp
{
	// Token: 0x02000044 RID: 68
	internal class TcpServerTransportSink : IServerChannelSink, IChannelSinkBase
	{
		// Token: 0x06000249 RID: 585 RVA: 0x0000BECC File Offset: 0x0000AECC
		internal TcpServerTransportSink(IServerChannelSink nextSink, bool impersonate)
		{
			this._nextSink = nextSink;
			this._impersonate = impersonate;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000BEE4 File Offset: 0x0000AEE4
		internal void ServiceRequest(object state)
		{
			TcpServerSocketHandler tcpServerSocketHandler = (TcpServerSocketHandler)state;
			ITransportHeaders transportHeaders = tcpServerSocketHandler.ReadHeaders();
			Stream requestStream = tcpServerSocketHandler.GetRequestStream();
			transportHeaders["__CustomErrorsEnabled"] = tcpServerSocketHandler.CustomErrorsEnabled();
			ServerChannelSinkStack serverChannelSinkStack = new ServerChannelSinkStack();
			serverChannelSinkStack.Push(this, tcpServerSocketHandler);
			WindowsIdentity impersonationIdentity = tcpServerSocketHandler.ImpersonationIdentity;
			WindowsImpersonationContext windowsImpersonationContext = null;
			IPrincipal principal = null;
			bool flag = false;
			if (impersonationIdentity != null)
			{
				principal = Thread.CurrentPrincipal;
				flag = true;
				if (this._impersonate)
				{
					Thread.CurrentPrincipal = new WindowsPrincipal(impersonationIdentity);
					windowsImpersonationContext = impersonationIdentity.Impersonate();
				}
				else
				{
					Thread.CurrentPrincipal = new GenericPrincipal(impersonationIdentity, null);
				}
			}
			ITransportHeaders transportHeaders2;
			Stream stream;
			ServerProcessing serverProcessing;
			try
			{
				try
				{
					IMessage message;
					serverProcessing = this._nextSink.ProcessMessage(serverChannelSinkStack, null, transportHeaders, requestStream, out message, out transportHeaders2, out stream);
				}
				finally
				{
					if (flag)
					{
						Thread.CurrentPrincipal = principal;
					}
					if (this._impersonate)
					{
						windowsImpersonationContext.Undo();
					}
				}
			}
			catch
			{
				throw;
			}
			switch (serverProcessing)
			{
			case ServerProcessing.Complete:
				serverChannelSinkStack.Pop(this);
				tcpServerSocketHandler.SendResponse(transportHeaders2, stream);
				break;
			case ServerProcessing.OneWay:
				tcpServerSocketHandler.SendResponse(transportHeaders2, stream);
				break;
			case ServerProcessing.Async:
				serverChannelSinkStack.StoreAndDispatch(this, tcpServerSocketHandler);
				break;
			}
			if (serverProcessing != ServerProcessing.Async)
			{
				if (tcpServerSocketHandler.CanServiceAnotherRequest())
				{
					tcpServerSocketHandler.BeginReadMessage();
					return;
				}
				tcpServerSocketHandler.Close();
			}
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000C028 File Offset: 0x0000B028
		public ServerProcessing ProcessMessage(IServerChannelSinkStack sinkStack, IMessage requestMsg, ITransportHeaders requestHeaders, Stream requestStream, out IMessage responseMsg, out ITransportHeaders responseHeaders, out Stream responseStream)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000C030 File Offset: 0x0000B030
		public void AsyncProcessResponse(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers, Stream stream)
		{
			TcpServerSocketHandler tcpServerSocketHandler = (TcpServerSocketHandler)state;
			tcpServerSocketHandler.SendResponse(headers, stream);
			if (tcpServerSocketHandler.CanServiceAnotherRequest())
			{
				tcpServerSocketHandler.BeginReadMessage();
				return;
			}
			tcpServerSocketHandler.Close();
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000C065 File Offset: 0x0000B065
		public Stream GetResponseStream(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers)
		{
			return null;
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600024E RID: 590 RVA: 0x0000C068 File Offset: 0x0000B068
		public IServerChannelSink NextChannelSink
		{
			get
			{
				return this._nextSink;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600024F RID: 591 RVA: 0x0000C070 File Offset: 0x0000B070
		public IDictionary Properties
		{
			get
			{
				return null;
			}
		}

		// Token: 0x040001A6 RID: 422
		private const int s_MaxSize = 33554432;

		// Token: 0x040001A7 RID: 423
		private IServerChannelSink _nextSink;

		// Token: 0x040001A8 RID: 424
		private bool _impersonate;
	}
}
