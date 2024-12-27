using System;

namespace System.Net
{
	// Token: 0x02000509 RID: 1289
	internal class StaticProxy : ProxyChain
	{
		// Token: 0x060027FF RID: 10239 RVA: 0x000A4EDA File Offset: 0x000A3EDA
		internal StaticProxy(Uri destination, Uri proxy)
			: base(destination)
		{
			if (proxy == null)
			{
				throw new ArgumentNullException("proxy");
			}
			this.m_Proxy = proxy;
		}

		// Token: 0x06002800 RID: 10240 RVA: 0x000A4EFE File Offset: 0x000A3EFE
		protected override bool GetNextProxy(out Uri proxy)
		{
			proxy = this.m_Proxy;
			if (proxy == null)
			{
				return false;
			}
			this.m_Proxy = null;
			return true;
		}

		// Token: 0x0400274A RID: 10058
		private Uri m_Proxy;
	}
}
