using System;
using System.Drawing;

namespace System.Windows.Forms.Design.Behavior
{
	public sealed class Adorner
	{
		public Adorner()
		{
			this.glyphs = new GlyphCollection();
			this.enabled = true;
		}

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

		public GlyphCollection Glyphs
		{
			get
			{
				return this.glyphs;
			}
		}

		public void Invalidate()
		{
			if (this.behaviorService != null)
			{
				this.behaviorService.Invalidate();
			}
		}

		public void Invalidate(Rectangle rectangle)
		{
			if (this.behaviorService != null)
			{
				this.behaviorService.Invalidate(rectangle);
			}
		}

		public void Invalidate(Region region)
		{
			if (this.behaviorService != null)
			{
				this.behaviorService.Invalidate(region);
			}
		}

		private BehaviorService behaviorService;

		private GlyphCollection glyphs;

		private bool enabled;
	}
}
