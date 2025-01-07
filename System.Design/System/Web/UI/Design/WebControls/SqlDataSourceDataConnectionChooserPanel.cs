using System;
using System.Collections;
using System.ComponentModel.Design.Data;
using System.Design;
using System.Drawing;
using System.Web.UI.Design.Util;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace System.Web.UI.Design.WebControls
{
	internal class SqlDataSourceDataConnectionChooserPanel : SqlDataSourceConnectionPanel
	{
		public SqlDataSourceDataConnectionChooserPanel(SqlDataSourceDesigner sqlDataSourceDesigner, IDataEnvironment dataEnvironment)
			: base(sqlDataSourceDesigner)
		{
			this._sqlDataSourceDesigner = sqlDataSourceDesigner;
			this._sqlDataSource = (SqlDataSource)this._sqlDataSourceDesigner.Component;
			this._dataEnvironment = dataEnvironment;
			this.InitializeComponent();
			this.InitializeUI();
			DesignerDataConnection designerDataConnection = new DesignerDataConnection(SR.GetString("SqlDataSourceDataConnectionChooserPanel_CustomConnectionName"), this._sqlDataSource.ProviderName, this._sqlDataSource.ConnectionString);
			ExpressionBindingCollection expressions = ((IExpressionsAccessor)this._sqlDataSource).Expressions;
			ExpressionBinding expressionBinding = expressions["ConnectionString"];
			if (expressionBinding != null && string.Equals(expressionBinding.ExpressionPrefix, "ConnectionStrings", StringComparison.OrdinalIgnoreCase))
			{
				string text = expressionBinding.Expression;
				string text2 = "." + "ConnectionString".ToLowerInvariant();
				if (text.ToLowerInvariant().EndsWith(text2, StringComparison.Ordinal))
				{
					text = text.Substring(0, text.Length - text2.Length);
				}
				ICollection connections = this._dataEnvironment.Connections;
				if (connections != null)
				{
					foreach (object obj in connections)
					{
						DesignerDataConnection designerDataConnection2 = (DesignerDataConnection)obj;
						if (designerDataConnection2.IsConfigured && string.Equals(designerDataConnection2.Name, text, StringComparison.OrdinalIgnoreCase))
						{
							designerDataConnection = designerDataConnection2;
							break;
						}
					}
				}
			}
			this.SetConnectionSettings(designerDataConnection);
		}

		public override DesignerDataConnection DataConnection
		{
			get
			{
				return ((SqlDataSourceDataConnectionChooserPanel.DataConnectionItem)this._connectionsComboBox.SelectedItem).DesignerDataConnection;
			}
		}

		private void CheckShouldAllowNext()
		{
			if (base.ParentWizard != null)
			{
				base.ParentWizard.NextButton.Enabled = this._connectionsComboBox.SelectedItem != null;
			}
		}

		private void InitializeComponent()
		{
			this._chooseLabel = new global::System.Windows.Forms.Label();
			this._connectionsComboBox = new AutoSizeComboBox();
			this._newConnectionButton = new global::System.Windows.Forms.Button();
			this._connectionTableLayoutPanel = new TableLayoutPanel();
			this._detailsButton = new SqlDataSourceDataConnectionChooserPanel.DetailsButton();
			this._connectionStringLabel = new global::System.Windows.Forms.Label();
			this._dividerLabel = new global::System.Windows.Forms.Label();
			this._connectionStringTextBox = new global::System.Windows.Forms.TextBox();
			this._connectionTableLayoutPanel.SuspendLayout();
			base.SuspendLayout();
			this._chooseLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this._chooseLabel.Location = new Point(0, 0);
			this._chooseLabel.Name = "_chooseLabel";
			this._chooseLabel.Size = new Size(544, 16);
			this._chooseLabel.TabIndex = 10;
			this._connectionTableLayoutPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this._connectionTableLayoutPanel.ColumnCount = 2;
			this._connectionTableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
			this._connectionTableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
			this._connectionTableLayoutPanel.Controls.Add(this._newConnectionButton, 1, 0);
			this._connectionTableLayoutPanel.Controls.Add(this._connectionsComboBox, 0, 0);
			this._connectionTableLayoutPanel.Location = new Point(0, 18);
			this._connectionTableLayoutPanel.Name = "_connectionTableLayoutPanel";
			this._connectionTableLayoutPanel.RowCount = 1;
			this._connectionTableLayoutPanel.RowStyles.Add(new RowStyle());
			this._connectionTableLayoutPanel.Size = new Size(544, 23);
			this._connectionTableLayoutPanel.TabIndex = 20;
			this._connectionsComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this._connectionsComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			this._connectionsComboBox.Location = new Point(0, 0);
			this._connectionsComboBox.Margin = new Padding(0, 0, 3, 0);
			this._connectionsComboBox.Name = "_connectionsComboBox";
			this._connectionsComboBox.Size = new Size(463, 21);
			this._connectionsComboBox.Sorted = true;
			this._connectionsComboBox.TabIndex = 10;
			this._connectionsComboBox.SelectedIndexChanged += this.OnConnectionsComboBoxSelectedIndexChanged;
			this._newConnectionButton.AutoSize = true;
			this._newConnectionButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this._newConnectionButton.Location = new Point(469, 0);
			this._newConnectionButton.Margin = new Padding(3, 0, 0, 0);
			this._newConnectionButton.MinimumSize = new Size(75, 23);
			this._newConnectionButton.Name = "_newConnectionButton";
			this._newConnectionButton.Padding = new Padding(10, 0, 10, 0);
			this._newConnectionButton.Size = new Size(75, 23);
			this._newConnectionButton.TabIndex = 20;
			this._newConnectionButton.Click += this.OnNewConnectionButtonClick;
			this._detailsButton.Location = new Point(0, 51);
			this._detailsButton.Name = "_detailsButton";
			this._detailsButton.Size = new Size(15, 15);
			this._detailsButton.TabIndex = 30;
			this._detailsButton.Click += this.OnDetailsButtonClick;
			this._connectionStringLabel.AutoSize = true;
			this._connectionStringLabel.Location = new Point(21, 51);
			this._connectionStringLabel.Name = "_connectionStringLabel";
			this._connectionStringLabel.Padding = new Padding(0, 0, 6, 0);
			this._connectionStringLabel.Size = new Size(92, 16);
			this._connectionStringLabel.TabIndex = 40;
			this._dividerLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this._dividerLabel.BackColor = SystemColors.ControlDark;
			this._dividerLabel.Location = new Point(30, 57);
			this._dividerLabel.Name = "_dividerLabel";
			this._dividerLabel.Size = new Size(514, 1);
			this._dividerLabel.TabIndex = 50;
			this._connectionStringTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this._connectionStringTextBox.BackColor = SystemColors.Control;
			this._connectionStringTextBox.Location = new Point(21, 71);
			this._connectionStringTextBox.Multiline = true;
			this._connectionStringTextBox.Name = "_connectionStringTextBox";
			this._connectionStringTextBox.ReadOnly = true;
			this._connectionStringTextBox.ScrollBars = global::System.Windows.Forms.ScrollBars.Vertical;
			this._connectionStringTextBox.Size = new Size(523, 90);
			this._connectionStringTextBox.TabIndex = 60;
			this._connectionStringTextBox.Text = "";
			this._connectionStringTextBox.Visible = false;
			base.Controls.Add(this._connectionStringLabel);
			base.Controls.Add(this._dividerLabel);
			base.Controls.Add(this._detailsButton);
			base.Controls.Add(this._connectionStringTextBox);
			base.Controls.Add(this._chooseLabel);
			base.Controls.Add(this._connectionTableLayoutPanel);
			base.Name = "SqlDataSourceDataConnectionChooserPanel";
			base.Size = new Size(544, 274);
			this._connectionTableLayoutPanel.ResumeLayout(false);
			this._connectionTableLayoutPanel.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private void InitializeUI()
		{
			this._newConnectionButton.Text = SR.GetString("SqlDataSourceDataConnectionChooserPanel_NewConnectionButton");
			this._chooseLabel.Text = SR.GetString("SqlDataSourceDataConnectionChooserPanel_ChooseLabel");
			this._connectionStringLabel.Text = SR.GetString("SqlDataSourceDataConnectionChooserPanel_ConnectionStringLabel");
			this._detailsButton.AccessibleName = SR.GetString("SqlDataSourceDataConnectionChooserPanel_DetailsButtonName");
			this._detailsButton.AccessibleDescription = SR.GetString("SqlDataSourceDataConnectionChooserPanel_DetailsButtonDesc");
			ICollection connections = this._dataEnvironment.Connections;
			foreach (object obj in connections)
			{
				DesignerDataConnection designerDataConnection = (DesignerDataConnection)obj;
				this._connectionsComboBox.Items.Add(new SqlDataSourceDataConnectionChooserPanel.DataConnectionItem(designerDataConnection));
			}
			this._connectionsComboBox.InvalidateDropDownWidth();
			base.Caption = SR.GetString("SqlDataSourceDataConnectionChooserPanel_PanelCaption");
			this.UpdateFonts();
		}

		protected internal override void OnComplete()
		{
			if (this._needsToPersistConnectionInfo)
			{
				SqlDataSourceSaveConfiguredConnectionPanel.PersistConnectionSettings(this._sqlDataSource, this._sqlDataSourceDesigner, this.DataConnection);
			}
		}

		private void OnConnectionsComboBoxSelectedIndexChanged(object sender, EventArgs e)
		{
			this.CheckShouldAllowNext();
			SqlDataSourceDataConnectionChooserPanel.DataConnectionItem dataConnectionItem = this._connectionsComboBox.SelectedItem as SqlDataSourceDataConnectionChooserPanel.DataConnectionItem;
			if (dataConnectionItem == null)
			{
				return;
			}
			this._connectionStringTextBox.Text = dataConnectionItem.DesignerDataConnection.ConnectionString;
		}

		private void OnDetailsButtonClick(object sender, EventArgs e)
		{
			this._connectionStringTextBox.Visible = !this._connectionStringTextBox.Visible;
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.UpdateFonts();
		}

		private void OnNewConnectionButtonClick(object sender, EventArgs e)
		{
			DesignerDataConnection designerDataConnection = this._dataEnvironment.BuildConnection(this, null);
			if (designerDataConnection != null && !this.SelectConnection(designerDataConnection))
			{
				SqlDataSourceDataConnectionChooserPanel.DataConnectionItem dataConnectionItem = new SqlDataSourceDataConnectionChooserPanel.DataConnectionItem(designerDataConnection);
				this._connectionsComboBox.Items.Add(dataConnectionItem);
				this._connectionsComboBox.SelectedItem = dataConnectionItem;
				this._connectionsComboBox.InvalidateDropDownWidth();
			}
		}

		public override bool OnNext()
		{
			if (!base.CheckValidProvider())
			{
				return false;
			}
			DesignerDataConnection dataConnection = this.DataConnection;
			if (!dataConnection.IsConfigured)
			{
				this._needsToPersistConnectionInfo = false;
				SqlDataSourceSaveConfiguredConnectionPanel sqlDataSourceSaveConfiguredConnectionPanel = base.NextPanel as SqlDataSourceSaveConfiguredConnectionPanel;
				if (sqlDataSourceSaveConfiguredConnectionPanel == null)
				{
					sqlDataSourceSaveConfiguredConnectionPanel = ((SqlDataSourceWizardForm)base.ParentWizard).GetSaveConfiguredConnectionPanel();
					base.NextPanel = sqlDataSourceSaveConfiguredConnectionPanel;
				}
				if (!SqlDataSourceDesigner.ConnectionsEqual(dataConnection, sqlDataSourceSaveConfiguredConnectionPanel.CurrentConnection))
				{
					sqlDataSourceSaveConfiguredConnectionPanel.SetConnectionInfo(dataConnection);
				}
				return true;
			}
			this._needsToPersistConnectionInfo = true;
			return base.OnNext();
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if (base.Visible)
			{
				this.CheckShouldAllowNext();
			}
		}

		private bool SelectConnection(DesignerDataConnection conn)
		{
			if (conn.IsConfigured)
			{
				using (IEnumerator enumerator = this._connectionsComboBox.Items.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						SqlDataSourceDataConnectionChooserPanel.DataConnectionItem dataConnectionItem = (SqlDataSourceDataConnectionChooserPanel.DataConnectionItem)obj;
						DesignerDataConnection designerDataConnection = dataConnectionItem.DesignerDataConnection;
						if (designerDataConnection.IsConfigured && designerDataConnection.Name == conn.Name)
						{
							this._connectionsComboBox.SelectedItem = dataConnectionItem;
							return true;
						}
					}
					return false;
				}
			}
			foreach (object obj2 in this._connectionsComboBox.Items)
			{
				SqlDataSourceDataConnectionChooserPanel.DataConnectionItem dataConnectionItem2 = (SqlDataSourceDataConnectionChooserPanel.DataConnectionItem)obj2;
				DesignerDataConnection designerDataConnection2 = dataConnectionItem2.DesignerDataConnection;
				if (!designerDataConnection2.IsConfigured && SqlDataSourceDesigner.ConnectionsEqual(designerDataConnection2, conn))
				{
					this._connectionsComboBox.SelectedItem = dataConnectionItem2;
					return true;
				}
			}
			return false;
		}

		private void SetConnectionSettings(DesignerDataConnection conn)
		{
			bool flag = this.SelectConnection(conn);
			string text = conn.ProviderName;
			string connectionString = conn.ConnectionString;
			if (!flag && (text.Length > 0 || connectionString.Length > 0))
			{
				if (text.Length == 0)
				{
					text = "System.Data.SqlClient";
				}
				this._connectionsComboBox.Items.Insert(0, new SqlDataSourceDataConnectionChooserPanel.DataConnectionItem(new DesignerDataConnection(conn.Name, text, connectionString)));
				this._connectionsComboBox.SelectedIndex = 0;
				this._connectionsComboBox.InvalidateDropDownWidth();
			}
			this._connectionStringTextBox.Text = connectionString;
		}

		private void UpdateFonts()
		{
			this._chooseLabel.Font = new Font(this.Font, FontStyle.Bold);
		}

		private AutoSizeComboBox _connectionsComboBox;

		private global::System.Windows.Forms.Label _chooseLabel;

		private global::System.Windows.Forms.Button _newConnectionButton;

		private global::System.Windows.Forms.TextBox _connectionStringTextBox;

		private global::System.Windows.Forms.Label _connectionStringLabel;

		private TableLayoutPanel _connectionTableLayoutPanel;

		private global::System.Windows.Forms.Label _dividerLabel;

		private SqlDataSourceDataConnectionChooserPanel.DetailsButton _detailsButton;

		private SqlDataSource _sqlDataSource;

		private SqlDataSourceDesigner _sqlDataSourceDesigner;

		private IDataEnvironment _dataEnvironment;

		private bool _needsToPersistConnectionInfo;

		private sealed class DataConnectionItem
		{
			public DataConnectionItem(DesignerDataConnection designerDataConnection)
			{
				this._designerDataConnection = designerDataConnection;
			}

			public DesignerDataConnection DesignerDataConnection
			{
				get
				{
					return this._designerDataConnection;
				}
			}

			public override string ToString()
			{
				return this._designerDataConnection.Name;
			}

			private DesignerDataConnection _designerDataConnection;
		}

		private sealed class DetailsButton : global::System.Windows.Forms.Button
		{
			protected override void OnClick(EventArgs e)
			{
				this._details = !this._details;
				base.OnClick(e);
				base.Invalidate();
			}

			protected override void OnPaint(PaintEventArgs e)
			{
				base.OnPaint(e);
				e.Graphics.DrawLine(SystemPens.ControlText, base.Width / 2 - 3, base.Height / 2, base.Width / 2 + 3, base.Height / 2);
				if (!this._details)
				{
					e.Graphics.DrawLine(SystemPens.ControlText, base.Width / 2, base.Height / 2 - 3, base.Width / 2, base.Height / 2 + 3);
				}
			}

			private const int PlusLineLength = 3;

			private bool _details;
		}
	}
}
