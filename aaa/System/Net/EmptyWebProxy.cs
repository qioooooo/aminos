using System;

namespace System.Net
{
	// Token: 0x020004D9 RID: 1241
	[Serializable]
	internal sealed class EmptyWebProxy : IAutoWebProxy, IWebProxy
	{
		// Token: 0x06002695 RID: 9877 RVA: 0x0009DDA2 File Offset: 0x0009CDA2
		public Uri GetProxy(Uri uri)
		{
			return uri;
		}

		// Token: 0x06002696 RID: 9878 RVA: 0x0009DDA5 File Offset: 0x0009CDA5
		public bool IsBypassed(Uri uri)
		{
			return true;
		}

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06002697 RID: 9879 RVA: 0x0009DDA8 File Offset: 0x0009CDA8
		// (set) Token: 0x06002698 RID: 9880 RVA: 0x0009DDB0 File Offset: 0x0009CDB0
		public ICredentials Credentials
		{
			get
			{
				return this.m_credentials;
			}
			set
			{
				this.m_credentials = value;
			}
		}

		// Token: 0x06002699 RID: 9881 RVA: 0x0009DDB9 File Offset: 0x0009CDB9
		ProxyChain IAutoWebProxy.GetProxies(Uri destination)
		{
			return new DirectProxy(destination);
		}

		// Token: 0x0400262E RID: 9774
		[NonSerialized]
		private ICredentials m_credentials;
	}
}
