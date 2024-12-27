using System;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x0200069C RID: 1692
	internal class ChannelServicesData
	{
		// Token: 0x04001F44 RID: 8004
		internal long remoteCalls;

		// Token: 0x04001F45 RID: 8005
		internal CrossContextChannel xctxmessageSink;

		// Token: 0x04001F46 RID: 8006
		internal CrossAppDomainChannel xadmessageSink;

		// Token: 0x04001F47 RID: 8007
		internal bool fRegisterWellKnownChannels;
	}
}
