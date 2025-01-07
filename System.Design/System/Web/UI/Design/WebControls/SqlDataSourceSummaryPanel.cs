using System;
using System.ComponentModel;
using System.ComponentModel.Design.Data;
using System.Data;
using System.Data.Common;
using System.Design;
using System.Drawing;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	internal class SqlDataSourceSummaryPanel : WizardPanel
	{
		public SqlDataSourceSummaryPanel(SqlDataSourceDesigner sqlDataSourceDesigner)
		{
			this._sqlDataSourceDesigner = sqlDataSourceDesigner;
			this.InitializeComponent();
			this.InitializeUI();
		}

		public void SetQueries(DesignerDataConnection dataConnection, SqlDataSourceQuery selectQuery, SqlDataSourceQuery insertQuery, SqlDataSourceQuery updateQuery, SqlDataSourceQuery deleteQuery)
		{
			this._dataConnection = dataConnection;
			this._selectQuery = selectQuery;
			this._insertQuery = insertQuery;
			this._updateQuery = updateQuery;
			this._deleteQuery = deleteQuery;
			this._previewTextBox.Text = this._selectQuery.Command;
		}

		private void InitializeComponent()
		{
			this._resultsGridView = new DataGridView();
			this._testQueryButton = new global::System.Windows.Forms.Button();
			this._previewTextBox = new global::System.Windows.Forms.TextBox();
			this._previewLabel = new global::System.Windows.Forms.Label();
			this._helpLabel = new global::System.Windows.Forms.Label();
			((ISupportInitialize)this._resultsGridView).BeginInit();
			base.SuspendLayout();
			this._helpLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this._helpLabel.Location = new Point(0, 0);
			this._helpLabel.Name = "_helpLabel";
			this._helpLabel.Size = new Size(544, 32);
			this._helpLabel.TabIndex = 10;
			this._resultsGridView.AllowUserToAddRows = false;
			this._resultsGridView.AllowUserToDeleteRows = false;
			this._resultsGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this._resultsGridView.Location = new Point(0, 38);
			this._resultsGridView.MultiSelect = false;
			this._resultsGridView.Name = "_resultsGridView";
			this._resultsGridView.ReadOnly = true;
			this._resultsGridView.RowHeadersVisible = false;
			this._resultsGridView.Size = new Size(544, 141);
			this._resultsGridView.TabIndex = 20;
			this._resultsGridView.DataError += this.OnResultsGridViewDataError;
			this._testQueryButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this._testQueryButton.Location = new Point(424, 185);
			this._testQueryButton.Name = "_testQueryButton";
			this._testQueryButton.Size = new Size(120, 23);
			this._testQueryButton.TabIndex = 30;
			this._testQueryButton.Click += this.OnTestQueryButtonClick;
			this._previewLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this._previewLabel.Location = new Point(0, 214);
			this._previewLabel.Name = "_previewLabel";
			this._previewLabel.Size = new Size(544, 16);
			this._previewLabel.TabIndex = 40;
			this._previewTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this._previewTextBox.BackColor = SystemColors.Control;
			this._previewTextBox.Location = new Point(0, 232);
			this._previewTextBox.Multiline = true;
			this._previewTextBox.Name = "_previewTextBox";
			this._previewTextBox.ReadOnly = true;
			this._previewTextBox.ScrollBars = global::System.Windows.Forms.ScrollBars.Vertical;
			this._previewTextBox.Size = new Size(544, 42);
			this._previewTextBox.TabIndex = 50;
			this._previewTextBox.Text = "";
			base.Controls.Add(this._helpLabel);
			base.Controls.Add(this._previewLabel);
			base.Controls.Add(this._previewTextBox);
			base.Controls.Add(this._testQueryButton);
			base.Controls.Add(this._resultsGridView);
			base.Name = "SqlDataSourceSummaryPanel";
			base.Size = new Size(544, 274);
			((ISupportInitialize)this._resultsGridView).EndInit();
			base.ResumeLayout(false);
		}

		private void InitializeUI()
		{
			base.Caption = SR.GetString("SqlDataSourceSummaryPanel_PanelCaption");
			this._testQueryButton.Text = SR.GetString("SqlDataSourceSummaryPanel_TestQueryButton");
			this._previewLabel.Text = SR.GetString("SqlDataSource_General_PreviewLabel");
			this._helpLabel.Text = SR.GetString("SqlDataSourceSummaryPanel_HelpLabel");
			this._resultsGridView.AccessibleName = SR.GetString("SqlDataSourceSummaryPanel_ResultsAccessibleName");
		}

		protected internal override void OnComplete()
		{
			SqlDataSource sqlDataSource = (SqlDataSource)this._sqlDataSourceDesigner.Component;
			if (sqlDataSource.DeleteCommand != this._deleteQuery.Command)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(sqlDataSource)["DeleteCommand"];
				propertyDescriptor.SetValue(sqlDataSource, this._deleteQuery.Command);
			}
			if (sqlDataSource.DeleteCommandType != this._deleteQuery.CommandType)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(sqlDataSource)["DeleteCommandType"];
				propertyDescriptor.SetValue(sqlDataSource, this._deleteQuery.CommandType);
			}
			if (sqlDataSource.InsertCommand != this._insertQuery.Command)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(sqlDataSource)["InsertCommand"];
				propertyDescriptor.SetValue(sqlDataSource, this._insertQuery.Command);
			}
			if (sqlDataSource.InsertCommandType != this._insertQuery.CommandType)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(sqlDataSource)["InsertCommandType"];
				propertyDescriptor.SetValue(sqlDataSource, this._insertQuery.CommandType);
			}
			if (sqlDataSource.SelectCommand != this._selectQuery.Command)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(sqlDataSource)["SelectCommand"];
				propertyDescriptor.SetValue(sqlDataSource, this._selectQuery.Command);
			}
			if (sqlDataSource.SelectCommandType != this._selectQuery.CommandType)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(sqlDataSource)["SelectCommandType"];
				propertyDescriptor.SetValue(sqlDataSource, this._selectQuery.CommandType);
			}
			if (sqlDataSource.UpdateCommand != this._updateQuery.Command)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(sqlDataSource)["UpdateCommand"];
				propertyDescriptor.SetValue(sqlDataSource, this._updateQuery.Command);
			}
			if (sqlDataSource.UpdateCommandType != this._updateQuery.CommandType)
			{
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(sqlDataSource)["UpdateCommandType"];
				propertyDescriptor.SetValue(sqlDataSource, this._updateQuery.CommandType);
			}
			this._sqlDataSourceDesigner.CopyList(this._selectQuery.Parameters, sqlDataSource.SelectParameters);
			this._sqlDataSourceDesigner.CopyList(this._insertQuery.Parameters, sqlDataSource.InsertParameters);
			this._sqlDataSourceDesigner.CopyList(this._updateQuery.Parameters, sqlDataSource.UpdateParameters);
			this._sqlDataSourceDesigner.CopyList(this._deleteQuery.Parameters, sqlDataSource.DeleteParameters);
			ParameterCollection parameterCollection = new ParameterCollection();
			foreach (object obj in this._selectQuery.Parameters)
			{
				Parameter parameter = (Parameter)obj;
				parameterCollection.Add(parameter);
			}
			this._sqlDataSourceDesigner.RefreshSchema(this._dataConnection, this._selectQuery.Command, this._selectQuery.CommandType, parameterCollection, true);
		}

		public override bool OnNext()
		{
			return true;
		}

		public override void OnPrevious()
		{
		}

		private void OnResultsGridViewDataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			e.ThrowException = false;
		}

		private void OnTestQueryButtonClick(object sender, EventArgs e)
		{
			ParameterCollection parameterCollection = new ParameterCollection();
			foreach (object obj in this._selectQuery.Parameters)
			{
				Parameter parameter = (Parameter)obj;
				if (parameter.DbType == DbType.Object)
				{
					parameterCollection.Add(new Parameter(parameter.Name, parameter.Type, parameter.DefaultValue));
				}
				else
				{
					parameterCollection.Add(new Parameter(parameter.Name, parameter.DbType, parameter.DefaultValue));
				}
			}
			if (parameterCollection.Count > 0)
			{
				SqlDataSourceParameterValueEditorForm sqlDataSourceParameterValueEditorForm = new SqlDataSourceParameterValueEditorForm(base.ServiceProvider, parameterCollection);
				DialogResult dialogResult = UIServiceHelper.ShowDialog(base.ServiceProvider, sqlDataSourceParameterValueEditorForm);
				if (dialogResult == DialogResult.Cancel)
				{
					return;
				}
			}
			this._resultsGridView.DataSource = null;
			DbCommand dbCommand = null;
			Cursor cursor = Cursor.Current;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				DbProviderFactory dbProviderFactory = SqlDataSourceDesigner.GetDbProviderFactory(this._dataConnection.ProviderName);
				DbConnection dbConnection = null;
				try
				{
					dbConnection = SqlDataSourceDesigner.GetDesignTimeConnection(base.ServiceProvider, this._dataConnection);
				}
				catch (Exception ex)
				{
					if (dbConnection == null)
					{
						UIServiceHelper.ShowError(base.ServiceProvider, ex, SR.GetString("SqlDataSourceSummaryPanel_CouldNotCreateConnection"));
						return;
					}
				}
				if (dbConnection == null)
				{
					UIServiceHelper.ShowError(base.ServiceProvider, SR.GetString("SqlDataSourceSummaryPanel_CouldNotCreateConnection"));
				}
				else
				{
					dbCommand = this._sqlDataSourceDesigner.BuildSelectCommand(dbProviderFactory, dbConnection, this._selectQuery.Command, parameterCollection, this._selectQuery.CommandType);
					DbDataAdapter dbDataAdapter = SqlDataSourceDesigner.CreateDataAdapter(dbProviderFactory, dbCommand);
					dbDataAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
					DataSet dataSet = new DataSet();
					dbDataAdapter.Fill(dataSet);
					if (dataSet.Tables.Count == 0)
					{
						UIServiceHelper.ShowError(base.ServiceProvider, SR.GetString("SqlDataSourceSummaryPanel_CannotExecuteQueryNoTables"));
					}
					else
					{
						this._resultsGridView.DataSource = dataSet.Tables[0];
						foreach (object obj2 in this._resultsGridView.Columns)
						{
							DataGridViewColumn dataGridViewColumn = (DataGridViewColumn)obj2;
							dataGridViewColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
						}
						this._resultsGridView.AutoResizeColumnHeadersHeight();
						this._resultsGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
					}
				}
			}
			catch (Exception ex2)
			{
				UIServiceHelper.ShowError(base.ServiceProvider, ex2, SR.GetString("SqlDataSourceSummaryPanel_CannotExecuteQuery"));
			}
			finally
			{
				if (dbCommand != null && dbCommand.Connection.State == ConnectionState.Open)
				{
					dbCommand.Connection.Close();
				}
				Cursor.Current = cursor;
			}
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if (base.Visible)
			{
				base.ParentWizard.NextButton.Enabled = false;
				base.ParentWizard.FinishButton.Enabled = true;
			}
		}

		public void ResetUI()
		{
			this._resultsGridView.DataSource = null;
		}

		private global::System.Windows.Forms.TextBox _previewTextBox;

		private global::System.Windows.Forms.Label _previewLabel;

		private global::System.Windows.Forms.Button _testQueryButton;

		private global::System.Windows.Forms.Label _helpLabel;

		private DataGridView _resultsGridView;

		private SqlDataSourceDesigner _sqlDataSourceDesigner;

		private DesignerDataConnection _dataConnection;

		private SqlDataSourceQuery _selectQuery;

		private SqlDataSourceQuery _insertQuery;

		private SqlDataSourceQuery _updateQuery;

		private SqlDataSourceQuery _deleteQuery;
	}
}
