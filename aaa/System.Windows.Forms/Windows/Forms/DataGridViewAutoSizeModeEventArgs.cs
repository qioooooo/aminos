using System;

namespace System.Windows.Forms
{
	// Token: 0x02000304 RID: 772
	public class DataGridViewAutoSizeModeEventArgs : EventArgs
	{
		// Token: 0x06003145 RID: 12613 RVA: 0x000A9865 File Offset: 0x000A8865
		public DataGridViewAutoSizeModeEventArgs(bool previousModeAutoSized)
		{
			this.previousModeAutoSized = previousModeAutoSized;
		}

		// Token: 0x17000868 RID: 2152
		// (get) Token: 0x06003146 RID: 12614 RVA: 0x000A9874 File Offset: 0x000A8874
		public bool PreviousModeAutoSized
		{
			get
			{
				return this.previousModeAutoSized;
			}
		}

		// Token: 0x04001A40 RID: 6720
		private bool previousModeAutoSized;
	}
}
