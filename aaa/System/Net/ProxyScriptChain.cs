using System;

namespace System.Net
{
	// Token: 0x02000507 RID: 1287
	internal class ProxyScriptChain : ProxyChain
	{
		// Token: 0x060027FA RID: 10234 RVA: 0x000A4E06 File Offset: 0x000A3E06
		internal ProxyScriptChain(WebProxy proxy, Uri destination)
			: base(destination)
		{
			this.m_Proxy = proxy;
		}

		// Token: 0x060027FB RID: 10235 RVA: 0x000A4E18 File Offset: 0x000A3E18
		protected override bool GetNextProxy(out Uri proxy)
		{
			if (this.m_CurrentIndex < 0)
			{
				proxy = null;
				return false;
			}
			if (this.m_CurrentIndex == 0)
			{
				this.m_ScriptProxies = this.m_Proxy.GetProxiesAuto(base.Destination, ref this.m_SyncStatus);
			}
			if (this.m_ScriptProxies == null || this.m_CurrentIndex >= this.m_ScriptProxies.Length)
			{
				proxy = this.m_Proxy.GetProxyAutoFailover(base.Destination);
				this.m_CurrentIndex = -1;
				return true;
			}
			proxy = this.m_ScriptProxies[this.m_CurrentIndex++];
			return true;
		}

		// Token: 0x060027FC RID: 10236 RVA: 0x000A4EA7 File Offset: 0x000A3EA7
		internal override void Abort()
		{
			this.m_Proxy.AbortGetProxiesAuto(ref this.m_SyncStatus);
		}

		// Token: 0x04002745 RID: 10053
		private WebProxy m_Proxy;

		// Token: 0x04002746 RID: 10054
		private Uri[] m_ScriptProxies;

		// Token: 0x04002747 RID: 10055
		private int m_CurrentIndex;

		// Token: 0x04002748 RID: 10056
		private int m_SyncStatus;
	}
}
