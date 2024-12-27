using System;
using System.Runtime.InteropServices;

namespace System.Configuration.Internal
{
	// Token: 0x0200001A RID: 26
	[ComVisible(false)]
	public interface IInternalConfigClientHost
	{
		// Token: 0x06000176 RID: 374
		bool IsExeConfig(string configPath);

		// Token: 0x06000177 RID: 375
		bool IsRoamingUserConfig(string configPath);

		// Token: 0x06000178 RID: 376
		bool IsLocalUserConfig(string configPath);

		// Token: 0x06000179 RID: 377
		string GetExeConfigPath();

		// Token: 0x0600017A RID: 378
		string GetRoamingUserConfigPath();

		// Token: 0x0600017B RID: 379
		string GetLocalUserConfigPath();
	}
}
