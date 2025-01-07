using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Design.Data;
using System.Data.Common;
using System.Design;
using System.Drawing;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	internal partial class SqlDataSourceQueryEditorForm : DesignerForm
	{
		public SqlDataSourceQueryEditorForm(IServiceProvider serviceProvider, SqlDataSourceDesigner sqlDataSourceDesigner, string providerName, string connectionString, DataSourceOperation operation, SqlDataSourceCommandType commandType, string command, IList originalParameters)
			: base(serviceProvider)
		{
			this._sqlDataSourceDesigner = sqlDataSourceDesigner;
			this.InitializeComponent();
			this.InitializeUI();
			if (string.IsNullOrEmpty(providerName))
			{
				providerName = "System.Data.SqlClient";
			}
			this._dataConnection = new DesignerDataConnection(string.Empty, providerName, connectionString);
			this._commandType = commandType;
			this._commandTextBox.Text = command;
			this._originalParameters = originalParameters;
			string text = Enum.GetName(typeof(DataSourceOperation), operation).ToUpperInvariant();
			this._commandLabel.Text = SR.GetString("SqlDataSourceQueryEditorForm_CommandLabel", new object[] { text });
			ArrayList arrayList = new ArrayList(originalParameters.Count);
			sqlDataSourceDesigner.CopyList(originalParameters, arrayList);
			this._parameterEditorUserControl.AddParameters((Parameter[])arrayList.ToArray(typeof(Parameter)));
			this._commandTextBox.Select(0, 0);
			switch (operation)
			{
			case DataSourceOperation.Delete:
				this._queryBuilderMode = QueryBuilderMode.Delete;
				return;
			case DataSourceOperation.Insert:
				this._queryBuilderMode = QueryBuilderMode.Insert;
				return;
			case DataSourceOperation.Select:
				this._queryBuilderMode = QueryBuilderMode.Select;
				return;
			case DataSourceOperation.Update:
				this._queryBuilderMode = QueryBuilderMode.Update;
				return;
			default:
				return;
			}
		}

		public string Command
		{
			get
			{
				return this._commandTextBox.Text;
			}
		}

		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.SqlDataSource.QueryEditor";
			}
		}

		private void InitializeUI()
		{
			this._okButton.Text = SR.GetString("OK");
			this._cancelButton.Text = SR.GetString("Cancel");
			this._inferParametersButton.Text = SR.GetString("SqlDataSourceQueryEditorForm_InferParametersButton");
			this._queryBuilderButton.Text = SR.GetString("SqlDataSourceQueryEditorForm_QueryBuilderButton");
			this.Text = SR.GetString("SqlDataSourceQueryEditorForm_Caption");
			this._dataEnvironment = (IDataEnvironment)base.ServiceProvider.GetService(typeof(IDataEnvironment));
			this._queryBuilderButton.Enabled = this._dataEnvironment != null;
		}

		private void OnCancelButtonClick(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		private void OnInferParametersButtonClick(object sender, EventArgs e)
		{
			if (this._commandTextBox.Text.Trim().Length == 0)
			{
				UIServiceHelper.ShowError(base.ServiceProvider, SR.GetString("SqlDataSourceQueryEditorForm_InferNeedsCommand"));
				return;
			}
			Parameter[] array = this._sqlDataSourceDesigner.InferParameterNames(this._dataConnection, this._commandTextBox.Text, this._commandType);
			if (array != null)
			{
				Parameter[] parameters = this._parameterEditorUserControl.GetParameters();
				StringCollection stringCollection = new StringCollection();
				foreach (Parameter parameter in parameters)
				{
					stringCollection.Add(parameter.Name);
				}
				bool flag = true;
				try
				{
					DbProviderFactory dbProviderFactory = SqlDataSourceDesigner.GetDbProviderFactory(this._dataConnection.ProviderName);
					flag = SqlDataSourceDesigner.SupportsNamedParameters(dbProviderFactory);
				}
				catch
				{
				}
				if (flag)
				{
					List<Parameter> list = new List<Parameter>();
					foreach (Parameter parameter2 in array)
					{
						if (!stringCollection.Contains(parameter2.Name))
						{
							list.Add(parameter2);
						}
						else
						{
							stringCollection.Remove(parameter2.Name);
						}
					}
					this._parameterEditorUserControl.AddParameters(list.ToArray());
					return;
				}
				List<Parameter> list2 = new List<Parameter>();
				foreach (Parameter parameter3 in array)
				{
					list2.Add(parameter3);
				}
				foreach (Parameter parameter4 in parameters)
				{
					Parameter parameter5 = null;
					foreach (Parameter parameter6 in list2)
					{
						if (parameter6.Direction == parameter4.Direction)
						{
							parameter5 = parameter6;
							break;
						}
					}
					if (parameter5 != null)
					{
						list2.Remove(parameter5);
					}
				}
				this._parameterEditorUserControl.AddParameters(list2.ToArray());
			}
		}

		private void OnOkButtonClick(object sender, EventArgs e)
		{
			this._sqlDataSourceDesigner.CopyList(this._parameterEditorUserControl.GetParameters(), this._originalParameters);
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		private void OnQueryBuilderButtonClick(object sender, EventArgs e)
		{
			if (this._dataConnection.ConnectionString == null || this._dataConnection.ConnectionString.Trim().Length == 0)
			{
				UIServiceHelper.ShowError(base.ServiceProvider, SR.GetString("SqlDataSourceQueryEditorForm_QueryBuilderNeedsConnectionString"));
				return;
			}
			string text = this._dataEnvironment.BuildQuery(this, this._dataConnection, this._queryBuilderMode, this._commandTextBox.Text);
			if (text != null && text.Length > 0)
			{
				this._commandTextBox.Text = text;
			}
			this._commandTextBox.Focus();
			this._commandTextBox.Select(0, 0);
		}

		private QueryBuilderMode _queryBuilderMode;

		private IDataEnvironment _dataEnvironment;

		private SqlDataSourceCommandType _commandType;

		private DesignerDataConnection _dataConnection;

		private IList _originalParameters;
	}
}
