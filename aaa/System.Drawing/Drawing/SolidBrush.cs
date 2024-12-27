using System;
using System.Drawing.Internal;
using System.Runtime.InteropServices;

namespace System.Drawing
{
	// Token: 0x02000062 RID: 98
	public sealed class SolidBrush : Brush, ISystemColorTracker
	{
		// Token: 0x0600062B RID: 1579 RVA: 0x00019A6C File Offset: 0x00018A6C
		public SolidBrush(Color color)
		{
			this.color = color;
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateSolidFill(this.color.ToArgb(), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeBrushInternal(zero);
			if (color.IsSystemColor)
			{
				SystemColorTracker.Add(this);
			}
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00019ACA File Offset: 0x00018ACA
		internal SolidBrush(Color color, bool immutable)
			: this(color)
		{
			this.immutable = immutable;
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x00019ADA File Offset: 0x00018ADA
		internal SolidBrush(IntPtr nativeBrush)
		{
			base.SetNativeBrushInternal(nativeBrush);
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00019AF4 File Offset: 0x00018AF4
		public override object Clone()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCloneBrush(new HandleRef(this, base.NativeBrush), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new SolidBrush(zero);
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00019B2C File Offset: 0x00018B2C
		protected override void Dispose(bool disposing)
		{
			if (!disposing)
			{
				this.immutable = false;
			}
			else if (this.immutable)
			{
				throw new ArgumentException(SR.GetString("CantChangeImmutableObjects", new object[] { "Brush" }));
			}
			base.Dispose(disposing);
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x00019B74 File Offset: 0x00018B74
		// (set) Token: 0x06000631 RID: 1585 RVA: 0x00019BC8 File Offset: 0x00018BC8
		public Color Color
		{
			get
			{
				if (this.color == Color.Empty)
				{
					int num = 0;
					int num2 = SafeNativeMethods.Gdip.GdipGetSolidFillColor(new HandleRef(this, base.NativeBrush), out num);
					if (num2 != 0)
					{
						throw SafeNativeMethods.Gdip.StatusException(num2);
					}
					this.color = Color.FromArgb(num);
				}
				return this.color;
			}
			set
			{
				if (this.immutable)
				{
					throw new ArgumentException(SR.GetString("CantChangeImmutableObjects", new object[] { "Brush" }));
				}
				if (this.color != value)
				{
					Color color = this.color;
					this.InternalSetColor(value);
					if (value.IsSystemColor && !color.IsSystemColor)
					{
						SystemColorTracker.Add(this);
					}
				}
			}
		}

		// Token: 0x06000632 RID: 1586 RVA: 0x00019C34 File Offset: 0x00018C34
		private void InternalSetColor(Color value)
		{
			int num = SafeNativeMethods.Gdip.GdipSetSolidFillColor(new HandleRef(this, base.NativeBrush), value.ToArgb());
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.color = value;
		}

		// Token: 0x06000633 RID: 1587 RVA: 0x00019C6B File Offset: 0x00018C6B
		void ISystemColorTracker.OnSystemColorChanged()
		{
			if (base.NativeBrush != IntPtr.Zero)
			{
				this.InternalSetColor(this.color);
			}
		}

		// Token: 0x0400047D RID: 1149
		private Color color = Color.Empty;

		// Token: 0x0400047E RID: 1150
		private bool immutable;
	}
}
