using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x02000475 RID: 1141
	[ComVisible(true)]
	public class LinkLabelLinkClickedEventArgs : EventArgs
	{
		// Token: 0x0600433A RID: 17210 RVA: 0x000F0971 File Offset: 0x000EF971
		public LinkLabelLinkClickedEventArgs(LinkLabel.Link link)
		{
			this.link = link;
			this.button = MouseButtons.Left;
		}

		// Token: 0x0600433B RID: 17211 RVA: 0x000F098B File Offset: 0x000EF98B
		public LinkLabelLinkClickedEventArgs(LinkLabel.Link link, MouseButtons button)
			: this(link)
		{
			this.button = button;
		}

		// Token: 0x17000D2F RID: 3375
		// (get) Token: 0x0600433C RID: 17212 RVA: 0x000F099B File Offset: 0x000EF99B
		public MouseButtons Button
		{
			get
			{
				return this.button;
			}
		}

		// Token: 0x17000D30 RID: 3376
		// (get) Token: 0x0600433D RID: 17213 RVA: 0x000F09A3 File Offset: 0x000EF9A3
		public LinkLabel.Link Link
		{
			get
			{
				return this.link;
			}
		}

		// Token: 0x040020C9 RID: 8393
		private readonly LinkLabel.Link link;

		// Token: 0x040020CA RID: 8394
		private readonly MouseButtons button;
	}
}
