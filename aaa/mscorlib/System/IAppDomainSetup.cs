using System;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x0200005C RID: 92
	[Guid("27FFF232-A7A8-40dd-8D4A-734AD59FCD41")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComVisible(true)]
	public interface IAppDomainSetup
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600055B RID: 1371
		// (set) Token: 0x0600055C RID: 1372
		string ApplicationBase { get; set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600055D RID: 1373
		// (set) Token: 0x0600055E RID: 1374
		string ApplicationName { get; set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600055F RID: 1375
		// (set) Token: 0x06000560 RID: 1376
		string CachePath { get; set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000561 RID: 1377
		// (set) Token: 0x06000562 RID: 1378
		string ConfigurationFile { get; set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000563 RID: 1379
		// (set) Token: 0x06000564 RID: 1380
		string DynamicBase { get; set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000565 RID: 1381
		// (set) Token: 0x06000566 RID: 1382
		string LicenseFile { get; set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000567 RID: 1383
		// (set) Token: 0x06000568 RID: 1384
		string PrivateBinPath { get; set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000569 RID: 1385
		// (set) Token: 0x0600056A RID: 1386
		string PrivateBinPathProbe { get; set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x0600056B RID: 1387
		// (set) Token: 0x0600056C RID: 1388
		string ShadowCopyDirectories { get; set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600056D RID: 1389
		// (set) Token: 0x0600056E RID: 1390
		string ShadowCopyFiles { get; set; }
	}
}
