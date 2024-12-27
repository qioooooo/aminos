using System;

namespace System.Net
{
	// Token: 0x020003C5 RID: 965
	[Obsolete("This class has been deprecated. Please use WebRequest.DefaultWebProxy instead to access and set the global default proxy. Use 'null' instead of GetEmptyWebProxy. http://go.microsoft.com/fwlink/?linkid=14202")]
	public class GlobalProxySelection
	{
		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06001E5C RID: 7772 RVA: 0x00074594 File Offset: 0x00073594
		// (set) Token: 0x06001E5D RID: 7773 RVA: 0x000745C2 File Offset: 0x000735C2
		public static IWebProxy Select
		{
			get
			{
				IWebProxy defaultWebProxy = WebRequest.DefaultWebProxy;
				if (defaultWebProxy == null)
				{
					return GlobalProxySelection.GetEmptyWebProxy();
				}
				WebRequest.WebProxyWrapper webProxyWrapper = defaultWebProxy as WebRequest.WebProxyWrapper;
				if (webProxyWrapper != null)
				{
					return webProxyWrapper.WebProxy;
				}
				return defaultWebProxy;
			}
			set
			{
				WebRequest.DefaultWebProxy = value;
			}
		}

		// Token: 0x06001E5E RID: 7774 RVA: 0x000745CA File Offset: 0x000735CA
		public static IWebProxy GetEmptyWebProxy()
		{
			return new EmptyWebProxy();
		}
	}
}
