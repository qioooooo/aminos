using System;
using System.Runtime.InteropServices;

namespace System.Configuration.Internal
{
	// Token: 0x020000BD RID: 189
	[ComVisible(false)]
	public interface IInternalConfigRoot
	{
		// Token: 0x0600070E RID: 1806
		void Init(IInternalConfigHost host, bool isDesignTime);

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x0600070F RID: 1807
		bool IsDesignTime { get; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000710 RID: 1808
		// (remove) Token: 0x06000711 RID: 1809
		event InternalConfigEventHandler ConfigChanged;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000712 RID: 1810
		// (remove) Token: 0x06000713 RID: 1811
		event InternalConfigEventHandler ConfigRemoved;

		// Token: 0x06000714 RID: 1812
		object GetSection(string section, string configPath);

		// Token: 0x06000715 RID: 1813
		string GetUniqueConfigPath(string configPath);

		// Token: 0x06000716 RID: 1814
		IInternalConfigRecord GetUniqueConfigRecord(string configPath);

		// Token: 0x06000717 RID: 1815
		IInternalConfigRecord GetConfigRecord(string configPath);

		// Token: 0x06000718 RID: 1816
		void RemoveConfig(string configPath);
	}
}
