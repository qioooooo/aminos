using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x020006A0 RID: 1696
	[ComVisible(true)]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
	public class ClientChannelSinkStack : IClientChannelSinkStack, IClientResponseChannelSinkStack
	{
		// Token: 0x06003DA6 RID: 15782 RVA: 0x000D3A41 File Offset: 0x000D2A41
		public ClientChannelSinkStack()
		{
		}

		// Token: 0x06003DA7 RID: 15783 RVA: 0x000D3A49 File Offset: 0x000D2A49
		public ClientChannelSinkStack(IMessageSink replySink)
		{
			this._replySink = replySink;
		}

		// Token: 0x06003DA8 RID: 15784 RVA: 0x000D3A58 File Offset: 0x000D2A58
		public void Push(IClientChannelSink sink, object state)
		{
			this._stack = new ClientChannelSinkStack.SinkStack
			{
				PrevStack = this._stack,
				Sink = sink,
				State = state
			};
		}

		// Token: 0x06003DA9 RID: 15785 RVA: 0x000D3A8C File Offset: 0x000D2A8C
		public object Pop(IClientChannelSink sink)
		{
			if (this._stack == null)
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_Channel_PopOnEmptySinkStack"));
			}
			while (this._stack.Sink != sink)
			{
				this._stack = this._stack.PrevStack;
				if (this._stack == null)
				{
					break;
				}
			}
			if (this._stack.Sink == null)
			{
				throw new RemotingException(Environment.GetResourceString("Remoting_Channel_PopFromSinkStackWithoutPush"));
			}
			object state = this._stack.State;
			this._stack = this._stack.PrevStack;
			return state;
		}

		// Token: 0x06003DAA RID: 15786 RVA: 0x000D3B14 File Offset: 0x000D2B14
		public void AsyncProcessResponse(ITransportHeaders headers, Stream stream)
		{
			if (this._replySink != null)
			{
				if (this._stack == null)
				{
					throw new RemotingException(Environment.GetResourceString("Remoting_Channel_CantCallAPRWhenStackEmpty"));
				}
				IClientChannelSink sink = this._stack.Sink;
				object state = this._stack.State;
				this._stack = this._stack.PrevStack;
				sink.AsyncProcessResponse(this, state, headers, stream);
			}
		}

		// Token: 0x06003DAB RID: 15787 RVA: 0x000D3B74 File Offset: 0x000D2B74
		public void DispatchReplyMessage(IMessage msg)
		{
			if (this._replySink != null)
			{
				this._replySink.SyncProcessMessage(msg);
			}
		}

		// Token: 0x06003DAC RID: 15788 RVA: 0x000D3B8B File Offset: 0x000D2B8B
		public void DispatchException(Exception e)
		{
			this.DispatchReplyMessage(new ReturnMessage(e, null));
		}

		// Token: 0x04001F49 RID: 8009
		private ClientChannelSinkStack.SinkStack _stack;

		// Token: 0x04001F4A RID: 8010
		private IMessageSink _replySink;

		// Token: 0x020006A1 RID: 1697
		private class SinkStack
		{
			// Token: 0x04001F4B RID: 8011
			public ClientChannelSinkStack.SinkStack PrevStack;

			// Token: 0x04001F4C RID: 8012
			public IClientChannelSink Sink;

			// Token: 0x04001F4D RID: 8013
			public object State;
		}
	}
}
