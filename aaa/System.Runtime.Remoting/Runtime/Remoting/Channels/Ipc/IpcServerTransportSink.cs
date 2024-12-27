using System;
using System.Collections;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Security.Principal;
using System.Threading;

namespace System.Runtime.Remoting.Channels.Ipc
{
	// Token: 0x02000051 RID: 81
	internal class IpcServerTransportSink : IServerChannelSink, IChannelSinkBase
	{
		// Token: 0x06000292 RID: 658 RVA: 0x0000CF50 File Offset: 0x0000BF50
		public IpcServerTransportSink(IServerChannelSink nextSink, bool secure, bool impersonate)
		{
			this._nextSink = nextSink;
			this._secure = secure;
			this._impersonate = impersonate;
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000293 RID: 659 RVA: 0x0000CF6D File Offset: 0x0000BF6D
		// (set) Token: 0x06000294 RID: 660 RVA: 0x0000CF75 File Offset: 0x0000BF75
		internal bool IsSecured
		{
			get
			{
				return this._secure;
			}
			set
			{
				this._secure = value;
			}
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000CF80 File Offset: 0x0000BF80
		internal void ServiceRequest(object state)
		{
			IpcServerHandler ipcServerHandler = (IpcServerHandler)state;
			ITransportHeaders transportHeaders = ipcServerHandler.ReadHeaders();
			Stream requestStream = ipcServerHandler.GetRequestStream();
			transportHeaders["__CustomErrorsEnabled"] = false;
			ServerChannelSinkStack serverChannelSinkStack = new ServerChannelSinkStack();
			serverChannelSinkStack.Push(this, ipcServerHandler);
			IMessage message = null;
			ITransportHeaders transportHeaders2 = null;
			Stream stream = null;
			IPrincipal principal = null;
			bool flag = false;
			bool flag2 = false;
			ServerProcessing serverProcessing = ServerProcessing.Complete;
			try
			{
				if (this._secure)
				{
					IpcPort port = ipcServerHandler.Port;
					port.ImpersonateClient();
					principal = Thread.CurrentPrincipal;
					flag2 = true;
					flag = true;
					WindowsIdentity current = WindowsIdentity.GetCurrent();
					if (!this._impersonate)
					{
						NativePipe.RevertToSelf();
						Thread.CurrentPrincipal = new GenericPrincipal(current, null);
						flag = false;
					}
					else
					{
						if (current.ImpersonationLevel != TokenImpersonationLevel.Impersonation && current.ImpersonationLevel != TokenImpersonationLevel.Delegation)
						{
							throw new RemotingException(CoreChannel.GetResourceString("Remoting_Ipc_TokenImpersonationFailure"));
						}
						Thread.CurrentPrincipal = new WindowsPrincipal(current);
					}
				}
				serverProcessing = this._nextSink.ProcessMessage(serverChannelSinkStack, null, transportHeaders, requestStream, out message, out transportHeaders2, out stream);
			}
			catch (Exception ex)
			{
				ipcServerHandler.CloseOnFatalError(ex);
			}
			catch
			{
				ipcServerHandler.CloseOnFatalError(new Exception(CoreChannel.GetResourceString("Remoting_nonClsCompliantException")));
			}
			finally
			{
				if (flag2)
				{
					Thread.CurrentPrincipal = principal;
				}
				if (flag)
				{
					NativePipe.RevertToSelf();
					flag = false;
				}
			}
			switch (serverProcessing)
			{
			case ServerProcessing.Complete:
				serverChannelSinkStack.Pop(this);
				ipcServerHandler.SendResponse(transportHeaders2, stream);
				break;
			case ServerProcessing.OneWay:
				ipcServerHandler.SendResponse(transportHeaders2, stream);
				break;
			case ServerProcessing.Async:
				serverChannelSinkStack.StoreAndDispatch(this, ipcServerHandler);
				break;
			}
			if (serverProcessing != ServerProcessing.Async)
			{
				ipcServerHandler.BeginReadMessage();
			}
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000D124 File Offset: 0x0000C124
		public ServerProcessing ProcessMessage(IServerChannelSinkStack sinkStack, IMessage requestMsg, ITransportHeaders requestHeaders, Stream requestStream, out IMessage responseMsg, out ITransportHeaders responseHeaders, out Stream responseStream)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000D12C File Offset: 0x0000C12C
		public void AsyncProcessResponse(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers, Stream stream)
		{
			IpcServerHandler ipcServerHandler = (IpcServerHandler)state;
			ipcServerHandler.SendResponse(headers, stream);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000D14C File Offset: 0x0000C14C
		public Stream GetResponseStream(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers)
		{
			return null;
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0000D14F File Offset: 0x0000C14F
		public IServerChannelSink NextChannelSink
		{
			get
			{
				return this._nextSink;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600029A RID: 666 RVA: 0x0000D157 File Offset: 0x0000C157
		public IDictionary Properties
		{
			get
			{
				return null;
			}
		}

		// Token: 0x040001E1 RID: 481
		private const int s_MaxSize = 33554432;

		// Token: 0x040001E2 RID: 482
		private IServerChannelSink _nextSink;

		// Token: 0x040001E3 RID: 483
		private bool _secure;

		// Token: 0x040001E4 RID: 484
		private bool _impersonate;
	}
}
