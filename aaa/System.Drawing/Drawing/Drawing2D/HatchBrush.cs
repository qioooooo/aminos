using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Drawing2D
{
	// Token: 0x020000B8 RID: 184
	public sealed class HatchBrush : Brush
	{
		// Token: 0x06000B66 RID: 2918 RVA: 0x000223A7 File Offset: 0x000213A7
		public HatchBrush(HatchStyle hatchstyle, Color foreColor)
			: this(hatchstyle, foreColor, Color.FromArgb(-16777216))
		{
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x000223BC File Offset: 0x000213BC
		public HatchBrush(HatchStyle hatchstyle, Color foreColor, Color backColor)
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateHatchBrush((int)hatchstyle, foreColor.ToArgb(), backColor.ToArgb(), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeBrushInternal(zero);
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x000223FD File Offset: 0x000213FD
		internal HatchBrush(IntPtr nativeBrush)
		{
			base.SetNativeBrushInternal(nativeBrush);
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x0002240C File Offset: 0x0002140C
		public override object Clone()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCloneBrush(new HandleRef(this, base.NativeBrush), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new HatchBrush(zero);
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000B6A RID: 2922 RVA: 0x00022444 File Offset: 0x00021444
		public HatchStyle HatchStyle
		{
			get
			{
				int num = 0;
				int num2 = SafeNativeMethods.Gdip.GdipGetHatchStyle(new HandleRef(this, base.NativeBrush), out num);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				return (HatchStyle)num;
			}
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000B6B RID: 2923 RVA: 0x00022474 File Offset: 0x00021474
		public Color ForegroundColor
		{
			get
			{
				int num2;
				int num = SafeNativeMethods.Gdip.GdipGetHatchForegroundColor(new HandleRef(this, base.NativeBrush), out num2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return Color.FromArgb(num2);
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000B6C RID: 2924 RVA: 0x000224A8 File Offset: 0x000214A8
		public Color BackgroundColor
		{
			get
			{
				int num2;
				int num = SafeNativeMethods.Gdip.GdipGetHatchBackgroundColor(new HandleRef(this, base.NativeBrush), out num2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return Color.FromArgb(num2);
			}
		}
	}
}
