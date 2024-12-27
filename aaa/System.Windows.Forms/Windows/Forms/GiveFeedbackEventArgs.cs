using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x02000416 RID: 1046
	[ComVisible(true)]
	public class GiveFeedbackEventArgs : EventArgs
	{
		// Token: 0x06003E56 RID: 15958 RVA: 0x000E32D8 File Offset: 0x000E22D8
		public GiveFeedbackEventArgs(DragDropEffects effect, bool useDefaultCursors)
		{
			this.effect = effect;
			this.useDefaultCursors = useDefaultCursors;
		}

		// Token: 0x17000BED RID: 3053
		// (get) Token: 0x06003E57 RID: 15959 RVA: 0x000E32EE File Offset: 0x000E22EE
		public DragDropEffects Effect
		{
			get
			{
				return this.effect;
			}
		}

		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x06003E58 RID: 15960 RVA: 0x000E32F6 File Offset: 0x000E22F6
		// (set) Token: 0x06003E59 RID: 15961 RVA: 0x000E32FE File Offset: 0x000E22FE
		public bool UseDefaultCursors
		{
			get
			{
				return this.useDefaultCursors;
			}
			set
			{
				this.useDefaultCursors = value;
			}
		}

		// Token: 0x04001EC0 RID: 7872
		private readonly DragDropEffects effect;

		// Token: 0x04001EC1 RID: 7873
		private bool useDefaultCursors;
	}
}
