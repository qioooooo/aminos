using System;
using System.Collections;
using System.ComponentModel.Design.Data;
using System.Data;
using System.Design;
using System.Drawing;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	internal class SqlDataSourceCustomCommandPanel : WizardPanel
	{
		public SqlDataSourceCustomCommandPanel(SqlDataSourceDesigner sqlDataSourceDesigner)
		{
			this._sqlDataSourceDesigner = sqlDataSourceDesigner;
			this.InitializeComponent();
			this.InitializeUI();
			this._selectCommandEditor.SetCommandData(this._sqlDataSourceDesigner, QueryBuilderMode.Select);
			this._insertCommandEditor.SetCommandData(this._sqlDataSourceDesigner, QueryBuilderMode.Insert);
			this._updateCommandEditor.SetCommandData(this._sqlDataSourceDesigner, QueryBuilderMode.Update);
			this._deleteCommandEditor.SetCommandData(this._sqlDataSourceDesigner, QueryBuilderMode.Delete);
		}

		private void InitializeComponent()
		{
			this._commandsTabControl = new TabControl();
			this._selectTabPage = new TabPage();
			this._selectCommandEditor = new SqlDataSourceCustomCommandEditor();
			this._updateTabPage = new TabPage();
			this._updateCommandEditor = new SqlDataSourceCustomCommandEditor();
			this._insertTabPage = new TabPage();
			this._insertCommandEditor = new SqlDataSourceCustomCommandEditor();
			this._deleteTabPage = new TabPage();
			this._deleteCommandEditor = new SqlDataSourceCustomCommandEditor();
			this._helpLabel = new global::System.Windows.Forms.Label();
			this._commandsTabControl.SuspendLayout();
			this._selectTabPage.SuspendLayout();
			this._updateTabPage.SuspendLayout();
			this._insertTabPage.SuspendLayout();
			this._deleteTabPage.SuspendLayout();
			base.SuspendLayout();
			this._helpLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this._helpLabel.Location = new Point(0, 0);
			this._helpLabel.Name = "_helpLabel";
			this._helpLabel.Size = new Size(544, 16);
			this._helpLabel.TabIndex = 10;
			this._commandsTabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this._commandsTabControl.Controls.Add(this._selectTabPage);
			this._commandsTabControl.Controls.Add(this._updateTabPage);
			this._commandsTabControl.Controls.Add(this._insertTabPage);
			this._commandsTabControl.Controls.Add(this._deleteTabPage);
			this._commandsTabControl.Location = new Point(0, 22);
			this._commandsTabControl.Name = "_commandsTabControl";
			this._commandsTabControl.SelectedIndex = 0;
			this._commandsTabControl.ShowToolTips = true;
			this._commandsTabControl.Size = new Size(544, 252);
			this._commandsTabControl.TabIndex = 20;
			this._selectTabPage.Controls.Add(this._selectCommandEditor);
			this._selectTabPage.Location = new Point(4, 22);
			this._selectTabPage.Name = "_selectTabPage";
			this._selectTabPage.Size = new Size(536, 226);
			this._selectTabPage.TabIndex = 10;
			this._selectTabPage.Text = "SELECT";
			this._selectCommandEditor.Dock = DockStyle.Fill;
			this._selectCommandEditor.Location = new Point(0, 0);
			this._selectCommandEditor.Name = "_selectCommandEditor";
			this._selectCommandEditor.TabIndex = 10;
			this._selectCommandEditor.CommandChanged += this.OnSelectCommandChanged;
			this._updateTabPage.Controls.Add(this._updateCommandEditor);
			this._updateTabPage.Location = new Point(4, 22);
			this._updateTabPage.Name = "_updateTabPage";
			this._updateTabPage.Size = new Size(536, 226);
			this._updateTabPage.TabIndex = 20;
			this._updateTabPage.Text = "UPDATE";
			this._updateCommandEditor.Dock = DockStyle.Fill;
			this._updateCommandEditor.Location = new Point(0, 0);
			this._updateCommandEditor.Name = "_updateCommandEditor";
			this._updateCommandEditor.TabIndex = 10;
			this._insertTabPage.Controls.Add(this._insertCommandEditor);
			this._insertTabPage.Location = new Point(4, 22);
			this._insertTabPage.Name = "_insertTabPage";
			this._insertTabPage.Size = new Size(536, 226);
			this._insertTabPage.TabIndex = 30;
			this._insertTabPage.Text = "INSERT";
			this._insertCommandEditor.Dock = DockStyle.Fill;
			this._insertCommandEditor.Location = new Point(0, 0);
			this._insertCommandEditor.Name = "_insertCommandEditor";
			this._insertCommandEditor.TabIndex = 10;
			this._deleteTabPage.Controls.Add(this._deleteCommandEditor);
			this._deleteTabPage.Location = new Point(4, 22);
			this._deleteTabPage.Name = "_deleteTabPage";
			this._deleteTabPage.Size = new Size(522, 226);
			this._deleteTabPage.TabIndex = 40;
			this._deleteTabPage.Text = "DELETE";
			this._deleteCommandEditor.Dock = DockStyle.Fill;
			this._deleteCommandEditor.Location = new Point(0, 0);
			this._deleteCommandEditor.Name = "_deleteCommandEditor";
			this._deleteCommandEditor.TabIndex = 10;
			base.Controls.Add(this._helpLabel);
			base.Controls.Add(this._commandsTabControl);
			base.Name = "SqlDataSourceCustomCommandPanel";
			base.Size = new Size(544, 274);
			this._commandsTabControl.ResumeLayout(false);
			this._selectTabPage.ResumeLayout(false);
			this._updateTabPage.ResumeLayout(false);
			this._insertTabPage.ResumeLayout(false);
			this._deleteTabPage.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		private void InitializeUI()
		{
			this._helpLabel.Text = SR.GetString("SqlDataSourceCustomCommandPanel_HelpLabel");
			base.Caption = SR.GetString("SqlDataSourceCustomCommandPanel_PanelCaption");
		}

		public override bool OnNext()
		{
			SqlDataSourceQuery query = this._selectCommandEditor.GetQuery();
			SqlDataSourceQuery query2 = this._insertCommandEditor.GetQuery();
			SqlDataSourceQuery query3 = this._updateCommandEditor.GetQuery();
			SqlDataSourceQuery query4 = this._deleteCommandEditor.GetQuery();
			if (query == null || query2 == null || query3 == null || query4 == null)
			{
				return false;
			}
			int num = 0;
			foreach (object obj in query.Parameters)
			{
				Parameter parameter = (Parameter)obj;
				if (parameter.Direction == ParameterDirection.Input || parameter.Direction == ParameterDirection.InputOutput)
				{
					num++;
				}
			}
			if (num == 0)
			{
				SqlDataSourceSummaryPanel sqlDataSourceSummaryPanel = base.NextPanel as SqlDataSourceSummaryPanel;
				if (sqlDataSourceSummaryPanel == null)
				{
					sqlDataSourceSummaryPanel = ((SqlDataSourceWizardForm)base.ParentWizard).GetSummaryPanel();
					base.NextPanel = sqlDataSourceSummaryPanel;
				}
				sqlDataSourceSummaryPanel.SetQueries(this._dataConnection, query, query2, query3, query4);
				return true;
			}
			SqlDataSourceConfigureParametersPanel sqlDataSourceConfigureParametersPanel = base.NextPanel as SqlDataSourceConfigureParametersPanel;
			if (sqlDataSourceConfigureParametersPanel == null)
			{
				sqlDataSourceConfigureParametersPanel = ((SqlDataSourceWizardForm)base.ParentWizard).GetConfigureParametersPanel();
				base.NextPanel = sqlDataSourceConfigureParametersPanel;
				SqlDataSource sqlDataSource = (SqlDataSource)this._sqlDataSourceDesigner.Component;
				Parameter[] array = new Parameter[sqlDataSource.SelectParameters.Count];
				for (int i = 0; i < sqlDataSource.SelectParameters.Count; i++)
				{
					Parameter parameter2 = sqlDataSource.SelectParameters[i];
					Parameter parameter3 = (Parameter)((ICloneable)parameter2).Clone();
					this._sqlDataSourceDesigner.RegisterClone(parameter2, parameter3);
					array[i] = parameter3;
				}
				sqlDataSourceConfigureParametersPanel.InitializeParameters(array);
			}
			sqlDataSourceConfigureParametersPanel.SetQueries(this._dataConnection, query, query2, query3, query4);
			return true;
		}

		public override void OnPrevious()
		{
		}

		private void OnSelectCommandChanged(object sender, EventArgs e)
		{
			this.UpdateEnabledState();
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if (base.Visible)
			{
				this.UpdateEnabledState();
			}
		}

		public void ResetUI()
		{
		}

		public void SetQueries(DesignerDataConnection dataConnection, SqlDataSourceQuery selectQuery, SqlDataSourceQuery insertQuery, SqlDataSourceQuery updateQuery, SqlDataSourceQuery deleteQuery)
		{
			if (!SqlDataSourceDesigner.ConnectionsEqual(this._dataConnection, dataConnection))
			{
				this._dataConnection = dataConnection;
				Cursor cursor = Cursor.Current;
				ArrayList arrayList = null;
				try
				{
					Cursor.Current = Cursors.WaitCursor;
					IDataEnvironment dataEnvironment = (IDataEnvironment)this._sqlDataSourceDesigner.Component.Site.GetService(typeof(IDataEnvironment));
					if (dataEnvironment != null)
					{
						IDesignerDataSchema connectionSchema = dataEnvironment.GetConnectionSchema(this._dataConnection);
						if (connectionSchema != null && connectionSchema.SupportsSchemaClass(DesignerDataSchemaClass.StoredProcedures))
						{
							ICollection schemaItems = connectionSchema.GetSchemaItems(DesignerDataSchemaClass.StoredProcedures);
							if (schemaItems != null && schemaItems.Count > 0)
							{
								arrayList = new ArrayList();
								foreach (object obj in schemaItems)
								{
									DesignerDataStoredProcedure designerDataStoredProcedure = (DesignerDataStoredProcedure)obj;
									if (!designerDataStoredProcedure.Name.ToLowerInvariant().StartsWith("AspNet_".ToLowerInvariant(), StringComparison.Ordinal))
									{
										arrayList.Add(designerDataStoredProcedure);
									}
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					UIServiceHelper.ShowError(base.ServiceProvider, ex, SR.GetString("SqlDataSourceConnectionPanel_CouldNotGetConnectionSchema"));
				}
				finally
				{
					Cursor.Current = cursor;
				}
				this._selectCommandEditor.SetConnection(this._dataConnection);
				this._selectCommandEditor.SetStoredProcedures(arrayList);
				this._insertCommandEditor.SetConnection(this._dataConnection);
				this._insertCommandEditor.SetStoredProcedures(arrayList);
				this._updateCommandEditor.SetConnection(this._dataConnection);
				this._updateCommandEditor.SetStoredProcedures(arrayList);
				this._deleteCommandEditor.SetConnection(this._dataConnection);
				this._deleteCommandEditor.SetStoredProcedures(arrayList);
				this._selectCommandEditor.SetQuery(selectQuery);
				this._insertCommandEditor.SetQuery(insertQuery);
				this._updateCommandEditor.SetQuery(updateQuery);
				this._deleteCommandEditor.SetQuery(deleteQuery);
			}
		}

		private void UpdateEnabledState()
		{
			bool hasQuery = this._selectCommandEditor.HasQuery;
			base.ParentWizard.NextButton.Enabled = hasQuery;
			base.ParentWizard.FinishButton.Enabled = false;
		}

		private TabPage _selectTabPage;

		private TabPage _updateTabPage;

		private TabPage _insertTabPage;

		private TabPage _deleteTabPage;

		private SqlDataSourceCustomCommandEditor _selectCommandEditor;

		private SqlDataSourceCustomCommandEditor _insertCommandEditor;

		private SqlDataSourceCustomCommandEditor _updateCommandEditor;

		private SqlDataSourceCustomCommandEditor _deleteCommandEditor;

		private TabControl _commandsTabControl;

		private global::System.Windows.Forms.Label _helpLabel;

		private SqlDataSourceDesigner _sqlDataSourceDesigner;

		private DesignerDataConnection _dataConnection;
	}
}
