using System;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x020001A8 RID: 424
	public interface IDesignerLoaderHost2 : IDesignerLoaderHost, IDesignerHost, IServiceContainer, IServiceProvider
	{
		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000D13 RID: 3347
		// (set) Token: 0x06000D14 RID: 3348
		bool IgnoreErrorsDuringReload { get; set; }

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000D15 RID: 3349
		// (set) Token: 0x06000D16 RID: 3350
		bool CanReloadWithErrors { get; set; }
	}
}
