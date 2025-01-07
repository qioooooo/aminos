using System;
using System.Collections;
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
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed class FormatPage : BaseDataListPage
	{
		protected override string HelpKeyword
		{
			get
			{
				if (base.IsDataGridMode)
				{
					return "net.Asp.DataGridProperties.Format";
				}
				return "net.Asp.DataListProperties.Format";
			}
		}

		private void InitFontList()
		{
			try
			{
				FontFamily[] families = FontFamily.Families;
				for (int i = 0; i < families.Length; i++)
				{
					if (this.fontNameCombo.Items.Count == 0 || this.fontNameCombo.FindStringExact(families[i].Name) == -1)
					{
						this.fontNameCombo.Items.Add(families[i].Name);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		private void InitForm()
		{
			global::System.Windows.Forms.Label label = new global::System.Windows.Forms.Label();
			this.formatTree = new global::System.Windows.Forms.TreeView();
			this.stylePanel = new global::System.Windows.Forms.Panel();
			GroupLabel groupLabel = new GroupLabel();
			global::System.Windows.Forms.Label label2 = new global::System.Windows.Forms.Label();
			this.foreColorCombo = new ColorComboBox();
			this.foreColorPickerButton = new global::System.Windows.Forms.Button();
			global::System.Windows.Forms.Label label3 = new global::System.Windows.Forms.Label();
			this.backColorCombo = new ColorComboBox();
			this.backColorPickerButton = new global::System.Windows.Forms.Button();
			global::System.Windows.Forms.Label label4 = new global::System.Windows.Forms.Label();
			this.fontNameCombo = new ComboBox();
			global::System.Windows.Forms.Label label5 = new global::System.Windows.Forms.Label();
			this.fontSizeCombo = new UnsettableComboBox();
			this.fontSizeUnit = new UnitControl();
			this.boldCheck = new global::System.Windows.Forms.CheckBox();
			this.italicCheck = new global::System.Windows.Forms.CheckBox();
			this.underlineCheck = new global::System.Windows.Forms.CheckBox();
			this.strikeOutCheck = new global::System.Windows.Forms.CheckBox();
			this.overlineCheck = new global::System.Windows.Forms.CheckBox();
			GroupLabel groupLabel2 = new GroupLabel();
			global::System.Windows.Forms.Label label6 = new global::System.Windows.Forms.Label();
			this.horzAlignCombo = new UnsettableComboBox();
			this.vertAlignLabel = new global::System.Windows.Forms.Label();
			this.vertAlignCombo = new UnsettableComboBox();
			this.allowWrappingCheck = new global::System.Windows.Forms.CheckBox();
			GroupLabel groupLabel3 = null;
			global::System.Windows.Forms.Label label7 = null;
			if (base.IsDataGridMode)
			{
				this.columnPanel = new global::System.Windows.Forms.Panel();
				groupLabel3 = new GroupLabel();
				label7 = new global::System.Windows.Forms.Label();
				this.widthUnit = new UnitControl();
			}
			label.SetBounds(4, 4, 111, 14);
			label.Text = SR.GetString("BDLFmt_Objects");
			label.TabStop = false;
			label.TabIndex = 2;
			this.formatTree.SetBounds(4, 20, 162, 350);
			this.formatTree.HideSelection = false;
			this.formatTree.TabIndex = 3;
			this.formatTree.AfterSelect += this.OnSelChangedFormatObject;
			this.stylePanel.SetBounds(177, 4, 230, 370);
			this.stylePanel.TabIndex = 6;
			this.stylePanel.Visible = false;
			groupLabel.SetBounds(0, 2, 224, 14);
			groupLabel.Text = SR.GetString("BDLFmt_AppearanceGroup");
			groupLabel.TabStop = false;
			groupLabel.TabIndex = 1;
			label2.SetBounds(8, 19, 160, 14);
			label2.Text = SR.GetString("BDLFmt_ForeColor");
			label2.TabStop = false;
			label2.TabIndex = 2;
			this.foreColorCombo.SetBounds(8, 37, 102, 22);
			this.foreColorCombo.TabIndex = 3;
			this.foreColorCombo.TextChanged += this.OnFormatChanged;
			this.foreColorCombo.SelectedIndexChanged += this.OnFormatChanged;
			this.foreColorPickerButton.SetBounds(114, 36, 24, 22);
			this.foreColorPickerButton.TabIndex = 4;
			this.foreColorPickerButton.Text = "...";
			this.foreColorPickerButton.FlatStyle = FlatStyle.System;
			this.foreColorPickerButton.Click += this.OnClickForeColorPicker;
			this.foreColorPickerButton.AccessibleName = SR.GetString("BDLFmt_ChooseColorButton");
			this.foreColorPickerButton.AccessibleDescription = SR.GetString("BDLFmt_ChooseForeColorDesc");
			label3.SetBounds(8, 62, 160, 14);
			label3.Text = SR.GetString("BDLFmt_BackColor");
			label3.TabStop = false;
			label3.TabIndex = 5;
			this.backColorCombo.SetBounds(8, 78, 102, 22);
			this.backColorCombo.TabIndex = 6;
			this.backColorCombo.TextChanged += this.OnFormatChanged;
			this.backColorCombo.SelectedIndexChanged += this.OnFormatChanged;
			this.backColorPickerButton.SetBounds(114, 77, 24, 22);
			this.backColorPickerButton.TabIndex = 7;
			this.backColorPickerButton.Text = "...";
			this.backColorPickerButton.FlatStyle = FlatStyle.System;
			this.backColorPickerButton.Click += this.OnClickBackColorPicker;
			this.backColorPickerButton.AccessibleName = SR.GetString("BDLFmt_ChooseColorButton");
			this.backColorPickerButton.AccessibleDescription = SR.GetString("BDLFmt_ChooseBackColorDesc");
			label4.SetBounds(8, 104, 160, 14);
			label4.Text = SR.GetString("BDLFmt_FontName");
			label4.TabStop = false;
			label4.TabIndex = 8;
			this.fontNameCombo.SetBounds(8, 120, 200, 22);
			this.fontNameCombo.Sorted = true;
			this.fontNameCombo.TabIndex = 9;
			this.fontNameCombo.SelectedIndexChanged += this.OnFontNameChanged;
			this.fontNameCombo.TextChanged += this.OnFontNameChanged;
			label5.SetBounds(8, 146, 160, 14);
			label5.Text = SR.GetString("BDLFmt_FontSize");
			label5.TabStop = false;
			label5.TabIndex = 10;
			this.fontSizeCombo.SetBounds(8, 162, 100, 22);
			this.fontSizeCombo.TabIndex = 11;
			this.fontSizeCombo.MaxDropDownItems = 11;
			this.fontSizeCombo.DropDownStyle = ComboBoxStyle.DropDownList;
			this.fontSizeCombo.Items.AddRange(new object[]
			{
				SR.GetString("BDLFmt_FS_Smaller"),
				SR.GetString("BDLFmt_FS_Larger"),
				SR.GetString("BDLFmt_FS_XXSmall"),
				SR.GetString("BDLFmt_FS_XSmall"),
				SR.GetString("BDLFmt_FS_Small"),
				SR.GetString("BDLFmt_FS_Medium"),
				SR.GetString("BDLFmt_FS_Large"),
				SR.GetString("BDLFmt_FS_XLarge"),
				SR.GetString("BDLFmt_FS_XXLarge"),
				SR.GetString("BDLFmt_FS_Custom")
			});
			this.fontSizeCombo.SelectedIndexChanged += this.OnFontSizeChanged;
			this.fontSizeUnit.SetBounds(112, 162, 96, 22);
			this.fontSizeUnit.AllowNegativeValues = false;
			this.fontSizeUnit.TabIndex = 12;
			this.fontSizeUnit.Changed += this.OnFormatChanged;
			this.fontSizeUnit.ValueAccessibleDescription = SR.GetString("BDLFmt_FontSizeValueDesc");
			this.fontSizeUnit.ValueAccessibleName = SR.GetString("BDLFmt_FontSizeValueName");
			this.fontSizeUnit.UnitAccessibleDescription = SR.GetString("BDLFmt_FontSizeUnitDesc");
			this.fontSizeUnit.UnitAccessibleName = SR.GetString("BDLFmt_FontSizeUnitName");
			this.boldCheck.SetBounds(8, 186, 106, 20);
			this.boldCheck.Text = SR.GetString("BDLFmt_FontBold");
			this.boldCheck.TabIndex = 13;
			this.boldCheck.TextAlign = ContentAlignment.MiddleLeft;
			this.boldCheck.FlatStyle = FlatStyle.System;
			this.boldCheck.CheckedChanged += this.OnFormatChanged;
			this.italicCheck.SetBounds(8, 204, 106, 20);
			this.italicCheck.Text = SR.GetString("BDLFmt_FontItalic");
			this.italicCheck.TabIndex = 14;
			this.italicCheck.TextAlign = ContentAlignment.MiddleLeft;
			this.italicCheck.FlatStyle = FlatStyle.System;
			this.italicCheck.CheckedChanged += this.OnFormatChanged;
			this.underlineCheck.SetBounds(8, 222, 106, 20);
			this.underlineCheck.Text = SR.GetString("BDLFmt_FontUnderline");
			this.underlineCheck.TabIndex = 15;
			this.underlineCheck.TextAlign = ContentAlignment.MiddleLeft;
			this.underlineCheck.FlatStyle = FlatStyle.System;
			this.underlineCheck.CheckedChanged += this.OnFormatChanged;
			this.strikeOutCheck.SetBounds(120, 186, 106, 20);
			this.strikeOutCheck.Text = SR.GetString("BDLFmt_FontStrikeout");
			this.strikeOutCheck.TabIndex = 16;
			this.strikeOutCheck.TextAlign = ContentAlignment.MiddleLeft;
			this.strikeOutCheck.FlatStyle = FlatStyle.System;
			this.strikeOutCheck.CheckedChanged += this.OnFormatChanged;
			this.overlineCheck.SetBounds(120, 204, 106, 20);
			this.overlineCheck.Text = SR.GetString("BDLFmt_FontOverline");
			this.overlineCheck.TabIndex = 17;
			this.overlineCheck.TextAlign = ContentAlignment.MiddleLeft;
			this.overlineCheck.FlatStyle = FlatStyle.System;
			this.overlineCheck.CheckedChanged += this.OnFormatChanged;
			groupLabel2.SetBounds(0, 246, 224, 14);
			groupLabel2.Text = SR.GetString("BDLFmt_AlignmentGroup");
			groupLabel2.TabStop = false;
			groupLabel2.TabIndex = 18;
			label6.SetBounds(8, 264, 160, 14);
			label6.Text = SR.GetString("BDLFmt_HorzAlign");
			label6.TabStop = false;
			label6.TabIndex = 19;
			this.horzAlignCombo.SetBounds(8, 280, 190, 22);
			this.horzAlignCombo.DropDownStyle = ComboBoxStyle.DropDownList;
			this.horzAlignCombo.Items.AddRange(new object[]
			{
				SR.GetString("BDLFmt_HA_Left"),
				SR.GetString("BDLFmt_HA_Center"),
				SR.GetString("BDLFmt_HA_Right"),
				SR.GetString("BDLFmt_HA_Justify")
			});
			this.horzAlignCombo.TabIndex = 20;
			this.horzAlignCombo.SelectedIndexChanged += this.OnFormatChanged;
			this.vertAlignLabel.SetBounds(8, 306, 160, 14);
			this.vertAlignLabel.Text = SR.GetString("BDLFmt_VertAlign");
			this.vertAlignLabel.TabStop = false;
			this.vertAlignLabel.TabIndex = 21;
			this.vertAlignCombo.SetBounds(8, 322, 190, 22);
			this.vertAlignCombo.DropDownStyle = ComboBoxStyle.DropDownList;
			this.vertAlignCombo.Items.AddRange(new object[]
			{
				SR.GetString("BDLFmt_VA_Top"),
				SR.GetString("BDLFmt_VA_Middle"),
				SR.GetString("BDLFmt_VA_Bottom")
			});
			this.vertAlignCombo.TabIndex = 22;
			this.vertAlignCombo.SelectedIndexChanged += this.OnFormatChanged;
			this.allowWrappingCheck.SetBounds(8, 348, 200, 17);
			this.allowWrappingCheck.Text = SR.GetString("BDLFmt_AllowWrapping");
			this.allowWrappingCheck.TabIndex = 24;
			this.allowWrappingCheck.FlatStyle = FlatStyle.System;
			this.allowWrappingCheck.CheckedChanged += this.OnFormatChanged;
			if (base.IsDataGridMode)
			{
				this.columnPanel.SetBounds(177, 4, 279, 350);
				this.columnPanel.TabIndex = 7;
				this.columnPanel.Visible = false;
				groupLabel3.SetBounds(0, 0, 279, 14);
				groupLabel3.Text = SR.GetString("BDLFmt_LayoutGroup");
				groupLabel3.TabStop = false;
				groupLabel3.TabIndex = 0;
				label7.SetBounds(8, 20, 64, 14);
				label7.Text = SR.GetString("BDLFmt_Width");
				label7.TabStop = false;
				label7.TabIndex = 1;
				this.widthUnit.SetBounds(80, 17, 102, 22);
				this.widthUnit.AllowNegativeValues = false;
				this.widthUnit.DefaultUnit = 0;
				this.widthUnit.TabIndex = 2;
				this.widthUnit.Changed += this.OnFormatChanged;
				this.widthUnit.ValueAccessibleName = SR.GetString("BDLFmt_WidthValueName");
				this.widthUnit.ValueAccessibleDescription = SR.GetString("BDLFmt_WidthValueDesc");
				this.widthUnit.UnitAccessibleName = SR.GetString("BDLFmt_WidthUnitName");
				this.widthUnit.UnitAccessibleDescription = SR.GetString("BDLFmt_WidthUnitDesc");
			}
			this.Text = SR.GetString("BDLFmt_Text");
			base.AccessibleDescription = SR.GetString("BDLFmt_Desc");
			base.Size = new Size(408, 370);
			base.CommitOnDeactivate = true;
			base.Icon = new Icon(base.GetType(), "FormatPage.ico");
			this.stylePanel.Controls.Clear();
			this.stylePanel.Controls.AddRange(new Control[]
			{
				this.allowWrappingCheck, this.vertAlignCombo, this.vertAlignLabel, this.horzAlignCombo, label6, groupLabel2, this.overlineCheck, this.strikeOutCheck, this.underlineCheck, this.italicCheck,
				this.boldCheck, this.fontSizeUnit, this.fontSizeCombo, label5, this.fontNameCombo, label4, this.backColorPickerButton, this.backColorCombo, label3, this.foreColorPickerButton,
				this.foreColorCombo, label2, groupLabel
			});
			if (base.IsDataGridMode)
			{
				this.columnPanel.Controls.Clear();
				this.columnPanel.Controls.AddRange(new Control[] { this.widthUnit, label7, groupLabel3 });
				base.Controls.Clear();
				base.Controls.AddRange(new Control[] { this.columnPanel, this.stylePanel, this.formatTree, label });
				return;
			}
			base.Controls.Clear();
			base.Controls.AddRange(new Control[] { this.stylePanel, this.formatTree, label });
		}

		private void InitFormatTree()
		{
			if (base.IsDataGridMode)
			{
				global::System.Web.UI.WebControls.DataGrid dataGrid = (global::System.Web.UI.WebControls.DataGrid)base.GetBaseControl();
				FormatPage.FormatObject formatObject = new FormatPage.FormatStyle(dataGrid.ControlStyle);
				formatObject.LoadFormatInfo();
				FormatPage.FormatTreeNode formatTreeNode = new FormatPage.FormatTreeNode(SR.GetString("BDLFmt_Node_EntireDG"), formatObject);
				this.formatTree.Nodes.Add(formatTreeNode);
				this.formatNodes.Add(formatTreeNode);
				formatObject = new FormatPage.FormatStyle(dataGrid.HeaderStyle);
				formatObject.LoadFormatInfo();
				formatTreeNode = new FormatPage.FormatTreeNode(SR.GetString("BDLFmt_Node_Header"), formatObject);
				this.formatTree.Nodes.Add(formatTreeNode);
				this.formatNodes.Add(formatTreeNode);
				formatObject = new FormatPage.FormatStyle(dataGrid.FooterStyle);
				formatObject.LoadFormatInfo();
				formatTreeNode = new FormatPage.FormatTreeNode(SR.GetString("BDLFmt_Node_Footer"), formatObject);
				this.formatTree.Nodes.Add(formatTreeNode);
				this.formatNodes.Add(formatTreeNode);
				formatObject = new FormatPage.FormatStyle(dataGrid.PagerStyle);
				formatObject.LoadFormatInfo();
				formatTreeNode = new FormatPage.FormatTreeNode(SR.GetString("BDLFmt_Node_Pager"), formatObject);
				this.formatTree.Nodes.Add(formatTreeNode);
				this.formatNodes.Add(formatTreeNode);
				FormatPage.FormatTreeNode formatTreeNode2 = new FormatPage.FormatTreeNode(SR.GetString("BDLFmt_Node_Items"), null);
				this.formatTree.Nodes.Add(formatTreeNode2);
				formatObject = new FormatPage.FormatStyle(dataGrid.ItemStyle);
				formatObject.LoadFormatInfo();
				formatTreeNode = new FormatPage.FormatTreeNode(SR.GetString("BDLFmt_Node_NormalItems"), formatObject);
				formatTreeNode2.Nodes.Add(formatTreeNode);
				this.formatNodes.Add(formatTreeNode);
				formatObject = new FormatPage.FormatStyle(dataGrid.AlternatingItemStyle);
				formatObject.LoadFormatInfo();
				formatTreeNode = new FormatPage.FormatTreeNode(SR.GetString("BDLFmt_Node_AltItems"), formatObject);
				formatTreeNode2.Nodes.Add(formatTreeNode);
				this.formatNodes.Add(formatTreeNode);
				formatObject = new FormatPage.FormatStyle(dataGrid.SelectedItemStyle);
				formatObject.LoadFormatInfo();
				formatTreeNode = new FormatPage.FormatTreeNode(SR.GetString("BDLFmt_Node_SelItems"), formatObject);
				formatTreeNode2.Nodes.Add(formatTreeNode);
				this.formatNodes.Add(formatTreeNode);
				formatObject = new FormatPage.FormatStyle(dataGrid.EditItemStyle);
				formatObject.LoadFormatInfo();
				formatTreeNode = new FormatPage.FormatTreeNode(SR.GetString("BDLFmt_Node_EditItems"), formatObject);
				formatTreeNode2.Nodes.Add(formatTreeNode);
				this.formatNodes.Add(formatTreeNode);
				DataGridColumnCollection columns = dataGrid.Columns;
				int count = columns.Count;
				if (count != 0)
				{
					FormatPage.FormatTreeNode formatTreeNode3 = new FormatPage.FormatTreeNode(SR.GetString("BDLFmt_Node_Columns"), null);
					this.formatTree.Nodes.Add(formatTreeNode3);
					for (int i = 0; i < count; i++)
					{
						DataGridColumn dataGridColumn = columns[i];
						string text = "Columns[" + i.ToString(NumberFormatInfo.CurrentInfo) + "]";
						string headerText = dataGridColumn.HeaderText;
						if (headerText.Length != 0)
						{
							text = text + " - " + headerText;
						}
						formatObject = new FormatPage.FormatColumn(dataGridColumn);
						formatObject.LoadFormatInfo();
						FormatPage.FormatTreeNode formatTreeNode4 = new FormatPage.FormatTreeNode(text, formatObject);
						formatTreeNode3.Nodes.Add(formatTreeNode4);
						this.formatNodes.Add(formatTreeNode4);
						formatObject = new FormatPage.FormatStyle(dataGridColumn.HeaderStyle);
						formatObject.LoadFormatInfo();
						formatTreeNode = new FormatPage.FormatTreeNode(SR.GetString("BDLFmt_Node_Header"), formatObject);
						formatTreeNode4.Nodes.Add(formatTreeNode);
						this.formatNodes.Add(formatTreeNode);
						formatObject = new FormatPage.FormatStyle(dataGridColumn.FooterStyle);
						formatObject.LoadFormatInfo();
						formatTreeNode = new FormatPage.FormatTreeNode(SR.GetString("BDLFmt_Node_Footer"), formatObject);
						formatTreeNode4.Nodes.Add(formatTreeNode);
						this.formatNodes.Add(formatTreeNode);
						formatObject = new FormatPage.FormatStyle(dataGridColumn.ItemStyle);
						formatObject.LoadFormatInfo();
						formatTreeNode = new FormatPage.FormatTreeNode(SR.GetString("BDLFmt_Node_Items"), formatObject);
						formatTreeNode4.Nodes.Add(formatTreeNode);
						this.formatNodes.Add(formatTreeNode);
					}
					return;
				}
			}
			else
			{
				DataList dataList = (DataList)base.GetBaseControl();
				FormatPage.FormatObject formatObject = new FormatPage.FormatStyle(dataList.ControlStyle);
				formatObject.LoadFormatInfo();
				FormatPage.FormatTreeNode formatTreeNode = new FormatPage.FormatTreeNode(SR.GetString("BDLFmt_Node_EntireDL"), formatObject);
				this.formatTree.Nodes.Add(formatTreeNode);
				this.formatNodes.Add(formatTreeNode);
				formatObject = new FormatPage.FormatStyle(dataList.HeaderStyle);
				formatObject.LoadFormatInfo();
				formatTreeNode = new FormatPage.FormatTreeNode(SR.GetString("BDLFmt_Node_Header"), formatObject);
				this.formatTree.Nodes.Add(formatTreeNode);
				this.formatNodes.Add(formatTreeNode);
				formatObject = new FormatPage.FormatStyle(dataList.FooterStyle);
				formatObject.LoadFormatInfo();
				formatTreeNode = new FormatPage.FormatTreeNode(SR.GetString("BDLFmt_Node_Footer"), formatObject);
				this.formatTree.Nodes.Add(formatTreeNode);
				this.formatNodes.Add(formatTreeNode);
				FormatPage.FormatTreeNode formatTreeNode5 = new FormatPage.FormatTreeNode(SR.GetString("BDLFmt_Node_Items"), null);
				this.formatTree.Nodes.Add(formatTreeNode5);
				formatObject = new FormatPage.FormatStyle(dataList.ItemStyle);
				formatObject.LoadFormatInfo();
				formatTreeNode = new FormatPage.FormatTreeNode(SR.GetString("BDLFmt_Node_NormalItems"), formatObject);
				formatTreeNode5.Nodes.Add(formatTreeNode);
				this.formatNodes.Add(formatTreeNode);
				formatObject = new FormatPage.FormatStyle(dataList.AlternatingItemStyle);
				formatObject.LoadFormatInfo();
				formatTreeNode = new FormatPage.FormatTreeNode(SR.GetString("BDLFmt_Node_AltItems"), formatObject);
				formatTreeNode5.Nodes.Add(formatTreeNode);
				this.formatNodes.Add(formatTreeNode);
				formatObject = new FormatPage.FormatStyle(dataList.SelectedItemStyle);
				formatObject.LoadFormatInfo();
				formatTreeNode = new FormatPage.FormatTreeNode(SR.GetString("BDLFmt_Node_SelItems"), formatObject);
				formatTreeNode5.Nodes.Add(formatTreeNode);
				this.formatNodes.Add(formatTreeNode);
				formatObject = new FormatPage.FormatStyle(dataList.EditItemStyle);
				formatObject.LoadFormatInfo();
				formatTreeNode = new FormatPage.FormatTreeNode(SR.GetString("BDLFmt_Node_EditItems"), formatObject);
				formatTreeNode5.Nodes.Add(formatTreeNode);
				this.formatNodes.Add(formatTreeNode);
				formatObject = new FormatPage.FormatStyle(dataList.SeparatorStyle);
				formatObject.LoadFormatInfo();
				formatTreeNode = new FormatPage.FormatTreeNode(SR.GetString("BDLFmt_Node_Separators"), formatObject);
				this.formatTree.Nodes.Add(formatTreeNode);
				this.formatNodes.Add(formatTreeNode);
			}
		}

		private void InitFormatUI()
		{
			this.foreColorCombo.Color = null;
			this.backColorCombo.Color = null;
			this.fontNameCombo.Text = string.Empty;
			this.fontNameCombo.SelectedIndex = -1;
			this.fontSizeCombo.SelectedIndex = -1;
			this.fontSizeUnit.Value = null;
			this.italicCheck.Checked = false;
			this.underlineCheck.Checked = false;
			this.strikeOutCheck.Checked = false;
			this.overlineCheck.Checked = false;
			this.horzAlignCombo.SelectedIndex = -1;
			this.vertAlignCombo.SelectedIndex = -1;
			this.allowWrappingCheck.Checked = false;
			if (base.IsDataGridMode)
			{
				this.widthUnit.Value = null;
				this.columnPanel.Visible = false;
			}
			this.stylePanel.Visible = false;
		}

		private void InitPage()
		{
			this.formatNodes = new ArrayList();
			this.propChangesPending = false;
			this.fontNameChanged = false;
			this.currentFormatNode = null;
			this.currentFormatObject = null;
			this.formatTree.Nodes.Clear();
			this.InitFormatUI();
		}

		protected override void LoadComponent()
		{
			if (base.IsFirstActivate())
			{
				this.InitFontList();
			}
			this.InitPage();
			this.InitFormatTree();
		}

		private void LoadFormatProperties()
		{
			if (this.currentFormatObject != null)
			{
				base.EnterLoadingMode();
				this.InitFormatUI();
				if (this.currentFormatObject is FormatPage.FormatStyle)
				{
					FormatPage.FormatStyle formatStyle = (FormatPage.FormatStyle)this.currentFormatObject;
					this.foreColorCombo.Color = formatStyle.foreColor;
					this.backColorCombo.Color = formatStyle.backColor;
					int num = -1;
					if (formatStyle.fontName.Length != 0)
					{
						num = this.fontNameCombo.FindStringExact(formatStyle.fontName);
					}
					if (num != -1)
					{
						this.fontNameCombo.SelectedIndex = num;
					}
					else
					{
						this.fontNameCombo.Text = formatStyle.fontName;
					}
					this.boldCheck.Checked = formatStyle.bold;
					this.italicCheck.Checked = formatStyle.italic;
					this.underlineCheck.Checked = formatStyle.underline;
					this.strikeOutCheck.Checked = formatStyle.strikeOut;
					this.overlineCheck.Checked = formatStyle.overline;
					if (formatStyle.fontType != -1)
					{
						this.fontSizeCombo.SelectedIndex = formatStyle.fontType;
						if (formatStyle.fontType == 10)
						{
							this.fontSizeUnit.Value = formatStyle.fontSize;
						}
					}
					if (formatStyle.horzAlignment == 0)
					{
						this.horzAlignCombo.SelectedIndex = -1;
					}
					else
					{
						this.horzAlignCombo.SelectedIndex = formatStyle.horzAlignment;
					}
					if (formatStyle.vertAlignment == 0)
					{
						this.vertAlignCombo.SelectedIndex = -1;
					}
					else
					{
						this.vertAlignCombo.SelectedIndex = formatStyle.vertAlignment;
					}
					this.allowWrappingCheck.Checked = formatStyle.allowWrapping;
				}
				else
				{
					FormatPage.FormatColumn formatColumn = (FormatPage.FormatColumn)this.currentFormatObject;
					this.widthUnit.Value = formatColumn.width;
				}
				base.ExitLoadingMode();
			}
			this.UpdateEnabledVisibleState();
		}

		private void OnClickBackColorPicker(object source, EventArgs e)
		{
			string text = this.backColorCombo.Color;
			text = ColorBuilder.BuildColor(base.GetBaseControl(), this, text);
			if (text != null)
			{
				this.backColorCombo.Color = text;
				this.OnFormatChanged(this.backColorCombo, EventArgs.Empty);
			}
		}

		private void OnClickForeColorPicker(object source, EventArgs e)
		{
			string text = this.foreColorCombo.Color;
			text = ColorBuilder.BuildColor(base.GetBaseControl(), this, text);
			if (text != null)
			{
				this.foreColorCombo.Color = text;
				this.OnFormatChanged(this.foreColorCombo, EventArgs.Empty);
			}
		}

		private void OnFontNameChanged(object source, EventArgs e)
		{
			if (base.IsLoading())
			{
				return;
			}
			this.fontNameChanged = true;
			this.OnFormatChanged(this.fontNameCombo, EventArgs.Empty);
		}

		private void OnFontSizeChanged(object source, EventArgs e)
		{
			if (base.IsLoading())
			{
				return;
			}
			this.UpdateEnabledVisibleState();
			this.OnFormatChanged(this.fontSizeCombo, EventArgs.Empty);
		}

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);
			if (this.formatTree.Nodes.Count != 0)
			{
				IntPtr handle = this.formatTree.Handle;
				this.formatTree.SelectedNode = this.formatTree.Nodes[0];
			}
		}

		private void OnFormatChanged(object source, EventArgs e)
		{
			if (base.IsLoading())
			{
				return;
			}
			if (this.currentFormatNode != null)
			{
				this.SetDirty();
				this.propChangesPending = true;
				this.currentFormatNode.Dirty = true;
			}
		}

		private void OnSelChangedFormatObject(object source, TreeViewEventArgs e)
		{
			if (this.propChangesPending)
			{
				this.SaveFormatProperties();
			}
			this.currentFormatNode = (FormatPage.FormatTreeNode)this.formatTree.SelectedNode;
			if (this.currentFormatNode != null)
			{
				this.currentFormatObject = this.currentFormatNode.FormatObject;
			}
			else
			{
				this.currentFormatObject = null;
			}
			this.LoadFormatProperties();
		}

		protected override void SaveComponent()
		{
			if (this.propChangesPending)
			{
				this.SaveFormatProperties();
			}
			foreach (object obj in this.formatNodes)
			{
				FormatPage.FormatTreeNode formatTreeNode = (FormatPage.FormatTreeNode)obj;
				if (formatTreeNode.Dirty)
				{
					FormatPage.FormatObject formatObject = formatTreeNode.FormatObject;
					formatObject.SaveFormatInfo();
					formatTreeNode.Dirty = false;
				}
			}
			BaseDataListDesigner baseDesigner = base.GetBaseDesigner();
			baseDesigner.OnStylesChanged();
		}

		private void SaveFormatProperties()
		{
			if (this.currentFormatObject != null)
			{
				if (this.currentFormatObject is FormatPage.FormatStyle)
				{
					FormatPage.FormatStyle formatStyle = (FormatPage.FormatStyle)this.currentFormatObject;
					formatStyle.foreColor = this.foreColorCombo.Color;
					formatStyle.backColor = this.backColorCombo.Color;
					if (this.fontNameChanged)
					{
						formatStyle.fontName = this.fontNameCombo.Text.Trim();
						formatStyle.fontNameChanged = true;
						this.fontNameChanged = false;
					}
					formatStyle.bold = this.boldCheck.Checked;
					formatStyle.italic = this.italicCheck.Checked;
					formatStyle.underline = this.underlineCheck.Checked;
					formatStyle.strikeOut = this.strikeOutCheck.Checked;
					formatStyle.overline = this.overlineCheck.Checked;
					if (this.fontSizeCombo.IsSet())
					{
						formatStyle.fontType = this.fontSizeCombo.SelectedIndex;
						if (formatStyle.fontType == 10)
						{
							formatStyle.fontSize = this.fontSizeUnit.Value;
						}
					}
					else
					{
						formatStyle.fontType = -1;
					}
					int num = this.horzAlignCombo.SelectedIndex;
					if (num == -1)
					{
						num = 0;
					}
					formatStyle.horzAlignment = num;
					num = this.vertAlignCombo.SelectedIndex;
					if (num == -1)
					{
						num = 0;
					}
					formatStyle.vertAlignment = num;
					formatStyle.allowWrapping = this.allowWrappingCheck.Checked;
				}
				else
				{
					FormatPage.FormatColumn formatColumn = (FormatPage.FormatColumn)this.currentFormatObject;
					formatColumn.width = this.widthUnit.Value;
				}
				this.currentFormatNode.Dirty = true;
			}
			this.propChangesPending = false;
		}

		public override void SetComponent(IComponent component)
		{
			base.SetComponent(component);
			this.InitForm();
		}

		private void UpdateEnabledVisibleState()
		{
			if (this.currentFormatObject == null)
			{
				this.stylePanel.Visible = false;
				if (base.IsDataGridMode)
				{
					this.columnPanel.Visible = false;
					return;
				}
			}
			else if (this.currentFormatObject is FormatPage.FormatStyle)
			{
				this.stylePanel.Visible = true;
				if (base.IsDataGridMode)
				{
					this.columnPanel.Visible = false;
				}
				this.fontSizeUnit.Enabled = this.fontSizeCombo.SelectedIndex == 10;
				if (((FormatPage.FormatStyle)this.currentFormatObject).IsTableItemStyle)
				{
					this.vertAlignLabel.Visible = true;
					this.vertAlignCombo.Visible = true;
					this.allowWrappingCheck.Visible = true;
					return;
				}
				this.vertAlignLabel.Visible = false;
				this.vertAlignCombo.Visible = false;
				this.allowWrappingCheck.Visible = false;
				return;
			}
			else
			{
				this.stylePanel.Visible = false;
				this.columnPanel.Visible = true;
			}
		}

		private const int IDX_ENTIRE = 0;

		private const int IDX_PAGER = 1;

		private const int IDX_HEADER = 0;

		private const int IDX_FOOTER = 1;

		private const int IDX_ROW_NORMAL = 2;

		private const int IDX_ROW_ALT = 3;

		private const int IDX_ROW_SELECTED = 4;

		private const int IDX_ROW_EDIT = 5;

		private const int ROW_TYPE_COUNT = 6;

		private const int COL_ROW_TYPE_COUNT = 3;

		private const int IDX_ITEM_NORMAL = 2;

		private const int IDX_ITEM_ALT = 3;

		private const int IDX_ITEM_SELECTED = 4;

		private const int IDX_ITEM_EDIT = 5;

		private const int IDX_ITEM_SEPARATOR = 6;

		private const int ITEM_TYPE_COUNT = 7;

		private const int IDX_FSIZE_SMALLER = 1;

		private const int IDX_FSIZE_LARGER = 2;

		private const int IDX_FSIZE_XXSMALL = 3;

		private const int IDX_FSIZE_XSMALL = 4;

		private const int IDX_FSIZE_SMALL = 5;

		private const int IDX_FSIZE_MEDIUM = 6;

		private const int IDX_FSIZE_LARGE = 7;

		private const int IDX_FSIZE_XLARGE = 8;

		private const int IDX_FSIZE_XXLARGE = 9;

		private const int IDX_FSIZE_CUSTOM = 10;

		private const int IDX_HALIGN_NOTSET = 0;

		private const int IDX_HALIGN_LEFT = 1;

		private const int IDX_HALIGN_CENTER = 2;

		private const int IDX_HALIGN_RIGHT = 3;

		private const int IDX_HALIGN_JUSTIFY = 4;

		private const int IDX_VALIGN_NOTSET = 0;

		private const int IDX_VALIGN_TOP = 1;

		private const int IDX_VALIGN_MIDDLE = 2;

		private const int IDX_VALIGN_BOTTOM = 3;

		private global::System.Windows.Forms.TreeView formatTree;

		private global::System.Windows.Forms.Panel stylePanel;

		private ColorComboBox foreColorCombo;

		private global::System.Windows.Forms.Button foreColorPickerButton;

		private ColorComboBox backColorCombo;

		private global::System.Windows.Forms.Button backColorPickerButton;

		private ComboBox fontNameCombo;

		private UnsettableComboBox fontSizeCombo;

		private UnitControl fontSizeUnit;

		private global::System.Windows.Forms.CheckBox boldCheck;

		private global::System.Windows.Forms.CheckBox italicCheck;

		private global::System.Windows.Forms.CheckBox underlineCheck;

		private global::System.Windows.Forms.CheckBox strikeOutCheck;

		private global::System.Windows.Forms.CheckBox overlineCheck;

		private global::System.Windows.Forms.Panel columnPanel;

		private UnitControl widthUnit;

		private global::System.Windows.Forms.CheckBox allowWrappingCheck;

		private UnsettableComboBox horzAlignCombo;

		private global::System.Windows.Forms.Label vertAlignLabel;

		private UnsettableComboBox vertAlignCombo;

		private FormatPage.FormatObject currentFormatObject;

		private FormatPage.FormatTreeNode currentFormatNode;

		private bool propChangesPending;

		private bool fontNameChanged;

		private ArrayList formatNodes;

		private class FormatTreeNode : global::System.Windows.Forms.TreeNode
		{
			public FormatTreeNode(string text, FormatPage.FormatObject formatObject)
				: base(text)
			{
				this.formatObject = formatObject;
			}

			public bool Dirty
			{
				get
				{
					return this.dirty;
				}
				set
				{
					this.dirty = value;
				}
			}

			public FormatPage.FormatObject FormatObject
			{
				get
				{
					return this.formatObject;
				}
			}

			protected FormatPage.FormatObject formatObject;

			protected bool dirty;
		}

		private abstract class FormatObject
		{
			public abstract void LoadFormatInfo();

			public abstract void SaveFormatInfo();
		}

		private class FormatStyle : FormatPage.FormatObject
		{
			public FormatStyle(Style runtimeStyle)
			{
				this.runtimeStyle = runtimeStyle;
			}

			public bool IsTableItemStyle
			{
				get
				{
					return this.runtimeStyle is TableItemStyle;
				}
			}

			public override void LoadFormatInfo()
			{
				Color color = this.runtimeStyle.BackColor;
				this.backColor = ColorTranslator.ToHtml(color);
				color = this.runtimeStyle.ForeColor;
				this.foreColor = ColorTranslator.ToHtml(color);
				FontInfo font = this.runtimeStyle.Font;
				this.fontName = font.Name;
				this.fontNameChanged = false;
				this.bold = font.Bold;
				this.italic = font.Italic;
				this.underline = font.Underline;
				this.strikeOut = font.Strikeout;
				this.overline = font.Overline;
				this.fontType = -1;
				FontUnit size = font.Size;
				if (!size.IsEmpty)
				{
					this.fontSize = null;
					switch (size.Type)
					{
					case FontSize.AsUnit:
						this.fontType = 10;
						this.fontSize = size.ToString(CultureInfo.CurrentCulture);
						break;
					case FontSize.Smaller:
						this.fontType = 1;
						break;
					case FontSize.Larger:
						this.fontType = 2;
						break;
					case FontSize.XXSmall:
						this.fontType = 3;
						break;
					case FontSize.XSmall:
						this.fontType = 4;
						break;
					case FontSize.Small:
						this.fontType = 5;
						break;
					case FontSize.Medium:
						this.fontType = 6;
						break;
					case FontSize.Large:
						this.fontType = 7;
						break;
					case FontSize.XLarge:
						this.fontType = 8;
						break;
					case FontSize.XXLarge:
						this.fontType = 9;
						break;
					}
				}
				TableItemStyle tableItemStyle = null;
				HorizontalAlign horizontalAlign;
				if (this.runtimeStyle is TableItemStyle)
				{
					tableItemStyle = (TableItemStyle)this.runtimeStyle;
					horizontalAlign = tableItemStyle.HorizontalAlign;
					this.allowWrapping = tableItemStyle.Wrap;
				}
				else
				{
					horizontalAlign = ((TableStyle)this.runtimeStyle).HorizontalAlign;
				}
				this.horzAlignment = 0;
				switch (horizontalAlign)
				{
				case HorizontalAlign.Left:
					this.horzAlignment = 1;
					break;
				case HorizontalAlign.Center:
					this.horzAlignment = 2;
					break;
				case HorizontalAlign.Right:
					this.horzAlignment = 3;
					break;
				case HorizontalAlign.Justify:
					this.horzAlignment = 4;
					break;
				}
				if (tableItemStyle != null)
				{
					VerticalAlign verticalAlign = tableItemStyle.VerticalAlign;
					this.vertAlignment = 0;
					switch (verticalAlign)
					{
					case VerticalAlign.Top:
						this.vertAlignment = 1;
						return;
					case VerticalAlign.Middle:
						this.vertAlignment = 2;
						return;
					case VerticalAlign.Bottom:
						this.vertAlignment = 3;
						break;
					default:
						return;
					}
				}
			}

			public override void SaveFormatInfo()
			{
				try
				{
					this.runtimeStyle.BackColor = ColorTranslator.FromHtml(this.backColor);
					this.runtimeStyle.ForeColor = ColorTranslator.FromHtml(this.foreColor);
				}
				catch
				{
				}
				FontInfo font = this.runtimeStyle.Font;
				if (this.fontNameChanged)
				{
					font.Name = this.fontName;
					this.fontNameChanged = false;
				}
				font.Bold = this.bold;
				font.Italic = this.italic;
				font.Underline = this.underline;
				font.Strikeout = this.strikeOut;
				font.Overline = this.overline;
				if (this.fontType != -1)
				{
					switch (this.fontType)
					{
					case 1:
						break;
					case 2:
						font.Size = FontUnit.Larger;
						goto IL_017D;
					case 3:
						font.Size = FontUnit.XXSmall;
						goto IL_017D;
					case 4:
						font.Size = FontUnit.XSmall;
						goto IL_017D;
					case 5:
						font.Size = FontUnit.Small;
						goto IL_017D;
					case 6:
						font.Size = FontUnit.Medium;
						goto IL_017D;
					case 7:
						font.Size = FontUnit.Large;
						goto IL_017D;
					case 8:
						font.Size = FontUnit.XLarge;
						goto IL_017D;
					case 9:
						font.Size = FontUnit.XXLarge;
						goto IL_017D;
					case 10:
						try
						{
							font.Size = new FontUnit(this.fontSize, CultureInfo.InvariantCulture);
							goto IL_017D;
						}
						catch
						{
							goto IL_017D;
						}
						break;
					default:
						goto IL_017D;
					}
					font.Size = FontUnit.Smaller;
				}
				else
				{
					font.Size = FontUnit.Empty;
				}
				IL_017D:
				TableItemStyle tableItemStyle = null;
				HorizontalAlign horizontalAlign = HorizontalAlign.NotSet;
				switch (this.horzAlignment)
				{
				case 0:
					horizontalAlign = HorizontalAlign.NotSet;
					break;
				case 1:
					horizontalAlign = HorizontalAlign.Left;
					break;
				case 2:
					horizontalAlign = HorizontalAlign.Center;
					break;
				case 3:
					horizontalAlign = HorizontalAlign.Right;
					break;
				case 4:
					horizontalAlign = HorizontalAlign.Justify;
					break;
				}
				if (this.runtimeStyle is TableItemStyle)
				{
					tableItemStyle = (TableItemStyle)this.runtimeStyle;
					tableItemStyle.HorizontalAlign = horizontalAlign;
					if (!this.allowWrapping)
					{
						tableItemStyle.Wrap = false;
					}
				}
				else
				{
					((TableStyle)this.runtimeStyle).HorizontalAlign = horizontalAlign;
				}
				if (tableItemStyle != null)
				{
					switch (this.vertAlignment)
					{
					case 0:
						tableItemStyle.VerticalAlign = VerticalAlign.NotSet;
						return;
					case 1:
						tableItemStyle.VerticalAlign = VerticalAlign.Top;
						return;
					case 2:
						tableItemStyle.VerticalAlign = VerticalAlign.Middle;
						return;
					case 3:
						tableItemStyle.VerticalAlign = VerticalAlign.Bottom;
						break;
					default:
						return;
					}
				}
			}

			public string foreColor;

			public string backColor;

			public string fontName;

			public bool fontNameChanged;

			public int fontType;

			public string fontSize;

			public bool bold;

			public bool italic;

			public bool underline;

			public bool strikeOut;

			public bool overline;

			public int horzAlignment;

			public int vertAlignment;

			public bool allowWrapping;

			protected Style runtimeStyle;
		}

		private class FormatColumn : FormatPage.FormatObject
		{
			public FormatColumn(DataGridColumn runtimeColumn)
			{
				this.runtimeColumn = runtimeColumn;
			}

			public override void LoadFormatInfo()
			{
				TableItemStyle headerStyle = this.runtimeColumn.HeaderStyle;
				if (!headerStyle.Width.IsEmpty)
				{
					this.width = headerStyle.Width.ToString(NumberFormatInfo.CurrentInfo);
					return;
				}
				this.width = null;
			}

			public override void SaveFormatInfo()
			{
				TableItemStyle headerStyle = this.runtimeColumn.HeaderStyle;
				if (this.width == null)
				{
					headerStyle.Width = Unit.Empty;
					return;
				}
				try
				{
					headerStyle.Width = new Unit(this.width, CultureInfo.InvariantCulture);
				}
				catch
				{
				}
			}

			public string width;

			protected DataGridColumn runtimeColumn;
		}
	}
}
