using System;
using System.ComponentModel;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls.ListControls
{
	// Token: 0x0200050D RID: 1293
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed class BordersPage : BaseDataListPage
	{
		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x06002E1D RID: 11805 RVA: 0x001051E1 File Offset: 0x001041E1
		protected override string HelpKeyword
		{
			get
			{
				if (base.IsDataGridMode)
				{
					return "net.Asp.DataGridProperties.Borders";
				}
				return "net.Asp.DataListProperties.Borders";
			}
		}

		// Token: 0x06002E1E RID: 11806 RVA: 0x001051F8 File Offset: 0x001041F8
		private void InitForm()
		{
			GroupLabel groupLabel = new GroupLabel();
			global::System.Windows.Forms.Label label = new global::System.Windows.Forms.Label();
			this.cellPaddingEdit = new NumberEdit();
			global::System.Windows.Forms.Label label2 = new global::System.Windows.Forms.Label();
			this.cellSpacingEdit = new NumberEdit();
			GroupLabel groupLabel2 = new GroupLabel();
			global::System.Windows.Forms.Label label3 = new global::System.Windows.Forms.Label();
			this.gridLinesCombo = new ComboBox();
			global::System.Windows.Forms.Label label4 = new global::System.Windows.Forms.Label();
			this.borderColorCombo = new ColorComboBox();
			this.borderColorPickerButton = new global::System.Windows.Forms.Button();
			global::System.Windows.Forms.Label label5 = new global::System.Windows.Forms.Label();
			this.borderWidthUnit = new UnitControl();
			groupLabel.SetBounds(4, 4, 300, 16);
			groupLabel.Text = SR.GetString("BDLBor_CellMarginsGroup");
			groupLabel.TabStop = false;
			groupLabel.TabIndex = 0;
			label.Text = SR.GetString("BDLBor_CellPadding");
			label.SetBounds(12, 24, 120, 14);
			label.TabStop = false;
			label.TabIndex = 1;
			this.cellPaddingEdit.SetBounds(12, 40, 70, 20);
			this.cellPaddingEdit.AllowDecimal = false;
			this.cellPaddingEdit.AllowNegative = false;
			this.cellPaddingEdit.TabIndex = 2;
			this.cellPaddingEdit.TextChanged += this.OnBordersChanged;
			label2.Text = SR.GetString("BDLBor_CellSpacing");
			label2.SetBounds(160, 24, 120, 14);
			label2.TabStop = false;
			label2.TabIndex = 3;
			this.cellSpacingEdit.SetBounds(160, 40, 70, 20);
			this.cellSpacingEdit.AllowDecimal = false;
			this.cellSpacingEdit.AllowNegative = false;
			this.cellSpacingEdit.TabIndex = 4;
			this.cellSpacingEdit.TextChanged += this.OnBordersChanged;
			groupLabel2.SetBounds(4, 70, 300, 16);
			groupLabel2.Text = SR.GetString("BDLBor_BorderLinesGroup");
			groupLabel2.TabStop = false;
			groupLabel2.TabIndex = 5;
			label3.Text = SR.GetString("BDLBor_GridLines");
			label3.SetBounds(12, 90, 150, 14);
			label3.TabStop = false;
			label3.TabIndex = 6;
			this.gridLinesCombo.SetBounds(12, 106, 140, 21);
			this.gridLinesCombo.DropDownStyle = ComboBoxStyle.DropDownList;
			this.gridLinesCombo.Items.Clear();
			this.gridLinesCombo.Items.AddRange(new object[]
			{
				SR.GetString("BDLBor_GL_Horz"),
				SR.GetString("BDLBor_GL_Vert"),
				SR.GetString("BDLBor_GL_Both"),
				SR.GetString("BDLBor_GL_None")
			});
			this.gridLinesCombo.TabIndex = 7;
			this.gridLinesCombo.SelectedIndexChanged += this.OnBordersChanged;
			label4.Text = SR.GetString("BDLBor_BorderColor");
			label4.SetBounds(12, 134, 150, 14);
			label4.TabStop = false;
			label4.TabIndex = 8;
			this.borderColorCombo.SetBounds(12, 150, 140, 21);
			this.borderColorCombo.TabIndex = 9;
			this.borderColorCombo.TextChanged += this.OnBordersChanged;
			this.borderColorCombo.SelectedIndexChanged += this.OnBordersChanged;
			this.borderColorPickerButton.SetBounds(156, 149, 24, 22);
			this.borderColorPickerButton.Text = "...";
			this.borderColorPickerButton.TabIndex = 10;
			this.borderColorPickerButton.FlatStyle = FlatStyle.System;
			this.borderColorPickerButton.Click += this.OnClickColorPicker;
			this.borderColorPickerButton.AccessibleName = SR.GetString("BDLBor_ChooseColorButton");
			this.borderColorPickerButton.AccessibleDescription = SR.GetString("BDLBor_ChooseColorDesc");
			label5.Text = SR.GetString("BDLBor_BorderWidth");
			label5.SetBounds(12, 178, 150, 14);
			label5.TabStop = false;
			label5.TabIndex = 11;
			this.borderWidthUnit.SetBounds(12, 194, 102, 22);
			this.borderWidthUnit.AllowNegativeValues = false;
			this.borderWidthUnit.AllowPercentValues = false;
			this.borderWidthUnit.DefaultUnit = 0;
			this.borderWidthUnit.TabIndex = 12;
			this.borderWidthUnit.Changed += this.OnBordersChanged;
			this.borderWidthUnit.ValueAccessibleDescription = SR.GetString("BDLBor_BorderWidthValueDesc");
			this.borderWidthUnit.ValueAccessibleName = SR.GetString("BDLBor_BorderWidthValueName");
			this.borderWidthUnit.UnitAccessibleDescription = SR.GetString("BDLBor_BorderWidthUnitDesc");
			this.borderWidthUnit.UnitAccessibleName = SR.GetString("BDLBor_BorderWidthUnitName");
			this.Text = SR.GetString("BDLBor_Text");
			base.AccessibleDescription = SR.GetString("BDLBor_Desc");
			base.Size = new Size(308, 156);
			base.CommitOnDeactivate = true;
			base.Icon = new Icon(base.GetType(), "BordersPage.ico");
			base.Controls.Clear();
			base.Controls.AddRange(new Control[]
			{
				this.borderWidthUnit, label5, this.borderColorPickerButton, this.borderColorCombo, label4, this.gridLinesCombo, label3, groupLabel2, this.cellSpacingEdit, label2,
				this.cellPaddingEdit, label, groupLabel
			});
		}

		// Token: 0x06002E1F RID: 11807 RVA: 0x00105772 File Offset: 0x00104772
		private void InitPage()
		{
			this.cellPaddingEdit.Clear();
			this.cellSpacingEdit.Clear();
			this.gridLinesCombo.SelectedIndex = -1;
			this.borderColorCombo.Color = null;
			this.borderWidthUnit.Value = null;
		}

		// Token: 0x06002E20 RID: 11808 RVA: 0x001057B0 File Offset: 0x001047B0
		protected override void LoadComponent()
		{
			this.InitPage();
			BaseDataList baseControl = base.GetBaseControl();
			int cellPadding = baseControl.CellPadding;
			if (cellPadding != -1)
			{
				this.cellPaddingEdit.Text = cellPadding.ToString(NumberFormatInfo.CurrentInfo);
			}
			int cellSpacing = baseControl.CellSpacing;
			if (cellSpacing != -1)
			{
				this.cellSpacingEdit.Text = cellSpacing.ToString(NumberFormatInfo.CurrentInfo);
			}
			switch (baseControl.GridLines)
			{
			case GridLines.None:
				this.gridLinesCombo.SelectedIndex = 3;
				break;
			case GridLines.Horizontal:
				this.gridLinesCombo.SelectedIndex = 0;
				break;
			case GridLines.Vertical:
				this.gridLinesCombo.SelectedIndex = 1;
				break;
			case GridLines.Both:
				this.gridLinesCombo.SelectedIndex = 2;
				break;
			}
			this.borderColorCombo.Color = ColorTranslator.ToHtml(baseControl.BorderColor);
			this.borderWidthUnit.Value = baseControl.BorderWidth.ToString(CultureInfo.CurrentCulture);
		}

		// Token: 0x06002E21 RID: 11809 RVA: 0x00105898 File Offset: 0x00104898
		private void OnBordersChanged(object source, EventArgs e)
		{
			if (base.IsLoading())
			{
				return;
			}
			this.SetDirty();
		}

		// Token: 0x06002E22 RID: 11810 RVA: 0x001058AC File Offset: 0x001048AC
		private void OnClickColorPicker(object source, EventArgs e)
		{
			string text = this.borderColorCombo.Color;
			text = ColorBuilder.BuildColor(base.GetBaseControl(), this, text);
			if (text != null)
			{
				this.borderColorCombo.Color = text;
				this.OnBordersChanged(this.borderColorCombo, EventArgs.Empty);
			}
		}

		// Token: 0x06002E23 RID: 11811 RVA: 0x001058F4 File Offset: 0x001048F4
		protected override void SaveComponent()
		{
			BaseDataList baseControl = base.GetBaseControl();
			try
			{
				string text = this.cellPaddingEdit.Text.Trim();
				if (text.Length != 0)
				{
					baseControl.CellPadding = int.Parse(text, CultureInfo.CurrentCulture);
				}
				else
				{
					baseControl.CellPadding = -1;
				}
			}
			catch
			{
				if (baseControl.CellPadding != -1)
				{
					this.cellPaddingEdit.Text = baseControl.CellPadding.ToString(NumberFormatInfo.CurrentInfo);
				}
				else
				{
					this.cellPaddingEdit.Clear();
				}
			}
			try
			{
				string text2 = this.cellSpacingEdit.Text.Trim();
				if (text2.Length != 0)
				{
					baseControl.CellSpacing = int.Parse(text2, CultureInfo.CurrentCulture);
				}
				else
				{
					baseControl.CellSpacing = -1;
				}
			}
			catch
			{
				if (baseControl.CellSpacing != -1)
				{
					this.cellSpacingEdit.Text = baseControl.CellSpacing.ToString(NumberFormatInfo.CurrentInfo);
				}
				else
				{
					this.cellSpacingEdit.Clear();
				}
			}
			switch (this.gridLinesCombo.SelectedIndex)
			{
			case 0:
				baseControl.GridLines = GridLines.Horizontal;
				break;
			case 1:
				baseControl.GridLines = GridLines.Vertical;
				break;
			case 2:
				baseControl.GridLines = GridLines.Both;
				break;
			case 3:
				baseControl.GridLines = GridLines.None;
				break;
			}
			try
			{
				string color = this.borderColorCombo.Color;
				baseControl.BorderColor = ColorTranslator.FromHtml(color);
			}
			catch
			{
				this.borderColorCombo.Color = ColorTranslator.ToHtml(baseControl.BorderColor);
			}
			try
			{
				string value = this.borderWidthUnit.Value;
				Unit unit = Unit.Empty;
				if (value != null)
				{
					unit = Unit.Parse(value, CultureInfo.CurrentCulture);
				}
				baseControl.BorderWidth = unit;
			}
			catch
			{
				this.borderWidthUnit.Value = baseControl.BorderWidth.ToString(CultureInfo.CurrentCulture);
			}
		}

		// Token: 0x06002E24 RID: 11812 RVA: 0x00105AE4 File Offset: 0x00104AE4
		public override void SetComponent(IComponent component)
		{
			base.SetComponent(component);
			this.InitForm();
		}

		// Token: 0x04001F58 RID: 8024
		private const int IDX_GRID_HORIZONTAL = 0;

		// Token: 0x04001F59 RID: 8025
		private const int IDX_GRID_VERTICAL = 1;

		// Token: 0x04001F5A RID: 8026
		private const int IDX_GRID_BOTH = 2;

		// Token: 0x04001F5B RID: 8027
		private const int IDX_GRID_NEITHER = 3;

		// Token: 0x04001F5C RID: 8028
		private NumberEdit cellPaddingEdit;

		// Token: 0x04001F5D RID: 8029
		private NumberEdit cellSpacingEdit;

		// Token: 0x04001F5E RID: 8030
		private ComboBox gridLinesCombo;

		// Token: 0x04001F5F RID: 8031
		private ColorComboBox borderColorCombo;

		// Token: 0x04001F60 RID: 8032
		private global::System.Windows.Forms.Button borderColorPickerButton;

		// Token: 0x04001F61 RID: 8033
		private UnitControl borderWidthUnit;
	}
}
