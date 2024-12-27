using System;

namespace System.Windows.Forms
{
	// Token: 0x02000321 RID: 801
	internal class DataGridViewCellStyleChangedEventArgs : EventArgs
	{
		// Token: 0x060033B4 RID: 13236 RVA: 0x000B573B File Offset: 0x000B473B
		internal DataGridViewCellStyleChangedEventArgs()
		{
		}

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x060033B5 RID: 13237 RVA: 0x000B5743 File Offset: 0x000B4743
		// (set) Token: 0x060033B6 RID: 13238 RVA: 0x000B574B File Offset: 0x000B474B
		internal bool ChangeAffectsPreferredSize
		{
			get
			{
				return this.changeAffectsPreferredSize;
			}
			set
			{
				this.changeAffectsPreferredSize = value;
			}
		}

		// Token: 0x04001AE8 RID: 6888
		private bool changeAffectsPreferredSize;
	}
}
