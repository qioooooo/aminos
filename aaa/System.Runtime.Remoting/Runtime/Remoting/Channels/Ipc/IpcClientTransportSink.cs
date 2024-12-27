using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Security.Principal;

namespace System.Runtime.Remoting.Channels.Ipc
{
	// Token: 0x02000055 RID: 85
	internal class IpcClientTransportSink : BaseChannelSinkWithProperties, IClientChannelSink, IChannelSinkBase
	{
		// Token: 0x060002AE RID: 686 RVA: 0x0000D450 File Offset: 0x0000C450
		internal IpcClientTransportSink(string channelURI, IpcClientChannel channel)
		{
			this._channel = channel;
			string text2;
			string text = IpcChannelHelper.ParseURL(channelURI, out text2);
			int num = text.IndexOf("://");
			num += 3;
			this._portName = text.Substring(num);
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0000D4AD File Offset: 0x0000C4AD
		internal ConnectionCache Cache
		{
			get
			{
				return this.portCache;
			}
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000D4B8 File Offset: 0x0000C4B8
		public void ProcessMessage(IMessage msg, ITransportHeaders requestHeaders, Stream requestStream, out ITransportHeaders responseHeaders, out Stream responseStream)
		{
			if (this.authSet && !this._channel.IsSecured)
			{
				throw new RemotingException(CoreChannel.GetResourceString("Remoting_Ipc_AuthenticationConfig"));
			}
			IpcPort connection = this.portCache.GetConnection(this._portName, this._channel.IsSecured, this._tokenImpersonationLevel, this._timeout);
			IMethodCallMessage methodCallMessage = (IMethodCallMessage)msg;
			long length = requestStream.Length;
			Stream stream = new PipeStream(connection);
			IpcClientHandler ipcClientHandler = new IpcClientHandler(connection, stream, this);
			ipcClientHandler.SendRequest(msg, requestHeaders, requestStream);
			responseHeaders = ipcClientHandler.ReadHeaders();
			responseStream = ipcClientHandler.GetResponseStream();
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000D54E File Offset: 0x0000C54E
		private IClientChannelSinkStack AsyncProcessMessage(IMessage msg, ITransportHeaders requestHeaders, Stream requestStream, out ITransportHeaders responseHeaders, out Stream responseStream, IClientChannelSinkStack sinkStack)
		{
			this.ProcessMessage(msg, requestHeaders, requestStream, out responseHeaders, out responseStream);
			return sinkStack;
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000D560 File Offset: 0x0000C560
		public void AsyncProcessRequest(IClientChannelSinkStack sinkStack, IMessage msg, ITransportHeaders headers, Stream requestStream)
		{
			AsyncCallback asyncCallback = new AsyncCallback(this.AsyncFinishedCallback);
			AsyncMessageDelegate asyncMessageDelegate = new AsyncMessageDelegate(this.AsyncProcessMessage);
			ITransportHeaders transportHeaders;
			Stream stream;
			asyncMessageDelegate.BeginInvoke(msg, headers, requestStream, out transportHeaders, out stream, sinkStack, asyncCallback, null);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000D59C File Offset: 0x0000C59C
		private void AsyncFinishedCallback(IAsyncResult ar)
		{
			IClientChannelSinkStack clientChannelSinkStack = null;
			try
			{
				AsyncMessageDelegate asyncMessageDelegate = (AsyncMessageDelegate)((AsyncResult)ar).AsyncDelegate;
				ITransportHeaders transportHeaders;
				Stream stream;
				clientChannelSinkStack = asyncMessageDelegate.EndInvoke(out transportHeaders, out stream, ar);
				clientChannelSinkStack.AsyncProcessResponse(transportHeaders, stream);
			}
			catch (Exception ex)
			{
				try
				{
					if (clientChannelSinkStack != null)
					{
						clientChannelSinkStack.DispatchException(ex);
					}
				}
				catch
				{
				}
			}
			catch
			{
				try
				{
					if (clientChannelSinkStack != null)
					{
						clientChannelSinkStack.DispatchException(new Exception(CoreChannel.GetResourceString("Remoting_nonClsCompliantException")));
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000D63C File Offset: 0x0000C63C
		public void AsyncProcessResponse(IClientResponseChannelSinkStack sinkStack, object state, ITransportHeaders headers, Stream stream)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000D643 File Offset: 0x0000C643
		public Stream GetRequestStream(IMessage msg, ITransportHeaders headers)
		{
			return null;
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x0000D646 File Offset: 0x0000C646
		public IClientChannelSink NextChannelSink
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700009F RID: 159
		public override object this[object key]
		{
			get
			{
				string text = key as string;
				if (text == null)
				{
					return null;
				}
				string text2;
				if ((text2 = text.ToLower(CultureInfo.InvariantCulture)) != null)
				{
					if (text2 == "tokenimpersonationlevel")
					{
						return this._tokenImpersonationLevel.ToString();
					}
					if (text2 == "connectiontimeout")
					{
						return this._timeout;
					}
				}
				return null;
			}
			set
			{
				string text = key as string;
				if (text == null)
				{
					return;
				}
				string text2;
				if ((text2 = text.ToLower(CultureInfo.InvariantCulture)) != null)
				{
					if (text2 == "tokenimpersonationlevel")
					{
						this._tokenImpersonationLevel = (TokenImpersonationLevel)((value is TokenImpersonationLevel) ? value : Enum.Parse(typeof(TokenImpersonationLevel), (string)value, true));
						this.authSet = true;
						return;
					}
					if (!(text2 == "connectiontimeout"))
					{
						return;
					}
					this._timeout = Convert.ToInt32(value, CultureInfo.InvariantCulture);
				}
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000D737 File Offset: 0x0000C737
		public override ICollection Keys
		{
			get
			{
				ICollection collection = IpcClientTransportSink.s_keySet;
				return IpcClientTransportSink.s_keySet;
			}
		}

		// Token: 0x040001EB RID: 491
		private const string TokenImpersonationLevelKey = "tokenimpersonationlevel";

		// Token: 0x040001EC RID: 492
		private const string ConnectionTimeoutKey = "connectiontimeout";

		// Token: 0x040001ED RID: 493
		private ConnectionCache portCache = new ConnectionCache();

		// Token: 0x040001EE RID: 494
		private IpcClientChannel _channel;

		// Token: 0x040001EF RID: 495
		private string _portName;

		// Token: 0x040001F0 RID: 496
		private bool authSet;

		// Token: 0x040001F1 RID: 497
		private TokenImpersonationLevel _tokenImpersonationLevel = TokenImpersonationLevel.Identification;

		// Token: 0x040001F2 RID: 498
		private int _timeout = 1000;

		// Token: 0x040001F3 RID: 499
		private static ICollection s_keySet;
	}
}
