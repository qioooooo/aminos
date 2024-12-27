using System;
using System.Configuration;
using System.Runtime.InteropServices;

namespace System.Web.UI.Design
{
	// Token: 0x02000384 RID: 900
	[Guid("cff39fa8-5607-4b6d-86f3-cc80b3cfe2dd")]
	public interface IWebApplication : IServiceProvider
	{
		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x06002154 RID: 8532
		IProjectItem RootProjectItem { get; }

		// Token: 0x06002155 RID: 8533
		IProjectItem GetProjectItemFromUrl(string appRelativeUrl);

		// Token: 0x06002156 RID: 8534
		Configuration OpenWebConfiguration(bool isReadOnly);
	}
}
