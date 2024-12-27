using System;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Remoting
{
	// Token: 0x02000720 RID: 1824
	[Serializable]
	internal sealed class EnvoyInfo : IEnvoyInfo
	{
		// Token: 0x060041BC RID: 16828 RVA: 0x000E05B0 File Offset: 0x000DF5B0
		internal static IEnvoyInfo CreateEnvoyInfo(ServerIdentity serverID)
		{
			IEnvoyInfo envoyInfo = null;
			if (serverID != null)
			{
				if (serverID.EnvoyChain == null)
				{
					serverID.RaceSetEnvoyChain(serverID.ServerContext.CreateEnvoyChain(serverID.TPOrObject));
				}
				if (!(serverID.EnvoyChain is EnvoyTerminatorSink))
				{
					envoyInfo = new EnvoyInfo(serverID.EnvoyChain);
				}
			}
			return envoyInfo;
		}

		// Token: 0x060041BD RID: 16829 RVA: 0x000E05FE File Offset: 0x000DF5FE
		private EnvoyInfo(IMessageSink sinks)
		{
			this.EnvoySinks = sinks;
		}

		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x060041BE RID: 16830 RVA: 0x000E060D File Offset: 0x000DF60D
		// (set) Token: 0x060041BF RID: 16831 RVA: 0x000E0615 File Offset: 0x000DF615
		public IMessageSink EnvoySinks
		{
			get
			{
				return this.envoySinks;
			}
			set
			{
				this.envoySinks = value;
			}
		}

		// Token: 0x040020CA RID: 8394
		private IMessageSink envoySinks;
	}
}
