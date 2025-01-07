using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Data;
using System.Data;
using System.Data.Common;
using System.Design;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	internal class SqlDataSourceCustomCommandEditor : UserControl
	{
		public SqlDataSourceCustomCommandEditor()
		{
			this.InitializeComponent();
			this.InitializeUI();
		}

		public bool HasQuery
		{
			get
			{
				if (this._sqlRadioButton.Checked)
				{
					return this._commandTextBox.Text.Trim().Length > 0;
				}
				SqlDataSourceCustomCommandEditor.StoredProcedureItem storedProcedureItem = this._storedProcedureComboBox.SelectedItem as SqlDataSourceCustomCommandEditor.StoredProcedureItem;
				return storedProcedureItem != null;
			}
		}

		public event EventHandler CommandChanged
		{
			add
			{
				base.Events.AddHandler(SqlDataSourceCustomCommandEditor.EventCommandChanged, value);
			}
			remove
			{
				base.Events.RemoveHandler(SqlDataSourceCustomCommandEditor.EventCommandChanged, value);
			}
		}

		private void InitializeComponent()
		{
			this._commandTextBox = new global::System.Windows.Forms.TextBox();
			this._queryBuilderButton = new global::System.Windows.Forms.Button();
			this._sqlRadioButton = new global::System.Windows.Forms.RadioButton();
			this._storedProcedureRadioButton = new global::System.Windows.Forms.RadioButton();
			this._storedProcedureComboBox = new AutoSizeComboBox();
			this._storedProcedurePanel = new global::System.Windows.Forms.Panel();
			this._sqlPanel = new global::System.Windows.Forms.Panel();
			this._storedProcedurePanel.SuspendLayout();
			this._sqlPanel.SuspendLayout();
			base.SuspendLayout();
			this._sqlRadioButton.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this._sqlRadioButton.Location = new Point(12, 12);
			this._sqlRadioButton.Name = "_sqlRadioButton";
			this._sqlRadioButton.Size = new Size(489, 20);
			this._sqlRadioButton.TabIndex = 10;
			this._sqlRadioButton.CheckedChanged += this.OnSqlRadioButtonCheckedChanged;
			this._sqlPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this._sqlPanel.Controls.Add(this._queryBuilderButton);
			this._sqlPanel.Controls.Add(this._commandTextBox);
			this._sqlPanel.Location = new Point(28, 32);
			this._sqlPanel.Name = "_sqlPanel";
			this._sqlPanel.Size = new Size(480, 121);
			this._sqlPanel.TabIndex = 20;
			this._storedProcedureRadioButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this._storedProcedureRadioButton.Location = new Point(12, 160);
			this._storedProcedureRadioButton.Name = "_storedProcedureRadioButton";
			this._storedProcedureRadioButton.Size = new Size(489, 20);
			this._storedProcedureRadioButton.TabIndex = 30;
			this._storedProcedurePanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this._storedProcedurePanel.Controls.Add(this._storedProcedureComboBox);
			this._storedProcedurePanel.Location = new Point(28, 180);
			this._storedProcedurePanel.Name = "_storedProcedurePanel";
			this._storedProcedurePanel.Size = new Size(265, 21);
			this._storedProcedurePanel.TabIndex = 40;
			this._commandTextBox.AcceptsReturn = true;
			this._commandTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this._commandTextBox.Location = new Point(0, 0);
			this._commandTextBox.Multiline = true;
			this._commandTextBox.Name = "_commandTextBox";
			this._commandTextBox.ScrollBars = global::System.Windows.Forms.ScrollBars.Vertical;
			this._commandTextBox.Size = new Size(480, 93);
			this._commandTextBox.TabIndex = 20;
			this._commandTextBox.TextChanged += this.OnCommandTextBoxTextChanged;
			this._queryBuilderButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this._queryBuilderButton.Location = new Point(330, 98);
			this._queryBuilderButton.Name = "_queryBuilderButton";
			this._queryBuilderButton.Size = new Size(150, 23);
			this._queryBuilderButton.TabIndex = 30;
			this._queryBuilderButton.Click += this.OnQueryBuilderButtonClick;
			this._storedProcedureComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this._storedProcedureComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			this._storedProcedureComboBox.Location = new Point(0, 0);
			this._storedProcedureComboBox.Name = "_storedProcedureComboBox";
			this._storedProcedureComboBox.Size = new Size(265, 21);
			this._storedProcedureComboBox.TabIndex = 10;
			this._storedProcedureComboBox.SelectedIndexChanged += this.OnStoredProcedureComboBoxSelectedIndexChanged;
			base.Controls.Add(this._sqlRadioButton);
			base.Controls.Add(this._sqlPanel);
			base.Controls.Add(this._storedProcedureRadioButton);
			base.Controls.Add(this._storedProcedurePanel);
			base.Name = "SqlDataSourceCustomCommandEditor";
			base.Size = new Size(522, 230);
			this._storedProcedurePanel.ResumeLayout(false);
			this._sqlPanel.ResumeLayout(false);
			this._sqlPanel.PerformLayout();
			base.ResumeLayout(false);
		}

		public SqlDataSourceQuery GetQuery()
		{
			Cursor cursor = Cursor.Current;
			SqlDataSourceQuery sqlDataSourceQuery;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				if (this._sqlRadioButton.Checked)
				{
					if (this._commandTextBox.Text.Trim().Length > 0)
					{
						SqlDataSourceCommandType sqlDataSourceCommandType;
						if (string.Equals(this._commandTextBox.Text, this._originalCommand, StringComparison.OrdinalIgnoreCase))
						{
							sqlDataSourceCommandType = this._commandType;
						}
						else
						{
							sqlDataSourceCommandType = SqlDataSourceCommandType.Text;
						}
						DbProviderFactory dbProviderFactory = SqlDataSourceDesigner.GetDbProviderFactory(this._dataConnection.ProviderName);
						ICollection collection;
						if (this._editorMode == QueryBuilderMode.Select || SqlDataSourceDesigner.SupportsNamedParameters(dbProviderFactory))
						{
							Parameter[] array = this._sqlDataSourceDesigner.InferParameterNames(this._dataConnection, this._commandTextBox.Text, sqlDataSourceCommandType);
							if (array == null)
							{
								return null;
							}
							ArrayList arrayList = new ArrayList(array);
							collection = this.MergeParameters(this._parameters, arrayList, SqlDataSourceDesigner.SupportsNamedParameters(dbProviderFactory));
						}
						else
						{
							collection = this._parameters;
						}
						sqlDataSourceQuery = new SqlDataSourceQuery(this._commandTextBox.Text, sqlDataSourceCommandType, collection);
					}
					else
					{
						sqlDataSourceQuery = new SqlDataSourceQuery(string.Empty, SqlDataSourceCommandType.Text, new Parameter[0]);
					}
				}
				else
				{
					SqlDataSourceCustomCommandEditor.StoredProcedureItem storedProcedureItem = this._storedProcedureComboBox.SelectedItem as SqlDataSourceCustomCommandEditor.StoredProcedureItem;
					if (storedProcedureItem == null)
					{
						sqlDataSourceQuery = new SqlDataSourceQuery(string.Empty, SqlDataSourceCommandType.Text, new Parameter[0]);
					}
					else
					{
						ArrayList arrayList2 = new ArrayList();
						ICollection collection2 = null;
						try
						{
							collection2 = storedProcedureItem.DesignerDataStoredProcedure.Parameters;
						}
						catch (Exception ex)
						{
							UIServiceHelper.ShowError(this._sqlDataSourceDesigner.Component.Site, ex, SR.GetString("SqlDataSourceCustomCommandEditor_CouldNotGetStoredProcedureSchema"));
							return null;
						}
						DbProviderFactory dbProviderFactory = SqlDataSourceDesigner.GetDbProviderFactory(this._dataConnection.ProviderName);
						if (collection2 != null && collection2.Count > 0)
						{
							foreach (object obj in collection2)
							{
								DesignerDataParameter designerDataParameter = (DesignerDataParameter)obj;
								string text = SqlDataSourceDesigner.StripParameterPrefix(designerDataParameter.Name);
								Parameter parameter = SqlDataSourceDesigner.CreateParameter(dbProviderFactory, text, designerDataParameter.DataType);
								parameter.Direction = designerDataParameter.Direction;
								arrayList2.Add(parameter);
							}
						}
						ICollection collection3 = this.MergeParameters(this._parameters, arrayList2, SqlDataSourceDesigner.SupportsNamedParameters(dbProviderFactory));
						sqlDataSourceQuery = new SqlDataSourceQuery(storedProcedureItem.DesignerDataStoredProcedure.Name, SqlDataSourceCommandType.StoredProcedure, collection3);
					}
				}
			}
			finally
			{
				Cursor.Current = cursor;
			}
			return sqlDataSourceQuery;
		}

		private void InitializeUI()
		{
			this._queryBuilderButton.Text = SR.GetString("SqlDataSourceCustomCommandEditor_QueryBuilderButton");
			this._sqlRadioButton.Text = SR.GetString("SqlDataSourceCustomCommandEditor_SqlLabel");
			this._storedProcedureRadioButton.Text = SR.GetString("SqlDataSourceCustomCommandEditor_StoredProcedureLabel");
		}

		private ICollection MergeParameters(ICollection originalParameters, ArrayList newParameters, bool useNamedParameters)
		{
			List<Parameter> list = new List<Parameter>();
			foreach (object obj in originalParameters)
			{
				Parameter parameter = (Parameter)obj;
				list.Add(parameter);
			}
			List<Parameter> list2 = new List<Parameter>();
			for (int i = 0; i < newParameters.Count; i++)
			{
				Parameter parameter2 = (Parameter)newParameters[i];
				Parameter parameter3 = null;
				foreach (Parameter parameter4 in list)
				{
					bool flag = (useNamedParameters ? (string.Equals(parameter4.Name, parameter2.Name, StringComparison.OrdinalIgnoreCase) && parameter4.Direction == parameter2.Direction) : (parameter4.Direction == parameter2.Direction));
					bool flag2 = parameter4.Direction == ParameterDirection.ReturnValue && parameter2.Direction == ParameterDirection.ReturnValue;
					if (flag || flag2)
					{
						parameter3 = parameter4;
						break;
					}
				}
				if (parameter3 != null)
				{
					list2.Add(parameter3);
					list.Remove(parameter3);
				}
				else if (parameter2.Direction == ParameterDirection.Input || parameter2.Direction == ParameterDirection.InputOutput)
				{
					list2.Add(parameter2);
				}
			}
			return list2;
		}

		private void OnCommandChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[SqlDataSourceCustomCommandEditor.EventCommandChanged] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		private void OnCommandTextBoxTextChanged(object sender, EventArgs e)
		{
			this.OnCommandChanged(EventArgs.Empty);
		}

		private void OnQueryBuilderButtonClick(object sender, EventArgs e)
		{
			IServiceProvider site = this._sqlDataSourceDesigner.Component.Site;
			if (this._dataConnection.ConnectionString != null && this._dataConnection.ConnectionString.Trim().Length == 0)
			{
				UIServiceHelper.ShowError(site, SR.GetString("SqlDataSourceCustomCommandEditor_NoConnectionString"));
				return;
			}
			DesignerDataConnection designerDataConnection = this._dataConnection;
			if (string.IsNullOrEmpty(this._dataConnection.ProviderName))
			{
				designerDataConnection = new DesignerDataConnection(this._dataConnection.Name, "System.Data.SqlClient", this._dataConnection.ConnectionString, this._dataConnection.IsConfigured);
			}
			string text = this._dataEnvironment.BuildQuery(this, designerDataConnection, this._editorMode, this._commandTextBox.Text);
			if (text != null && text.Length > 0)
			{
				this._commandTextBox.Text = text;
				this._commandTextBox.Focus();
				this._commandTextBox.Select(0, 0);
			}
		}

		private void OnSqlRadioButtonCheckedChanged(object sender, EventArgs e)
		{
			this.UpdateEnabledState();
		}

		private void OnStoredProcedureComboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			this.OnCommandChanged(EventArgs.Empty);
		}

		public void SetCommandData(SqlDataSourceDesigner sqlDataSourceDesigner, QueryBuilderMode editorMode)
		{
			this._sqlDataSourceDesigner = sqlDataSourceDesigner;
			this._editorMode = editorMode;
			this._queryBuilderButton.Enabled = false;
			IServiceProvider site = this._sqlDataSourceDesigner.Component.Site;
			if (site != null)
			{
				this._dataEnvironment = (IDataEnvironment)site.GetService(typeof(IDataEnvironment));
			}
		}

		public void SetConnection(DesignerDataConnection dataConnection)
		{
			this._dataConnection = dataConnection;
		}

		public void SetQuery(SqlDataSourceQuery query)
		{
			this._storedProcedureComboBox.SelectedIndex = -1;
			if (this._storedProcedures != null)
			{
				foreach (object obj in this._storedProcedureComboBox.Items)
				{
					SqlDataSourceCustomCommandEditor.StoredProcedureItem storedProcedureItem = (SqlDataSourceCustomCommandEditor.StoredProcedureItem)obj;
					if (storedProcedureItem.DesignerDataStoredProcedure.Name == query.Command)
					{
						this._storedProcedureComboBox.SelectedItem = storedProcedureItem;
						break;
					}
				}
			}
			if (this._storedProcedureComboBox.SelectedIndex != -1)
			{
				this._sqlRadioButton.Checked = false;
				this._storedProcedureRadioButton.Checked = true;
			}
			else
			{
				this._sqlRadioButton.Checked = true;
				this._storedProcedureRadioButton.Checked = false;
				if (this._storedProcedureComboBox.Items.Count > 0)
				{
					this._storedProcedureComboBox.SelectedIndex = 0;
				}
			}
			if (!this._queryInitialized)
			{
				this._commandTextBox.Text = query.Command;
				this._originalCommand = query.Command;
				this._commandType = query.CommandType;
				this._parameters = query.Parameters;
				this._queryInitialized = true;
			}
			this.UpdateEnabledState();
		}

		public void SetStoredProcedures(ICollection storedProcedures)
		{
			this._storedProcedures = storedProcedures;
			bool flag = this._storedProcedures != null && this._storedProcedures.Count > 0;
			this._storedProcedureRadioButton.Enabled = flag;
			this._storedProcedureComboBox.Items.Clear();
			if (flag)
			{
				List<SqlDataSourceCustomCommandEditor.StoredProcedureItem> list = new List<SqlDataSourceCustomCommandEditor.StoredProcedureItem>();
				foreach (object obj in this._storedProcedures)
				{
					DesignerDataStoredProcedure designerDataStoredProcedure = (DesignerDataStoredProcedure)obj;
					list.Add(new SqlDataSourceCustomCommandEditor.StoredProcedureItem(designerDataStoredProcedure));
				}
				list.Sort((SqlDataSourceCustomCommandEditor.StoredProcedureItem a, SqlDataSourceCustomCommandEditor.StoredProcedureItem b) => string.Compare(a.DesignerDataStoredProcedure.Name, b.DesignerDataStoredProcedure.Name, StringComparison.InvariantCultureIgnoreCase));
				this._storedProcedureComboBox.Items.AddRange(list.ToArray());
				this._storedProcedureComboBox.InvalidateDropDownWidth();
			}
		}

		private void UpdateEnabledState()
		{
			bool @checked = this._sqlRadioButton.Checked;
			this._commandTextBox.Enabled = @checked;
			this._queryBuilderButton.Enabled = @checked;
			this._storedProcedureComboBox.Enabled = !@checked;
			this.OnCommandChanged(EventArgs.Empty);
		}

		private static readonly object EventCommandChanged = new object();

		private global::System.Windows.Forms.TextBox _commandTextBox;

		private global::System.Windows.Forms.Button _queryBuilderButton;

		private global::System.Windows.Forms.RadioButton _sqlRadioButton;

		private global::System.Windows.Forms.RadioButton _storedProcedureRadioButton;

		private AutoSizeComboBox _storedProcedureComboBox;

		private global::System.Windows.Forms.Panel _sqlPanel;

		private global::System.Windows.Forms.Panel _storedProcedurePanel;

		private QueryBuilderMode _editorMode;

		private SqlDataSourceDesigner _sqlDataSourceDesigner;

		private DesignerDataConnection _dataConnection;

		private ICollection _storedProcedures;

		private IDataEnvironment _dataEnvironment;

		private ICollection _parameters;

		private string _originalCommand;

		private SqlDataSourceCommandType _commandType;

		private bool _queryInitialized;

		[CompilerGenerated]
		private static Comparison<SqlDataSourceCustomCommandEditor.StoredProcedureItem> <>9__CachedAnonymousMethodDelegate1;

		private sealed class StoredProcedureItem
		{
			public StoredProcedureItem(DesignerDataStoredProcedure designerDataStoredProcedure)
			{
				this._designerDataStoredProcedure = designerDataStoredProcedure;
			}

			public DesignerDataStoredProcedure DesignerDataStoredProcedure
			{
				get
				{
					return this._designerDataStoredProcedure;
				}
			}

			public override string ToString()
			{
				return this._designerDataStoredProcedure.Name;
			}

			private DesignerDataStoredProcedure _designerDataStoredProcedure;
		}
	}
}
