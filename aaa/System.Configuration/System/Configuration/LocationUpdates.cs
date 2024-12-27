using System;

namespace System.Configuration
{
	// Token: 0x02000077 RID: 119
	internal class LocationUpdates
	{
		// Token: 0x06000455 RID: 1109 RVA: 0x00014425 File Offset: 0x00013425
		internal LocationUpdates(OverrideModeSetting overrideMode, bool inheritInChildApps)
		{
			this._overrideMode = overrideMode;
			this._inheritInChildApps = inheritInChildApps;
			this._sectionUpdates = new SectionUpdates(string.Empty);
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000456 RID: 1110 RVA: 0x0001444B File Offset: 0x0001344B
		internal OverrideModeSetting OverrideMode
		{
			get
			{
				return this._overrideMode;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x00014453 File Offset: 0x00013453
		internal bool InheritInChildApps
		{
			get
			{
				return this._inheritInChildApps;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000458 RID: 1112 RVA: 0x0001445B File Offset: 0x0001345B
		internal SectionUpdates SectionUpdates
		{
			get
			{
				return this._sectionUpdates;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x00014463 File Offset: 0x00013463
		internal bool IsDefault
		{
			get
			{
				return this._overrideMode.IsDefaultForLocationTag && this._inheritInChildApps;
			}
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x0001447A File Offset: 0x0001347A
		internal void CompleteUpdates()
		{
			this._sectionUpdates.CompleteUpdates();
		}

		// Token: 0x04000331 RID: 817
		private OverrideModeSetting _overrideMode;

		// Token: 0x04000332 RID: 818
		private bool _inheritInChildApps;

		// Token: 0x04000333 RID: 819
		private SectionUpdates _sectionUpdates;
	}
}
