using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Data;
using System.Design;
using System.Drawing;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	internal class SqlDataSourceConfigureParametersPanel : WizardPanel
	{
		public SqlDataSourceConfigureParametersPanel(SqlDataSourceDesigner sqlDataSourceDesigner)
		{
			this._sqlDataSourceDesigner = sqlDataSourceDesigner;
			this.InitializeComponent();
			this.InitializeUI();
			this._parameterEditorUserControl.SetAllowCollectionChanges(false);
		}

		private static Parameter CreateMergedParameter(Parameter parameter, List<Parameter> unusedOldParameters)
		{
			Parameter parameter2 = null;
			foreach (Parameter parameter3 in unusedOldParameters)
			{
				if (SqlDataSourceConfigureParametersPanel.ParametersMatch(parameter, parameter3))
				{
					parameter2 = parameter3;
					break;
				}
			}
			if (parameter2 != null)
			{
				unusedOldParameters.Remove(parameter2);
			}
			else
			{
				parameter2 = parameter;
			}
			return parameter2;
		}

		private void InitializeComponent()
		{
			this._previewTextBox = new global::System.Windows.Forms.TextBox();
			this._previewLabel = new global::System.Windows.Forms.Label();
			this._helpLabel = new global::System.Windows.Forms.Label();
			this._parameterEditorUserControl = new ParameterEditorUserControl(this._sqlDataSourceDesigner.Component.Site, (SqlDataSource)this._sqlDataSourceDesigner.Component);
			base.SuspendLayout();
			this._helpLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this._helpLabel.Location = new Point(0, 0);
			this._helpLabel.Name = "_helpLabel";
			this._helpLabel.Size = new Size(544, 32);
			this._helpLabel.TabIndex = 10;
			this._parameterEditorUserControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this._parameterEditorUserControl.Location = new Point(0, 38);
			this._parameterEditorUserControl.Name = "_parameterEditorUserControl";
			this._parameterEditorUserControl.Size = new Size(544, 152);
			this._parameterEditorUserControl.TabIndex = 20;
			this._parameterEditorUserControl.ParametersChanged += this.OnParameterEditorUserControlParametersChanged;
			this._previewLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this._previewLabel.Location = new Point(0, 214);
			this._previewLabel.Name = "_previewLabel";
			this._previewLabel.Size = new Size(544, 16);
			this._previewLabel.TabIndex = 30;
			this._previewTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this._previewTextBox.BackColor = SystemColors.Control;
			this._previewTextBox.Location = new Point(0, 232);
			this._previewTextBox.Multiline = true;
			this._previewTextBox.Name = "_previewTextBox";
			this._previewTextBox.ReadOnly = true;
			this._previewTextBox.ScrollBars = global::System.Windows.Forms.ScrollBars.Vertical;
			this._previewTextBox.Size = new Size(544, 42);
			this._previewTextBox.TabIndex = 40;
			this._previewTextBox.Text = "";
			base.Controls.Add(this._parameterEditorUserControl);
			base.Controls.Add(this._helpLabel);
			base.Controls.Add(this._previewLabel);
			base.Controls.Add(this._previewTextBox);
			base.Name = "SqlDataSourceConfigureParametersPanel";
			base.Size = new Size(544, 274);
			base.ResumeLayout(false);
		}

		public void InitializeParameters(Parameter[] selectParameters)
		{
			this._parameterEditorUserControl.AddParameters(selectParameters);
		}

		private void InitializeUI()
		{
			base.Caption = SR.GetString("SqlDataSourceConfigureParametersPanel_PanelCaption");
			this._helpLabel.Text = SR.GetString("SqlDataSourceConfigureParametersPanel_HelpLabel");
			this._previewLabel.Text = SR.GetString("SqlDataSource_General_PreviewLabel");
		}

		private static Parameter[] MergeParameters(Parameter[] oldParameters, Parameter[] newParameters)
		{
			Parameter[] array = new Parameter[newParameters.Length];
			List<Parameter> list = new List<Parameter>();
			foreach (Parameter parameter in oldParameters)
			{
				list.Add(parameter);
			}
			for (int j = 0; j < newParameters.Length; j++)
			{
				array[j] = SqlDataSourceConfigureParametersPanel.CreateMergedParameter(newParameters[j], list);
			}
			return array;
		}

		public override bool OnNext()
		{
			SqlDataSourceQuery sqlDataSourceQuery = new SqlDataSourceQuery(this._selectQuery.Command, this._selectQuery.CommandType, this._parameterEditorUserControl.GetParameters());
			SqlDataSourceSummaryPanel sqlDataSourceSummaryPanel = base.NextPanel as SqlDataSourceSummaryPanel;
			if (sqlDataSourceSummaryPanel == null)
			{
				sqlDataSourceSummaryPanel = ((SqlDataSourceWizardForm)base.ParentWizard).GetSummaryPanel();
				base.NextPanel = sqlDataSourceSummaryPanel;
			}
			sqlDataSourceSummaryPanel.SetQueries(this._dataConnection, sqlDataSourceQuery, this._insertQuery, this._updateQuery, this._deleteQuery);
			return true;
		}

		private void OnParameterEditorUserControlParametersChanged(object sender, EventArgs e)
		{
			this.UpdateUI();
		}

		public override void OnPrevious()
		{
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if (base.Visible)
			{
				this.UpdateUI();
				base.ParentWizard.FinishButton.Enabled = false;
			}
		}

		private static bool ParametersMatch(Parameter parameter1, Parameter parameter2)
		{
			return string.Equals(parameter1.Name, parameter2.Name, StringComparison.OrdinalIgnoreCase) && parameter1.Direction == parameter2.Direction && parameter1.DbType == parameter2.DbType && (((parameter1.Type == TypeCode.Object || parameter1.Type == TypeCode.Empty) && (parameter2.Type == TypeCode.Object || parameter2.Type == TypeCode.Empty)) || parameter1.Type == parameter2.Type);
		}

		public void ResetUI()
		{
			this._parameterEditorUserControl.ClearParameters();
		}

		public void SetQueries(DesignerDataConnection dataConnection, SqlDataSourceQuery selectQuery, SqlDataSourceQuery insertQuery, SqlDataSourceQuery updateQuery, SqlDataSourceQuery deleteQuery)
		{
			this._dataConnection = dataConnection;
			this._selectQuery = selectQuery;
			this._insertQuery = insertQuery;
			this._updateQuery = updateQuery;
			this._deleteQuery = deleteQuery;
			this._previewTextBox.Text = this._selectQuery.Command;
			Parameter[] array = new Parameter[this._selectQuery.Parameters.Count];
			this._selectQuery.Parameters.CopyTo(array, 0);
			Parameter[] array2 = SqlDataSourceConfigureParametersPanel.MergeParameters(this._parameterEditorUserControl.GetParameters(), array);
			this._parameterEditorUserControl.ClearParameters();
			this._parameterEditorUserControl.AddParameters(array2);
		}

		private void UpdateUI()
		{
			base.ParentWizard.NextButton.Enabled = this._parameterEditorUserControl.ParametersConfigured;
		}

		private global::System.Windows.Forms.TextBox _previewTextBox;

		private global::System.Windows.Forms.Label _previewLabel;

		private global::System.Windows.Forms.Label _helpLabel;

		private ParameterEditorUserControl _parameterEditorUserControl;

		private SqlDataSourceDesigner _sqlDataSourceDesigner;

		private DesignerDataConnection _dataConnection;

		private SqlDataSourceQuery _selectQuery;

		private SqlDataSourceQuery _insertQuery;

		private SqlDataSourceQuery _updateQuery;

		private SqlDataSourceQuery _deleteQuery;
	}
}
