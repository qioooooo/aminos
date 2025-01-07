namespace System.Windows.Forms.Design
{
	internal partial class DataGridAutoFormatDialog : global::System.Windows.Forms.Form
	{
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::System.Windows.Forms.Design.DataGridAutoFormatDialog));
			this.formats = new global::System.Windows.Forms.Label();
			this.schemeName = new global::System.Windows.Forms.ListBox();
			this.dataGrid = new global::System.Windows.Forms.Design.DataGridAutoFormatDialog.AutoFormatDataGrid();
			this.preview = new global::System.Windows.Forms.Label();
			this.button1 = new global::System.Windows.Forms.Button();
			this.button2 = new global::System.Windows.Forms.Button();
			this.okCancelTableLayoutPanel = new global::System.Windows.Forms.TableLayoutPanel();
			this.overarchingTableLayoutPanel = new global::System.Windows.Forms.TableLayoutPanel();
			((global::System.ComponentModel.ISupportInitialize)this.dataGrid).BeginInit();
			this.okCancelTableLayoutPanel.SuspendLayout();
			this.overarchingTableLayoutPanel.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.formats, "formats");
			this.formats.Margin = new global::System.Windows.Forms.Padding(0, 0, 3, 0);
			this.formats.Name = "formats";
			componentResourceManager.ApplyResources(this.schemeName, "schemeName");
			this.schemeName.DisplayMember = "SchemeName";
			this.schemeName.FormattingEnabled = true;
			this.schemeName.Margin = new global::System.Windows.Forms.Padding(0, 2, 3, 3);
			this.schemeName.Name = "schemeName";
			this.schemeName.SelectedIndexChanged += new global::System.EventHandler(this.SchemeName_SelectionChanged);
			componentResourceManager.ApplyResources(this.dataGrid, "dataGrid");
			this.dataGrid.DataMember = "";
			this.dataGrid.HeaderForeColor = global::System.Drawing.SystemColors.ControlText;
			this.dataGrid.Margin = new global::System.Windows.Forms.Padding(3, 2, 0, 3);
			this.dataGrid.Name = "dataGrid";
			componentResourceManager.ApplyResources(this.preview, "preview");
			this.preview.Margin = new global::System.Windows.Forms.Padding(3, 0, 0, 0);
			this.preview.Name = "preview";
			componentResourceManager.ApplyResources(this.button1, "button1");
			this.button1.DialogResult = global::System.Windows.Forms.DialogResult.OK;
			this.button1.Margin = new global::System.Windows.Forms.Padding(0, 0, 3, 0);
			this.button1.MinimumSize = new global::System.Drawing.Size(75, 23);
			this.button1.Name = "button1";
			this.button1.Padding = new global::System.Windows.Forms.Padding(10, 0, 10, 0);
			this.button1.Click += new global::System.EventHandler(this.Button1_Clicked);
			componentResourceManager.ApplyResources(this.button2, "button2");
			this.button2.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.button2.Margin = new global::System.Windows.Forms.Padding(3, 0, 0, 0);
			this.button2.MinimumSize = new global::System.Drawing.Size(75, 23);
			this.button2.Name = "button2";
			this.button2.Padding = new global::System.Windows.Forms.Padding(10, 0, 10, 0);
			componentResourceManager.ApplyResources(this.okCancelTableLayoutPanel, "okCancelTableLayoutPanel");
			this.overarchingTableLayoutPanel.SetColumnSpan(this.okCancelTableLayoutPanel, 2);
			this.okCancelTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 50f));
			this.okCancelTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 50f));
			this.okCancelTableLayoutPanel.Controls.Add(this.button1, 0, 0);
			this.okCancelTableLayoutPanel.Controls.Add(this.button2, 1, 0);
			this.okCancelTableLayoutPanel.Margin = new global::System.Windows.Forms.Padding(0, 6, 0, 0);
			this.okCancelTableLayoutPanel.Name = "okCancelTableLayoutPanel";
			this.okCancelTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle(global::System.Windows.Forms.SizeType.Percent, 50f));
			componentResourceManager.ApplyResources(this.overarchingTableLayoutPanel, "overarchingTableLayoutPanel");
			this.overarchingTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Absolute, 146f));
			this.overarchingTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Absolute, 182f));
			this.overarchingTableLayoutPanel.Controls.Add(this.okCancelTableLayoutPanel, 0, 2);
			this.overarchingTableLayoutPanel.Controls.Add(this.preview, 1, 0);
			this.overarchingTableLayoutPanel.Controls.Add(this.dataGrid, 1, 1);
			this.overarchingTableLayoutPanel.Controls.Add(this.formats, 0, 0);
			this.overarchingTableLayoutPanel.Controls.Add(this.schemeName, 0, 1);
			this.overarchingTableLayoutPanel.Name = "overarchingTableLayoutPanel";
			this.overarchingTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			this.overarchingTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle(global::System.Windows.Forms.SizeType.Percent, 50f));
			this.overarchingTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			base.AcceptButton = this.button1;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.CancelButton = this.button2;
			base.Controls.Add(this.overarchingTableLayoutPanel);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "DataGridAutoFormatDialog";
			base.ShowIcon = false;
			base.HelpRequested += new global::System.Windows.Forms.HelpEventHandler(this.AutoFormat_HelpRequested);
			((global::System.ComponentModel.ISupportInitialize)this.dataGrid).EndInit();
			this.okCancelTableLayoutPanel.ResumeLayout(false);
			this.okCancelTableLayoutPanel.PerformLayout();
			this.overarchingTableLayoutPanel.ResumeLayout(false);
			this.overarchingTableLayoutPanel.PerformLayout();
			base.ResumeLayout(false);
		}

		private global::System.Windows.Forms.Design.DataGridAutoFormatDialog.AutoFormatDataGrid dataGrid;

		private global::System.Windows.Forms.Button button2;

		private global::System.Windows.Forms.Button button1;

		private global::System.Windows.Forms.ListBox schemeName;

		private global::System.Windows.Forms.Label formats;

		private global::System.Windows.Forms.Label preview;

		private global::System.Windows.Forms.TableLayoutPanel okCancelTableLayoutPanel;

		private global::System.Windows.Forms.TableLayoutPanel overarchingTableLayoutPanel;
	}
}
