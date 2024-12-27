using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	// Token: 0x020003FC RID: 1020
	[TypeConverter(typeof(FlatButtonAppearanceConverter))]
	public class FlatButtonAppearance
	{
		// Token: 0x06003C41 RID: 15425 RVA: 0x000D9358 File Offset: 0x000D8358
		internal FlatButtonAppearance(ButtonBase owner)
		{
			this.owner = owner;
		}

		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x06003C42 RID: 15426 RVA: 0x000D93A5 File Offset: 0x000D83A5
		// (set) Token: 0x06003C43 RID: 15427 RVA: 0x000D93B0 File Offset: 0x000D83B0
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DefaultValue(1)]
		[Browsable(true)]
		[ApplicableToButton]
		[NotifyParentProperty(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("ButtonBorderSizeDescr")]
		public int BorderSize
		{
			get
			{
				return this.borderSize;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("BorderSize", value, SR.GetString("InvalidLowBoundArgumentEx", new object[]
					{
						"BorderSize",
						value.ToString(CultureInfo.CurrentCulture),
						0.ToString(CultureInfo.CurrentCulture)
					}));
				}
				if (this.borderSize != value)
				{
					this.borderSize = value;
					if (this.owner != null && this.owner.ParentInternal != null)
					{
						LayoutTransaction.DoLayoutIf(this.owner.AutoSize, this.owner.ParentInternal, this.owner, PropertyNames.FlatAppearanceBorderSize);
					}
					this.owner.Invalidate();
				}
			}
		}

		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x06003C44 RID: 15428 RVA: 0x000D9462 File Offset: 0x000D8462
		// (set) Token: 0x06003C45 RID: 15429 RVA: 0x000D946C File Offset: 0x000D846C
		[DefaultValue(typeof(Color), "")]
		[Browsable(true)]
		[ApplicableToButton]
		[NotifyParentProperty(true)]
		[SRCategory("CatAppearance")]
		[SRDescription("ButtonBorderColorDescr")]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public Color BorderColor
		{
			get
			{
				return this.borderColor;
			}
			set
			{
				if (value.Equals(Color.Transparent))
				{
					throw new NotSupportedException(SR.GetString("ButtonFlatAppearanceInvalidBorderColor"));
				}
				if (this.borderColor != value)
				{
					this.borderColor = value;
					this.owner.Invalidate();
				}
			}
		}

		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x06003C46 RID: 15430 RVA: 0x000D94C2 File Offset: 0x000D84C2
		// (set) Token: 0x06003C47 RID: 15431 RVA: 0x000D94CA File Offset: 0x000D84CA
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DefaultValue(typeof(Color), "")]
		[SRDescription("ButtonCheckedBackColorDescr")]
		[Browsable(true)]
		[NotifyParentProperty(true)]
		[SRCategory("CatAppearance")]
		public Color CheckedBackColor
		{
			get
			{
				return this.checkedBackColor;
			}
			set
			{
				if (this.checkedBackColor != value)
				{
					this.checkedBackColor = value;
					this.owner.Invalidate();
				}
			}
		}

		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x06003C48 RID: 15432 RVA: 0x000D94EC File Offset: 0x000D84EC
		// (set) Token: 0x06003C49 RID: 15433 RVA: 0x000D94F4 File Offset: 0x000D84F4
		[NotifyParentProperty(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DefaultValue(typeof(Color), "")]
		[SRDescription("ButtonMouseDownBackColorDescr")]
		[Browsable(true)]
		[ApplicableToButton]
		[SRCategory("CatAppearance")]
		public Color MouseDownBackColor
		{
			get
			{
				return this.mouseDownBackColor;
			}
			set
			{
				if (this.mouseDownBackColor != value)
				{
					this.mouseDownBackColor = value;
					this.owner.Invalidate();
				}
			}
		}

		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x06003C4A RID: 15434 RVA: 0x000D9516 File Offset: 0x000D8516
		// (set) Token: 0x06003C4B RID: 15435 RVA: 0x000D951E File Offset: 0x000D851E
		[NotifyParentProperty(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		[DefaultValue(typeof(Color), "")]
		[SRDescription("ButtonMouseOverBackColorDescr")]
		[Browsable(true)]
		[SRCategory("CatAppearance")]
		[ApplicableToButton]
		public Color MouseOverBackColor
		{
			get
			{
				return this.mouseOverBackColor;
			}
			set
			{
				if (this.mouseOverBackColor != value)
				{
					this.mouseOverBackColor = value;
					this.owner.Invalidate();
				}
			}
		}

		// Token: 0x04001E0B RID: 7691
		private ButtonBase owner;

		// Token: 0x04001E0C RID: 7692
		private int borderSize = 1;

		// Token: 0x04001E0D RID: 7693
		private Color borderColor = Color.Empty;

		// Token: 0x04001E0E RID: 7694
		private Color checkedBackColor = Color.Empty;

		// Token: 0x04001E0F RID: 7695
		private Color mouseDownBackColor = Color.Empty;

		// Token: 0x04001E10 RID: 7696
		private Color mouseOverBackColor = Color.Empty;
	}
}
