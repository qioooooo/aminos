using System;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Channels;

namespace System.Runtime.Remoting
{
	// Token: 0x020006AD RID: 1709
	internal class DelayLoadClientChannelEntry
	{
		// Token: 0x06003E1C RID: 15900 RVA: 0x000D5FC2 File Offset: 0x000D4FC2
		internal DelayLoadClientChannelEntry(RemotingXmlConfigFileData.ChannelEntry entry, bool ensureSecurity)
		{
			this._entry = entry;
			this._channel = null;
			this._bRegistered = false;
			this._ensureSecurity = ensureSecurity;
		}

		// Token: 0x17000A62 RID: 2658
		// (get) Token: 0x06003E1D RID: 15901 RVA: 0x000D5FE6 File Offset: 0x000D4FE6
		internal IChannelSender Channel
		{
			get
			{
				if (this._channel == null && !this._bRegistered)
				{
					this._channel = (IChannelSender)RemotingConfigHandler.CreateChannelFromConfigEntry(this._entry);
					this._entry = null;
				}
				return this._channel;
			}
		}

		// Token: 0x06003E1E RID: 15902 RVA: 0x000D601B File Offset: 0x000D501B
		internal void RegisterChannel()
		{
			ChannelServices.RegisterChannel(this._channel, this._ensureSecurity);
			this._bRegistered = true;
			this._channel = null;
		}

		// Token: 0x04001F79 RID: 8057
		private RemotingXmlConfigFileData.ChannelEntry _entry;

		// Token: 0x04001F7A RID: 8058
		private IChannelSender _channel;

		// Token: 0x04001F7B RID: 8059
		private bool _bRegistered;

		// Token: 0x04001F7C RID: 8060
		private bool _ensureSecurity;
	}
}
