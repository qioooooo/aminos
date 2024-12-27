using System;

namespace System.Net
{
	// Token: 0x02000508 RID: 1288
	internal class DirectProxy : ProxyChain
	{
		// Token: 0x060027FD RID: 10237 RVA: 0x000A4EBA File Offset: 0x000A3EBA
		internal DirectProxy(Uri destination)
			: base(destination)
		{
		}

		// Token: 0x060027FE RID: 10238 RVA: 0x000A4EC3 File Offset: 0x000A3EC3
		protected override bool GetNextProxy(out Uri proxy)
		{
			proxy = null;
			if (this.m_ProxyRetrieved)
			{
				return false;
			}
			this.m_ProxyRetrieved = true;
			return true;
		}

		// Token: 0x04002749 RID: 10057
		private bool m_ProxyRetrieved;
	}
}
