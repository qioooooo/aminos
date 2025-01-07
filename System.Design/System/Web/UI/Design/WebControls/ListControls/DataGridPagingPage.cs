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
	internal sealed class DataGridPagingPage : BaseDataListPage
	{
		protected override string HelpKeyword
		{
			get
			{
				return "net.Asp.DataGridProperties.Paging";
			}
		}

		private void InitForm()
		{
			GroupLabel groupLabel = new GroupLabel();
			this.allowPagingCheck = new global::System.Windows.Forms.CheckBox();
			this.allowCustomPagingCheck = new global::System.Windows.Forms.CheckBox();
			global::System.Windows.Forms.Label label = new global::System.Windows.Forms.Label();
			this.pageSizeEdit = new NumberEdit();
			global::System.Windows.Forms.Label label2 = new global::System.Windows.Forms.Label();
			GroupLabel groupLabel2 = new GroupLabel();
			this.visibleCheck = new global::System.Windows.Forms.CheckBox();
			global::System.Windows.Forms.Label label3 = new global::System.Windows.Forms.Label();
			this.posCombo = new ComboBox();
			global::System.Windows.Forms.Label label4 = new global::System.Windows.Forms.Label();
			this.modeCombo = new ComboBox();
			global::System.Windows.Forms.Label label5 = new global::System.Windows.Forms.Label();
			this.nextPageTextEdit = new global::System.Windows.Forms.TextBox();
			global::System.Windows.Forms.Label label6 = new global::System.Windows.Forms.Label();
			this.prevPageTextEdit = new global::System.Windows.Forms.TextBox();
			global::System.Windows.Forms.Label label7 = new global::System.Windows.Forms.Label();
			this.pageButtonCountEdit = new NumberEdit();
			groupLabel.SetBounds(4, 4, 431, 16);
			groupLabel.Text = SR.GetString("DGPg_PagingGroup");
			groupLabel.TabStop = false;
			groupLabel.TabIndex = 0;
			this.allowPagingCheck.SetBounds(12, 24, 180, 16);
			this.allowPagingCheck.Text = SR.GetString("DGPg_AllowPaging");
			this.allowPagingCheck.TextAlign = ContentAlignment.MiddleLeft;
			this.allowPagingCheck.TabIndex = 1;
			this.allowPagingCheck.FlatStyle = FlatStyle.System;
			this.allowPagingCheck.CheckedChanged += this.OnCheckChangedAllowPaging;
			this.allowCustomPagingCheck.SetBounds(220, 24, 180, 16);
			this.allowCustomPagingCheck.Text = SR.GetString("DGPg_AllowCustomPaging");
			this.allowCustomPagingCheck.TextAlign = ContentAlignment.MiddleLeft;
			this.allowCustomPagingCheck.TabIndex = 2;
			this.allowCustomPagingCheck.FlatStyle = FlatStyle.System;
			this.allowCustomPagingCheck.CheckedChanged += this.OnCheckChangedAllowCustomPaging;
			label.SetBounds(12, 50, 100, 14);
			label.Text = SR.GetString("DGPg_PageSize");
			label.TabStop = false;
			label.TabIndex = 3;
			this.pageSizeEdit.SetBounds(112, 46, 40, 24);
			this.pageSizeEdit.TabIndex = 4;
			this.pageSizeEdit.AllowDecimal = false;
			this.pageSizeEdit.AllowNegative = false;
			this.pageSizeEdit.TextChanged += this.OnTextChangedPageSize;
			label2.SetBounds(158, 50, 80, 14);
			label2.Text = SR.GetString("DGPg_Rows");
			label2.TabStop = false;
			label2.TabIndex = 5;
			groupLabel2.SetBounds(4, 78, 431, 14);
			groupLabel2.Text = SR.GetString("DGPg_NavigationGroup");
			groupLabel2.TabStop = false;
			groupLabel2.TabIndex = 6;
			this.visibleCheck.SetBounds(12, 100, 260, 16);
			this.visibleCheck.Text = SR.GetString("DGPg_Visible");
			this.visibleCheck.TextAlign = ContentAlignment.MiddleLeft;
			this.visibleCheck.TabIndex = 7;
			this.visibleCheck.FlatStyle = FlatStyle.System;
			this.visibleCheck.CheckedChanged += this.OnCheckChangedVisible;
			label3.SetBounds(12, 122, 150, 14);
			label3.Text = SR.GetString("DGPg_Position");
			label3.TabStop = false;
			label3.TabIndex = 8;
			this.posCombo.SetBounds(12, 138, 144, 21);
			this.posCombo.TabIndex = 9;
			this.posCombo.DropDownStyle = ComboBoxStyle.DropDownList;
			this.posCombo.Items.AddRange(new object[]
			{
				SR.GetString("DGPg_Pos_Top"),
				SR.GetString("DGPg_Pos_Bottom"),
				SR.GetString("DGPg_Pos_TopBottom")
			});
			this.posCombo.SelectedIndexChanged += this.OnPagerChanged;
			label4.SetBounds(12, 166, 150, 14);
			label4.Text = SR.GetString("DGPg_Mode");
			label4.TabStop = false;
			label4.TabIndex = 10;
			this.modeCombo.SetBounds(12, 182, 144, 64);
			this.modeCombo.TabIndex = 11;
			this.modeCombo.DropDownStyle = ComboBoxStyle.DropDownList;
			this.modeCombo.Items.AddRange(new object[]
			{
				SR.GetString("DGPg_Mode_Buttons"),
				SR.GetString("DGPg_Mode_Numbers")
			});
			this.modeCombo.SelectedIndexChanged += this.OnPagerChanged;
			label5.SetBounds(12, 210, 200, 14);
			label5.Text = SR.GetString("DGPg_NextPage");
			label5.TabStop = false;
			label5.TabIndex = 12;
			this.nextPageTextEdit.SetBounds(12, 226, 144, 24);
			this.nextPageTextEdit.TabIndex = 13;
			this.nextPageTextEdit.TextChanged += this.OnPagerChanged;
			label6.SetBounds(220, 210, 200, 14);
			label6.Text = SR.GetString("DGPg_PrevPage");
			label6.TabStop = false;
			label6.TabIndex = 14;
			this.prevPageTextEdit.SetBounds(220, 226, 140, 24);
			this.prevPageTextEdit.TabIndex = 15;
			this.prevPageTextEdit.TextChanged += this.OnPagerChanged;
			label7.SetBounds(12, 254, 200, 14);
			label7.Text = SR.GetString("DGPg_ButtonCount");
			label7.TabStop = false;
			label7.TabIndex = 16;
			this.pageButtonCountEdit.SetBounds(12, 270, 40, 24);
			this.pageButtonCountEdit.TabIndex = 17;
			this.pageButtonCountEdit.AllowDecimal = false;
			this.pageButtonCountEdit.AllowNegative = false;
			this.pageButtonCountEdit.TextChanged += this.OnPagerChanged;
			this.Text = SR.GetString("DGPg_Text");
			base.AccessibleDescription = SR.GetString("DGPg_Desc");
			base.Size = new Size(464, 300);
			base.CommitOnDeactivate = true;
			base.Icon = new Icon(base.GetType(), "DataGridPagingPage.ico");
			base.Controls.Clear();
			base.Controls.AddRange(new Control[]
			{
				this.pageButtonCountEdit, label7, this.prevPageTextEdit, label6, this.nextPageTextEdit, label5, this.modeCombo, label4, this.posCombo, label3,
				this.visibleCheck, groupLabel2, label2, this.pageSizeEdit, label, this.allowCustomPagingCheck, this.allowPagingCheck, groupLabel
			});
		}

		private void InitPage()
		{
			this.pageSizeEdit.Clear();
			this.visibleCheck.Checked = false;
			this.posCombo.SelectedIndex = -1;
			this.modeCombo.SelectedIndex = -1;
			this.nextPageTextEdit.Clear();
			this.prevPageTextEdit.Clear();
		}

		protected override void LoadComponent()
		{
			this.InitPage();
			global::System.Web.UI.WebControls.DataGrid dataGrid = (global::System.Web.UI.WebControls.DataGrid)base.GetBaseControl();
			DataGridPagerStyle pagerStyle = dataGrid.PagerStyle;
			this.allowPagingCheck.Checked = dataGrid.AllowPaging;
			this.allowCustomPagingCheck.Checked = dataGrid.AllowCustomPaging;
			this.pageSizeEdit.Text = dataGrid.PageSize.ToString(NumberFormatInfo.CurrentInfo);
			this.visibleCheck.Checked = pagerStyle.Visible;
			switch (pagerStyle.Mode)
			{
			case PagerMode.NextPrev:
				this.modeCombo.SelectedIndex = 0;
				break;
			case PagerMode.NumericPages:
				this.modeCombo.SelectedIndex = 1;
				break;
			}
			switch (pagerStyle.Position)
			{
			case PagerPosition.Bottom:
				this.posCombo.SelectedIndex = 1;
				break;
			case PagerPosition.Top:
				this.posCombo.SelectedIndex = 0;
				break;
			case PagerPosition.TopAndBottom:
				this.posCombo.SelectedIndex = 2;
				break;
			}
			this.nextPageTextEdit.Text = pagerStyle.NextPageText;
			this.prevPageTextEdit.Text = pagerStyle.PrevPageText;
			this.pageButtonCountEdit.Text = pagerStyle.PageButtonCount.ToString(NumberFormatInfo.CurrentInfo);
			this.UpdateEnabledVisibleState();
		}

		private void OnCheckChangedAllowCustomPaging(object source, EventArgs e)
		{
			if (base.IsLoading())
			{
				return;
			}
			this.SetDirty();
		}

		private void OnCheckChangedAllowPaging(object source, EventArgs e)
		{
			if (base.IsLoading())
			{
				return;
			}
			this.SetDirty();
			this.UpdateEnabledVisibleState();
		}

		private void OnCheckChangedVisible(object source, EventArgs e)
		{
			if (base.IsLoading())
			{
				return;
			}
			this.SetDirty();
			this.UpdateEnabledVisibleState();
		}

		private void OnPagerChanged(object source, EventArgs e)
		{
			if (base.IsLoading())
			{
				return;
			}
			this.SetDirty();
			if (source == this.modeCombo)
			{
				this.UpdateEnabledVisibleState();
			}
		}

		private void OnTextChangedPageSize(object source, EventArgs e)
		{
			if (base.IsLoading())
			{
				return;
			}
			this.SetDirty();
			this.UpdateEnabledVisibleState();
		}

		protected override void SaveComponent()
		{
			global::System.Web.UI.WebControls.DataGrid dataGrid = (global::System.Web.UI.WebControls.DataGrid)base.GetBaseControl();
			DataGridPagerStyle pagerStyle = dataGrid.PagerStyle;
			dataGrid.AllowPaging = this.allowPagingCheck.Checked;
			dataGrid.AllowCustomPaging = this.allowCustomPagingCheck.Checked;
			string text = this.pageSizeEdit.Text.Trim();
			if (text.Length != 0)
			{
				try
				{
					dataGrid.PageSize = int.Parse(text, CultureInfo.InvariantCulture);
				}
				catch
				{
					this.pageSizeEdit.Text = dataGrid.PageSize.ToString(NumberFormatInfo.CurrentInfo);
				}
			}
			pagerStyle.Visible = this.visibleCheck.Checked;
			switch (this.modeCombo.SelectedIndex)
			{
			case 0:
				pagerStyle.Mode = PagerMode.NextPrev;
				break;
			case 1:
				pagerStyle.Mode = PagerMode.NumericPages;
				break;
			}
			switch (this.posCombo.SelectedIndex)
			{
			case 0:
				pagerStyle.Position = PagerPosition.Top;
				break;
			case 1:
				pagerStyle.Position = PagerPosition.Bottom;
				break;
			case 2:
				pagerStyle.Position = PagerPosition.TopAndBottom;
				break;
			}
			pagerStyle.NextPageText = this.nextPageTextEdit.Text;
			pagerStyle.PrevPageText = this.prevPageTextEdit.Text;
			string text2 = this.pageButtonCountEdit.Text.Trim();
			if (text2.Length != 0)
			{
				try
				{
					pagerStyle.PageButtonCount = int.Parse(text2, CultureInfo.InvariantCulture);
				}
				catch
				{
					this.pageButtonCountEdit.Text = pagerStyle.PageButtonCount.ToString(NumberFormatInfo.CurrentInfo);
				}
			}
		}

		public override void SetComponent(IComponent component)
		{
			base.SetComponent(component);
			this.InitForm();
		}

		private void UpdateEnabledVisibleState()
		{
			int num = 0;
			string text = this.pageSizeEdit.Text.Trim();
			if (text.Length != 0)
			{
				int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out num);
			}
			bool @checked = this.allowPagingCheck.Checked;
			bool flag = @checked && num != 0;
			bool checked2 = this.visibleCheck.Checked;
			bool flag2 = this.modeCombo.SelectedIndex == 0;
			this.allowCustomPagingCheck.Enabled = @checked;
			this.pageSizeEdit.Enabled = @checked;
			this.visibleCheck.Enabled = flag;
			this.posCombo.Enabled = flag && checked2;
			this.modeCombo.Enabled = flag && checked2;
			this.nextPageTextEdit.Enabled = flag && checked2 && flag2;
			this.prevPageTextEdit.Enabled = flag && checked2 && flag2;
			this.pageButtonCountEdit.Enabled = flag && checked2 && !flag2;
		}

		private const int IDX_POS_TOP = 0;

		private const int IDX_POS_BOTTOM = 1;

		private const int IDX_POS_TOPANDBOTTOM = 2;

		private const int IDX_MODE_PAGEBUTTONS = 0;

		private const int IDX_MODE_PAGENUMBERS = 1;

		private global::System.Windows.Forms.CheckBox allowPagingCheck;

		private NumberEdit pageSizeEdit;

		private global::System.Windows.Forms.CheckBox visibleCheck;

		private ComboBox posCombo;

		private ComboBox modeCombo;

		private global::System.Windows.Forms.TextBox nextPageTextEdit;

		private global::System.Windows.Forms.TextBox prevPageTextEdit;

		private NumberEdit pageButtonCountEdit;

		private global::System.Windows.Forms.CheckBox allowCustomPagingCheck;
	}
}
