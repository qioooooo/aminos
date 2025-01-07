namespace System.Windows.Forms.Design
{
	internal partial class DataGridViewCellStyleBuilder : global::System.Windows.Forms.Form
	{
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::System.Windows.Forms.Design.DataGridViewCellStyleBuilder));
			this.cellStyleProperties = new global::System.Windows.Forms.PropertyGrid();
			this.sampleViewTableLayoutPanel = new global::System.Windows.Forms.TableLayoutPanel();
			this.sampleViewGridsTableLayoutPanel = new global::System.Windows.Forms.TableLayoutPanel();
			this.normalLabel = new global::System.Windows.Forms.Label();
			this.sampleDataGridView = new global::System.Windows.Forms.DataGridView();
			this.selectedLabel = new global::System.Windows.Forms.Label();
			this.sampleDataGridViewSelected = new global::System.Windows.Forms.DataGridView();
			this.label1 = new global::System.Windows.Forms.Label();
			this.okButton = new global::System.Windows.Forms.Button();
			this.cancelButton = new global::System.Windows.Forms.Button();
			this.okCancelTableLayoutPanel = new global::System.Windows.Forms.TableLayoutPanel();
			this.previewGroupBox = new global::System.Windows.Forms.GroupBox();
			this.overarchingTableLayoutPanel = new global::System.Windows.Forms.TableLayoutPanel();
			this.sampleViewTableLayoutPanel.SuspendLayout();
			this.sampleViewGridsTableLayoutPanel.SuspendLayout();
			((global::System.ComponentModel.ISupportInitialize)this.sampleDataGridView).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.sampleDataGridViewSelected).BeginInit();
			this.okCancelTableLayoutPanel.SuspendLayout();
			this.previewGroupBox.SuspendLayout();
			this.overarchingTableLayoutPanel.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.cellStyleProperties, "cellStyleProperties");
			this.cellStyleProperties.LineColor = global::System.Drawing.SystemColors.ScrollBar;
			this.cellStyleProperties.Margin = new global::System.Windows.Forms.Padding(0, 0, 0, 3);
			this.cellStyleProperties.Name = "cellStyleProperties";
			this.cellStyleProperties.ToolbarVisible = false;
			componentResourceManager.ApplyResources(this.sampleViewTableLayoutPanel, "sampleViewTableLayoutPanel");
			this.sampleViewTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Absolute, 423f));
			this.sampleViewTableLayoutPanel.Controls.Add(this.sampleViewGridsTableLayoutPanel, 0, 1);
			this.sampleViewTableLayoutPanel.Controls.Add(this.label1, 0, 0);
			this.sampleViewTableLayoutPanel.Name = "sampleViewTableLayoutPanel";
			this.sampleViewTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			this.sampleViewTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			componentResourceManager.ApplyResources(this.sampleViewGridsTableLayoutPanel, "sampleViewGridsTableLayoutPanel");
			this.sampleViewGridsTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 10f));
			this.sampleViewGridsTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 30f));
			this.sampleViewGridsTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 20f));
			this.sampleViewGridsTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 30f));
			this.sampleViewGridsTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 10f));
			this.sampleViewGridsTableLayoutPanel.Controls.Add(this.normalLabel, 1, 0);
			this.sampleViewGridsTableLayoutPanel.Controls.Add(this.sampleDataGridView, 1, 1);
			this.sampleViewGridsTableLayoutPanel.Controls.Add(this.selectedLabel, 3, 0);
			this.sampleViewGridsTableLayoutPanel.Controls.Add(this.sampleDataGridViewSelected, 3, 1);
			this.sampleViewGridsTableLayoutPanel.Margin = new global::System.Windows.Forms.Padding(0, 3, 0, 0);
			this.sampleViewGridsTableLayoutPanel.Name = "sampleViewGridsTableLayoutPanel";
			this.sampleViewGridsTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			this.sampleViewGridsTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			componentResourceManager.ApplyResources(this.normalLabel, "normalLabel");
			this.normalLabel.Margin = new global::System.Windows.Forms.Padding(0);
			this.normalLabel.Name = "normalLabel";
			this.sampleDataGridView.AllowUserToAddRows = false;
			componentResourceManager.ApplyResources(this.sampleDataGridView, "sampleDataGridView");
			this.sampleDataGridView.ColumnHeadersVisible = false;
			this.sampleDataGridView.Margin = new global::System.Windows.Forms.Padding(0);
			this.sampleDataGridView.Name = "sampleDataGridView";
			this.sampleDataGridView.ReadOnly = true;
			this.sampleDataGridView.RowHeadersVisible = false;
			this.sampleDataGridView.CellStateChanged += new global::System.Windows.Forms.DataGridViewCellStateChangedEventHandler(this.sampleDataGridView_CellStateChanged);
			componentResourceManager.ApplyResources(this.selectedLabel, "selectedLabel");
			this.selectedLabel.Margin = new global::System.Windows.Forms.Padding(0);
			this.selectedLabel.Name = "selectedLabel";
			this.sampleDataGridViewSelected.AllowUserToAddRows = false;
			componentResourceManager.ApplyResources(this.sampleDataGridViewSelected, "sampleDataGridViewSelected");
			this.sampleDataGridViewSelected.ColumnHeadersVisible = false;
			this.sampleDataGridViewSelected.Margin = new global::System.Windows.Forms.Padding(0);
			this.sampleDataGridViewSelected.Name = "sampleDataGridViewSelected";
			this.sampleDataGridViewSelected.ReadOnly = true;
			this.sampleDataGridViewSelected.RowHeadersVisible = false;
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Margin = new global::System.Windows.Forms.Padding(0, 0, 0, 3);
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.okButton, "okButton");
			this.okButton.DialogResult = global::System.Windows.Forms.DialogResult.OK;
			this.okButton.Margin = new global::System.Windows.Forms.Padding(0, 0, 3, 0);
			this.okButton.Name = "okButton";
			componentResourceManager.ApplyResources(this.cancelButton, "cancelButton");
			this.cancelButton.DialogResult = global::System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Margin = new global::System.Windows.Forms.Padding(3, 0, 0, 0);
			this.cancelButton.Name = "cancelButton";
			componentResourceManager.ApplyResources(this.okCancelTableLayoutPanel, "okCancelTableLayoutPanel");
			this.okCancelTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 50f));
			this.okCancelTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle(global::System.Windows.Forms.SizeType.Percent, 50f));
			this.okCancelTableLayoutPanel.Controls.Add(this.okButton, 0, 0);
			this.okCancelTableLayoutPanel.Controls.Add(this.cancelButton, 1, 0);
			this.okCancelTableLayoutPanel.Margin = new global::System.Windows.Forms.Padding(0, 3, 0, 0);
			this.okCancelTableLayoutPanel.Name = "okCancelTableLayoutPanel";
			this.okCancelTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			componentResourceManager.ApplyResources(this.previewGroupBox, "previewGroupBox");
			this.previewGroupBox.Controls.Add(this.sampleViewTableLayoutPanel);
			this.previewGroupBox.Margin = new global::System.Windows.Forms.Padding(0, 3, 0, 3);
			this.previewGroupBox.Name = "previewGroupBox";
			this.previewGroupBox.TabStop = false;
			componentResourceManager.ApplyResources(this.overarchingTableLayoutPanel, "overarchingTableLayoutPanel");
			this.overarchingTableLayoutPanel.ColumnStyles.Add(new global::System.Windows.Forms.ColumnStyle());
			this.overarchingTableLayoutPanel.Controls.Add(this.cellStyleProperties, 0, 0);
			this.overarchingTableLayoutPanel.Controls.Add(this.okCancelTableLayoutPanel, 0, 2);
			this.overarchingTableLayoutPanel.Controls.Add(this.previewGroupBox, 0, 1);
			this.overarchingTableLayoutPanel.Name = "overarchingTableLayoutPanel";
			this.overarchingTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle(global::System.Windows.Forms.SizeType.Percent, 100f));
			this.overarchingTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			this.overarchingTableLayoutPanel.RowStyles.Add(new global::System.Windows.Forms.RowStyle());
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(this.overarchingTableLayoutPanel);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedDialog;
			base.HelpButton = true;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "DataGridViewCellStyleBuilder";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.HelpButtonClicked += new global::System.ComponentModel.CancelEventHandler(this.DataGridViewCellStyleBuilder_HelpButtonClicked);
			base.HelpRequested += new global::System.Windows.Forms.HelpEventHandler(this.DataGridViewCellStyleBuilder_HelpRequested);
			base.Load += new global::System.EventHandler(this.DataGridViewCellStyleBuilder_Load);
			this.sampleViewTableLayoutPanel.ResumeLayout(false);
			this.sampleViewTableLayoutPanel.PerformLayout();
			this.sampleViewGridsTableLayoutPanel.ResumeLayout(false);
			this.sampleViewGridsTableLayoutPanel.PerformLayout();
			((global::System.ComponentModel.ISupportInitialize)this.sampleDataGridView).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.sampleDataGridViewSelected).EndInit();
			this.okCancelTableLayoutPanel.ResumeLayout(false);
			this.okCancelTableLayoutPanel.PerformLayout();
			this.previewGroupBox.ResumeLayout(false);
			this.previewGroupBox.PerformLayout();
			this.overarchingTableLayoutPanel.ResumeLayout(false);
			this.overarchingTableLayoutPanel.PerformLayout();
			base.ResumeLayout(false);
		}

		private global::System.Windows.Forms.PropertyGrid cellStyleProperties;

		private global::System.Windows.Forms.GroupBox previewGroupBox;

		private global::System.Windows.Forms.Button okButton;

		private global::System.Windows.Forms.Button cancelButton;

		private global::System.Windows.Forms.Label label1;

		private global::System.Windows.Forms.DataGridView sampleDataGridView;

		private global::System.Windows.Forms.DataGridView sampleDataGridViewSelected;

		private global::System.Windows.Forms.TableLayoutPanel sampleViewTableLayoutPanel;

		private global::System.Windows.Forms.TableLayoutPanel okCancelTableLayoutPanel;

		private global::System.Windows.Forms.TableLayoutPanel overarchingTableLayoutPanel;

		private global::System.Windows.Forms.TableLayoutPanel sampleViewGridsTableLayoutPanel;

		private global::System.ComponentModel.Container components;

		private global::System.Windows.Forms.Label normalLabel;

		private global::System.Windows.Forms.Label selectedLabel;
	}
}
