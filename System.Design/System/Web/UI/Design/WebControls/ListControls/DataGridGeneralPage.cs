using System;
using System.ComponentModel;
using System.Design;
using System.Drawing;
using System.Security.Permissions;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls.ListControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed class DataGridGeneralPage : BaseDataListPage
	{
		protected override string HelpKeyword
		{
			get
			{
				return "net.Asp.DataGridProperties.General";
			}
		}

		private void InitForm()
		{
			GroupLabel groupLabel = new GroupLabel();
			this.showHeaderCheck = new global::System.Windows.Forms.CheckBox();
			this.showFooterCheck = new global::System.Windows.Forms.CheckBox();
			GroupLabel groupLabel2 = new GroupLabel();
			this.allowSortingCheck = new global::System.Windows.Forms.CheckBox();
			groupLabel.SetBounds(4, 4, 431, 16);
			groupLabel.Text = SR.GetString("DGGen_HeaderFooterGroup");
			groupLabel.TabIndex = 8;
			groupLabel.TabStop = false;
			this.showHeaderCheck.SetBounds(12, 24, 160, 16);
			this.showHeaderCheck.TabIndex = 9;
			this.showHeaderCheck.Text = SR.GetString("DGGen_ShowHeader");
			this.showHeaderCheck.TextAlign = ContentAlignment.MiddleLeft;
			this.showHeaderCheck.FlatStyle = FlatStyle.System;
			this.showHeaderCheck.CheckedChanged += this.OnCheckChangedShowHeader;
			this.showFooterCheck.SetBounds(12, 44, 160, 16);
			this.showFooterCheck.TabIndex = 10;
			this.showFooterCheck.Text = SR.GetString("DGGen_ShowFooter");
			this.showFooterCheck.TextAlign = ContentAlignment.MiddleLeft;
			this.showFooterCheck.FlatStyle = FlatStyle.System;
			this.showFooterCheck.CheckedChanged += this.OnCheckChangedShowFooter;
			groupLabel2.SetBounds(4, 70, 431, 16);
			groupLabel2.Text = SR.GetString("DGGen_BehaviorGroup");
			groupLabel2.TabIndex = 11;
			groupLabel2.TabStop = false;
			this.allowSortingCheck.SetBounds(12, 88, 160, 16);
			this.allowSortingCheck.Text = SR.GetString("DGGen_AllowSorting");
			this.allowSortingCheck.TabIndex = 12;
			this.allowSortingCheck.TextAlign = ContentAlignment.MiddleLeft;
			this.allowSortingCheck.FlatStyle = FlatStyle.System;
			this.allowSortingCheck.CheckedChanged += this.OnCheckChangedAllowSorting;
			this.Text = SR.GetString("DGGen_Text");
			base.AccessibleDescription = SR.GetString("DGGen_Desc");
			base.Size = new Size(464, 272);
			base.CommitOnDeactivate = true;
			base.Icon = new Icon(base.GetType(), "DataGridGeneralPage.ico");
			base.Controls.Clear();
			base.Controls.AddRange(new Control[] { this.allowSortingCheck, groupLabel2, this.showFooterCheck, this.showHeaderCheck, groupLabel });
		}

		private void InitPage()
		{
			this.showHeaderCheck.Checked = false;
			this.showFooterCheck.Checked = false;
			this.allowSortingCheck.Checked = false;
		}

		protected override void LoadComponent()
		{
			this.InitPage();
			global::System.Web.UI.WebControls.DataGrid dataGrid = (global::System.Web.UI.WebControls.DataGrid)base.GetBaseControl();
			this.showHeaderCheck.Checked = dataGrid.ShowHeader;
			this.showFooterCheck.Checked = dataGrid.ShowFooter;
			this.allowSortingCheck.Checked = dataGrid.AllowSorting;
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

		private void OnCheckChangedAllowSorting(object source, EventArgs e)
		{
			if (base.IsLoading())
			{
				return;
			}
			this.SetDirty();
		}

		protected override void SaveComponent()
		{
			global::System.Web.UI.WebControls.DataGrid dataGrid = (global::System.Web.UI.WebControls.DataGrid)base.GetBaseControl();
			dataGrid.ShowHeader = this.showHeaderCheck.Checked;
			dataGrid.ShowFooter = this.showFooterCheck.Checked;
			dataGrid.AllowSorting = this.allowSortingCheck.Checked;
		}

		public override void SetComponent(IComponent component)
		{
			base.SetComponent(component);
			this.InitForm();
		}

		private global::System.Windows.Forms.CheckBox showHeaderCheck;

		private global::System.Windows.Forms.CheckBox showFooterCheck;

		private global::System.Windows.Forms.CheckBox allowSortingCheck;
	}
}
