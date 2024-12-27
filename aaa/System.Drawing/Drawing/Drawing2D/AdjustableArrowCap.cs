using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Drawing2D
{
	// Token: 0x0200006D RID: 109
	public sealed class AdjustableArrowCap : CustomLineCap
	{
		// Token: 0x06000715 RID: 1813 RVA: 0x0001B68E File Offset: 0x0001A68E
		internal AdjustableArrowCap(IntPtr nativeCap)
			: base(nativeCap)
		{
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x0001B697 File Offset: 0x0001A697
		public AdjustableArrowCap(float width, float height)
			: this(width, height, true)
		{
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x0001B6A4 File Offset: 0x0001A6A4
		public AdjustableArrowCap(float width, float height, bool isFilled)
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreateAdjustableArrowCap(height, width, isFilled, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			base.SetNativeLineCap(zero);
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0001B6DC File Offset: 0x0001A6DC
		private void _SetHeight(float height)
		{
			int num = SafeNativeMethods.Gdip.GdipSetAdjustableArrowCapHeight(new HandleRef(this, this.nativeCap), height);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x0001B70C File Offset: 0x0001A70C
		private float _GetHeight()
		{
			float num2;
			int num = SafeNativeMethods.Gdip.GdipGetAdjustableArrowCapHeight(new HandleRef(this, this.nativeCap), out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2;
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x0600071A RID: 1818 RVA: 0x0001B73D File Offset: 0x0001A73D
		// (set) Token: 0x0600071B RID: 1819 RVA: 0x0001B745 File Offset: 0x0001A745
		public float Height
		{
			get
			{
				return this._GetHeight();
			}
			set
			{
				this._SetHeight(value);
			}
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0001B750 File Offset: 0x0001A750
		private void _SetWidth(float width)
		{
			int num = SafeNativeMethods.Gdip.GdipSetAdjustableArrowCapWidth(new HandleRef(this, this.nativeCap), width);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0001B780 File Offset: 0x0001A780
		private float _GetWidth()
		{
			float num2;
			int num = SafeNativeMethods.Gdip.GdipGetAdjustableArrowCapWidth(new HandleRef(this, this.nativeCap), out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2;
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x0600071E RID: 1822 RVA: 0x0001B7B1 File Offset: 0x0001A7B1
		// (set) Token: 0x0600071F RID: 1823 RVA: 0x0001B7B9 File Offset: 0x0001A7B9
		public float Width
		{
			get
			{
				return this._GetWidth();
			}
			set
			{
				this._SetWidth(value);
			}
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0001B7C4 File Offset: 0x0001A7C4
		private void _SetMiddleInset(float middleInset)
		{
			int num = SafeNativeMethods.Gdip.GdipSetAdjustableArrowCapMiddleInset(new HandleRef(this, this.nativeCap), middleInset);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0001B7F4 File Offset: 0x0001A7F4
		private float _GetMiddleInset()
		{
			float num2;
			int num = SafeNativeMethods.Gdip.GdipGetAdjustableArrowCapMiddleInset(new HandleRef(this, this.nativeCap), out num2);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return num2;
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000722 RID: 1826 RVA: 0x0001B825 File Offset: 0x0001A825
		// (set) Token: 0x06000723 RID: 1827 RVA: 0x0001B82D File Offset: 0x0001A82D
		public float MiddleInset
		{
			get
			{
				return this._GetMiddleInset();
			}
			set
			{
				this._SetMiddleInset(value);
			}
		}

		// Token: 0x06000724 RID: 1828 RVA: 0x0001B838 File Offset: 0x0001A838
		private void _SetFillState(bool isFilled)
		{
			int num = SafeNativeMethods.Gdip.GdipSetAdjustableArrowCapFillState(new HandleRef(this, this.nativeCap), isFilled);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x0001B868 File Offset: 0x0001A868
		private bool _IsFilled()
		{
			bool flag = false;
			int num = SafeNativeMethods.Gdip.GdipGetAdjustableArrowCapFillState(new HandleRef(this, this.nativeCap), out flag);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return flag;
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000726 RID: 1830 RVA: 0x0001B89B File Offset: 0x0001A89B
		// (set) Token: 0x06000727 RID: 1831 RVA: 0x0001B8A3 File Offset: 0x0001A8A3
		public bool Filled
		{
			get
			{
				return this._IsFilled();
			}
			set
			{
				this._SetFillState(value);
			}
		}
	}
}
