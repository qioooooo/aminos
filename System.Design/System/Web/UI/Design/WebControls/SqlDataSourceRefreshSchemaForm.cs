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
	internal partial class SqlDataSourceRefreshSchemaForm : DesignerForm
	{
		public SqlDataSourceRefreshSchemaForm(IServiceProvider serviceProvider, SqlDataSourceDesigner sqlDataSourceDesigner, ParameterCollection parameters)
			: base(serviceProvider)
		{
			this._sqlDataSourceDesigner = sqlDataSourceDesigner;
			this._sqlDataSource = (SqlDataSource)this._sqlDataSourceDesigner.Component;
			this._connectionString = this._sqlDataSourceDesigner.ConnectionString;
			this._providerName = this._sqlDataSourceDesigner.ProviderName;
			this._selectCommand = this._sqlDataSourceDesigner.SelectCommand;
			this._selectCommandType = this._sqlDataSource.SelectCommandType;
			this.InitializeComponent();
			this.InitializeUI();
			Array values = Enum.GetValues(typeof(TypeCode));
			Array.Sort(values, new SqlDataSourceRefreshSchemaForm.TypeCodeComparer());
			foreach (object obj in values)
			{
				TypeCode typeCode = (TypeCode)obj;
				((DataGridViewComboBoxColumn)this._parametersDataGridView.Columns[1]).Items.Add(typeCode);
			}
			Array values2 = Enum.GetValues(typeof(DbType));
			Array.Sort(values2, new SqlDataSourceRefreshSchemaForm.DbTypeComparer());
			foreach (object obj2 in values2)
			{
				DbType dbType = (DbType)obj2;
				((DataGridViewComboBoxColumn)this._parametersDataGridView.Columns[2]).Items.Add(dbType);
			}
			ArrayList arrayList = new ArrayList(parameters.Count);
			foreach (object obj3 in parameters)
			{
				Parameter parameter = (Parameter)obj3;
				arrayList.Add(new SqlDataSourceRefreshSchemaForm.ParameterItem(parameter));
			}
			this._parametersDataGridView.DataSource = arrayList;
			this._commandTextBox.Text = this._selectCommand;
			this._commandTextBox.Select(0, 0);
		}

		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.SqlDataSource.RefreshSchema";
			}
		}

		private void InitializeUI()
		{
			this.Text = SR.GetString("SqlDataSourceRefreshSchemaForm_Title", new object[] { this._sqlDataSource.ID });
			this._helpLabel.Text = SR.GetString("SqlDataSourceRefreshSchemaForm_HelpLabel");
			this._commandLabel.Text = SR.GetString("SqlDataSource_General_PreviewLabel");
			this._parametersLabel.Text = SR.GetString("SqlDataSourceRefreshSchemaForm_ParametersLabel");
			this._parametersDataGridView.AccessibleName = SR.GetString("SqlDataSourceParameterValueEditorForm_ParametersGridAccessibleName");
			this._okButton.Text = SR.GetString("OK");
			this._cancelButton.Text = SR.GetString("Cancel");
			this._parametersDataGridView.Columns[0].HeaderText = SR.GetString("SqlDataSourceParameterValueEditorForm_ParameterColumnHeader");
			this._parametersDataGridView.Columns[1].HeaderText = SR.GetString("SqlDataSourceParameterValueEditorForm_TypeColumnHeader");
			this._parametersDataGridView.Columns[2].HeaderText = SR.GetString("SqlDataSourceParameterValueEditorForm_DbTypeColumnHeader");
			this._parametersDataGridView.Columns[3].HeaderText = SR.GetString("SqlDataSourceParameterValueEditorForm_ValueColumnHeader");
		}

		private void OnOkButtonClick(object sender, EventArgs e)
		{
			ICollection collection = (ICollection)this._parametersDataGridView.DataSource;
			ParameterCollection parameterCollection = new ParameterCollection();
			foreach (object obj in collection)
			{
				SqlDataSourceRefreshSchemaForm.ParameterItem parameterItem = (SqlDataSourceRefreshSchemaForm.ParameterItem)obj;
				if (parameterItem.DbType == DbType.Object)
				{
					parameterCollection.Add(new Parameter(parameterItem.Name, parameterItem.Type, parameterItem.DefaultValue));
				}
				else
				{
					parameterCollection.Add(new Parameter(parameterItem.Name, parameterItem.DbType, parameterItem.DefaultValue));
				}
			}
			bool flag = this._sqlDataSourceDesigner.RefreshSchema(new DesignerDataConnection(string.Empty, this._providerName, this._connectionString), this._selectCommand, this._selectCommandType, parameterCollection, false);
			if (flag)
			{
				base.DialogResult = DialogResult.OK;
				base.Close();
			}
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

		private SqlDataSourceDesigner _sqlDataSourceDesigner;

		private SqlDataSource _sqlDataSource;

		private string _connectionString;

		private string _providerName;

		private string _selectCommand;

		private SqlDataSourceCommandType _selectCommandType;

		private sealed class ParameterItem
		{
			public ParameterItem(Parameter p)
			{
				this._name = p.Name;
				this._dbType = p.DbType;
				this._type = p.Type;
				this._defaultValue = p.DefaultValue;
			}

			public string Name
			{
				get
				{
					return this._name;
				}
			}

			public DbType DbType
			{
				get
				{
					return this._dbType;
				}
				set
				{
					this._dbType = value;
				}
			}

			public TypeCode Type
			{
				get
				{
					return this._type;
				}
				set
				{
					this._type = value;
				}
			}

			public string DefaultValue
			{
				get
				{
					return this._defaultValue;
				}
				set
				{
					this._defaultValue = value;
				}
			}

			private string _name;

			private DbType _dbType;

			private TypeCode _type;

			private string _defaultValue;
		}

		private sealed class TypeCodeComparer : IComparer
		{
			int IComparer.Compare(object x, object y)
			{
				return string.Compare(Enum.GetName(typeof(TypeCode), x), Enum.GetName(typeof(TypeCode), y), StringComparison.OrdinalIgnoreCase);
			}
		}

		private sealed class DbTypeComparer : IComparer
		{
			int IComparer.Compare(object x, object y)
			{
				return string.Compare(Enum.GetName(typeof(DbType), x), Enum.GetName(typeof(DbType), y), StringComparison.OrdinalIgnoreCase);
			}
		}
	}
}
