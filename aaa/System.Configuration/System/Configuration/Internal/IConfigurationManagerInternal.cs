using System;
using System.Runtime.InteropServices;

namespace System.Configuration.Internal
{
	// Token: 0x020000B8 RID: 184
	[ComVisible(false)]
	public interface IConfigurationManagerInternal
	{
		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060006F1 RID: 1777
		bool SupportsUserConfig { get; }

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x060006F2 RID: 1778
		bool SetConfigurationSystemInProgress { get; }

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x060006F3 RID: 1779
		string MachineConfigPath { get; }

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x060006F4 RID: 1780
		string ApplicationConfigUri { get; }

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x060006F5 RID: 1781
		string ExeProductName { get; }

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x060006F6 RID: 1782
		string ExeProductVersion { get; }

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x060006F7 RID: 1783
		string ExeRoamingConfigDirectory { get; }

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x060006F8 RID: 1784
		string ExeRoamingConfigPath { get; }

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x060006F9 RID: 1785
		string ExeLocalConfigDirectory { get; }

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x060006FA RID: 1786
		string ExeLocalConfigPath { get; }

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x060006FB RID: 1787
		string UserConfigFilename { get; }
	}
}
