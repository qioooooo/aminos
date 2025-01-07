using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Data;
using System.Data.Common;
using System.Design;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	internal class SqlDataSourceSaveConfiguredConnectionPanel : WizardPanel
	{
		public SqlDataSourceSaveConfiguredConnectionPanel(SqlDataSourceDesigner sqlDataSourceDesigner, IDataEnvironment dataEnvironment)
		{
			this._sqlDataSourceDesigner = sqlDataSourceDesigner;
			this._sqlDataSource = (SqlDataSource)this._sqlDataSourceDesigner.Component;
			this._dataEnvironment = dataEnvironment;
			this.InitializeComponent();
			this.InitializeUI();
		}

		internal DesignerDataConnection CurrentConnection
		{
			get
			{
				return this._dataConnection;
			}
		}

		private void CheckShouldAllowNext()
		{
			if (base.ParentWizard != null)
			{
				base.ParentWizard.NextButton.Enabled = !this._saveCheckBox.Checked || this._nameTextBox.Text.Trim().Length > 0;
			}
		}

		private string CreateDefaultConnectionName()
		{
			ICollection connections = this._dataEnvironment.Connections;
			StringDictionary stringDictionary = new StringDictionary();
			if (connections != null)
			{
				foreach (object obj in connections)
				{
					DesignerDataConnection designerDataConnection = (DesignerDataConnection)obj;
					if (designerDataConnection != null && designerDataConnection.IsConfigured)
					{
						stringDictionary.Add(designerDataConnection.Name, null);
					}
				}
			}
			int num = 2;
			string connectionName = SqlDataSourceSaveConfiguredConnectionPanel.ConnectionStringHelper.GetConnectionName(this._dataConnection);
			string text = connectionName;
			while (stringDictionary.ContainsKey(text))
			{
				text = connectionName + num.ToString(CultureInfo.InvariantCulture);
				num++;
			}
			return text;
		}

		private void InitializeComponent()
		{
			this._saveLabel = new global::System.Windows.Forms.Label();
			this._saveCheckBox = new global::System.Windows.Forms.CheckBox();
			this._nameTextBox = new global::System.Windows.Forms.TextBox();
			this._helpLabel = new global::System.Windows.Forms.Label();
			this._nameHelpLabel = new global::System.Windows.Forms.Label();
			base.SuspendLayout();
			this._helpLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this._helpLabel.Location = new Point(0, 0);
			this._helpLabel.Name = "_helpLabel";
			this._helpLabel.Size = new Size(544, 56);
			this._helpLabel.TabIndex = 10;
			this._saveLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this._saveLabel.Location = new Point(0, 75);
			this._saveLabel.Name = "_saveLabel";
			this._saveLabel.Size = new Size(544, 16);
			this._saveLabel.TabIndex = 20;
			this._saveCheckBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this._saveCheckBox.Location = new Point(0, 93);
			this._saveCheckBox.Name = "_saveCheckBox";
			this._saveCheckBox.Size = new Size(544, 18);
			this._saveCheckBox.TabIndex = 30;
			this._saveCheckBox.CheckedChanged += this.OnSaveCheckBoxCheckedChanged;
			this._nameHelpLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this._nameHelpLabel.Location = new Point(0, 0);
			this._nameHelpLabel.Name = "_nameHelpLabel";
			this._nameHelpLabel.Size = new Size(0, 0);
			this._nameHelpLabel.TabIndex = 40;
			this._nameTextBox.Location = new Point(19, 113);
			this._nameTextBox.Name = "_nameTextBox";
			this._nameTextBox.Size = new Size(300, 20);
			this._nameTextBox.TabIndex = 50;
			this._nameTextBox.TextChanged += this.OnNameTextBoxTextChanged;
			base.Controls.Add(this._nameHelpLabel);
			base.Controls.Add(this._saveCheckBox);
			base.Controls.Add(this._saveLabel);
			base.Controls.Add(this._nameTextBox);
			base.Controls.Add(this._helpLabel);
			base.Name = "SqlDataSourceSaveConfiguredConnectionPanel";
			base.Size = new Size(544, 274);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private void InitializeUI()
		{
			this._helpLabel.Text = SR.GetString("SqlDataSourceSaveConfiguredConnectionPanel_HelpLabel");
			this._saveLabel.Text = SR.GetString("SqlDataSourceSaveConfiguredConnectionPanel_SaveLabel");
			this._saveCheckBox.Text = SR.GetString("SqlDataSourceSaveConfiguredConnectionPanel_SaveCheckBox");
			this._nameHelpLabel.Text = SR.GetString("SqlDataSourceSaveConfiguredConnectionPanel_NameTextBoxDescription");
			base.Caption = SR.GetString("SqlDataSourceSaveConfiguredConnectionPanel_PanelCaption");
		}

		protected internal override void OnComplete()
		{
			DesignerDataConnection designerDataConnection = this._dataConnection;
			if (this._saveCheckBox.Checked)
			{
				try
				{
					designerDataConnection = this._dataEnvironment.ConfigureConnection(this, this._dataConnection, this._nameTextBox.Text.Trim());
				}
				catch (Exception ex)
				{
					if (ex != CheckoutException.Canceled)
					{
						UIServiceHelper.ShowError(base.ServiceProvider, ex, SR.GetString("SqlDataSourceSaveConfiguredConnectionPanel_CouldNotSaveConnection"));
					}
				}
			}
			SqlDataSourceSaveConfiguredConnectionPanel.PersistConnectionSettings(this._sqlDataSource, this._sqlDataSourceDesigner, designerDataConnection);
			this._sqlDataSourceDesigner.SaveConfiguredConnectionState = designerDataConnection.IsConfigured;
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.UpdateFonts();
		}

		private void OnSaveCheckBoxCheckedChanged(object sender, EventArgs e)
		{
			this._nameTextBox.Enabled = this._saveCheckBox.Checked;
			this.CheckShouldAllowNext();
		}

		private void OnNameTextBoxTextChanged(object sender, EventArgs e)
		{
			this.CheckShouldAllowNext();
		}

		public override bool OnNext()
		{
			if (this._saveCheckBox.Checked)
			{
				ICollection connections = this._dataEnvironment.Connections;
				StringDictionary stringDictionary = new StringDictionary();
				foreach (object obj in connections)
				{
					DesignerDataConnection designerDataConnection = (DesignerDataConnection)obj;
					if (designerDataConnection.IsConfigured)
					{
						stringDictionary.Add(designerDataConnection.Name, null);
					}
				}
				if (stringDictionary.ContainsKey(this._nameTextBox.Text))
				{
					UIServiceHelper.ShowError(base.ServiceProvider, SR.GetString("SqlDataSourceSaveConfiguredConnectionPanel_DuplicateName", new object[] { this._nameTextBox.Text }));
					this._nameTextBox.Focus();
					return false;
				}
			}
			WizardPanel wizardPanel = SqlDataSourceConnectionPanel.CreateCommandPanel((SqlDataSourceWizardForm)base.ParentWizard, this._dataConnection, base.NextPanel);
			if (wizardPanel == null)
			{
				return false;
			}
			base.NextPanel = wizardPanel;
			return true;
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			base.ParentWizard.FinishButton.Enabled = false;
			if (base.Visible)
			{
				this.CheckShouldAllowNext();
				return;
			}
			base.ParentWizard.NextButton.Enabled = true;
		}

		internal static void PersistConnectionSettings(SqlDataSource sqlDataSource, SqlDataSourceDesigner sqlDataSourceDesigner, DesignerDataConnection dataConnection)
		{
			if (dataConnection.IsConfigured)
			{
				ExpressionBindingCollection expressions = ((IExpressionsAccessor)sqlDataSource).Expressions;
				PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(sqlDataSource)["ProviderName"];
				propertyDescriptor.ResetValue(sqlDataSource);
				if (dataConnection.ProviderName != "System.Data.SqlClient")
				{
					expressions.Add(new ExpressionBinding(propertyDescriptor.Name, propertyDescriptor.PropertyType, "ConnectionStrings", dataConnection.Name + ".ProviderName"));
				}
				propertyDescriptor = TypeDescriptor.GetProperties(sqlDataSource)["ConnectionString"];
				propertyDescriptor.ResetValue(sqlDataSource);
				expressions.Add(new ExpressionBinding(propertyDescriptor.Name, propertyDescriptor.PropertyType, "ConnectionStrings", dataConnection.Name));
				return;
			}
			if (sqlDataSource.ProviderName != dataConnection.ProviderName)
			{
				PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(sqlDataSource)["ProviderName"];
				propertyDescriptor2.ResetValue(sqlDataSource);
				propertyDescriptor2.SetValue(sqlDataSource, dataConnection.ProviderName);
			}
			if (sqlDataSource.ConnectionString != dataConnection.ConnectionString)
			{
				PropertyDescriptor propertyDescriptor2 = TypeDescriptor.GetProperties(sqlDataSource)["ConnectionString"];
				propertyDescriptor2.ResetValue(sqlDataSource);
				propertyDescriptor2.SetValue(sqlDataSource, dataConnection.ConnectionString);
			}
		}

		public void ResetUI()
		{
			this.UpdateFonts();
			this._saveCheckBox.Checked = true;
			this._nameTextBox.Text = string.Empty;
		}

		public void SetConnectionInfo(DesignerDataConnection dataConnection)
		{
			this._dataConnection = dataConnection;
			this.ResetUI();
			bool saveConfiguredConnectionState = this._sqlDataSourceDesigner.SaveConfiguredConnectionState;
			DesignerDataConnection designerDataConnection = new DesignerDataConnection(string.Empty, this._sqlDataSourceDesigner.ProviderName, this._sqlDataSourceDesigner.ConnectionString);
			if (SqlDataSourceDesigner.ConnectionsEqual(dataConnection, designerDataConnection))
			{
				if (!saveConfiguredConnectionState)
				{
					this._saveCheckBox.Checked = false;
				}
				if (this._nameTextBox.Text.Length == 0)
				{
					this._nameTextBox.Text = this.CreateDefaultConnectionName();
					return;
				}
			}
			else
			{
				this._nameTextBox.Text = this.CreateDefaultConnectionName();
			}
		}

		private void UpdateFonts()
		{
			Font font = new Font(this.Font, FontStyle.Bold);
			this._saveLabel.Font = font;
		}

		internal const string ConnectionStringExpressionPrefix = "ConnectionStrings";

		internal const string ConnectionStringExpressionConnectionSuffix = "ConnectionString";

		internal const string ConnectionStringExpressionProviderSuffix = "ProviderName";

		private global::System.Windows.Forms.Label _helpLabel;

		private global::System.Windows.Forms.Label _saveLabel;

		private global::System.Windows.Forms.CheckBox _saveCheckBox;

		private global::System.Windows.Forms.TextBox _nameTextBox;

		private global::System.Windows.Forms.Label _nameHelpLabel;

		private IDataEnvironment _dataEnvironment;

		private SqlDataSourceDesigner _sqlDataSourceDesigner;

		private SqlDataSource _sqlDataSource;

		private DesignerDataConnection _dataConnection;

		private static class ConnectionStringHelper
		{
			public static string GetConnectionName(DesignerDataConnection connection)
			{
				DbProviderFactory dbProviderFactory = SqlDataSourceDesigner.GetDbProviderFactory(connection.ProviderName);
				DbConnectionStringBuilder dbConnectionStringBuilder = dbProviderFactory.CreateConnectionStringBuilder();
				if (dbConnectionStringBuilder == null)
				{
					dbConnectionStringBuilder = new DbConnectionStringBuilder();
				}
				string text = null;
				try
				{
					dbConnectionStringBuilder.ConnectionString = connection.ConnectionString;
					if (SqlDataSourceSaveConfiguredConnectionPanel.ConnectionStringHelper.IsLocalDbFileConnectionString(connection.ProviderName, dbConnectionStringBuilder))
					{
						string filePathKey = SqlDataSourceSaveConfiguredConnectionPanel.ConnectionStringHelper.GetFilePathKey(connection.ProviderName, dbConnectionStringBuilder);
						if (!string.IsNullOrEmpty(filePathKey))
						{
							string text2 = dbConnectionStringBuilder[filePathKey] as string;
							if (!string.IsNullOrEmpty(text2))
							{
								text = Path.GetFileNameWithoutExtension(text2) + "ConnectionString";
							}
						}
					}
					object obj;
					if (text == null && dbConnectionStringBuilder.TryGetValue("Database", out obj))
					{
						string text3 = obj as string;
						if (!SqlDataSourceSaveConfiguredConnectionPanel.ConnectionStringHelper.StringIsEmpty(text3))
						{
							text = text3 + "ConnectionString";
						}
					}
				}
				catch (Exception)
				{
				}
				if (text == null)
				{
					text = "ConnectionString";
				}
				return text.Trim();
			}

			private static string GetFilePathKey(string providerName, DbConnectionStringBuilder connectionStringBuilder)
			{
				if (SqlDataSourceSaveConfiguredConnectionPanel.ConnectionStringHelper.IsAccessConnectionString(providerName, connectionStringBuilder))
				{
					return "Data Source";
				}
				if (SqlDataSourceSaveConfiguredConnectionPanel.ConnectionStringHelper.IsSqlLocalConnectionString(providerName, connectionStringBuilder))
				{
					return "AttachDbFileName";
				}
				return null;
			}

			private static bool IsAccessConnectionString(string providerName, DbConnectionStringBuilder connectionStringBuilder)
			{
				if (string.Equals(providerName, "System.Data.OleDb", StringComparison.OrdinalIgnoreCase))
				{
					string text = connectionStringBuilder["provider"] as string;
					if (!string.IsNullOrEmpty(text) && text.ToUpperInvariant().StartsWith("MICROSOFT.JET", StringComparison.Ordinal))
					{
						return true;
					}
				}
				return false;
			}

			private static bool IsLocalDbFileConnectionString(string providerName, DbConnectionStringBuilder connectionStringBuilder)
			{
				return SqlDataSourceSaveConfiguredConnectionPanel.ConnectionStringHelper.IsSqlLocalConnectionString(providerName, connectionStringBuilder) || SqlDataSourceSaveConfiguredConnectionPanel.ConnectionStringHelper.IsAccessConnectionString(providerName, connectionStringBuilder);
			}

			private static bool IsSqlLocalConnectionString(string providerName, DbConnectionStringBuilder connectionStringBuilder)
			{
				return string.Equals(providerName, "System.Data.SqlClient", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(connectionStringBuilder["AttachDbFileName"] as string);
			}

			private static bool StringIsEmpty(string s)
			{
				return string.IsNullOrEmpty(s) || s.Trim().Length == 0;
			}

			private const string DefaultConnectionName = "ConnectionString";

			private const string JetOleDbProviderName = "MICROSOFT.JET";

			private const string SqlClientProviderName = "System.Data.SqlClient";

			private const string OleDbProviderName = "System.Data.OleDb";
		}
	}
}
