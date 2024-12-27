using System;
using System.Drawing;

namespace System.Windows.Forms
{
	// Token: 0x020001DB RID: 475
	public sealed class AmbientProperties
	{
		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06001281 RID: 4737 RVA: 0x00010E34 File Offset: 0x0000FE34
		// (set) Token: 0x06001282 RID: 4738 RVA: 0x00010E3C File Offset: 0x0000FE3C
		public Color BackColor
		{
			get
			{
				return this.backColor;
			}
			set
			{
				this.backColor = value;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06001283 RID: 4739 RVA: 0x00010E45 File Offset: 0x0000FE45
		// (set) Token: 0x06001284 RID: 4740 RVA: 0x00010E4D File Offset: 0x0000FE4D
		public Cursor Cursor
		{
			get
			{
				return this.cursor;
			}
			set
			{
				this.cursor = value;
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06001285 RID: 4741 RVA: 0x00010E56 File Offset: 0x0000FE56
		// (set) Token: 0x06001286 RID: 4742 RVA: 0x00010E5E File Offset: 0x0000FE5E
		public Font Font
		{
			get
			{
				return this.font;
			}
			set
			{
				this.font = value;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06001287 RID: 4743 RVA: 0x00010E67 File Offset: 0x0000FE67
		// (set) Token: 0x06001288 RID: 4744 RVA: 0x00010E6F File Offset: 0x0000FE6F
		public Color ForeColor
		{
			get
			{
				return this.foreColor;
			}
			set
			{
				this.foreColor = value;
			}
		}

		// Token: 0x0400101B RID: 4123
		private Color backColor;

		// Token: 0x0400101C RID: 4124
		private Color foreColor;

		// Token: 0x0400101D RID: 4125
		private Cursor cursor;

		// Token: 0x0400101E RID: 4126
		private Font font;
	}
}
