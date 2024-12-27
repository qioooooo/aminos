using System;

namespace System.Windows.Forms
{
	// Token: 0x0200049C RID: 1180
	public class ListViewVirtualItemsSelectionRangeChangedEventArgs : EventArgs
	{
		// Token: 0x0600467A RID: 18042 RVA: 0x000FFD6B File Offset: 0x000FED6B
		public ListViewVirtualItemsSelectionRangeChangedEventArgs(int startIndex, int endIndex, bool isSelected)
		{
			if (startIndex > endIndex)
			{
				throw new ArgumentException(SR.GetString("ListViewStartIndexCannotBeLargerThanEndIndex"));
			}
			this.startIndex = startIndex;
			this.endIndex = endIndex;
			this.isSelected = isSelected;
		}

		// Token: 0x17000DFE RID: 3582
		// (get) Token: 0x0600467B RID: 18043 RVA: 0x000FFD9C File Offset: 0x000FED9C
		public int EndIndex
		{
			get
			{
				return this.endIndex;
			}
		}

		// Token: 0x17000DFF RID: 3583
		// (get) Token: 0x0600467C RID: 18044 RVA: 0x000FFDA4 File Offset: 0x000FEDA4
		public bool IsSelected
		{
			get
			{
				return this.isSelected;
			}
		}

		// Token: 0x17000E00 RID: 3584
		// (get) Token: 0x0600467D RID: 18045 RVA: 0x000FFDAC File Offset: 0x000FEDAC
		public int StartIndex
		{
			get
			{
				return this.startIndex;
			}
		}

		// Token: 0x040021A6 RID: 8614
		private int startIndex;

		// Token: 0x040021A7 RID: 8615
		private int endIndex;

		// Token: 0x040021A8 RID: 8616
		private bool isSelected;
	}
}
