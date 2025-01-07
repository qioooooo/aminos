using System;
using System.Collections;
using System.Data;
using System.Design;
using System.Drawing;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	internal partial class SqlDataSourceParameterValueEditorForm : DesignerForm
	{
		public SqlDataSourceParameterValueEditorForm(IServiceProvider serviceProvider, ParameterCollection parameters)
			: base(serviceProvider)
		{
			this._parameterItems = new ArrayList();
			foreach (object obj in parameters)
			{
				Parameter parameter = (Parameter)obj;
				this._parameterItems.Add(new SqlDataSourceParameterValueEditorForm.ParameterItem(parameter));
			}
			this.InitializeComponent();
			this.InitializeUI();
			string[] names = Enum.GetNames(typeof(TypeCode));
			Array.Sort<string>(names);
			((DataGridViewComboBoxColumn)this._parametersDataGridView.Columns[1]).Items.AddRange(names);
			string[] names2 = Enum.GetNames(typeof(DbType));
			Array.Sort<string>(names2);
			((DataGridViewComboBoxColumn)this._parametersDataGridView.Columns[2]).Items.AddRange(names2);
			this._parametersDataGridView.DataSource = this._parameterItems;
		}

		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.SqlDataSource.ParameterValueEditor";
			}
		}

		private void InitializeUI()
		{
			this._helpLabel.Text = SR.GetString("SqlDataSourceParameterValueEditorForm_HelpLabel");
			this._parametersDataGridView.AccessibleName = SR.GetString("SqlDataSourceParameterValueEditorForm_ParametersGridAccessibleName");
			this._cancelButton.Text = SR.GetString("Cancel");
			this._okButton.Text = SR.GetString("OK");
			this.Text = SR.GetString("SqlDataSourceParameterValueEditorForm_Caption");
			this._parametersDataGridView.Columns[0].HeaderText = SR.GetString("SqlDataSourceParameterValueEditorForm_ParameterColumnHeader");
			this._parametersDataGridView.Columns[1].HeaderText = SR.GetString("SqlDataSourceParameterValueEditorForm_TypeColumnHeader");
			this._parametersDataGridView.Columns[2].HeaderText = SR.GetString("SqlDataSourceParameterValueEditorForm_DbTypeColumnHeader");
			this._parametersDataGridView.Columns[3].HeaderText = SR.GetString("SqlDataSourceParameterValueEditorForm_ValueColumnHeader");
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if (base.Visible)
			{
				int num = (int)Math.Floor((double)(this._parametersDataGridView.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 2 * SystemInformation.Border3DSize.Width) / 4.5);
				this._parametersDataGridView.Columns[0].Width = (int)((double)num * 1.5);
				this._parametersDataGridView.Columns[1].Width = num;
				this._parametersDataGridView.Columns[2].Width = num;
				this._parametersDataGridView.Columns[3].Width = num;
				this._parametersDataGridView.AutoResizeColumnHeadersHeight();
				for (int i = 0; i < this._parametersDataGridView.Rows.Count; i++)
				{
					this._parametersDataGridView.AutoResizeRow(i, DataGridViewAutoSizeRowMode.AllCells);
				}
			}
		}

		private void OnOkButtonClick(object sender, EventArgs e)
		{
			ParameterCollection parameterCollection = new ParameterCollection();
			foreach (object obj in this._parameterItems)
			{
				SqlDataSourceParameterValueEditorForm.ParameterItem parameterItem = (SqlDataSourceParameterValueEditorForm.ParameterItem)obj;
				if (parameterItem.Parameter.DbType == DbType.Object)
				{
					parameterCollection.Add(new Parameter(parameterItem.Parameter.Name, parameterItem.Parameter.Type, parameterItem.Parameter.DefaultValue));
				}
				else
				{
					parameterCollection.Add(new Parameter(parameterItem.Parameter.Name, parameterItem.Parameter.DbType, parameterItem.Parameter.DefaultValue));
				}
			}
			try
			{
				parameterCollection.GetValues(null, null);
			}
			catch (Exception ex)
			{
				UIServiceHelper.ShowError(base.ServiceProvider, ex, SR.GetString("SqlDataSourceParameterValueEditorForm_InvalidParameter"));
				return;
			}
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		private void OnCancelButtonClick(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		private ArrayList _parameterItems;

		private class ParameterItem
		{
			public ParameterItem(Parameter parameter)
			{
				this._parameter = parameter;
			}

			public string DbType
			{
				get
				{
					return this._parameter.DbType.ToString();
				}
				set
				{
					this._parameter.DbType = (DbType)Enum.Parse(typeof(DbType), value);
				}
			}

			public string DefaultValue
			{
				get
				{
					return this._parameter.DefaultValue;
				}
				set
				{
					this._parameter.DefaultValue = value;
				}
			}

			public string Name
			{
				get
				{
					return this._parameter.Name;
				}
				set
				{
					this._parameter.Name = value;
				}
			}

			public Parameter Parameter
			{
				get
				{
					return this._parameter;
				}
			}

			public string Type
			{
				get
				{
					return this._parameter.Type.ToString();
				}
				set
				{
					this._parameter.Type = (TypeCode)Enum.Parse(typeof(TypeCode), value);
				}
			}

			private Parameter _parameter;
		}
	}
}
