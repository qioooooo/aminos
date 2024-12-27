using System;

namespace System.Windows.Forms
{
	// Token: 0x02000429 RID: 1065
	public class HScrollProperties : ScrollProperties
	{
		// Token: 0x06003F60 RID: 16224 RVA: 0x000E60D3 File Offset: 0x000E50D3
		public HScrollProperties(ScrollableControl container)
			: base(container)
		{
		}

		// Token: 0x17000C35 RID: 3125
		// (get) Token: 0x06003F61 RID: 16225 RVA: 0x000E60DC File Offset: 0x000E50DC
		internal override int PageSize
		{
			get
			{
				return base.ParentControl.ClientRectangle.Width;
			}
		}

		// Token: 0x17000C36 RID: 3126
		// (get) Token: 0x06003F62 RID: 16226 RVA: 0x000E60FC File Offset: 0x000E50FC
		internal override int Orientation
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000C37 RID: 3127
		// (get) Token: 0x06003F63 RID: 16227 RVA: 0x000E60FF File Offset: 0x000E50FF
		internal override int HorizontalDisplayPosition
		{
			get
			{
				return -this.value;
			}
		}

		// Token: 0x17000C38 RID: 3128
		// (get) Token: 0x06003F64 RID: 16228 RVA: 0x000E6108 File Offset: 0x000E5108
		internal override int VerticalDisplayPosition
		{
			get
			{
				return base.ParentControl.DisplayRectangle.Y;
			}
		}
	}
}
