using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Design;
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
	internal partial class SqlDataSourceConfigureFilterForm : DesignerForm
	{
		public SqlDataSourceConfigureFilterForm(SqlDataSourceDesigner sqlDataSourceDesigner, SqlDataSourceTableQuery tableQuery)
			: base(sqlDataSourceDesigner.Component.Site)
		{
			this._sqlDataSourceDesigner = sqlDataSourceDesigner;
			this._tableQuery = tableQuery.Clone();
			this.InitializeComponent();
			this.InitializeUI();
			this.CreateParameterList();
			foreach (SqlDataSourceConfigureFilterForm.ParameterEditor parameterEditor in SqlDataSourceConfigureFilterForm._parameterEditors.Values)
			{
				parameterEditor.Visible = false;
				this._propertiesPanel.Controls.Add(parameterEditor);
				this._sourceComboBox.Items.Add(parameterEditor);
				parameterEditor.ParameterChanged += this.OnParameterChanged;
			}
			this._sourceComboBox.InvalidateDropDownWidth();
			Cursor cursor = Cursor.Current;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				foreach (object obj in tableQuery.DesignerDataTable.Columns)
				{
					DesignerDataColumn designerDataColumn = (DesignerDataColumn)obj;
					this._columnsComboBox.Items.Add(new SqlDataSourceConfigureFilterForm.ColumnItem(designerDataColumn));
				}
				this._columnsComboBox.InvalidateDropDownWidth();
				foreach (SqlDataSourceFilterClause sqlDataSourceFilterClause in this._tableQuery.FilterClauses)
				{
					SqlDataSourceConfigureFilterForm.FilterClauseItem filterClauseItem = new SqlDataSourceConfigureFilterForm.FilterClauseItem(this._sqlDataSourceDesigner.Component.Site, this._tableQuery, sqlDataSourceFilterClause, (SqlDataSource)this._sqlDataSourceDesigner.Component);
					this._whereClausesListView.Items.Add(filterClauseItem);
					filterClauseItem.Refresh();
				}
				if (this._whereClausesListView.Items.Count > 0)
				{
					this._whereClausesListView.Items[0].Selected = true;
					this._whereClausesListView.Items[0].Focused = true;
				}
				this._okButton.Enabled = false;
				this.UpdateDeleteButton();
				this.UpdateOperators();
			}
			finally
			{
				Cursor.Current = cursor;
			}
		}

		public IList<SqlDataSourceFilterClause> FilterClauses
		{
			get
			{
				return this._tableQuery.FilterClauses;
			}
		}

		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.SqlDataSource.ConfigureFilter";
			}
		}

		private void CreateParameterList()
		{
			SqlDataSourceConfigureFilterForm._parameterEditors = new Dictionary<Type, SqlDataSourceConfigureFilterForm.ParameterEditor>();
			SqlDataSourceConfigureFilterForm._parameterEditors.Add(typeof(Parameter), new SqlDataSourceConfigureFilterForm.StaticParameterEditor(base.ServiceProvider));
			SqlDataSourceConfigureFilterForm._parameterEditors.Add(typeof(ControlParameter), new SqlDataSourceConfigureFilterForm.ControlParameterEditor(base.ServiceProvider, (SqlDataSource)this._sqlDataSourceDesigner.Component));
			SqlDataSourceConfigureFilterForm._parameterEditors.Add(typeof(CookieParameter), new SqlDataSourceConfigureFilterForm.CookieParameterEditor(base.ServiceProvider));
			SqlDataSourceConfigureFilterForm._parameterEditors.Add(typeof(FormParameter), new SqlDataSourceConfigureFilterForm.FormParameterEditor(base.ServiceProvider));
			SqlDataSourceConfigureFilterForm._parameterEditors.Add(typeof(ProfileParameter), new SqlDataSourceConfigureFilterForm.ProfileParameterEditor(base.ServiceProvider));
			SqlDataSourceConfigureFilterForm._parameterEditors.Add(typeof(QueryStringParameter), new SqlDataSourceConfigureFilterForm.QueryStringParameterEditor(base.ServiceProvider));
			SqlDataSourceConfigureFilterForm._parameterEditors.Add(typeof(SessionParameter), new SqlDataSourceConfigureFilterForm.SessionParameterEditor(base.ServiceProvider));
		}

		private void InitializeUI()
		{
			this._helpLabel.Text = SR.GetString("SqlDataSourceConfigureFilterForm_HelpLabel");
			this._columnLabel.Text = SR.GetString("SqlDataSourceConfigureFilterForm_ColumnLabel");
			this._operatorLabel.Text = SR.GetString("SqlDataSourceConfigureFilterForm_OperatorLabel");
			this._whereClausesLabel.Text = SR.GetString("SqlDataSourceConfigureFilterForm_WhereLabel");
			this._expressionLabel.Text = SR.GetString("SqlDataSourceConfigureFilterForm_ExpressionLabel");
			this._valueLabel.Text = SR.GetString("SqlDataSourceConfigureFilterForm_ValueLabel");
			this._expressionColumnHeader.Text = SR.GetString("SqlDataSourceConfigureFilterForm_ExpressionColumnHeader");
			this._valueColumnHeader.Text = SR.GetString("SqlDataSourceConfigureFilterForm_ValueColumnHeader");
			this._propertiesGroupBox.Text = SR.GetString("SqlDataSourceConfigureFilterForm_ParameterPropertiesGroup");
			this._sourceLabel.Text = SR.GetString("SqlDataSourceConfigureFilterForm_SourceLabel");
			this._addButton.Text = SR.GetString("SqlDataSourceConfigureFilterForm_AddButton");
			this._removeButton.Text = SR.GetString("SqlDataSourceConfigureFilterForm_RemoveButton");
			this._okButton.Text = SR.GetString("OK");
			this._cancelButton.Text = SR.GetString("Cancel");
			this.Text = SR.GetString("SqlDataSourceConfigureFilterForm_Caption");
		}

		private SqlDataSourceFilterClause GetCurrentFilterClause()
		{
			SqlDataSourceConfigureFilterForm.OperatorItem operatorItem = this._operatorsComboBox.SelectedItem as SqlDataSourceConfigureFilterForm.OperatorItem;
			if (operatorItem == null)
			{
				return null;
			}
			SqlDataSourceConfigureFilterForm.ColumnItem columnItem = this._columnsComboBox.SelectedItem as SqlDataSourceConfigureFilterForm.ColumnItem;
			if (columnItem == null)
			{
				return null;
			}
			Parameter parameter;
			string text;
			if (operatorItem.IsBinary)
			{
				SqlDataSourceConfigureFilterForm.ParameterEditor parameterEditor = this._sourceComboBox.SelectedItem as SqlDataSourceConfigureFilterForm.ParameterEditor;
				if (parameterEditor == null)
				{
					return null;
				}
				parameter = parameterEditor.Parameter;
				if (parameter != null)
				{
					SqlDataSourceQuery selectQuery = this._tableQuery.GetSelectQuery();
					StringCollection stringCollection = new StringCollection();
					if (selectQuery != null && selectQuery.Parameters != null)
					{
						foreach (object obj in selectQuery.Parameters)
						{
							Parameter parameter2 = (Parameter)obj;
							stringCollection.Add(parameter2.Name);
						}
					}
					SqlDataSourceColumnData sqlDataSourceColumnData = new SqlDataSourceColumnData(this._tableQuery.DesignerDataConnection, columnItem.DesignerDataColumn, stringCollection);
					parameter.Name = sqlDataSourceColumnData.WebParameterName;
					DbProviderFactory dbProviderFactory = SqlDataSourceDesigner.GetDbProviderFactory(this._tableQuery.DesignerDataConnection.ProviderName);
					if (SqlDataSourceDesigner.IsNewSqlServer2008Type(dbProviderFactory, columnItem.DesignerDataColumn.DataType))
					{
						parameter.DbType = columnItem.DesignerDataColumn.DataType;
						parameter.Type = TypeCode.Empty;
					}
					else
					{
						parameter.DbType = DbType.Object;
						parameter.Type = SqlDataSourceDesigner.ConvertDbTypeToTypeCode(columnItem.DesignerDataColumn.DataType);
					}
					text = sqlDataSourceColumnData.ParameterPlaceholder;
				}
				else
				{
					text = string.Empty;
				}
			}
			else
			{
				text = "";
				parameter = null;
			}
			return new SqlDataSourceFilterClause(this._tableQuery.DesignerDataConnection, this._tableQuery.DesignerDataTable, columnItem.DesignerDataColumn, operatorItem.OperatorFormat, operatorItem.IsBinary, text, parameter);
		}

		private void OnAddButtonClick(object sender, EventArgs e)
		{
			SqlDataSourceFilterClause currentFilterClause = this.GetCurrentFilterClause();
			SqlDataSourceConfigureFilterForm.FilterClauseItem filterClauseItem = new SqlDataSourceConfigureFilterForm.FilterClauseItem(this._sqlDataSourceDesigner.Component.Site, this._tableQuery, currentFilterClause, (SqlDataSource)this._sqlDataSourceDesigner.Component);
			this._whereClausesListView.Items.Add(filterClauseItem);
			filterClauseItem.Selected = true;
			filterClauseItem.Focused = true;
			filterClauseItem.EnsureVisible();
			this._tableQuery.FilterClauses.Add(currentFilterClause);
			this._columnsComboBox.SelectedIndex = -1;
			this._okButton.Enabled = true;
			filterClauseItem.Refresh();
		}

		private void OnCancelButtonClick(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
			base.Close();
		}

		private void OnColumnsComboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			this.UpdateOperators();
		}

		private void OnOkButtonClick(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
			base.Close();
		}

		private void OnOperatorsComboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			this.UpdateParameter();
		}

		private void OnParameterChanged(object sender, EventArgs e)
		{
			this.UpdateExpression();
			this.UpdateAddButtonEnabled();
		}

		private void OnRemoveButtonClick(object sender, EventArgs e)
		{
			if (this._whereClausesListView.SelectedItems.Count > 0)
			{
				int num = this._whereClausesListView.SelectedIndices[0];
				SqlDataSourceConfigureFilterForm.FilterClauseItem filterClauseItem = this._whereClausesListView.SelectedItems[0] as SqlDataSourceConfigureFilterForm.FilterClauseItem;
				this._whereClausesListView.Items.Remove(filterClauseItem);
				this._tableQuery.FilterClauses.Remove(filterClauseItem.FilterClause);
				this._okButton.Enabled = true;
				if (num < this._whereClausesListView.Items.Count)
				{
					ListViewItem listViewItem = this._whereClausesListView.Items[num];
					listViewItem.Selected = true;
					listViewItem.Focused = true;
					listViewItem.EnsureVisible();
					this._whereClausesListView.Focus();
					return;
				}
				if (this._whereClausesListView.Items.Count > 0)
				{
					ListViewItem listViewItem2 = this._whereClausesListView.Items[num - 1];
					listViewItem2.Selected = true;
					listViewItem2.Focused = true;
					listViewItem2.EnsureVisible();
					this._whereClausesListView.Focus();
				}
			}
		}

		private void OnSourceComboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			this.UpdateParameter();
		}

		private void OnWhereClausesListViewSelectedIndexChanged(object sender, EventArgs e)
		{
			this.UpdateDeleteButton();
		}

		private void UpdateDeleteButton()
		{
			this._removeButton.Enabled = this._whereClausesListView.SelectedItems.Count > 0;
		}

		private void UpdateAddButtonEnabled()
		{
			if (!(this._columnsComboBox.SelectedItem is SqlDataSourceConfigureFilterForm.ColumnItem))
			{
				this._addButton.Enabled = false;
				return;
			}
			SqlDataSourceConfigureFilterForm.OperatorItem operatorItem = this._operatorsComboBox.SelectedItem as SqlDataSourceConfigureFilterForm.OperatorItem;
			if (operatorItem == null)
			{
				this._addButton.Enabled = false;
				return;
			}
			SqlDataSourceConfigureFilterForm.ParameterEditor parameterEditor = this._sourceComboBox.SelectedItem as SqlDataSourceConfigureFilterForm.ParameterEditor;
			this._addButton.Enabled = !operatorItem.IsBinary ^ (parameterEditor != null && parameterEditor.HasCompleteInformation);
		}

		private void UpdateOperators()
		{
			if (this._columnsComboBox.SelectedItem == null)
			{
				this._operatorsComboBox.SelectedItem = -1;
				this._operatorsComboBox.Items.Clear();
				this._operatorsComboBox.Enabled = false;
				this._operatorLabel.Enabled = false;
				this.UpdateParameter();
				return;
			}
			this._operatorsComboBox.Enabled = true;
			this._operatorLabel.Enabled = true;
			this._operatorsComboBox.Items.Clear();
			this._operatorsComboBox.Items.Add(new SqlDataSourceConfigureFilterForm.OperatorItem("{0} = {1}", "=", true));
			this._operatorsComboBox.Items.Add(new SqlDataSourceConfigureFilterForm.OperatorItem("{0} < {1}", "<", true));
			this._operatorsComboBox.Items.Add(new SqlDataSourceConfigureFilterForm.OperatorItem("{0} > {1}", ">", true));
			this._operatorsComboBox.Items.Add(new SqlDataSourceConfigureFilterForm.OperatorItem("{0} <= {1}", "<=", true));
			this._operatorsComboBox.Items.Add(new SqlDataSourceConfigureFilterForm.OperatorItem("{0} >= {1}", ">=", true));
			this._operatorsComboBox.Items.Add(new SqlDataSourceConfigureFilterForm.OperatorItem("{0} <> {1}", "<>", true));
			SqlDataSourceConfigureFilterForm.ColumnItem columnItem = (SqlDataSourceConfigureFilterForm.ColumnItem)this._columnsComboBox.SelectedItem;
			DesignerDataColumn designerDataColumn = columnItem.DesignerDataColumn;
			if (designerDataColumn.Nullable)
			{
				this._operatorsComboBox.Items.Add(new SqlDataSourceConfigureFilterForm.OperatorItem("{0} IS NULL", "IS NULL", false));
				this._operatorsComboBox.Items.Add(new SqlDataSourceConfigureFilterForm.OperatorItem("{0} IS NOT NULL", "IS NOT NULL", false));
			}
			DbType dataType = designerDataColumn.DataType;
			if (dataType == DbType.String || dataType == DbType.AnsiString || dataType == DbType.AnsiStringFixedLength || dataType == DbType.StringFixedLength)
			{
				this._operatorsComboBox.Items.Add(new SqlDataSourceConfigureFilterForm.OperatorItem("{0} LIKE '%' + {1} + '%'", "LIKE", true));
				this._operatorsComboBox.Items.Add(new SqlDataSourceConfigureFilterForm.OperatorItem("{0} NOT LIKE '%' + {1} + '%'", "NOT LIKE", true));
				this._operatorsComboBox.Items.Add(new SqlDataSourceConfigureFilterForm.OperatorItem("CONTAINS({0}, {1})", "CONTAINS", true));
			}
			this._operatorsComboBox.InvalidateDropDownWidth();
			this._operatorsComboBox.SelectedIndex = 0;
			this.UpdateParameter();
		}

		private void UpdateExpression()
		{
			SqlDataSourceConfigureFilterForm.ParameterEditor parameterEditor = this._sourceComboBox.SelectedItem as SqlDataSourceConfigureFilterForm.ParameterEditor;
			if (this._operatorsComboBox.SelectedItem == null || parameterEditor == null)
			{
				this._expressionTextBox.Text = string.Empty;
				this._valueTextBox.Text = string.Empty;
				return;
			}
			SqlDataSourceFilterClause currentFilterClause = this.GetCurrentFilterClause();
			if (currentFilterClause != null)
			{
				this._expressionTextBox.Text = currentFilterClause.ToString();
			}
			else
			{
				this._expressionTextBox.Text = string.Empty;
			}
			if (parameterEditor.Parameter == null)
			{
				this._valueTextBox.Text = string.Empty;
				return;
			}
			bool flag;
			string parameterExpression = ParameterEditorUserControl.GetParameterExpression(this._sqlDataSourceDesigner.Component.Site, parameterEditor.Parameter, (SqlDataSource)this._sqlDataSourceDesigner.Component, out flag);
			if (flag)
			{
				this._valueTextBox.Text = string.Empty;
				return;
			}
			this._valueTextBox.Text = parameterExpression;
		}

		private void UpdateParameter()
		{
			SqlDataSourceConfigureFilterForm.OperatorItem operatorItem = this._operatorsComboBox.SelectedItem as SqlDataSourceConfigureFilterForm.OperatorItem;
			if (operatorItem != null && operatorItem.IsBinary)
			{
				this._expressionLabel.Enabled = true;
				this._expressionTextBox.Enabled = true;
				this._valueLabel.Enabled = true;
				this._valueTextBox.Enabled = true;
				this._propertiesGroupBox.Enabled = true;
				this._sourceLabel.Enabled = true;
				this._sourceComboBox.Enabled = true;
			}
			else
			{
				this._expressionLabel.Enabled = false;
				this._expressionTextBox.Enabled = false;
				this._valueLabel.Enabled = false;
				this._valueTextBox.Enabled = false;
				this._propertiesGroupBox.Enabled = false;
				this._sourceLabel.Enabled = false;
				this._sourceComboBox.Enabled = false;
				this._sourceComboBox.SelectedItem = null;
			}
			foreach (SqlDataSourceConfigureFilterForm.ParameterEditor parameterEditor in SqlDataSourceConfigureFilterForm._parameterEditors.Values)
			{
				parameterEditor.Visible = false;
			}
			SqlDataSourceConfigureFilterForm.ParameterEditor parameterEditor2 = this._sourceComboBox.SelectedItem as SqlDataSourceConfigureFilterForm.ParameterEditor;
			if (parameterEditor2 != null)
			{
				parameterEditor2.Visible = true;
				parameterEditor2.Initialize();
				this._propertiesPanel.Visible = true;
			}
			else
			{
				this._propertiesPanel.Visible = false;
			}
			this.UpdateExpression();
			this.UpdateAddButtonEnabled();
		}

		private static IDictionary<Type, SqlDataSourceConfigureFilterForm.ParameterEditor> _parameterEditors;

		private SqlDataSourceDesigner _sqlDataSourceDesigner;

		private SqlDataSourceTableQuery _tableQuery;

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
				return this._designerDataColumn.Name;
			}

			private DesignerDataColumn _designerDataColumn;
		}

		private sealed class OperatorItem
		{
			public OperatorItem(string operatorFormat, string operatorName, bool isBinary)
			{
				this._operatorName = operatorName;
				this._operatorFormat = operatorFormat;
				this._isBinary = isBinary;
			}

			public bool IsBinary
			{
				get
				{
					return this._isBinary;
				}
			}

			public string OperatorFormat
			{
				get
				{
					return this._operatorFormat;
				}
			}

			public string OperatorName
			{
				get
				{
					return this._operatorName;
				}
			}

			public override string ToString()
			{
				return this._operatorName;
			}

			private string _operatorName;

			private bool _isBinary;

			private string _operatorFormat;
		}

		private sealed class FilterClauseItem : ListViewItem
		{
			public FilterClauseItem(IServiceProvider serviceProvider, SqlDataSourceTableQuery tableQuery, SqlDataSourceFilterClause filterClause, SqlDataSource sqlDataSource)
			{
				this._filterClause = filterClause;
				this._tableQuery = tableQuery;
				this._serviceProvider = serviceProvider;
				this._sqlDataSource = sqlDataSource;
			}

			public SqlDataSourceFilterClause FilterClause
			{
				get
				{
					return this._filterClause;
				}
			}

			public void Refresh()
			{
				base.SubItems.Clear();
				base.Text = this._filterClause.ToString();
				ListView listView = base.ListView;
				IServiceProvider serviceProvider = null;
				if (listView != null)
				{
					serviceProvider = ((SqlDataSourceConfigureFilterForm)listView.Parent).ServiceProvider;
				}
				string text;
				if (this._filterClause.Parameter == null)
				{
					text = string.Empty;
				}
				else
				{
					bool flag;
					text = ParameterEditorUserControl.GetParameterExpression(serviceProvider, this._filterClause.Parameter, this._sqlDataSource, out flag);
					if (flag)
					{
						text = string.Empty;
					}
				}
				ListViewItem.ListViewSubItem listViewSubItem = new ListViewItem.ListViewSubItem();
				listViewSubItem.Text = text;
				base.SubItems.Add(listViewSubItem);
			}

			private SqlDataSourceFilterClause _filterClause;

			private SqlDataSourceTableQuery _tableQuery;

			private IServiceProvider _serviceProvider;

			private SqlDataSource _sqlDataSource;
		}

		private abstract class ParameterEditor : global::System.Windows.Forms.Panel
		{
			protected ParameterEditor(IServiceProvider serviceProvider)
			{
				this._serviceProvider = serviceProvider;
			}

			public abstract string EditorName { get; }

			public abstract bool HasCompleteInformation { get; }

			public abstract Parameter Parameter { get; }

			protected IServiceProvider ServiceProvider
			{
				get
				{
					return this._serviceProvider;
				}
			}

			public event EventHandler ParameterChanged
			{
				add
				{
					base.Events.AddHandler(SqlDataSourceConfigureFilterForm.ParameterEditor.EventParameterChanged, value);
				}
				remove
				{
					base.Events.RemoveHandler(SqlDataSourceConfigureFilterForm.ParameterEditor.EventParameterChanged, value);
				}
			}

			public abstract void Initialize();

			protected void OnParameterChanged()
			{
				EventHandler eventHandler = base.Events[SqlDataSourceConfigureFilterForm.ParameterEditor.EventParameterChanged] as EventHandler;
				if (eventHandler != null)
				{
					eventHandler(this, EventArgs.Empty);
				}
			}

			public override string ToString()
			{
				return this.EditorName;
			}

			protected const int ControlWidth = 220;

			private static readonly object EventParameterChanged = new object();

			private IServiceProvider _serviceProvider;
		}

		private sealed class StaticParameterEditor : SqlDataSourceConfigureFilterForm.ParameterEditor
		{
			public StaticParameterEditor(IServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				base.SuspendLayout();
				base.Size = new Size(220, 44);
				this._defaultValueLabel = new global::System.Windows.Forms.Label();
				this._defaultValueTextBox = new global::System.Windows.Forms.TextBox();
				this._defaultValueTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._defaultValueLabel.Location = new Point(0, 0);
				this._defaultValueLabel.Name = "StaticDefaultValueLabel";
				this._defaultValueLabel.Size = new Size(220, 16);
				this._defaultValueLabel.TabIndex = 10;
				this._defaultValueLabel.Text = SR.GetString("SqlDataSourceConfigureFilterForm_StaticParameterEditor_ValueLabel");
				this._defaultValueTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._defaultValueTextBox.Location = new Point(0, 23);
				this._defaultValueTextBox.Name = "StaticDefaultValueTextBox";
				this._defaultValueTextBox.Size = new Size(220, 20);
				this._defaultValueTextBox.TabIndex = 20;
				this._defaultValueTextBox.TextChanged += this.OnDefaultValueTextBoxTextChanged;
				base.Controls.Add(this._defaultValueLabel);
				base.Controls.Add(this._defaultValueTextBox);
				this.Dock = DockStyle.Fill;
				base.ResumeLayout();
			}

			public override string EditorName
			{
				get
				{
					return "None";
				}
			}

			public override bool HasCompleteInformation
			{
				get
				{
					return true;
				}
			}

			public override Parameter Parameter
			{
				get
				{
					return this._parameter;
				}
			}

			public override void Initialize()
			{
				this._parameter = new Parameter();
				this._defaultValueTextBox.Text = string.Empty;
			}

			private void OnDefaultValueTextBoxTextChanged(object s, EventArgs e)
			{
				this._parameter.DefaultValue = this._defaultValueTextBox.Text;
				base.OnParameterChanged();
			}

			private global::System.Windows.Forms.Label _defaultValueLabel;

			private global::System.Windows.Forms.TextBox _defaultValueTextBox;

			private Parameter _parameter;
		}

		private sealed class CookieParameterEditor : SqlDataSourceConfigureFilterForm.ParameterEditor
		{
			public CookieParameterEditor(IServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				base.SuspendLayout();
				base.Size = new Size(220, 44);
				this._cookieNameLabel = new global::System.Windows.Forms.Label();
				this._cookieNameTextBox = new global::System.Windows.Forms.TextBox();
				this._defaultValueLabel = new global::System.Windows.Forms.Label();
				this._defaultValueTextBox = new global::System.Windows.Forms.TextBox();
				this._cookieNameLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._cookieNameLabel.Location = new Point(0, 0);
				this._cookieNameLabel.Name = "CookieNameLabel";
				this._cookieNameLabel.Size = new Size(220, 16);
				this._cookieNameLabel.TabIndex = 10;
				this._cookieNameLabel.Text = SR.GetString("SqlDataSourceConfigureFilterForm_CookieParameterEditor_CookieNameLabel");
				this._cookieNameTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._cookieNameTextBox.Location = new Point(0, 23);
				this._cookieNameTextBox.Name = "CookieNameTextBox";
				this._cookieNameTextBox.Size = new Size(220, 20);
				this._cookieNameTextBox.TabIndex = 20;
				this._cookieNameTextBox.TextChanged += this.OnCookieNameTextBoxTextChanged;
				this._defaultValueLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._defaultValueLabel.Location = new Point(0, 48);
				this._defaultValueLabel.Name = "CookieDefaultValueLabel";
				this._defaultValueLabel.Size = new Size(220, 16);
				this._defaultValueLabel.TabIndex = 30;
				this._defaultValueLabel.Text = SR.GetString("SqlDataSourceConfigureFilterForm_ParameterEditor_DefaultValue");
				this._defaultValueTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._defaultValueTextBox.Location = new Point(0, 68);
				this._defaultValueTextBox.Name = "CookieDefaultValueTextBox";
				this._defaultValueTextBox.Size = new Size(220, 20);
				this._defaultValueTextBox.TabIndex = 40;
				this._defaultValueTextBox.TextChanged += this.OnDefaultValueTextBoxTextChanged;
				base.Controls.Add(this._cookieNameLabel);
				base.Controls.Add(this._cookieNameTextBox);
				base.Controls.Add(this._defaultValueLabel);
				base.Controls.Add(this._defaultValueTextBox);
				this.Dock = DockStyle.Fill;
				base.ResumeLayout();
			}

			public override string EditorName
			{
				get
				{
					return "Cookie";
				}
			}

			public override bool HasCompleteInformation
			{
				get
				{
					return this._parameter.CookieName.Length > 0;
				}
			}

			public override Parameter Parameter
			{
				get
				{
					return this._parameter;
				}
			}

			public override void Initialize()
			{
				this._parameter = new CookieParameter();
				this._cookieNameTextBox.Text = string.Empty;
				this._defaultValueTextBox.Text = string.Empty;
			}

			private void OnCookieNameTextBoxTextChanged(object s, EventArgs e)
			{
				this._parameter.CookieName = this._cookieNameTextBox.Text;
				base.OnParameterChanged();
			}

			private void OnDefaultValueTextBoxTextChanged(object s, EventArgs e)
			{
				this._parameter.DefaultValue = this._defaultValueTextBox.Text;
			}

			private global::System.Windows.Forms.Label _cookieNameLabel;

			private global::System.Windows.Forms.TextBox _cookieNameTextBox;

			private global::System.Windows.Forms.Label _defaultValueLabel;

			private global::System.Windows.Forms.TextBox _defaultValueTextBox;

			private CookieParameter _parameter;
		}

		private sealed class ControlParameterEditor : SqlDataSourceConfigureFilterForm.ParameterEditor
		{
			public ControlParameterEditor(IServiceProvider serviceProvider, Control control)
				: base(serviceProvider)
			{
				this._control = control;
				base.SuspendLayout();
				base.Size = new Size(220, 44);
				this._controlIDLabel = new global::System.Windows.Forms.Label();
				this._controlIDComboBox = new AutoSizeComboBox();
				this._defaultValueLabel = new global::System.Windows.Forms.Label();
				this._defaultValueTextBox = new global::System.Windows.Forms.TextBox();
				this._controlIDLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._controlIDLabel.Location = new Point(0, 0);
				this._controlIDLabel.Name = "ControlIDLabel";
				this._controlIDLabel.Size = new Size(220, 16);
				this._controlIDLabel.TabIndex = 10;
				this._controlIDLabel.Text = SR.GetString("SqlDataSourceConfigureFilterForm_ControlParameterEditor_ControlIDLabel");
				this._controlIDComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._controlIDComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
				this._controlIDComboBox.Location = new Point(0, 23);
				this._controlIDComboBox.Name = "ControlIDComboBox";
				this._controlIDComboBox.Size = new Size(220, 20);
				this._controlIDComboBox.Sorted = true;
				this._controlIDComboBox.TabIndex = 20;
				this._controlIDComboBox.SelectedIndexChanged += this.OnControlIDComboBoxSelectedIndexChanged;
				this._defaultValueLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._defaultValueLabel.Location = new Point(0, 48);
				this._defaultValueLabel.Name = "ControlDefaultValueLabel";
				this._defaultValueLabel.Size = new Size(220, 16);
				this._defaultValueLabel.TabIndex = 30;
				this._defaultValueLabel.Text = SR.GetString("SqlDataSourceConfigureFilterForm_ParameterEditor_DefaultValue");
				this._defaultValueTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._defaultValueTextBox.Location = new Point(0, 68);
				this._defaultValueTextBox.Name = "ControlDefaultValueTextBox";
				this._defaultValueTextBox.Size = new Size(220, 20);
				this._defaultValueTextBox.TabIndex = 40;
				this._defaultValueTextBox.TextChanged += this.OnDefaultValueTextBoxTextChanged;
				base.Controls.Add(this._controlIDLabel);
				base.Controls.Add(this._controlIDComboBox);
				base.Controls.Add(this._defaultValueLabel);
				base.Controls.Add(this._defaultValueTextBox);
				if (base.ServiceProvider != null)
				{
					IDesignerHost designerHost = (IDesignerHost)base.ServiceProvider.GetService(typeof(IDesignerHost));
					if (designerHost != null)
					{
						ParameterEditorUserControl.ControlItem[] controlItems = ParameterEditorUserControl.ControlItem.GetControlItems(designerHost, this._control);
						foreach (ParameterEditorUserControl.ControlItem controlItem in controlItems)
						{
							this._controlIDComboBox.Items.Add(controlItem);
						}
						this._controlIDComboBox.InvalidateDropDownWidth();
					}
				}
				this.Dock = DockStyle.Fill;
				base.ResumeLayout();
			}

			public override string EditorName
			{
				get
				{
					return "Control";
				}
			}

			public override bool HasCompleteInformation
			{
				get
				{
					return this._controlIDComboBox.SelectedItem != null;
				}
			}

			public override Parameter Parameter
			{
				get
				{
					return this._parameter;
				}
			}

			public override void Initialize()
			{
				this._parameter = new ControlParameter();
				this._controlIDComboBox.SelectedItem = null;
				this._defaultValueTextBox.Text = string.Empty;
			}

			private void OnControlIDComboBoxSelectedIndexChanged(object s, EventArgs e)
			{
				ParameterEditorUserControl.ControlItem controlItem = this._controlIDComboBox.SelectedItem as ParameterEditorUserControl.ControlItem;
				if (controlItem == null)
				{
					this._parameter.ControlID = string.Empty;
					this._parameter.PropertyName = string.Empty;
				}
				else
				{
					this._parameter.ControlID = controlItem.ControlID;
					this._parameter.PropertyName = controlItem.PropertyName;
				}
				base.OnParameterChanged();
			}

			private void OnDefaultValueTextBoxTextChanged(object s, EventArgs e)
			{
				this._parameter.DefaultValue = this._defaultValueTextBox.Text;
			}

			private global::System.Windows.Forms.Label _controlIDLabel;

			private AutoSizeComboBox _controlIDComboBox;

			private global::System.Windows.Forms.Label _defaultValueLabel;

			private global::System.Windows.Forms.TextBox _defaultValueTextBox;

			private ControlParameter _parameter;

			private Control _control;
		}

		private sealed class FormParameterEditor : SqlDataSourceConfigureFilterForm.ParameterEditor
		{
			public FormParameterEditor(IServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				base.SuspendLayout();
				base.Size = new Size(220, 44);
				this._formFieldLabel = new global::System.Windows.Forms.Label();
				this._formFieldTextBox = new global::System.Windows.Forms.TextBox();
				this._defaultValueLabel = new global::System.Windows.Forms.Label();
				this._defaultValueTextBox = new global::System.Windows.Forms.TextBox();
				this._formFieldLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._formFieldLabel.Location = new Point(0, 0);
				this._formFieldLabel.Name = "FormFieldLabel";
				this._formFieldLabel.Size = new Size(220, 16);
				this._formFieldLabel.TabIndex = 10;
				this._formFieldLabel.Text = SR.GetString("SqlDataSourceConfigureFilterForm_FormParameterEditor_FormFieldLabel");
				this._formFieldTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._formFieldTextBox.Location = new Point(0, 23);
				this._formFieldTextBox.Name = "FormFieldTextBox";
				this._formFieldTextBox.Size = new Size(220, 20);
				this._formFieldTextBox.TabIndex = 20;
				this._formFieldTextBox.TextChanged += this.OnFormFieldTextBoxTextChanged;
				this._defaultValueLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._defaultValueLabel.Location = new Point(0, 48);
				this._defaultValueLabel.Name = "FormDefaultValueLabel";
				this._defaultValueLabel.Size = new Size(220, 16);
				this._defaultValueLabel.TabIndex = 30;
				this._defaultValueLabel.Text = SR.GetString("SqlDataSourceConfigureFilterForm_ParameterEditor_DefaultValue");
				this._defaultValueTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._defaultValueTextBox.Location = new Point(0, 68);
				this._defaultValueTextBox.Name = "FormDefaultValueTextBox";
				this._defaultValueTextBox.Size = new Size(220, 20);
				this._defaultValueTextBox.TabIndex = 40;
				this._defaultValueTextBox.TextChanged += this.OnDefaultValueTextBoxTextChanged;
				base.Controls.Add(this._formFieldLabel);
				base.Controls.Add(this._formFieldTextBox);
				base.Controls.Add(this._defaultValueLabel);
				base.Controls.Add(this._defaultValueTextBox);
				this.Dock = DockStyle.Fill;
				base.ResumeLayout();
			}

			public override string EditorName
			{
				get
				{
					return "Form";
				}
			}

			public override bool HasCompleteInformation
			{
				get
				{
					return this._parameter.FormField.Length > 0;
				}
			}

			public override Parameter Parameter
			{
				get
				{
					return this._parameter;
				}
			}

			public override void Initialize()
			{
				this._parameter = new FormParameter();
				this._formFieldTextBox.Text = string.Empty;
				this._defaultValueTextBox.Text = string.Empty;
			}

			private void OnDefaultValueTextBoxTextChanged(object s, EventArgs e)
			{
				this._parameter.DefaultValue = this._defaultValueTextBox.Text;
			}

			private void OnFormFieldTextBoxTextChanged(object s, EventArgs e)
			{
				this._parameter.FormField = this._formFieldTextBox.Text;
				base.OnParameterChanged();
			}

			private global::System.Windows.Forms.Label _formFieldLabel;

			private global::System.Windows.Forms.TextBox _formFieldTextBox;

			private global::System.Windows.Forms.Label _defaultValueLabel;

			private global::System.Windows.Forms.TextBox _defaultValueTextBox;

			private FormParameter _parameter;
		}

		private sealed class SessionParameterEditor : SqlDataSourceConfigureFilterForm.ParameterEditor
		{
			public SessionParameterEditor(IServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				base.SuspendLayout();
				base.Size = new Size(220, 44);
				this._sessionFieldLabel = new global::System.Windows.Forms.Label();
				this._sessionFieldTextBox = new global::System.Windows.Forms.TextBox();
				this._defaultValueLabel = new global::System.Windows.Forms.Label();
				this._defaultValueTextBox = new global::System.Windows.Forms.TextBox();
				this._sessionFieldLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._sessionFieldLabel.Location = new Point(0, 0);
				this._sessionFieldLabel.Name = "SessionFieldLabel";
				this._sessionFieldLabel.Size = new Size(220, 16);
				this._sessionFieldLabel.TabIndex = 10;
				this._sessionFieldLabel.Text = SR.GetString("SqlDataSourceConfigureFilterForm_SessionParameterEditor_SessionFieldLabel");
				this._sessionFieldTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._sessionFieldTextBox.Location = new Point(0, 23);
				this._sessionFieldTextBox.Name = "SessionFieldTextBox";
				this._sessionFieldTextBox.Size = new Size(220, 20);
				this._sessionFieldTextBox.TabIndex = 20;
				this._sessionFieldTextBox.TextChanged += this.OnSessionFieldTextBoxTextChanged;
				this._defaultValueLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._defaultValueLabel.Location = new Point(0, 48);
				this._defaultValueLabel.Name = "SessionDefaultValueLabel";
				this._defaultValueLabel.Size = new Size(220, 16);
				this._defaultValueLabel.TabIndex = 30;
				this._defaultValueLabel.Text = SR.GetString("SqlDataSourceConfigureFilterForm_ParameterEditor_DefaultValue");
				this._defaultValueTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._defaultValueTextBox.Location = new Point(0, 68);
				this._defaultValueTextBox.Name = "SessionDefaultValueTextBox";
				this._defaultValueTextBox.Size = new Size(220, 20);
				this._defaultValueTextBox.TabIndex = 40;
				this._defaultValueTextBox.TextChanged += this.OnDefaultValueTextBoxTextChanged;
				base.Controls.Add(this._sessionFieldLabel);
				base.Controls.Add(this._sessionFieldTextBox);
				base.Controls.Add(this._defaultValueLabel);
				base.Controls.Add(this._defaultValueTextBox);
				this.Dock = DockStyle.Fill;
				base.ResumeLayout();
			}

			public override string EditorName
			{
				get
				{
					return "Session";
				}
			}

			public override bool HasCompleteInformation
			{
				get
				{
					return this._parameter.SessionField.Length > 0;
				}
			}

			public override Parameter Parameter
			{
				get
				{
					return this._parameter;
				}
			}

			public override void Initialize()
			{
				this._parameter = new SessionParameter();
				this._sessionFieldTextBox.Text = string.Empty;
				this._defaultValueTextBox.Text = string.Empty;
			}

			private void OnDefaultValueTextBoxTextChanged(object s, EventArgs e)
			{
				this._parameter.DefaultValue = this._defaultValueTextBox.Text;
			}

			private void OnSessionFieldTextBoxTextChanged(object s, EventArgs e)
			{
				this._parameter.SessionField = this._sessionFieldTextBox.Text;
				base.OnParameterChanged();
			}

			private global::System.Windows.Forms.Label _sessionFieldLabel;

			private global::System.Windows.Forms.TextBox _sessionFieldTextBox;

			private global::System.Windows.Forms.Label _defaultValueLabel;

			private global::System.Windows.Forms.TextBox _defaultValueTextBox;

			private SessionParameter _parameter;
		}

		private sealed class QueryStringParameterEditor : SqlDataSourceConfigureFilterForm.ParameterEditor
		{
			public QueryStringParameterEditor(IServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				base.SuspendLayout();
				base.Size = new Size(220, 44);
				this._queryStringFieldLabel = new global::System.Windows.Forms.Label();
				this._queryStringFieldTextBox = new global::System.Windows.Forms.TextBox();
				this._defaultValueLabel = new global::System.Windows.Forms.Label();
				this._defaultValueTextBox = new global::System.Windows.Forms.TextBox();
				this._queryStringFieldLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._queryStringFieldLabel.Location = new Point(0, 0);
				this._queryStringFieldLabel.Name = "QueryStringFieldLabel";
				this._queryStringFieldLabel.Size = new Size(220, 16);
				this._queryStringFieldLabel.TabIndex = 10;
				this._queryStringFieldLabel.Text = SR.GetString("SqlDataSourceConfigureFilterForm_QueryStringParameterEditor_QueryStringFieldLabel");
				this._queryStringFieldTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._queryStringFieldTextBox.Location = new Point(0, 23);
				this._queryStringFieldTextBox.Name = "QueryStringFieldTextBox";
				this._queryStringFieldTextBox.Size = new Size(220, 20);
				this._queryStringFieldTextBox.TabIndex = 20;
				this._queryStringFieldTextBox.TextChanged += this.OnQueryStringFieldTextBoxTextChanged;
				this._defaultValueLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._defaultValueLabel.Location = new Point(0, 48);
				this._defaultValueLabel.Name = "QueryStringDefaultValueLabel";
				this._defaultValueLabel.Size = new Size(220, 16);
				this._defaultValueLabel.TabIndex = 30;
				this._defaultValueLabel.Text = SR.GetString("SqlDataSourceConfigureFilterForm_ParameterEditor_DefaultValue");
				this._defaultValueTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._defaultValueTextBox.Location = new Point(0, 68);
				this._defaultValueTextBox.Name = "QueryStringDefaultValueTextBox";
				this._defaultValueTextBox.Size = new Size(220, 20);
				this._defaultValueTextBox.TabIndex = 40;
				this._defaultValueTextBox.TextChanged += this.OnDefaultValueTextBoxTextChanged;
				base.Controls.Add(this._queryStringFieldLabel);
				base.Controls.Add(this._queryStringFieldTextBox);
				base.Controls.Add(this._defaultValueLabel);
				base.Controls.Add(this._defaultValueTextBox);
				this.Dock = DockStyle.Fill;
				base.ResumeLayout();
			}

			public override string EditorName
			{
				get
				{
					return "QueryString";
				}
			}

			public override bool HasCompleteInformation
			{
				get
				{
					return this._parameter.QueryStringField.Length > 0;
				}
			}

			public override Parameter Parameter
			{
				get
				{
					return this._parameter;
				}
			}

			public override void Initialize()
			{
				this._parameter = new QueryStringParameter();
				this._queryStringFieldTextBox.Text = string.Empty;
				this._defaultValueTextBox.Text = string.Empty;
			}

			private void OnDefaultValueTextBoxTextChanged(object s, EventArgs e)
			{
				this._parameter.DefaultValue = this._defaultValueTextBox.Text;
			}

			private void OnQueryStringFieldTextBoxTextChanged(object s, EventArgs e)
			{
				this._parameter.QueryStringField = this._queryStringFieldTextBox.Text;
				base.OnParameterChanged();
			}

			private global::System.Windows.Forms.Label _queryStringFieldLabel;

			private global::System.Windows.Forms.TextBox _queryStringFieldTextBox;

			private global::System.Windows.Forms.Label _defaultValueLabel;

			private global::System.Windows.Forms.TextBox _defaultValueTextBox;

			private QueryStringParameter _parameter;
		}

		private sealed class ProfileParameterEditor : SqlDataSourceConfigureFilterForm.ParameterEditor
		{
			public ProfileParameterEditor(IServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				base.SuspendLayout();
				base.Size = new Size(220, 44);
				this._propertyNameLabel = new global::System.Windows.Forms.Label();
				this._propertyNameTextBox = new global::System.Windows.Forms.TextBox();
				this._defaultValueLabel = new global::System.Windows.Forms.Label();
				this._defaultValueTextBox = new global::System.Windows.Forms.TextBox();
				this._propertyNameLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._propertyNameLabel.Location = new Point(0, 0);
				this._propertyNameLabel.Name = "ProfilePropertyNameLabel";
				this._propertyNameLabel.Size = new Size(220, 16);
				this._propertyNameLabel.TabIndex = 10;
				this._propertyNameLabel.Text = SR.GetString("SqlDataSourceConfigureFilterForm_ProfileParameterEditor_PropertyNameLabel");
				this._propertyNameTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._propertyNameTextBox.Location = new Point(0, 23);
				this._propertyNameTextBox.Name = "ProfilePropertyNameTextBox";
				this._propertyNameTextBox.Size = new Size(220, 20);
				this._propertyNameTextBox.TabIndex = 20;
				this._propertyNameTextBox.TextChanged += this.OnPropertyNameTextBoxTextChanged;
				this._defaultValueLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._defaultValueLabel.Location = new Point(0, 48);
				this._defaultValueLabel.Name = "ProfileDefaultValueLabel";
				this._defaultValueLabel.Size = new Size(220, 16);
				this._defaultValueLabel.TabIndex = 30;
				this._defaultValueLabel.Text = SR.GetString("SqlDataSourceConfigureFilterForm_ParameterEditor_DefaultValue");
				this._defaultValueTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				this._defaultValueTextBox.Location = new Point(0, 68);
				this._defaultValueTextBox.Name = "ProfileDefaultValueTextBox";
				this._defaultValueTextBox.Size = new Size(220, 20);
				this._defaultValueTextBox.TabIndex = 40;
				this._defaultValueTextBox.TextChanged += this.OnDefaultValueTextBoxTextChanged;
				base.Controls.Add(this._propertyNameLabel);
				base.Controls.Add(this._propertyNameTextBox);
				base.Controls.Add(this._defaultValueLabel);
				base.Controls.Add(this._defaultValueTextBox);
				this.Dock = DockStyle.Fill;
				base.ResumeLayout();
			}

			public override string EditorName
			{
				get
				{
					return "Profile";
				}
			}

			public override bool HasCompleteInformation
			{
				get
				{
					return this._parameter.PropertyName.Length > 0;
				}
			}

			public override Parameter Parameter
			{
				get
				{
					return this._parameter;
				}
			}

			public override void Initialize()
			{
				this._parameter = new ProfileParameter();
				this._propertyNameTextBox.Text = string.Empty;
				this._defaultValueTextBox.Text = string.Empty;
			}

			private void OnDefaultValueTextBoxTextChanged(object s, EventArgs e)
			{
				this._parameter.DefaultValue = this._defaultValueTextBox.Text;
			}

			private void OnPropertyNameTextBoxTextChanged(object s, EventArgs e)
			{
				this._parameter.PropertyName = this._propertyNameTextBox.Text;
				base.OnParameterChanged();
			}

			private global::System.Windows.Forms.Label _propertyNameLabel;

			private global::System.Windows.Forms.TextBox _propertyNameTextBox;

			private global::System.Windows.Forms.Label _defaultValueLabel;

			private global::System.Windows.Forms.TextBox _defaultValueTextBox;

			private ProfileParameter _parameter;
		}
	}
}
