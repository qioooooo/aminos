using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x0200069F RID: 1695
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class AppearanceEditorPart : EditorPart
	{
		// Token: 0x17001516 RID: 5398
		// (get) Token: 0x060052E4 RID: 21220 RVA: 0x0014EC0A File Offset: 0x0014DC0A
		// (set) Token: 0x060052E5 RID: 21221 RVA: 0x0014EC12 File Offset: 0x0014DC12
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		[Themeable(false)]
		public override string DefaultButton
		{
			get
			{
				return base.DefaultButton;
			}
			set
			{
				base.DefaultButton = value;
			}
		}

		// Token: 0x17001517 RID: 5399
		// (get) Token: 0x060052E6 RID: 21222 RVA: 0x0014EC1B File Offset: 0x0014DC1B
		private bool HasError
		{
			get
			{
				return this._titleErrorMessage != null || this._heightErrorMessage != null || this._widthErrorMessage != null || this._chromeTypeErrorMessage != null || this._hiddenErrorMessage != null || this._directionErrorMessage != null;
			}
		}

		// Token: 0x17001518 RID: 5400
		// (get) Token: 0x060052E7 RID: 21223 RVA: 0x0014EC54 File Offset: 0x0014DC54
		// (set) Token: 0x060052E8 RID: 21224 RVA: 0x0014EC86 File Offset: 0x0014DC86
		[WebSysDefaultValue("AppearanceEditorPart_PartTitle")]
		public override string Title
		{
			get
			{
				string text = (string)this.ViewState["Title"];
				if (text == null)
				{
					return SR.GetString("AppearanceEditorPart_PartTitle");
				}
				return text;
			}
			set
			{
				this.ViewState["Title"] = value;
			}
		}

		// Token: 0x060052E9 RID: 21225 RVA: 0x0014EC9C File Offset: 0x0014DC9C
		public override bool ApplyChanges()
		{
			WebPart webPartToEdit = base.WebPartToEdit;
			if (webPartToEdit != null)
			{
				this.EnsureChildControls();
				bool allowLayoutChange = webPartToEdit.Zone.AllowLayoutChange;
				try
				{
					webPartToEdit.Title = this._title.Text;
				}
				catch (Exception ex)
				{
					this._titleErrorMessage = base.CreateErrorMessage(ex.Message);
				}
				if (allowLayoutChange)
				{
					try
					{
						TypeConverter converter = TypeDescriptor.GetConverter(typeof(PartChromeType));
						webPartToEdit.ChromeType = (PartChromeType)converter.ConvertFromString(this._chromeType.SelectedValue);
					}
					catch (Exception ex2)
					{
						this._chromeTypeErrorMessage = base.CreateErrorMessage(ex2.Message);
					}
				}
				try
				{
					TypeConverter converter2 = TypeDescriptor.GetConverter(typeof(ContentDirection));
					webPartToEdit.Direction = (ContentDirection)converter2.ConvertFromString(this._direction.SelectedValue);
				}
				catch (Exception ex3)
				{
					this._directionErrorMessage = base.CreateErrorMessage(ex3.Message);
				}
				if (allowLayoutChange)
				{
					Unit empty = Unit.Empty;
					string value = this._height.Value;
					if (!string.IsNullOrEmpty(value))
					{
						double num;
						if (double.TryParse(this._height.Value, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, CultureInfo.CurrentCulture, out num))
						{
							if (num < 0.0)
							{
								this._heightErrorMessage = SR.GetString("EditorPart_PropertyMinValue", new object[] { 0.ToString(CultureInfo.CurrentCulture) });
							}
							else if (num > 32767.0)
							{
								this._heightErrorMessage = SR.GetString("EditorPart_PropertyMaxValue", new object[] { 32767.ToString(CultureInfo.CurrentCulture) });
							}
							else
							{
								empty = new Unit(num, this._height.Type);
							}
						}
						else
						{
							this._heightErrorMessage = SR.GetString("EditorPart_PropertyMustBeDecimal");
						}
					}
					if (this._heightErrorMessage == null)
					{
						try
						{
							webPartToEdit.Height = empty;
						}
						catch (Exception ex4)
						{
							this._heightErrorMessage = base.CreateErrorMessage(ex4.Message);
						}
					}
				}
				if (allowLayoutChange)
				{
					Unit empty2 = Unit.Empty;
					string value2 = this._width.Value;
					if (!string.IsNullOrEmpty(value2))
					{
						double num2;
						if (double.TryParse(this._width.Value, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, CultureInfo.CurrentCulture, out num2))
						{
							if (num2 < 0.0)
							{
								this._widthErrorMessage = SR.GetString("EditorPart_PropertyMinValue", new object[] { 0.ToString(CultureInfo.CurrentCulture) });
							}
							else if (num2 > 32767.0)
							{
								this._widthErrorMessage = SR.GetString("EditorPart_PropertyMaxValue", new object[] { 32767.ToString(CultureInfo.CurrentCulture) });
							}
							else
							{
								empty2 = new Unit(num2, this._width.Type);
							}
						}
						else
						{
							this._widthErrorMessage = SR.GetString("EditorPart_PropertyMustBeDecimal");
						}
					}
					if (this._widthErrorMessage == null)
					{
						try
						{
							webPartToEdit.Width = empty2;
						}
						catch (Exception ex5)
						{
							this._widthErrorMessage = base.CreateErrorMessage(ex5.Message);
						}
					}
				}
				if (allowLayoutChange && webPartToEdit.AllowHide)
				{
					try
					{
						webPartToEdit.Hidden = this._hidden.Checked;
					}
					catch (Exception ex6)
					{
						this._hiddenErrorMessage = base.CreateErrorMessage(ex6.Message);
					}
				}
			}
			return !this.HasError;
		}

		// Token: 0x060052EA RID: 21226 RVA: 0x0014F030 File Offset: 0x0014E030
		protected internal override void CreateChildControls()
		{
			ControlCollection controls = this.Controls;
			controls.Clear();
			this._title = new TextBox();
			this._title.Columns = 30;
			controls.Add(this._title);
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(PartChromeType));
			this._chromeType = new DropDownList();
			this._chromeType.Items.Add(new ListItem(SR.GetString("PartChromeType_Default"), converter.ConvertToString(PartChromeType.Default)));
			this._chromeType.Items.Add(new ListItem(SR.GetString("PartChromeType_TitleAndBorder"), converter.ConvertToString(PartChromeType.TitleAndBorder)));
			this._chromeType.Items.Add(new ListItem(SR.GetString("PartChromeType_TitleOnly"), converter.ConvertToString(PartChromeType.TitleOnly)));
			this._chromeType.Items.Add(new ListItem(SR.GetString("PartChromeType_BorderOnly"), converter.ConvertToString(PartChromeType.BorderOnly)));
			this._chromeType.Items.Add(new ListItem(SR.GetString("PartChromeType_None"), converter.ConvertToString(PartChromeType.None)));
			controls.Add(this._chromeType);
			TypeConverter converter2 = TypeDescriptor.GetConverter(typeof(ContentDirection));
			this._direction = new DropDownList();
			this._direction.Items.Add(new ListItem(SR.GetString("ContentDirection_NotSet"), converter2.ConvertToString(ContentDirection.NotSet)));
			this._direction.Items.Add(new ListItem(SR.GetString("ContentDirection_LeftToRight"), converter2.ConvertToString(ContentDirection.LeftToRight)));
			this._direction.Items.Add(new ListItem(SR.GetString("ContentDirection_RightToLeft"), converter2.ConvertToString(ContentDirection.RightToLeft)));
			controls.Add(this._direction);
			this._height = new AppearanceEditorPart.UnitInput();
			controls.Add(this._height);
			this._width = new AppearanceEditorPart.UnitInput();
			controls.Add(this._width);
			this._hidden = new CheckBox();
			controls.Add(this._hidden);
			foreach (object obj in controls)
			{
				Control control = (Control)obj;
				control.EnableViewState = false;
			}
		}

		// Token: 0x060052EB RID: 21227 RVA: 0x0014F2A8 File Offset: 0x0014E2A8
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (this.Display && this.Visible && !this.HasError)
			{
				this.SyncChanges();
			}
		}

		// Token: 0x060052EC RID: 21228 RVA: 0x0014F2D0 File Offset: 0x0014E2D0
		protected internal override void RenderContents(HtmlTextWriter writer)
		{
			if (this.Page != null)
			{
				this.Page.VerifyRenderingInServerForm(this);
			}
			this.EnsureChildControls();
			string[] array = new string[]
			{
				SR.GetString("AppearanceEditorPart_Title"),
				SR.GetString("AppearanceEditorPart_ChromeType"),
				SR.GetString("AppearanceEditorPart_Direction"),
				SR.GetString("AppearanceEditorPart_Height"),
				SR.GetString("AppearanceEditorPart_Width"),
				SR.GetString("AppearanceEditorPart_Hidden")
			};
			WebControl[] array2 = new WebControl[] { this._title, this._chromeType, this._direction, this._height, this._width, this._hidden };
			string[] array3 = new string[] { this._titleErrorMessage, this._chromeTypeErrorMessage, this._directionErrorMessage, this._heightErrorMessage, this._widthErrorMessage, this._hiddenErrorMessage };
			base.RenderPropertyEditors(writer, array, null, array2, array3);
		}

		// Token: 0x060052ED RID: 21229 RVA: 0x0014F3E8 File Offset: 0x0014E3E8
		public override void SyncChanges()
		{
			WebPart webPartToEdit = base.WebPartToEdit;
			if (webPartToEdit != null)
			{
				bool allowLayoutChange = webPartToEdit.Zone.AllowLayoutChange;
				this.EnsureChildControls();
				this._title.Text = webPartToEdit.Title;
				TypeConverter converter = TypeDescriptor.GetConverter(typeof(PartChromeType));
				this._chromeType.SelectedValue = converter.ConvertToString(webPartToEdit.ChromeType);
				this._chromeType.Enabled = allowLayoutChange;
				TypeConverter converter2 = TypeDescriptor.GetConverter(typeof(ContentDirection));
				this._direction.SelectedValue = converter2.ConvertToString(webPartToEdit.Direction);
				this._height.Unit = webPartToEdit.Height;
				this._height.Enabled = allowLayoutChange;
				this._width.Unit = webPartToEdit.Width;
				this._width.Enabled = allowLayoutChange;
				this._hidden.Checked = webPartToEdit.Hidden;
				this._hidden.Enabled = allowLayoutChange && webPartToEdit.AllowHide;
			}
		}

		// Token: 0x04002E0A RID: 11786
		private const int TextBoxColumns = 30;

		// Token: 0x04002E0B RID: 11787
		private const int MinUnitValue = 0;

		// Token: 0x04002E0C RID: 11788
		private const int MaxUnitValue = 32767;

		// Token: 0x04002E0D RID: 11789
		private TextBox _title;

		// Token: 0x04002E0E RID: 11790
		private AppearanceEditorPart.UnitInput _height;

		// Token: 0x04002E0F RID: 11791
		private AppearanceEditorPart.UnitInput _width;

		// Token: 0x04002E10 RID: 11792
		private DropDownList _chromeType;

		// Token: 0x04002E11 RID: 11793
		private CheckBox _hidden;

		// Token: 0x04002E12 RID: 11794
		private DropDownList _direction;

		// Token: 0x04002E13 RID: 11795
		private string _titleErrorMessage;

		// Token: 0x04002E14 RID: 11796
		private string _heightErrorMessage;

		// Token: 0x04002E15 RID: 11797
		private string _widthErrorMessage;

		// Token: 0x04002E16 RID: 11798
		private string _chromeTypeErrorMessage;

		// Token: 0x04002E17 RID: 11799
		private string _hiddenErrorMessage;

		// Token: 0x04002E18 RID: 11800
		private string _directionErrorMessage;

		// Token: 0x020006A0 RID: 1696
		private sealed class UnitInput : CompositeControl
		{
			// Token: 0x17001519 RID: 5401
			// (get) Token: 0x060052EF RID: 21231 RVA: 0x0014F4F3 File Offset: 0x0014E4F3
			public string Value
			{
				get
				{
					if (this._value == null)
					{
						return string.Empty;
					}
					return this._value.Text;
				}
			}

			// Token: 0x1700151A RID: 5402
			// (get) Token: 0x060052F0 RID: 21232 RVA: 0x0014F50E File Offset: 0x0014E50E
			public UnitType Type
			{
				get
				{
					if (this._type == null)
					{
						return (UnitType)0;
					}
					return (UnitType)int.Parse(this._type.SelectedValue, CultureInfo.InvariantCulture);
				}
			}

			// Token: 0x1700151B RID: 5403
			// (set) Token: 0x060052F1 RID: 21233 RVA: 0x0014F530 File Offset: 0x0014E530
			public Unit Unit
			{
				set
				{
					this.EnsureChildControls();
					if (value == Unit.Empty)
					{
						this._value.Text = string.Empty;
						this._type.SelectedIndex = 0;
						return;
					}
					this._value.Text = value.Value.ToString(CultureInfo.CurrentCulture);
					this._type.SelectedValue = ((int)value.Type).ToString(CultureInfo.InvariantCulture);
				}
			}

			// Token: 0x060052F2 RID: 21234 RVA: 0x0014F5AC File Offset: 0x0014E5AC
			protected internal override void CreateChildControls()
			{
				this.Controls.Clear();
				this._value = new TextBox();
				this._value.Columns = 2;
				this.Controls.Add(this._value);
				this._type = new DropDownList();
				this._type.Items.Add(new ListItem(SR.GetString("AppearanceEditorPart_Pixels"), 1.ToString(CultureInfo.InvariantCulture)));
				this._type.Items.Add(new ListItem(SR.GetString("AppearanceEditorPart_Points"), 2.ToString(CultureInfo.InvariantCulture)));
				this._type.Items.Add(new ListItem(SR.GetString("AppearanceEditorPart_Picas"), 3.ToString(CultureInfo.InvariantCulture)));
				this._type.Items.Add(new ListItem(SR.GetString("AppearanceEditorPart_Inches"), 4.ToString(CultureInfo.InvariantCulture)));
				this._type.Items.Add(new ListItem(SR.GetString("AppearanceEditorPart_Millimeters"), 5.ToString(CultureInfo.InvariantCulture)));
				this._type.Items.Add(new ListItem(SR.GetString("AppearanceEditorPart_Centimeters"), 6.ToString(CultureInfo.InvariantCulture)));
				this._type.Items.Add(new ListItem(SR.GetString("AppearanceEditorPart_Percent"), 7.ToString(CultureInfo.InvariantCulture)));
				this._type.Items.Add(new ListItem(SR.GetString("AppearanceEditorPart_Em"), 8.ToString(CultureInfo.InvariantCulture)));
				this._type.Items.Add(new ListItem(SR.GetString("AppearanceEditorPart_Ex"), 9.ToString(CultureInfo.InvariantCulture)));
				this.Controls.Add(this._type);
			}

			// Token: 0x060052F3 RID: 21235 RVA: 0x0014F7A4 File Offset: 0x0014E7A4
			protected internal override void Render(HtmlTextWriter writer)
			{
				this.EnsureChildControls();
				this._value.ApplyStyle(base.ControlStyle);
				this._value.RenderControl(writer);
				writer.Write("&nbsp;");
				this._type.ApplyStyle(base.ControlStyle);
				this._type.RenderControl(writer);
			}

			// Token: 0x04002E19 RID: 11801
			private const int TextBoxColumns = 2;

			// Token: 0x04002E1A RID: 11802
			private TextBox _value;

			// Token: 0x04002E1B RID: 11803
			private DropDownList _type;
		}
	}
}
