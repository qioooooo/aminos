using System;

namespace System.Net
{
	// Token: 0x020004C6 RID: 1222
	internal struct TunnelStateObject
	{
		// Token: 0x060025B5 RID: 9653 RVA: 0x000962AC File Offset: 0x000952AC
		internal TunnelStateObject(HttpWebRequest r, Connection c)
		{
			this.Connection = c;
			this.OriginalRequest = r;
		}

		// Token: 0x04002571 RID: 9585
		internal Connection Connection;

		// Token: 0x04002572 RID: 9586
		internal HttpWebRequest OriginalRequest;
	}
}
