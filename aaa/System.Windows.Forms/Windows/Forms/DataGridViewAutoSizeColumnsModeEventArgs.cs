using System;

namespace System.Windows.Forms
{
	// Token: 0x020002FA RID: 762
	public class DataGridViewAutoSizeColumnsModeEventArgs : EventArgs
	{
		// Token: 0x06003143 RID: 12611 RVA: 0x000A984E File Offset: 0x000A884E
		public DataGridViewAutoSizeColumnsModeEventArgs(DataGridViewAutoSizeColumnMode[] previousModes)
		{
			this.previousModes = previousModes;
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x06003144 RID: 12612 RVA: 0x000A985D File Offset: 0x000A885D
		public DataGridViewAutoSizeColumnMode[] PreviousModes
		{
			get
			{
				return this.previousModes;
			}
		}

		// Token: 0x04001A08 RID: 6664
		private DataGridViewAutoSizeColumnMode[] previousModes;
	}
}
