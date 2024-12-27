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
	// Token: 0x020004D4 RID: 1236
	internal partial class SqlDataSourceParameterValueEditorForm : DesignerForm
	{
		// Token: 0x06002C73 RID: 11379 RVA: 0x000FA2C0 File Offset: 0x000F92C0
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

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x06002C74 RID: 11380 RVA: 0x000FA3C0 File Offset: 0x000F93C0
		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.SqlDataSource.ParameterValueEditor";
			}
		}

		// Token: 0x06002C76 RID: 11382 RVA: 0x000FA798 File Offset: 0x000F9798
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

		// Token: 0x06002C77 RID: 11383 RVA: 0x000FA88C File Offset: 0x000F988C
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

		// Token: 0x06002C78 RID: 11384 RVA: 0x000FA984 File Offset: 0x000F9984
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

		// Token: 0x06002C79 RID: 11385 RVA: 0x000FAA88 File Offset: 0x000F9A88
		private void OnCancelButtonClick(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		// Token: 0x04001E5C RID: 7772
		private ArrayList _parameterItems;

		// Token: 0x020004D5 RID: 1237
		private class ParameterItem
		{
			// Token: 0x06002C7A RID: 11386 RVA: 0x000FAA97 File Offset: 0x000F9A97
			public ParameterItem(Parameter parameter)
			{
				this._parameter = parameter;
			}

			// Token: 0x17000851 RID: 2129
			// (get) Token: 0x06002C7B RID: 11387 RVA: 0x000FAAA6 File Offset: 0x000F9AA6
			// (set) Token: 0x06002C7C RID: 11388 RVA: 0x000FAABD File Offset: 0x000F9ABD
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

			// Token: 0x17000852 RID: 2130
			// (get) Token: 0x06002C7D RID: 11389 RVA: 0x000FAADF File Offset: 0x000F9ADF
			// (set) Token: 0x06002C7E RID: 11390 RVA: 0x000FAAEC File Offset: 0x000F9AEC
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

			// Token: 0x17000853 RID: 2131
			// (get) Token: 0x06002C7F RID: 11391 RVA: 0x000FAAFA File Offset: 0x000F9AFA
			// (set) Token: 0x06002C80 RID: 11392 RVA: 0x000FAB07 File Offset: 0x000F9B07
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

			// Token: 0x17000854 RID: 2132
			// (get) Token: 0x06002C81 RID: 11393 RVA: 0x000FAB15 File Offset: 0x000F9B15
			public Parameter Parameter
			{
				get
				{
					return this._parameter;
				}
			}

			// Token: 0x17000855 RID: 2133
			// (get) Token: 0x06002C82 RID: 11394 RVA: 0x000FAB1D File Offset: 0x000F9B1D
			// (set) Token: 0x06002C83 RID: 11395 RVA: 0x000FAB34 File Offset: 0x000F9B34
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

			// Token: 0x04001E5D RID: 7773
			private Parameter _parameter;
		}
	}
}
