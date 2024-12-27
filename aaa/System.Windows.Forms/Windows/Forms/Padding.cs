using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;

namespace System.Windows.Forms
{
	// Token: 0x020005AD RID: 1453
	[TypeConverter(typeof(PaddingConverter))]
	[Serializable]
	public struct Padding
	{
		// Token: 0x06004B3E RID: 19262 RVA: 0x00110A3C File Offset: 0x0010FA3C
		public Padding(int all)
		{
			this._all = true;
			this._bottom = all;
			this._right = all;
			this._left = all;
			this._top = all;
		}

		// Token: 0x06004B3F RID: 19263 RVA: 0x00110A74 File Offset: 0x0010FA74
		public Padding(int left, int top, int right, int bottom)
		{
			this._top = top;
			this._left = left;
			this._right = right;
			this._bottom = bottom;
			this._all = this._top == this._left && this._top == this._right && this._top == this._bottom;
		}

		// Token: 0x17000EDD RID: 3805
		// (get) Token: 0x06004B40 RID: 19264 RVA: 0x00110AD1 File Offset: 0x0010FAD1
		// (set) Token: 0x06004B41 RID: 19265 RVA: 0x00110AE4 File Offset: 0x0010FAE4
		[RefreshProperties(RefreshProperties.All)]
		public int All
		{
			get
			{
				if (!this._all)
				{
					return -1;
				}
				return this._top;
			}
			set
			{
				if (!this._all || this._top != value)
				{
					this._all = true;
					this._bottom = value;
					this._right = value;
					this._left = value;
					this._top = value;
				}
			}
		}

		// Token: 0x17000EDE RID: 3806
		// (get) Token: 0x06004B42 RID: 19266 RVA: 0x00110B2B File Offset: 0x0010FB2B
		// (set) Token: 0x06004B43 RID: 19267 RVA: 0x00110B42 File Offset: 0x0010FB42
		[RefreshProperties(RefreshProperties.All)]
		public int Bottom
		{
			get
			{
				if (this._all)
				{
					return this._top;
				}
				return this._bottom;
			}
			set
			{
				if (this._all || this._bottom != value)
				{
					this._all = false;
					this._bottom = value;
				}
			}
		}

		// Token: 0x17000EDF RID: 3807
		// (get) Token: 0x06004B44 RID: 19268 RVA: 0x00110B63 File Offset: 0x0010FB63
		// (set) Token: 0x06004B45 RID: 19269 RVA: 0x00110B7A File Offset: 0x0010FB7A
		[RefreshProperties(RefreshProperties.All)]
		public int Left
		{
			get
			{
				if (this._all)
				{
					return this._top;
				}
				return this._left;
			}
			set
			{
				if (this._all || this._left != value)
				{
					this._all = false;
					this._left = value;
				}
			}
		}

		// Token: 0x17000EE0 RID: 3808
		// (get) Token: 0x06004B46 RID: 19270 RVA: 0x00110B9B File Offset: 0x0010FB9B
		// (set) Token: 0x06004B47 RID: 19271 RVA: 0x00110BB2 File Offset: 0x0010FBB2
		[RefreshProperties(RefreshProperties.All)]
		public int Right
		{
			get
			{
				if (this._all)
				{
					return this._top;
				}
				return this._right;
			}
			set
			{
				if (this._all || this._right != value)
				{
					this._all = false;
					this._right = value;
				}
			}
		}

		// Token: 0x17000EE1 RID: 3809
		// (get) Token: 0x06004B48 RID: 19272 RVA: 0x00110BD3 File Offset: 0x0010FBD3
		// (set) Token: 0x06004B49 RID: 19273 RVA: 0x00110BDB File Offset: 0x0010FBDB
		[RefreshProperties(RefreshProperties.All)]
		public int Top
		{
			get
			{
				return this._top;
			}
			set
			{
				if (this._all || this._top != value)
				{
					this._all = false;
					this._top = value;
				}
			}
		}

		// Token: 0x17000EE2 RID: 3810
		// (get) Token: 0x06004B4A RID: 19274 RVA: 0x00110BFC File Offset: 0x0010FBFC
		[Browsable(false)]
		public int Horizontal
		{
			get
			{
				return this.Left + this.Right;
			}
		}

		// Token: 0x17000EE3 RID: 3811
		// (get) Token: 0x06004B4B RID: 19275 RVA: 0x00110C0B File Offset: 0x0010FC0B
		[Browsable(false)]
		public int Vertical
		{
			get
			{
				return this.Top + this.Bottom;
			}
		}

		// Token: 0x17000EE4 RID: 3812
		// (get) Token: 0x06004B4C RID: 19276 RVA: 0x00110C1A File Offset: 0x0010FC1A
		[Browsable(false)]
		public Size Size
		{
			get
			{
				return new Size(this.Horizontal, this.Vertical);
			}
		}

