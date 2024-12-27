using System;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting.Channels.Ipc
{
	// Token: 0x02000052 RID: 82
	// (Invoke) Token: 0x0600029C RID: 668
	internal delegate IClientChannelSinkStack AsyncMessageDelegate(IMessage msg, ITransportHeaders requestHeaders, Stream requestStream, out ITransportHeaders responseHeaders, out Stream responseStream, IClientChannelSinkStack sinkStack);
}
