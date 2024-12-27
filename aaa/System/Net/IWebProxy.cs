using System;

namespace System.Net
{
	// Token: 0x020003AD RID: 941
	public interface IWebProxy
	{
		// Token: 0x06001D90 RID: 7568
		Uri GetProxy(Uri destination);

		// Token: 0x06001D91 RID: 7569
		bool IsBypassed(Uri host);

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06001D92 RID: 7570
		// (set) Token: 0x06001D93 RID: 7571
		ICredentials Credentials { get; set; }
	}
}
