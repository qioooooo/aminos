using System;

namespace System.ComponentModel.Design
{
	// Token: 0x02000185 RID: 389
	public interface IExtenderProviderService
	{
		// Token: 0x06000C73 RID: 3187
		void AddExtenderProvider(IExtenderProvider provider);

		// Token: 0x06000C74 RID: 3188
		void RemoveExtenderProvider(IExtenderProvider provider);
	}
}
