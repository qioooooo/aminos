using System;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	// Token: 0x020002E2 RID: 738
	public sealed class Adorner
	{
		// Token: 0x06001C58 RID: 7256 RVA: 0x0009F735 File Offset: 0x0009E735
		public Adorner()
		{
			this.glyphs = new GlyphCollection();
			this.enabled = true;
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06001C59 RID: 7257 RVA: 0x0009F74F File Offset: 0x0009E74F
		// (set) Token: 0x06001C5A RID: 7258 RVA: 0x0009F757 File Offset: 0x0009E757
		public BehaviorService BehaviorService
		{
			get
			{
				return this.behaviorService;
			}
			set
			{
				this.behaviorService = value;
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06001C5B RID: 7259 RVA: 0x0009F760 File Offset: 0x0009E760
		// (set) Token: 0x06001C5C RID: 7260 RVA: 0x0009F768 File Offset: 0x0009E768
		public bool Enabled
		{
			get
			{
				return this.enabled;
			}
			set
			{
				if (value != this.enabled)
				{
					this.enabled = value;
					this.Invalidate();
				}
			}
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06001C5D RID: 7261 RVA: 0x0009F780 File Offset: 0x0009E780
		public GlyphCollection Glyphs
		{
			get
			{
				return this.glyphs;
			}
		}

		// Token: 0x06001C5E RID: 7262 RVA: 0x0009F788 File Offset: 0x0009E788
		public void Invalidate()
		{
			if (this.behaviorService != null)
			{
				this.behaviorService.Invalidate();
			}
		}

		// Token: 0x06001C5F RID: 7263 RVA: 0x0009F79D File Offset: 0x0009E79D
		public void Invalidate(Rectangle rectangle)
		{
			if (this.behaviorService != null)
			{
				this.behaviorService.Invalidate(rectangle);
			}
		}

		// Token: 0x06001C60 RID: 7264 RVA: 0x0009F7B3 File Offset: 0x0009E7B3
		public void Invalidate(Region region)
		{
			if (this.behaviorService != null)
			{
				this.behaviorService.Invalidate(region);
			}
		}

		// Token: 0x040015E0 RID: 5600
		private BehaviorService behaviorService;

		// Token: 0x040015E1 RID: 5601
		private GlyphCollection glyphs;

		// Token: 0x040015E2 RID: 5602
		private bool enabled;
	}
}
