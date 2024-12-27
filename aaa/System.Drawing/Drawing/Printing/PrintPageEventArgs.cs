using System;

namespace System.Drawing.Printing
{
	// Token: 0x02000126 RID: 294
	public class PrintPageEventArgs : EventArgs
	{
		// Token: 0x06000F4D RID: 3917 RVA: 0x0002DC2F File Offset: 0x0002CC2F
		public PrintPageEventArgs(Graphics graphics, Rectangle marginBounds, Rectangle pageBounds, PageSettings pageSettings)
		{
			this.graphics = graphics;
			this.marginBounds = marginBounds;
			this.pageBounds = pageBounds;
			this.pageSettings = pageSettings;
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06000F4E RID: 3918 RVA: 0x0002DC54 File Offset: 0x0002CC54
		// (set) Token: 0x06000F4F RID: 3919 RVA: 0x0002DC5C File Offset: 0x0002CC5C
		public bool Cancel
		{
			get
			{
				return this.cancel;
			}
			set
			{
				this.cancel = value;
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06000F50 RID: 3920 RVA: 0x0002DC65 File Offset: 0x0002CC65
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06000F51 RID: 3921 RVA: 0x0002DC6D File Offset: 0x0002CC6D
		// (set) Token: 0x06000F52 RID: 3922 RVA: 0x0002DC75 File Offset: 0x0002CC75
		public bool HasMorePages
		{
			get
			{
				return this.hasMorePages;
			}
			set
			{
				this.hasMorePages = value;
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06000F53 RID: 3923 RVA: 0x0002DC7E File Offset: 0x0002CC7E
		public Rectangle MarginBounds
		{
			get
			{
				return this.marginBounds;
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06000F54 RID: 3924 RVA: 0x0002DC86 File Offset: 0x0002CC86
		public Rectangle PageBounds
		{
			get
			{
				return this.pageBounds;
			}
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06000F55 RID: 3925 RVA: 0x0002DC8E File Offset: 0x0002CC8E
		public PageSettings PageSettings
		{
			get
			{
				return this.pageSettings;
			}
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x0002DC96 File Offset: 0x0002CC96
		internal void Dispose()
		{
			this.graphics.Dispose();
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x0002DCA3 File Offset: 0x0002CCA3
		internal void SetGraphics(Graphics value)
		{
			this.graphics = value;
		}

		// Token: 0x04000C6E RID: 3182
		private bool hasMorePages;

		// Token: 0x04000C6F RID: 3183
		private bool cancel;

		// Token: 0x04000C70 RID: 3184
		private Graphics graphics;

		// Token: 0x04000C71 RID: 3185
		private readonly Rectangle marginBounds;

		// Token: 0x04000C72 RID: 3186
		private readonly Rectangle pageBounds;

		// Token: 0x04000C73 RID: 3187
		private readonly PageSettings pageSettings;
	}
}
