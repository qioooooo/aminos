using System;
using System.Runtime.InteropServices;

namespace System.Configuration.Internal
{
	// Token: 0x0200001C RID: 28
	[ComVisible(false)]
	public interface IInternalConfigSystem
	{
		// Token: 0x060001A4 RID: 420
		object GetSection(string configKey);

		// Token: 0x060001A5 RID: 421
		void RefreshConfig(string sectionName);

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001A6 RID: 422
		bool SupportsUserConfig { get; }
	}
}