		// Token: 0x06004B4D RID: 19277 RVA: 0x00110C2D File Offset: 0x0010FC2D
		public static Padding Add(Padding p1, Padding p2)
		{
			return p1 + p2;
		}

		// Token: 0x06004B4E RID: 19278 RVA: 0x00110C36 File Offset: 0x0010FC36
		public static Padding Subtract(Padding p1, Padding p2)
		{
			return p1 - p2;
		}

		// Token: 0x06004B4F RID: 19279 RVA: 0x00110C3F File Offset: 0x0010FC3F
		public override bool Equals(object other)
		{
			return other is Padding && (Padding)other == this;
		}

		// Token: 0x06004B50 RID: 19280 RVA: 0x00110C5C File Offset: 0x0010FC5C
		public static Padding operator +(Padding p1, Padding p2)
		{
			return new Padding(p1.Left + p2.Left, p1.Top + p2.Top, p1.Right + p2.Right, p1.Bottom + p2.Bottom);
		}

		// Token: 0x06004B51 RID: 19281 RVA: 0x00110CAC File Offset: 0x0010FCAC
		public static Padding operator -(Padding p1, Padding p2)
		{
			return new Padding(p1.Left - p2.Left, p1.Top - p2.Top, p1.Right - p2.Right, p1.Bottom - p2.Bottom);
		}

		// Token: 0x06004B52 RID: 19282 RVA: 0x00110CFC File Offset: 0x0010FCFC
		public static bool operator ==(Padding p1, Padding p2)
		{
			return p1.Left == p2.Left && p1.Top == p2.Top && p1.Right == p2.Right && p1.Bottom == p2.Bottom;
		}

		// Token: 0x06004B53 RID: 19283 RVA: 0x00110D4B File Offset: 0x0010FD4B
		public static bool operator !=(Padding p1, Padding p2)
		{
			return !(p1 == p2);
		}

		// Token: 0x06004B54 RID: 19284 RVA: 0x00110D57 File Offset: 0x0010FD57
		public override int GetHashCode()
		{
			return this.Left ^ WindowsFormsUtils.RotateLeft(this.Top, 8) ^ WindowsFormsUtils.RotateLeft(this.Right, 16) ^ WindowsFormsUtils.RotateLeft(this.Bottom, 24);
		}

		// Token: 0x06004B55 RID: 19285 RVA: 0x00110D88 File Offset: 0x0010FD88
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"{Left=",
				this.Left.ToString(CultureInfo.CurrentCulture),
				",Top=",
				this.Top.ToString(CultureInfo.CurrentCulture),
				",Right=",
				this.Right.ToString(CultureInfo.CurrentCulture),
				",Bottom=",
				this.Bottom.ToString(CultureInfo.CurrentCulture),
				"}"
			});
		}

		// Token: 0x06004B56 RID: 19286 RVA: 0x00110E24 File Offset: 0x0010FE24
		private void ResetAll()
		{
			this.All = 0;
		}

		// Token: 0x06004B57 RID: 19287 RVA: 0x00110E2D File Offset: 0x0010FE2D
		private void ResetBottom()
		{
			this.Bottom = 0;
		}

		// Token: 0x06004B58 RID: 19288 RVA: 0x00110E36 File Offset: 0x0010FE36
		private void ResetLeft()
		{
			this.Left = 0;
		}

		// Token: 0x06004B59 RID: 19289 RVA: 0x00110E3F File Offset: 0x0010FE3F
		private void ResetRight()
		{
			this.Right = 0;
		}

		// Token: 0x06004B5A RID: 19290 RVA: 0x00110E48 File Offset: 0x0010FE48
		private void ResetTop()
		{
			this.Top = 0;
		}

		// Token: 0x06004B5B RID: 19291 RVA: 0x00110E54 File Offset: 0x0010FE54
		internal void Scale(float dx, float dy)
		{
			this._top = (int)((float)this._top * dy);
			this._left = (int)((float)this._left * dx);
			this._right = (int)((float)this._right * dx);
			this._bottom = (int)((float)this._bottom * dy);
		}

		// Token: 0x06004B5C RID: 19292 RVA: 0x00110EA1 File Offset: 0x0010FEA1
		internal bool ShouldSerializeAll()
		{
			return this._all;
		}

		// Token: 0x06004B5D RID: 19293 RVA: 0x00110EA9 File Offset: 0x0010FEA9
		[Conditional("DEBUG")]
		private void Debug_SanityCheck()
		{
			if (this._all)
			{
			}
		}

		// Token: 0x040030F9 RID: 12537
		private bool _all;

		// Token: 0x040030FA RID: 12538
		private int _top;

		// Token: 0x040030FB RID: 12539
		private int _left;

		// Token: 0x040030FC RID: 12540
		private int _right;

		// Token: 0x040030FD RID: 12541
		private int _bottom;

		// Token: 0x040030FE RID: 12542
		public static readonly Padding Empty = new Padding(0);
	}
}
