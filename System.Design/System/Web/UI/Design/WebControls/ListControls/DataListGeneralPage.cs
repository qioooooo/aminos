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
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed class DataListGeneralPage : BaseDataListPage
	{
		protected override string HelpKeyword
		{
			get
			{
				return "net.Asp.DataListProperties.General";
			}
		}

		private void InitForm()
		{
			GroupLabel groupLabel = new GroupLabel();
			this.showHeaderCheck = new global::System.Windows.Forms.CheckBox();
			this.showFooterCheck = new global::System.Windows.Forms.CheckBox();
			GroupLabel groupLabel2 = new GroupLabel();
			global::System.Windows.Forms.Label label = new global::System.Windows.Forms.Label();
			this.repeatColumnsEdit = new NumberEdit();
			global::System.Windows.Forms.Label label2 = new global::System.Windows.Forms.Label();
			this.repeatDirectionCombo = new ComboBox();
			global::System.Windows.Forms.Label label3 = new global::System.Windows.Forms.Label();
			this.repeatLayoutCombo = new ComboBox();
			GroupLabel groupLabel3 = new GroupLabel();
			this.extractRowsCheck = new global::System.Windows.Forms.CheckBox();
			groupLabel.SetBounds(4, 4, 360, 16);
			groupLabel.Text = SR.GetString("DLGen_HeaderFooterGroup");
			groupLabel.TabIndex = 7;
			groupLabel.TabStop = false;
			this.showHeaderCheck.SetBounds(8, 24, 170, 16);
			this.showHeaderCheck.TabIndex = 8;
			this.showHeaderCheck.Text = SR.GetString("DLGen_ShowHeader");
			this.showHeaderCheck.TextAlign = ContentAlignment.MiddleLeft;
			this.showHeaderCheck.FlatStyle = FlatStyle.System;
			this.showHeaderCheck.CheckedChanged += this.OnCheckChangedShowHeader;
			this.showFooterCheck.SetBounds(8, 42, 170, 16);
			this.showFooterCheck.TabIndex = 9;
			this.showFooterCheck.Text = SR.GetString("DLGen_ShowFooter");
			this.showFooterCheck.TextAlign = ContentAlignment.MiddleLeft;
			this.showFooterCheck.FlatStyle = FlatStyle.System;
			this.showFooterCheck.CheckedChanged += this.OnCheckChangedShowFooter;
			groupLabel2.SetBounds(4, 68, 360, 16);
			groupLabel2.Text = SR.GetString("DLGen_RepeatLayoutGroup");
			groupLabel2.TabIndex = 10;
			groupLabel2.TabStop = false;
			label.SetBounds(8, 88, 106, 16);
			label.Text = SR.GetString("DLGen_RepeatColumns");
			label.TabStop = false;
			label.TabIndex = 11;
			this.repeatColumnsEdit.SetBounds(112, 84, 40, 21);
			this.repeatColumnsEdit.AllowDecimal = false;
			this.repeatColumnsEdit.AllowNegative = false;
			this.repeatColumnsEdit.TabIndex = 12;
			this.repeatColumnsEdit.TextChanged += this.OnChangedRepeatProps;
			label2.SetBounds(8, 113, 106, 16);
			label2.Text = SR.GetString("DLGen_RepeatDirection");
			label2.TabStop = false;
			label2.TabIndex = 13;
			this.repeatDirectionCombo.SetBounds(112, 109, 140, 56);
			this.repeatDirectionCombo.DropDownStyle = ComboBoxStyle.DropDownList;
			this.repeatDirectionCombo.Items.AddRange(new object[]
			{
				SR.GetString("DLGen_RD_Horz"),
				SR.GetString("DLGen_RD_Vert")
			});
			this.repeatDirectionCombo.TabIndex = 14;
			this.repeatDirectionCombo.SelectedIndexChanged += this.OnChangedRepeatProps;
			label3.SetBounds(8, 138, 106, 16);
			label3.Text = SR.GetString("DLGen_RepeatLayout");
			label3.TabStop = false;
			label3.TabIndex = 15;
			this.repeatLayoutCombo.SetBounds(112, 134, 140, 21);
			this.repeatLayoutCombo.DropDownStyle = ComboBoxStyle.DropDownList;
			this.repeatLayoutCombo.Items.AddRange(new object[]
			{
				SR.GetString("DLGen_RL_Table"),
				SR.GetString("DLGen_RL_Flow")
			});
			this.repeatLayoutCombo.TabIndex = 16;
			this.repeatLayoutCombo.SelectedIndexChanged += this.OnChangedRepeatProps;
			groupLabel3.SetBounds(4, 162, 360, 16);
			groupLabel3.Text = SR.GetString("DLGen_Templates");
			groupLabel3.TabIndex = 17;
			groupLabel3.TabStop = false;
			groupLabel3.Visible = false;
			this.extractRowsCheck.SetBounds(8, 182, 260, 16);
			this.extractRowsCheck.Text = SR.GetString("DLGen_ExtractRows");
			this.extractRowsCheck.TabIndex = 18;
			this.extractRowsCheck.Visible = false;
			this.extractRowsCheck.FlatStyle = FlatStyle.System;
			this.extractRowsCheck.CheckedChanged += this.OnCheckChangedExtractRows;
			this.Text = SR.GetString("DLGen_Text");
			base.AccessibleDescription = SR.GetString("DLGen_Desc");
			base.Size = new Size(368, 280);
			base.CommitOnDeactivate = true;
			base.Icon = new Icon(base.GetType(), "DataListGeneralPage.ico");
			base.Controls.Clear();
			base.Controls.AddRange(new Control[]
			{
				this.extractRowsCheck, groupLabel3, this.repeatLayoutCombo, label3, this.repeatDirectionCombo, label2, this.repeatColumnsEdit, label, groupLabel2, this.showFooterCheck,
				this.showHeaderCheck, groupLabel
			});
		}

		private void InitPage()
		{
			this.showHeaderCheck.Checked = false;
			this.showFooterCheck.Checked = false;
			this.repeatColumnsEdit.Clear();
			this.repeatDirectionCombo.SelectedIndex = -1;
			this.repeatLayoutCombo.SelectedIndex = -1;
			this.extractRowsCheck.Checked = false;
		}

		protected override void LoadComponent()
		{
			this.InitPage();
			DataList dataList = (DataList)base.GetBaseControl();
			this.showHeaderCheck.Checked = dataList.ShowHeader;
			this.showFooterCheck.Checked = dataList.ShowFooter;
			this.repeatColumnsEdit.Text = dataList.RepeatColumns.ToString(NumberFormatInfo.CurrentInfo);
			switch (dataList.RepeatDirection)
			{
			case RepeatDirection.Horizontal:
				this.repeatDirectionCombo.SelectedIndex = 0;
				break;
			case RepeatDirection.Vertical:
				this.repeatDirectionCombo.SelectedIndex = 1;
				break;
			}
			switch (dataList.RepeatLayout)
			{
			case RepeatLayout.Table:
				this.repeatLayoutCombo.SelectedIndex = 0;
				break;
			case RepeatLayout.Flow:
				this.repeatLayoutCombo.SelectedIndex = 1;
				break;
			}
			this.extractRowsCheck.Checked = dataList.ExtractTemplateRows;
		}

		private void OnCheckChangedExtractRows(object source, EventArgs e)
		{
			if (base.IsLoading())
			{
				return;
			}
			this.SetDirty();
		}

		private void OnChangedRepeatProps(object source, EventArgs e)
		{
			if (base.IsLoading())
			{
				return;
			}
			this.SetDirty();
		}

		private void OnCheckChangedShowHeader(object source, EventArgs e)
		{
			if (base.IsLoading())
			{
				return;
			}
			this.SetDirty();
		}

		private void OnCheckChangedShowFooter(object source, EventArgs e)
		{
			if (base.IsLoading())
			{
				return;
			}
			this.SetDirty();
		}

		protected override void SaveComponent()
		{
			DataList dataList = (DataList)base.GetBaseControl();
			dataList.ShowHeader = this.showHeaderCheck.Checked;
			dataList.ShowFooter = this.showFooterCheck.Checked;
			string text = this.repeatColumnsEdit.Text.Trim();
			if (text.Length != 0)
			{
				try
				{
					dataList.RepeatColumns = int.Parse(text, CultureInfo.CurrentCulture);
				}
				catch
				{
					this.repeatColumnsEdit.Text = dataList.RepeatColumns.ToString(CultureInfo.CurrentCulture);
				}
			}
			switch (this.repeatDirectionCombo.SelectedIndex)
			{
			case 0:
				dataList.RepeatDirection = RepeatDirection.Horizontal;
				break;
			case 1:
				dataList.RepeatDirection = RepeatDirection.Vertical;
				break;
			}
			switch (this.repeatLayoutCombo.SelectedIndex)
			{
			case 0:
				dataList.RepeatLayout = RepeatLayout.Table;
				break;
			case 1:
				dataList.RepeatLayout = RepeatLayout.Flow;
				break;
			}
			dataList.ExtractTemplateRows = this.extractRowsCheck.Checked;
		}

		public override void SetComponent(IComponent component)
		{
			base.SetComponent(component);
			this.InitForm();
		}

		private const int IDX_DIR_HORIZONTAL = 0;

		private const int IDX_DIR_VERTICAL = 1;

		private const int IDX_MODE_TABLE = 0;

		private const int IDX_MODE_FLOW = 1;

		private global::System.Windows.Forms.CheckBox showHeaderCheck;

		private global::System.Windows.Forms.CheckBox showFooterCheck;

		private NumberEdit repeatColumnsEdit;

		private ComboBox repeatDirectionCombo;

		private ComboBox repeatLayoutCombo;

		private global::System.Windows.Forms.CheckBox extractRowsCheck;
	}
}
