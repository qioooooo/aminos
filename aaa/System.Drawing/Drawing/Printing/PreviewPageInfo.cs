using System;

namespace System.Drawing.Printing
{
	// Token: 0x02000113 RID: 275
	public sealed class PreviewPageInfo
	{
		// Token: 0x06000E80 RID: 3712 RVA: 0x0002B11D File Offset: 0x0002A11D
		public PreviewPageInfo(Image image, Size physicalSize)
		{
			this.image = image;
			this.physicalSize = physicalSize;
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06000E81 RID: 3713 RVA: 0x0002B13E File Offset: 0x0002A13E
		public Image Image
		{
			get
			{
				return this.image;
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06000E82 RID: 3714 RVA: 0x0002B146 File Offset: 0x0002A146
		public Size PhysicalSize
		{
			get
			{
				return this.physicalSize;
			}
		}

		// Token: 0x04000C28 RID: 3112
		private Image image;

		// Token: 0x04000C29 RID: 3113
		private Size physicalSize = Size.Empty;
	}
}
