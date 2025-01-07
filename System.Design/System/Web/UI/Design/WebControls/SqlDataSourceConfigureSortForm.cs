using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Data;
using System.Design;
using System.Drawing;
using System.Web.UI.Design.Util;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	internal partial class SqlDataSourceConfigureSortForm : DesignerForm
	{
		public SqlDataSourceConfigureSortForm(SqlDataSourceDesigner sqlDataSourceDesigner, SqlDataSourceTableQuery tableQuery)
			: base(sqlDataSourceDesigner.Component.Site)
		{
			this._sqlDataSourceDesigner = sqlDataSourceDesigner;
			this._tableQuery = tableQuery.Clone();
			this.InitializeComponent();
			this.InitializeUI();
			Cursor cursor = Cursor.Current;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				this._loadingClauses = true;
				this._fieldComboBox1.Items.Add(new SqlDataSourceConfigureSortForm.ColumnItem(null));
				this._fieldComboBox2.Items.Add(new SqlDataSourceConfigureSortForm.ColumnItem(null));
				this._fieldComboBox3.Items.Add(new SqlDataSourceConfigureSortForm.ColumnItem(null));
				foreach (object obj in this._tableQuery.DesignerDataTable.Columns)
				{
					DesignerDataColumn designerDataColumn = (DesignerDataColumn)obj;
					this._fieldComboBox1.Items.Add(new SqlDataSourceConfigureSortForm.ColumnItem(designerDataColumn));
					this._fieldComboBox2.Items.Add(new SqlDataSourceConfigureSortForm.ColumnItem(designerDataColumn));
					this._fieldComboBox3.Items.Add(new SqlDataSourceConfigureSortForm.ColumnItem(designerDataColumn));
				}
				this._fieldComboBox1.InvalidateDropDownWidth();
				this._fieldComboBox2.InvalidateDropDownWidth();
				this._fieldComboBox3.InvalidateDropDownWidth();
				this._sortByGroupBox2.Enabled = false;
				this._sortByGroupBox3.Enabled = false;
				this._sortDirectionPanel1.Enabled = false;
				this._sortDirectionPanel2.Enabled = false;
				this._sortDirectionPanel3.Enabled = false;
				this._sortAscendingRadioButton1.Checked = true;
				this._sortAscendingRadioButton2.Checked = true;
				this._sortAscendingRadioButton3.Checked = true;
				if (this._tableQuery.OrderClauses.Count >= 1)
				{
					SqlDataSourceOrderClause sqlDataSourceOrderClause = this._tableQuery.OrderClauses[0];
					this.SelectFieldItem(this._fieldComboBox1, sqlDataSourceOrderClause.DesignerDataColumn);
					this._sortAscendingRadioButton1.Checked = !sqlDataSourceOrderClause.IsDescending;
					this._sortDescendingRadioButton1.Checked = sqlDataSourceOrderClause.IsDescending;
					if (this._tableQuery.OrderClauses.Count >= 2)
					{
						SqlDataSourceOrderClause sqlDataSourceOrderClause2 = this._tableQuery.OrderClauses[1];
						this.SelectFieldItem(this._fieldComboBox2, sqlDataSourceOrderClause2.DesignerDataColumn);
						this._sortAscendingRadioButton2.Checked = !sqlDataSourceOrderClause2.IsDescending;
						this._sortDescendingRadioButton2.Checked = sqlDataSourceOrderClause2.IsDescending;
						if (this._tableQuery.OrderClauses.Count >= 3)
						{
							SqlDataSourceOrderClause sqlDataSourceOrderClause3 = this._tableQuery.OrderClauses[2];
							this.SelectFieldItem(this._fieldComboBox3, sqlDataSourceOrderClause3.DesignerDataColumn);
							this._sortAscendingRadioButton3.Checked = !sqlDataSourceOrderClause3.IsDescending;
							this._sortDescendingRadioButton3.Checked = sqlDataSourceOrderClause3.IsDescending;
						}
					}
				}
				this._loadingClauses = false;
				this.UpdateOrderClauses();
				this.UpdatePreview();
			}
			finally
			{
				Cursor.Current = cursor;
			}
		}

		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.SqlDataSource.ConfigureSort";
			}
		}

		public IList<SqlDataSourceOrderClause> OrderClauses
		{
			get
			{
				return this._tableQuery.OrderClauses;
			}
		}

		private void InitializeUI()
		{
			this._helpLabel.Text = SR.GetString("SqlDataSourceConfigureSortForm_HelpLabel");
			this._previewLabel.Text = SR.GetString("SqlDataSource_General_PreviewLabel");
			this._sortByGroupBox1.Text = SR.GetString("SqlDataSourceConfigureSortForm_SortByLabel");
			this._sortByGroupBox2.Text = SR.GetString("SqlDataSourceConfigureSortForm_ThenByLabel");
			this._sortByGroupBox3.Text = SR.GetString("SqlDataSourceConfigureSortForm_ThenByLabel");
			this._sortAscendingRadioButton1.Text = SR.GetString("SqlDataSourceConfigureSortForm_AscendingLabel");
			this._sortDescendingRadioButton1.Text = SR.GetString("SqlDataSourceConfigureSortForm_DescendingLabel");
			this._sortAscendingRadioButton2.Text = SR.GetString("SqlDataSourceConfigureSortForm_AscendingLabel");
			this._sortDescendingRadioButton2.Text = SR.GetString("SqlDataSourceConfigureSortForm_DescendingLabel");
			this._sortAscendingRadioButton3.Text = SR.GetString("SqlDataSourceConfigureSortForm_AscendingLabel");
			this._sortDescendingRadioButton3.Text = SR.GetString("SqlDataSourceConfigureSortForm_DescendingLabel");
			this._sortAscendingRadioButton1.AccessibleDescription = SR.GetString("SqlDataSourceConfigureSortForm_SortDirection1");
			this._sortDescendingRadioButton1.AccessibleDescription = SR.GetString("SqlDataSourceConfigureSortForm_SortDirection1");
			this._sortAscendingRadioButton2.AccessibleDescription = SR.GetString("SqlDataSourceConfigureSortForm_SortDirection2");
			this._sortDescendingRadioButton2.AccessibleDescription = SR.GetString("SqlDataSourceConfigureSortForm_SortDirection2");
			this._sortAscendingRadioButton3.AccessibleDescription = SR.GetString("SqlDataSourceConfigureSortForm_SortDirection3");
			this._sortDescendingRadioButton3.AccessibleDescription = SR.GetString("SqlDataSourceConfigureSortForm_SortDirection3");
			this._fieldComboBox1.AccessibleName = SR.GetString("SqlDataSourceConfigureSortForm_SortColumn1");
			this._fieldComboBox2.AccessibleName = SR.GetString("SqlDataSourceConfigureSortForm_SortColumn2");
			this._fieldComboBox3.AccessibleName = SR.GetString("SqlDataSourceConfigureSortForm_SortColumn3");
			this._okButton.Text = SR.GetString("OK");
			this._cancelButton.Text = SR.GetString("Cancel");
			this.Text = SR.GetString("SqlDataSourceConfigureSortForm_Caption");
		}

		private void OnCancelButtonClick(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		private void OnFieldComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this._fieldComboBox1.SelectedIndex == -1 || (this._fieldComboBox1.SelectedIndex == 0 && ((SqlDataSourceConfigureSortForm.ColumnItem)this._fieldComboBox1.Items[0]).DesignerDataColumn == null))
			{
				this._sortDirectionPanel1.Enabled = false;
				this._sortAscendingRadioButton1.Checked = true;
				this._fieldComboBox2.SelectedIndex = -1;
				this._sortAscendingRadioButton2.Checked = true;
				this._sortByGroupBox2.Enabled = false;
				this._fieldComboBox2.Enabled = false;
			}
			else
			{
				this._sortDirectionPanel1.Enabled = true;
				this._sortByGroupBox2.Enabled = true;
				this._fieldComboBox2.Enabled = true;
			}
			this.UpdateOrderClauses();
			this.UpdatePreview();
		}

		private void OnFieldComboBox2SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this._fieldComboBox2.SelectedIndex == -1 || (this._fieldComboBox2.SelectedIndex == 0 && ((SqlDataSourceConfigureSortForm.ColumnItem)this._fieldComboBox2.Items[0]).DesignerDataColumn == null))
			{
				this._sortDirectionPanel2.Enabled = false;
				this._sortAscendingRadioButton2.Checked = true;
				this._fieldComboBox3.SelectedIndex = -1;
				this._sortAscendingRadioButton3.Checked = true;
				this._sortByGroupBox3.Enabled = false;
				this._fieldComboBox3.Enabled = false;
			}
			else
			{
				this._sortDirectionPanel2.Enabled = true;
				this._sortByGroupBox3.Enabled = true;
				this._fieldComboBox3.Enabled = true;
			}
			this.UpdateOrderClauses();
			this.UpdatePreview();
		}

		private void OnFieldComboBox3SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this._fieldComboBox3.SelectedIndex == -1 || (this._fieldComboBox3.SelectedIndex == 0 && ((SqlDataSourceConfigureSortForm.ColumnItem)this._fieldComboBox3.Items[0]).DesignerDataColumn == null))
			{
				this._sortDirectionPanel3.Enabled = false;
				this._sortAscendingRadioButton3.Checked = true;
			}
			else
			{
				this._sortDirectionPanel3.Enabled = true;
			}
			this.UpdateOrderClauses();
			this.UpdatePreview();
		}

		private void OnOkButtonClick(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		private void OnSortAscendingRadioButton1CheckedChanged(object sender, EventArgs e)
		{
			this.UpdateOrderClauses();
			this.UpdatePreview();
		}

		private void OnSortAscendingRadioButton2CheckedChanged(object sender, EventArgs e)
		{
			this.UpdateOrderClauses();
			this.UpdatePreview();
		}

		private void OnSortAscendingRadioButton3CheckedChanged(object sender, EventArgs e)
		{
			this.UpdateOrderClauses();
			this.UpdatePreview();
		}

		private void SelectFieldItem(ComboBox comboBox, DesignerDataColumn field)
		{
			foreach (object obj in comboBox.Items)
			{
				SqlDataSourceConfigureSortForm.ColumnItem columnItem = (SqlDataSourceConfigureSortForm.ColumnItem)obj;
				if (columnItem.DesignerDataColumn == field)
				{
					comboBox.SelectedItem = columnItem;
					break;
				}
			}
		}

		private void UpdateOrderClauses()
		{
			if (this._loadingClauses)
			{
				return;
			}
			this._tableQuery.OrderClauses.Clear();
			if (this._fieldComboBox1.SelectedIndex >= 1)
			{
				SqlDataSourceOrderClause sqlDataSourceOrderClause = new SqlDataSourceOrderClause(this._tableQuery.DesignerDataConnection, this._tableQuery.DesignerDataTable, ((SqlDataSourceConfigureSortForm.ColumnItem)this._fieldComboBox1.SelectedItem).DesignerDataColumn, !this._sortAscendingRadioButton1.Checked);
				this._tableQuery.OrderClauses.Add(sqlDataSourceOrderClause);
			}
			if (this._fieldComboBox2.SelectedIndex >= 1)
			{
				SqlDataSourceOrderClause sqlDataSourceOrderClause2 = new SqlDataSourceOrderClause(this._tableQuery.DesignerDataConnection, this._tableQuery.DesignerDataTable, ((SqlDataSourceConfigureSortForm.ColumnItem)this._fieldComboBox2.SelectedItem).DesignerDataColumn, !this._sortAscendingRadioButton2.Checked);
				this._tableQuery.OrderClauses.Add(sqlDataSourceOrderClause2);
			}
			if (this._fieldComboBox3.SelectedIndex >= 1)
			{
				SqlDataSourceOrderClause sqlDataSourceOrderClause3 = new SqlDataSourceOrderClause(this._tableQuery.DesignerDataConnection, this._tableQuery.DesignerDataTable, ((SqlDataSourceConfigureSortForm.ColumnItem)this._fieldComboBox3.SelectedItem).DesignerDataColumn, !this._sortAscendingRadioButton3.Checked);
				this._tableQuery.OrderClauses.Add(sqlDataSourceOrderClause3);
			}
		}

		private void UpdatePreview()
		{
			SqlDataSourceQuery selectQuery = this._tableQuery.GetSelectQuery();
			this._previewTextBox.Text = ((selectQuery == null) ? string.Empty : selectQuery.Command);
		}

		private SqlDataSourceDesigner _sqlDataSourceDesigner;

		private SqlDataSourceTableQuery _tableQuery;

		private bool _loadingClauses;

		private sealed class ColumnItem
		{
			public ColumnItem(DesignerDataColumn designerDataColumn)
			{
				this._designerDataColumn = designerDataColumn;
			}

			public DesignerDataColumn DesignerDataColumn
			{
				get
				{
					return this._designerDataColumn;
				}
			}

			public override string ToString()
			{
				if (this._designerDataColumn != null)
				{
					return this._designerDataColumn.Name;
				}
				return SR.GetString("SqlDataSourceConfigureSortForm_SortNone");
			}

			private DesignerDataColumn _designerDataColumn;
		}
	}
}
